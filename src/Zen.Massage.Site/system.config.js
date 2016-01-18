System.config({
    baseURL: '//localhost:1282/',
    map: {
        jquery: 'js/lib/jquery/jquery.js',
        tether: 'js/lib/tether/tether.js',
        bootstrap: '/js/lib/bootstrap/bootstrap.js'
		angular2: 'js/lib/angular2'
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
