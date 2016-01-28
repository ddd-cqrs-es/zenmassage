/// <binding BeforeBuild='copy:fromnode, compile' Clean='clean' ProjectOpened='copy:fromnode' />
'use strict';

var del = require('del'),
    merge = require('merge2'),
    gulp = require('gulp'),
    autoprefixer = require('gulp-autoprefixer'),
    concat = require('gulp-concat'),
    copy = require('gulp-copy'),
    cssmin = require('gulp-cssmin'),
    debug = require('gulp-debug'),
    environments = require('gulp-environments'),
    inject = require('gulp-inject'),
    plumber = require('gulp-plumber'),
    tsc = require('gulp-typescript'),
    //tslint = require('gulp-tslint'),
    sourcemaps = require('gulp-sourcemaps'),
    uglify = require('gulp-uglify'),
    rename = require('gulp-rename'),
    sass = require('gulp-sass'),
    sassLint = require('gulp-sass-lint'),
    stylus = require('gulp-stylus'),
    teamBuild = require('taco-team-build'),
    Config = require('./gulpfile.config'),
    tsProject = tsc.createProject('./scripts/tsconfig.json');

// Determine runtime environment (default to DEV for now)
var development = environments.development;
var production = environments.production;
if (!development() && !production()) {
    environments.current(development);
}

// Pull configuration
var config = new Config();

// Copy library components to wwwroot folders
gulp.task('copy:fromnode', function () {
    return merge([
        gulp.src([
            config.paths.nodeModulesRoot + 'jquery/dist/jquery.js'
            ])
            .pipe(gulp.dest(config.paths.libPath + 'jquery/')),
        gulp.src([
            config.paths.nodeModulesRoot + 'angular/angular.js',
            config.paths.nodeModulesRoot + 'angular/angular.min.js'
            ])
            .pipe(gulp.dest(config.paths.libPath + 'angular/')),
        gulp.src([
            config.paths.nodeModulesRoot + 'angular-animate/angular-animate.js',
            config.paths.nodeModulesRoot + 'angular-animate/angular-animate.min.js'
	        ])
            .pipe(gulp.dest(config.paths.libPath + 'angular/')),
        gulp.src([
            config.paths.nodeModulesRoot + 'angular-cookies/angular-cookies.js',
            config.paths.nodeModulesRoot + 'angular-cookies/angular-cookies.min.js'
            ])
            .pipe(gulp.dest(config.paths.libPath + 'angular/')),
        gulp.src([
            config.paths.nodeModulesRoot + 'angular-resource/angular-resource.js',
            config.paths.nodeModulesRoot + 'angular-resource/angular-resource.min.js'
            ])
            .pipe(gulp.dest(config.paths.libPath + 'angular/')),
        gulp.src([
            config.paths.nodeModulesRoot + 'angular-route/angular-route.js',
            config.paths.nodeModulesRoot + 'angular-route/angular-route.min.js'
            ])
            .pipe(gulp.dest(config.paths.libPath + 'angular/')),
        gulp.src([
            config.paths.nodeModulesRoot + 'angular-sanitize/angular-sanitize.js',
            config.paths.nodeModulesRoot + 'angular-sanitize/angular-sanitize.min.js'
            ])
            .pipe(gulp.dest(config.paths.libPath + 'angular/')),
        gulp.src([
            config.paths.nodeModulesRoot + 'onsenui/css/**/*.*'
            ])
            .pipe(gulp.dest(config.paths.libPath + 'onsen/css/')),
        gulp.src([
            config.paths.nodeModulesRoot + 'onsenui/js/angular-onsenui.js',
            config.paths.nodeModulesRoot + 'onsenui/js/angular-onsenui.min.js',
            config.paths.nodeModulesRoot + 'onsenui/js/onsenui.js',
            config.paths.nodeModulesRoot + 'onsenui/js/onsenui.min.js'
            ])
            .pipe(gulp.dest(config.paths.libPath + 'onsen/js/'))
    ]);
});

