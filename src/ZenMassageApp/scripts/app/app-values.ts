module ZenMassageApp {
    'use strict';

    export interface ICurrentUser {
        userId?: string;
        pebbleUserId?: string;
    }

    export interface IPebbleServices {
        pebble?: IPebble
    }

    var currentUser: ICurrentUser = {
    };

    var pebbleServices: IPebbleServices = {
    };

    angular.module('app')
        .value('currentUser', currentUser)
        .value('pebbleServices', pebbleServices);
}