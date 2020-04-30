import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export class OrderDetails extends Component {
    constructor(props) {
        super(props)
        this.state = {
            id: this.props.match.params.id
        }
    }

    componentDidMount() {

    }

    render() {
        return (
            <div>
                <header className="bg-primary py-3 mb-5">
                    <div className="container h-100">
                        <div className="row h-100 align-items-center">
                            <div className="col-lg-12">
                                <h1 className="display-4 text-white mt-5 mb-2">
                                    Order {this.state.id} Confirmed
                                </h1>
                                <p className="lead text-white-50">
                                    You can always access your past orders on your account.
                                </p>
                                <Link to="/order-history" className="btn btn-success btn-lg">See Order History</Link>
                            </div>
                        </div>
                    </div>
                </header>
                <div className="container">
                    <div className="my-3">
                        <h3>Order Details</h3>

                    </div>
                </div>
            </div>
        )
    }
}