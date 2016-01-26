module ZenMassageApp {
    'use strict';

    export interface ICurrentUser {
        userId?: string;
        pebbleUserId?: string;
    }

    var currentUser: ICurrentUser = {
    };

    angular.module('app').value('currentUser', currentUser);
}