/// <binding AfterBuild='postbuild:swagger' Clean='clean' />
'use strict';

var del = require('del'),
    merge = require('merge2'),
    gulp = require('gulp'),
    concat = require('gulp-concat'),
    copy = require('gulp-copy'),
    cssmin = require('gulp-cssmin'),
    debug = require('gulp-debug'),
    environments = require('gulp-environments'),
    inject = require('gulp-inject'),
    tsc = require('gulp-typescript'),
    //tslint = require('gulp-tslint'),
    sourcemaps = require('gulp-sourcemaps'),
    uglify = require('gulp-uglify'),
    sass = require('gulp-sass'),
    sassLint = require('gulp-sass-lint'),
    Builder = require('systemjs-builder'),
    Config = require('./gulpfile.config'),
    tsProject = tsc.createProject('./typescript/tsconfig.json');

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
            .pipe(gulp.dest(config.paths.jsLibPath + 'jquery/')),
        gulp.src([
            config.paths.nodeModulesRoot + 'tether/dist/js/tether.js'
            ])
            .pipe(gulp.dest(config.paths.jsLibPath + 'tether/')),
        gulp.src([
            config.paths.nodeModulesRoot + 'bootstrap/dist/js/bootstrap.js'
            ])
            .pipe(gulp.dest(config.paths.jsLibPath + 'bootstrap/')),
        gulp.src([
            config.paths.nodeModulesRoot + 'Slate/dist/js/slate.js',
            config.paths.nodeModulesRoot + 'Slate/dist/js/slate.min.js'
            ])
            .pipe(gulp.dest(config.paths.jsLibPath + 'slate/')),

        gulp.src([
            config.paths.nodeModulesRoot + 'Slate/dist/css/slate.css',
            config.paths.nodeModulesRoot + 'Slate/dist/css/slate.min.css'
            ])
            .pipe(gulp.dest(config.paths.sassOutputPath)),

        gulp.src([
            config.paths.nodeModulesRoot + 'Slate/dist/fonts/*.*'
            ])
            .pipe(gulp.dest(config.paths.fontsOutputPath))
    ]);
});

// Lint all custom TypeScript files.
gulp.task('lint:ts', function () {
    return gulp
        .src(config.paths.tsAppSelector)
        .pipe(tslint())
        .pipe(tslint.report('prose'));
});

// Compile all TypeScript and include references to library
gulp.task('compile:ts', ['copy:fromnode'], function () {
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

// Remove all generated JavaScript files from TS compilation
gulp.task('clean:ts', function (cb) {
    var files = [
        config.paths.tsOutputPath + '**/*.js',
        config.paths.tsOutputPath + '**/*.js.map',
        '!' + config.paths.tsOutputPath + 'lib'
    ];

    del(files, cb);
});

gulp.task('compile:sitecore', function (cb) {
    var builder = new Builder(config.paths.webroot, config.paths.root + 'system.config.js');
    builder.buildStatic(
        config.paths.jsLibPath + 'jquery/jquery.js + ' +
        config.paths.jsLibPath + 'tether/tether.js + ' +
        config.paths.jsLibPath + 'bootstrap/bootstrap.js',
        config.paths.tsOutputPath + 'sitecore.js')
    .then(function () {
        cb();
    })
    .catch(function (ex) {
        cb(ex);
    });
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

gulp.task('watch', function () {
    gulp.watch([config.paths.sassAppSelector], ['compile:sass']);
    gulp.watch([config.paths.tsAppSelector], ['compile:ts']);
});

gulp.task('default', ['compile:sitecore', 'compile:ts', 'compile:sass']);

gulp.task('postbuild:swagger', function() {
    return gulp
        .src(config.paths.swaggerSourcePath)
        .pipe(gulp.dest(config.paths.swaggerDestinationPath));
});

gulp.task('clean', ['clean:ts', 'clean:sass']);
