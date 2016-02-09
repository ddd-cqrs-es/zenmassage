var ZenMassageApp;
(function (ZenMassageApp) {
    'use strict';
    var ApplicationStartup = (function () {
        function ApplicationStartup($rootScope, $window, $cookies, currentUser, pebbleServices) {
            var _this = this;
            this.$rootScope = $rootScope;
            this.$window = $window;
            this.$cookies = $cookies;
            this.currentUser = currentUser;
            this.pebbleServices = pebbleServices;
            $window.onload = function () {
                _this.initialize();
            };
            // Setup global user state
            currentUser.userId = $cookies.userId;
            currentUser.pebbleUserId = $cookies.pebbleUserId;
            // Placeholder for detecting route change errors
            $rootScope.$on('$routeChangeError', function () { });
        }
        ApplicationStartup.prototype.initialize = function () {
            var _this = this;
            document.addEventListener('deviceready', function () { _this.onDeviceReady(); }, false);
        };
        ApplicationStartup.prototype.onDeviceReady = function () {
            var _this = this;
            // Handle the Cordova pause and resume events
            document.addEventListener('pause', function () { _this.onPause(); }, false);
            document.addEventListener('resume', function () { _this.onResume(); }, false);
            // TODO: Cordova has been loaded. Perform any initialization that requires Cordova here.
            var pebble = cordova.require('cordova-pebble.Pebble');
            if (typeof pebble !== 'undefined' && pebble !== null) {
                this.pebbleServices.pebble = pebble;
                pebble.setAppUUID('29207e29-1f35-4f89-9871-0a579e84d105', function (info) {
                    navigator.notification.alert('watch app linked', function () { });
                }, function (error) {
                    navigator.notification.alert('watch app not linked', function () { });
                });
            }
        };
        ApplicationStartup.prototype.onPause = function () {
            // TODO: This application has been suspended. Save application state here.
        };
        ApplicationStartup.prototype.onResume = function () {
            // TODO: This application has been reactivated. Restore application state here.
        };
        return ApplicationStartup;
    })();
    ApplicationStartup.$inject = ['$rootScope', '$window', '$cookies', 'currentUser', 'pebbleServices'];
    angular
        .module('app')
        .run(function ($rootScope, $window, $cookies, currentUser, pebbleServices) {
        return new ApplicationStartup($rootScope, $window, $cookies, currentUser, pebbleServices);
    });
})(ZenMassageApp || (ZenMassageApp = {}));
//# sourceMappingURL=app-run.js.map