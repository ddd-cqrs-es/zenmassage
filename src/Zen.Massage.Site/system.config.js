System.config({
    baseURL: '//localhost:1282/',
    map: {
        jquery: 'js/lib/jquery.js',
        tether: 'js/lib/tether.js',
        bootstrap: '/js/lib/bootstrap.js'
    },
    meta: {
        'js/lib/tether.js': {
            format: 'global',
            exports: 'Tether'
        }
    },
    bundles: {
        sitecore: ['jquery', 'tether', 'bootstrap']
    }
});
