(function () {
    'use strict';
    function config($locationProvider) {
        $locationProvider.html5Mode(true);
    }
    config.$inject = ['$locationProvider'];
    angular
        .module('app')
        .config(config);
})();
//# sourceMappingURL=app-config.js.map