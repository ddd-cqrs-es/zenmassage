var ZenMassageApp;
(function (ZenMassageApp) {
    'use strict';
    var SessionItem = (function () {
        function SessionItem($name, $duration) {
            this.$name = $name;
            this.$duration = $duration;
        }
        Object.defineProperty(SessionItem.prototype, "name", {
            get: function () {
                return this.$name;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(SessionItem.prototype, "duration", {
            get: function () {
                return this.$duration;
            },
            enumerable: true,
            configurable: true
        });
        return SessionItem;
    })();
})(ZenMassageApp || (ZenMassageApp = {}));

//# sourceMappingURL=SessionItem.js.map
