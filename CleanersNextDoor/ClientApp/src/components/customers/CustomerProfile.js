import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import { Authentication } from '../../services/authentication'

export class CustomerProfile extends Component {
    constructor(props) {
        super(props)
        this.state = {
            customer: null,
            loading: true
        }
    }

    componentDidMount() {
        this.populateProfileInformation()
    }

    async populateProfileInformation() {
        const claimId = await Authentication.getClaimId();
        const response = await fetch(`customers/${claimId}`);
        const data = await response.json();
        this.setState({ customer: data, loading: false });
    }

    static renderProfile(customer) {
        return (
            <div>
                <div className="row">
                    <div className="col">
                        <label className="d-block font-weight-bold">
                            Customer ID
                        </label>
                        {customer.id}
                    </div>
                    <div className="col">
                        <label className="d-block font-weight-bold">
                            Name
                        </label>
                        {customer.firstName + ' ' + customer.lastName}
                    </div>
                    <div className="col">
                        <label className="d-block font-weight-bold">
                            Email
                        </label>
                        {customer.email}
                    </div>
                    <div className="col">
                        <label className="d-block font-weight-bold">
                            Phone
                        </label>
                        {customer.phone}
                    </div>
                </div>
            </div>
        )
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : CustomerProfile.renderProfile(this.state.customer);
        return (
            <div>
                <header className="bg-primary py-3 mb-5">
                    <div className="container h-100">
                        <div className="row h-100 align-items-center">
                            <div className="col-lg-12">
                                <h1 className="display-4 text-white mt-5 mb-2">
                                    My Account
                                </h1>
                                <p className="lead text-white-50">
                                    Manage account information, recurring services, payment methods and more!
                                </p>
                            </div>
                        </div>
                    </div>
                </header>
                <div className="container">
                    <div>
                        <h3 className="border-bottom mb-3">
                            Account Information
                        </h3>
                        <div className="mb-3">
                            {contents}
                        </div>
                    </div>

                    <div>
                        <h3 className="border-bottom mb-3">
                            Settings
                        </h3>
                        <div className="row">
                            <div className="col-md-4 mb-3">
                                <Link className="text-decoration-none" to="/customers/edit-profile">
                                    <div className="card h-100 shadow">
                                        <div className="card-body">
                                            <div className="d-flex w-100 justify-content-between text-dark">
                                                <h5 className="mb-1">Account Information</h5>
                                            </div>
                                            <p className="mb-1 text-muted">Edit account information.</p>
                                        </div>
                                    </div>
                                </Link>
                            </div>
                            <div className="col-md-4 mb-3">
                                <Link className="text-decoration-none" to="/customers/payment-methods">
                                    <div className="card h-100 shadow">
                                        <div className="card-body">
                                            <div className="d-flex w-100 justify-content-between text-dark">
                                                <h5 className="mb-1">Payment Methods</h5>
                                            </div>
                                            <p className="mb-1 text-muted">Edit stored payment information.</p>
                                        </div>
                                    </div>
                                </Link>
                            </div>
                            <div className="col-md-4 mb-3">
                                <Link className="text-decoration-none" to="/customers/orders">
                                    <div className="card h-100 shadow">
                                        <div className="card-body">
                                            <div className="d-flex w-100 justify-content-between text-dark">
                                                <h5 className="mb-1">Orders</h5>
                                            </div>
                                            <p className="mb-1 text-muted">View past orders.</p>
                                        </div>
                                    </div>
                                </Link>
                            </div>
                            <div className="col-md-4 mb-3">
                                <Link className="text-decoration-none" to="/customers/orders">
                                    <div className="card h-100 shadow">
                                        <div className="card-body">
                                            <div className="d-flex w-100 justify-content-between text-dark">
                                                <h5 className="mb-1">Recurring Services</h5>
                                            </div>
                                            <p className="mb-1 text-muted">Schedule recurring services.</p>
                                        </div>
                                    </div>
                                </Link>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }

}