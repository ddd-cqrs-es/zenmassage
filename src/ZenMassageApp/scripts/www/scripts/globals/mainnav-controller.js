var ZenMassageApp;
(function (ZenMassageApp) {
    var MainNavController = (function () {
        function MainNavController() {
            // TODO: Get this value from configuration service
            var activeTabIndex = 1;
            switch (activeTabIndex) {
                case 1:
                    this.isSessionTabActive = true;
                    break;
                case 2:
                    this.isSettingTabActive = true;
                    break;
                default:
                    this.isBookingTabActive = true;
                    break;
            }
        }
        MainNavController.$inject = [''];
        return MainNavController;
    }());
    angular.module('app')
        .controller('mainnav', MainNavController);
})(ZenMassageApp || (ZenMassageApp = {}));
//# sourceMappingURL=mainnav-controller.js.map