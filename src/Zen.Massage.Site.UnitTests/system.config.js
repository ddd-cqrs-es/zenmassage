System.config({
    map: {
        systemjs: 'node_modules/systemjs/dist/system.js',
        'es6-module-loader': 'node_modules/es6-module-loader/dist/es6-module-loader.js',
        'es6-promise': 'node_modules/es6-promise/dist/es6-promise.js',
        'es6-shim': 'node_modules/es6-shim/es6-shim.js',
        jquery: 'node_modules/jquery/dist/jquery.js',
        tether: 'node_modules/tether/dist/js/tether.js',
        bootstrap: 'node_modules/bootstrap/dist/js/bootstrap.js',
        angular2: 'node_modules/angular2'
    },
    meta: {
        'node_modules/jquery/dist/jquery.js': {
            format: 'global',
            exports: '$'
        },
        'js/lib/tether.js': {
            format: 'global',
            exports: 'Tether'
        }
    },
    transpiler: null
});
