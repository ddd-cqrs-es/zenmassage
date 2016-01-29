module ZenMassageApp {
    'use strict';

    interface IAppCookieService extends ng.cookies.ICookiesService {
        userId: string;
        pebbleUserId: string;
    }

    class ApplicationStartup {

        constructor(
            private $rootScope: ng.IRootScopeService,
            private $window: ng.IWindowService,
            private $cookies: IAppCookieService,
            private currentUser: ICurrentUser,
            private pebbleServices: IPebbleServices) {

            $window.onload = (): void => {
                this.initialize();
            };

            // Setup global user state
            currentUser.userId = $cookies.userId;
            currentUser.pebbleUserId = $cookies.pebbleUserId;
        
            // Placeholder for detecting route change errors
            $rootScope.$on('$routeChangeError', (): void => { });
        }

        initialize(): void {
            document.addEventListener('deviceready', (): void=> { this.onDeviceReady(); }, false);
        }

        onDeviceReady(): void {
            // Handle the Cordova pause and resume events
            document.addEventListener('pause', (): void=> { this.onPause(); }, false);
            document.addEventListener('resume', (): void => { this.onResume(); }, false);

            // TODO: Cordova has been loaded. Perform any initialization that requires Cordova here.
            var pebble: IPebble = cordova.require('cordova-pebble.Pebble');
            if (typeof pebble !== 'undefined' && pebble !== null) {

                this.pebbleServices.pebble = pebble;

                pebble.setAppUUID(
                    '29207e29-1f35-4f89-9871-0a579e84d105',
                    (info): void => {
                        navigator.notification.alert(
                            'watch app linked',
                            (): void => { });
                    },
                    (error): void => {
                        navigator.notification.alert(
                            'watch app not linked',
                            (): void => { });
                    });
            }
        }

        onPause(): void {
            // TODO: This application has been suspended. Save application state here.
        }

        onResume(): void {
            // TODO: This application has been reactivated. Restore application state here.
        }

    }


    ApplicationStartup.$inject = ['$rootScope', '$window', '$cookies', 'currentUser', 'pebbleServices'];
    angular
        .module('app')
        .run((
            $rootScope: ng.IRootScopeService,
            $window: ng.IWindowService,
            $cookies: IAppCookieService,
            currentUser: ICurrentUser,
            pebbleServices: IPebbleServices) => {

            return new ApplicationStartup($rootScope, $window, $cookies, currentUser, pebbleServices);
        });
}
