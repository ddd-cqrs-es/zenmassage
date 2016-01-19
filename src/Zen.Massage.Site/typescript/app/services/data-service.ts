import {Http, RequestOptionsArgs} from 'angular2/http';
import {Observable} from 'rxjs/rx';

module ZenMassage {

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
        getCustomers(): Observable<ICustomer[]>;
        getOrders(id: number): Observable<IOrder[]>;
    }

    class Customer implements ICustomer {
        constructor(private _id: number, private _name: string, private _total: number) {
        }

        get id(): number {
            return this._id;
        }

        get name(): string {
            return this._name;
        }

        get total(): number {
            return this._total;
        }
    }

    class Order implements IOrder {
        constructor(private _product: string, private _total: number) {
        }

        get product(): string {
            return this._product;
        }

        get total(): number {
            return this._total;
        }
    }

    export class DataService implements IDataService {

        constructor(private http: Http) {
        }

        getCustomers(): Observable<ICustomer[]> {
            return this.http
                .get('customers.json')
                .map(response => {
                    return response.json();
                })
                .map((customers: Array<any>) => {
                    let results: Array<ICustomer> = [];
                    if (customers != null) {
                        customers.forEach(
                            (customer) => {
                                results.push(
                                    new Customer(
                                        customer.id,
                                        customer.name,
                                        customer.total));
                            });
                    }
                    return results;
                });
        }

        getOrders(id: number): Observable<IOrder[]> {
            return this.http
                .get('orders.json', { body: JSON.stringify({ id: id }) })
                .map(response => {
                    return response.json();
                })
                .map((orders: Array<any>) => {
                    let results: Array<IOrder> = [];
                    if (orders != null) {
                        orders.forEach(
                            (order) => {
                                results.push(
                                    new Order(
                                        order.product,
                                        order.total));
                            });
                    }
                    return results;
                });
        }
    }

//    angular.module('demoApp')
//        .service('demoApp.dataService', DataService);

}