((): void => {
    'use strict';

    angular.module(
        'app.core', [
            'ngCookies',
            'ngRoute',
            'ngResource',
            'ngSanitize'
        ]);
})();
