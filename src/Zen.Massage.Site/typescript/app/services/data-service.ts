import {Http} from '../../../node_modules/angular2/http';
import {Observable} from '../../../node_modules/rxjs/observable';

module demoApp {

    export interface ICustomer {
        id: number;
        name: string;
        total: number;
    }

    export interface IOrder {
        product: string;
        total: number;
    }

    export interface IDataService {
        getCustomers(): Observable<ICustomer>;    
    }

    export class DataService {

        constructor(private http: Http) { }

        getCustomers(): Observable<ICustomer> {
            var result = new Observable<ICustomer>();

            var res = this.http.get('customers.json');
            var items = res.subscribe(
                (resp) => {
                    result.resp.json();
                },
                (resp) => {

                },
                () => {
                });
            ..then(response => {
                return response.data;
            });
        }

        getOrder(id: number): Observable<IOrder[]> {
            return this.http.get('orders.json', { data: { id: id } }).then(response => {
                return response.data;
            });
        }
    }

    angular.module('demoApp')
        .service('demoApp.dataService', DataService);

}