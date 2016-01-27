((): void => {
    'use strict';

    function config($locationProvider: ng.ILocationProvider): void {
        $locationProvider.html5Mode(true);
    }

    config.$inject = ['$locationProvider'];

    angular
        .module('app')
        .config(config);
})();