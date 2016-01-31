module ZenMassageApp {
    'use strict';

    export interface ITreatmentController {
        bookingReference: string;
        startTime: Date;
        durationInMinutes: number;
    }

    class TreatmentController implements ITreatmentController {
        bookingReference: string;
        startTime: Date;
        durationInMinutes: number;
        
    }

    angular.module('app').controller('treatment', TreatmentController);
}