// Build onsen ui stylus files
gulp.task('compile:stylus', function () {
    return gulp.src([
        config.paths.nodeModulesRoot + 'onsenui/stylus/*-theme.styl'])
        .pipe(plumber())
        .pipe(stylus({ errors: true, define: { mylighten: mylighten } }))
        .pipe(autoprefixer('> 1%', 'last 2 version', 'ff 12', 'ie 11', 'opera 12', 'chrome 12', 'safari 12', 'android 2'))
        .pipe(rename(function (path) {
            path.dirname = '.';
            path.basename = 'onsen-css-components-' + path.basename;
            path.ext = 'css';
        }))
        .pipe(gulp.dest(config.paths.libPath + 'onsen/css/'));

    // needs for compile
    function mylighten(param) {
        if (param.rgba) {
            var result = param.clone();
            result.rgba.a = 0.2;
            return result;
        }
        throw new Error('mylighten() first argument must be color.');
    }
});

// Lint CSS
gulp.task('lint:sass', function () {
    return gulp
        .src(config.paths.sassAppSelector)
        .pipe(sassLint())
        .pipe(sassLint.format())
        .pipe(sassLint.failOnError());
});

gulp.task('compile:sass', function () {
    var sourceFiles = [
        config.paths.sassAppSelector
    ];
    var includePaths = [
        config.paths.nodeModulesRoot + 'bootstrap/scss/',
        config.paths.nodeModulesRoot + 'tether/src/css/'
    ];
    return gulp
        .src(sourceFiles)
        .pipe(sourcemaps.init())
        .pipe(development(sass({
            outputStyle: 'expanded',
            includePaths: includePaths
        }).on('error', sass.logError)))
        .pipe(production(sass({
            outputStyle: 'compressed',
            includePaths: includePaths
        }).on('error', sass.logError)))
        .pipe(autoprefixer('> 1%', 'last 2 version', 'ff 12', 'ie 11', 'opera 12', 'chrome 12', 'safari 12', 'android 2'))
        .pipe(sourcemaps.write())
        .pipe(gulp.dest(config.paths.sassOutputPath));
});

gulp.task('clean:sass', function (cb) {
    var files = [
        config.paths.sassOutputPath + '**/*.css',
        config.paths.sassOutputPath + '**/*.css.map',
        '!' + config.paths.sassOutputPath + 'lib'
    ];

    del(files, cb);
});

// Lint all custom TypeScript files.
//gulp.task('lint:ts', function () {
//    return gulp
//        .src(config.paths.tsAppSelector)
//        .pipe(tslint())
//        .pipe(tslint.report('prose'));
//});

// Compile all TypeScript and include references to library
gulp.task('compile:ts', function () {
    var sourceFiles = [
        config.paths.tsAppSelector,
        config.paths.tsTypingsSelector
    ];

    var tsResult = gulp
        .src(sourceFiles)
        .pipe(sourcemaps.init())
        .pipe(tsc(tsProject));

    tsResult.dts.pipe(gulp.dest(config.paths.tsOutputPath));
    return tsResult.js
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest(config.paths.tsOutputPath));
});

gulp.task('build:android', function () {
    var buildArgs = [];
    if (development()) {
        buildArgs.push('--debug');
    } else {
        buildArgs.push('--release');
    }
    buildArgs.push('--gradleArg=--no-daemon');

    return teamBuild
        .buildProject("android", buildArgs)
        .then(function() {
            return teamBuild.packageProject("android");
        });
});

// Remove all generated JavaScript files from TS compilation
gulp.task('clean:ts', function (cb) {
    var files = [
        config.paths.tsOutputPath + '**/*.js',
        config.paths.tsOutputPath + '**/*.js.map'
    ];

    del(files, cb);
});

gulp.task('clean:lib', function(cb) {
    var files = [
        config.paths.libPath + '**/*.*'
    ];

    del(files, cb);
});

gulp.task('clean', ['clean:lib', 'clean:sass', 'clean:ts']);

gulp.task('compile', ['compile:stylus', 'compile:sass', 'compile:ts']);

gulp.task('watch', function () {
    gulp.watch([config.paths.sassAppSelector], ['compile:sass']);
    gulp.watch([config.paths.tsAppSelector], ['compile:ts']);
});

gulp.task('default', ['compile']);
