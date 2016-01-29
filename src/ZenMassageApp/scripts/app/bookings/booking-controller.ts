module ZenMassageApp {
    'use strict';

    export interface IBookingController {
        bookings: IBookingItem[];

        bid(bookingReference: string): ng.IPromise<string>;
        cancel(bookingReference: string): ng.IPromise<string>;
    }

    export interface IBookingItem {
        bookingReference: string;
        clientReference: string;
        therapistReference: string;
        createdDate: Date;
        status: BookingStatus;
        customerName: string;
        gender: Gender;
        startTime?: Date;
        durationInMinutes: number;
        treatmentType: number;
        treatmentLocation: string;
    }

    enum BookingStatus {
        Provisional = 0,
        Tender = 1,
        BidByTherapist = 2,
        AcceptByClient = 3,
        Confirmed = 4,
        CancelledByTherapist = 5,
        CancelledByClient = 6,
        Completed = 7
    }

    enum Gender {
        Male = 0,
        Female = 1
    }

    class BookingController implements IBookingController {
        bookings: IBookingItem[];

        bid(bookingReference: string): ng.IPromise<string> {
            return null;
        }

        cancel(bookingReference: string): ng.IPromise<string> {
            return null;
        }

        constructor(
            private $q: ng.IQService,
            private bookingService: IBookingService) {
        }

    }

    class BookingItem implements IBookingItem {
        bookingReference: string;
        clientReference: string;
        therapistReference: string;
        createdDate: Date;
        status: number;
        customerName: string;
        gender: Gender;
        startTime: Date;
        durationInMinutes: number;
        treatmentType: number;
        treatmentLocation: string;
    }
}