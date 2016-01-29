module ZenMassageApp {
    
    export interface IMainNavController {
        isBookingTabActive: boolean;
        isSessionTabActive: boolean;
        isSettingTabActive: boolean;
    }

    class MainNavController implements IMainNavController {
        static $inject = [''];

        isBookingTabActive: boolean;
        isSessionTabActive: boolean;
        isSettingTabActive: boolean;

        constructor() {
            // TODO: Get this value from configuration service
            var activeTabIndex = 1;

            switch(activeTabIndex) {
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
    }

    angular.module('app')
        .controller('mainnav', MainNavController);
}