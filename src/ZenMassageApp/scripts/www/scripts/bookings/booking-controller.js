var ZenMassageApp;
(function (ZenMassageApp) {
    'use strict';
    var BookingStatus;
    (function (BookingStatus) {
        BookingStatus[BookingStatus["Provisional"] = 0] = "Provisional";
        BookingStatus[BookingStatus["Tender"] = 1] = "Tender";
        BookingStatus[BookingStatus["BidByTherapist"] = 2] = "BidByTherapist";
        BookingStatus[BookingStatus["AcceptByClient"] = 3] = "AcceptByClient";
        BookingStatus[BookingStatus["Confirmed"] = 4] = "Confirmed";
        BookingStatus[BookingStatus["CancelledByTherapist"] = 5] = "CancelledByTherapist";
        BookingStatus[BookingStatus["CancelledByClient"] = 6] = "CancelledByClient";
        BookingStatus[BookingStatus["Completed"] = 7] = "Completed";
    })(BookingStatus || (BookingStatus = {}));
    var Gender;
    (function (Gender) {
        Gender[Gender["Male"] = 0] = "Male";
        Gender[Gender["Female"] = 1] = "Female";
    })(Gender || (Gender = {}));
    var BookingController = (function () {
        function BookingController($q, bookingService) {
            this.$q = $q;
            this.bookingService = bookingService;
        }
        BookingController.prototype.bid = function (bookingReference) {
            return null;
        };
        BookingController.prototype.cancel = function (bookingReference) {
            return null;
        };
        return BookingController;
    }());
    var BookingItem = (function () {
        function BookingItem() {
        }
        return BookingItem;
    }());
})(ZenMassageApp || (ZenMassageApp = {}));
//# sourceMappingURL=booking-controller.js.map