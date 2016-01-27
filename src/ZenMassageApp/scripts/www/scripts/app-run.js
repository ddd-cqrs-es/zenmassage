var ZenMassageApp;
(function (ZenMassageApp) {
    'use strict';
    function startup($rootScope, $cookies, currentUser) {
        // Setup global user state
        currentUser.userId = $cookies.userId;
        currentUser.pebbleUserId = $cookies.pebbleUserId;
        // Placeholder for detecting route change errors
        $rootScope.$on('$routeChangeError', function () { });
    }
    startup.$inject = ['$rootScope', '$cookies', 'currentUser'];
    angular
        .module('app')
        .run(startup);
})(ZenMassageApp || (ZenMassageApp = {}));
//# sourceMappingURL=app-run.js.map