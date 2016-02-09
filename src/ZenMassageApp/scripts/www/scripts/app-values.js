var ZenMassageApp;
(function (ZenMassageApp) {
    'use strict';
    var currentUser = {};
    var pebbleServices = {};
    angular.module('app')
        .value('currentUser', currentUser)
        .value('pebbleServices', pebbleServices);
})(ZenMassageApp || (ZenMassageApp = {}));
//# sourceMappingURL=app-values.js.map