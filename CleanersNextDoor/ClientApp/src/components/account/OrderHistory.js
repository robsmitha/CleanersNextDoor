import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { customerService } from '../../services/customer.service';

export class OrderHistory extends Component {
    constructor(props) {
        super(props)
        this.state = {
            pastOrders: null,
            upcomingOrders: null,
            openOrders: null
        }
    }

    componentDidMount() {
        this.populateOrderHistory();
    }

    async populateOrderHistory() {
        let data = await customerService.getOrderHistory()
        this.setState({
            pastOrders: data !== null ? data.orders.filter(o => !o.isOpenOrder && !o.isUpcomingOrder) : [],
            openOrders: data !== null ? data.orders.filter(o => o.isOpenOrder) : [],
            upcomingOrders: data !== null ? data.orders.filter(o => o.isUpcomingOrder) : []
        })
    }

    render() {
        const { upcomingOrders, openOrders, pastOrders } = this.state
        return (
            <div>
                <header className="bg-primary py-3 mb-5">
                    <div className="container h-100">
                        <div className="row h-100 align-items-center">
                            <div className="col-lg-12">
                                <h1 className="display-4 text-white mt-5 mb-2">
                                    Order History
                                </h1>
                                <p className="lead text-white-50">
                                    You can always access your past orders on your account.
                                </p>
                                <Link to="/account" className="btn btn-success btn-lg">My Account</Link>
                            </div>
                        </div>
                    </div>
                </header>
                <div className="container">
                    <div className="my-3">
                        <h3>Orders</h3>
                        <div className="list-group list-group-flush">
                            <div className="list-group-item p-1 list-group-item-success">
                                <small className="font-weight-light text-muted">Upcoming</small>                                
                            </div>
                            {upcomingOrders == null
                                ? 'Loading Orders'
                                : upcomingOrders.map((o, index) =>
                                    <Link key={o.id}
                                        to={'/order-details/:id'.replace(':id', o.id)}
                                        className="list-group-item list-group-item-action p-1">
                                        <div className="d-flex w-100 justify-content-between">
                                            <h5 className="mb-1">{o.merchantName}</h5>
                                            <small>{o.displayOrderTotal}</small>
                                        </div>
                                        <div className="mb-1">
                                            
                                        </div>
                                        <small>{o.displayUpdated}</small>
                                    </Link>
                                )}
                            <div className="list-group-item p-1 list-group-item-primary">
                                <small className="font-weight-light text-muted">Open</small>
                            </div>
                            {openOrders == null
                                ? 'Loading Orders'
                                : openOrders.map((o, index) =>
                                    <Link key={o.id}
                                        to={'/request-service/:id'.replace(':id', o.merchantID)}
                                        className="list-group-item list-group-item-action p-1">
                                        <div className="d-flex w-100 justify-content-between">
                                            <h5 className="mb-1">{o.merchantName}</h5>
                                            <small>{o.displayOrderTotal}</small>
                                        </div>
                                        <div className="mb-1">

                                        </div>
                                        <small>{o.displayCreated}</small>
                                    </Link>
                                )}
                            <div className="list-group-item p-1 list-group-item-secondary">
                                <small className="font-weight-light text-muted">Past</small>
                            </div>
                            {pastOrders == null
                                ? 'Loading Orders'
                                : pastOrders.map((o, index) =>
                                    <Link key={o.id}
                                        to={'/order-details/:id'.replace(':id', o.id)}
                                        className="list-group-item list-group-item-action list-group-item-light p-1">
                                        <div className="d-flex w-100 justify-content-between">
                                            <h5 className="mb-1">{o.merchantName}</h5>
                                            <small>{o.displayOrderTotal}</small>
                                        </div>
                                        <div className="mb-1">

                                        </div>
                                        <small>{o.displayUpdated}</small>
                                    </Link>
                                )}
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}