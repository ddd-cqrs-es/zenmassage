module ZenMassageApp {
    'use strict';

    interface IAppCookieService extends ng.cookies.ICookiesService {
        userId: string;
        pebbleUserId: string;
    }

    function startup(
        $rootScope: ng.IRootScopeService,
        $cookies: IAppCookieService,
        currentUser: ICurrentUser): void {

        // Setup global user state
        currentUser.userId = $cookies.userId;
        currentUser.pebbleUserId = $cookies.pebbleUserId;
        
        // Placeholder for detecting route change errors
        $rootScope.$on('$routeChangeError', (): void => { });
    }

    startup.$inject = ['$rootScope', '$cookies', 'currentUser'];
    angular
        .module('app')
        .run(startup);
}
