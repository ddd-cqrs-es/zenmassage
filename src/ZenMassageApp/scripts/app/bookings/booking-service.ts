module ZenMassageApp {
    'use strict';

    export interface IBookingService {
        
    }

    class BookingService implements IBookingService {
        constructor(private $resource: ng.resource.IResourceService) {
            
        }

    }
}