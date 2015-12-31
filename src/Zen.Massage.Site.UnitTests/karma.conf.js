module.exports = function (config) {
    config.set({
        // frameworks to use
        frameworks: ['mocha', 'karma-systemjs'],

        // list of files / patterns to load in the browser
        files: [
            //The configuration should allow only test-main.js to be included
            'scripts/test-main.js',
            { pattern: '../Zen.Massage.Site/wwwroot/js/**/*.js', included: false },
            { pattern: '../Zen.Massage.Site/wwwroot/js/**/*.js.map', included: false },
            { pattern: '../Zen.Massage.Site/typescript/app/**/*.ts', included: false },
            { pattern: 'scripts/**/*.js', included: false },
            { pattern: 'scripts/**/*.js.map', included: false },
            { pattern: 'scripts/**/*.ts', included: false }
        ],

        // list of files to exclude
        exclude: [],

        // test results reporter to use
        // possible values: 'dots', 'progress', 'junit', 'growl', 'coverage'
        reporters: ['progress'],

        // web server port
        port: 9876,

        // enable / disable colors in the output (reporters and logs)
        colors: true,

        // level of logging
        // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
        logLevel: config.LOG_INFO,

        // enable / disable watching file and executing tests whenever any file changes
        autoWatch: true,

        // Start these browsers, currently available:
        // - Chrome
        // - ChromeCanary
        // - Firefox
        // - Opera
        // - Safari (only Mac)
        // - PhantomJS
        // - IE (only Windows)
        browsers: ['Chrome'],

        // If browser does not capture in given timeout [ms], kill it
        captureTimeout: 60000,

        // Continuous Integration mode
        // if true, it capture browsers, run tests and exit
        singleRun: false,

        systemjs: {
            configFile: 'system.config.js'
        }
    });
};
