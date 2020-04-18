import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { Row, Col, Container, Card, CardBody } from 'reactstrap'
import { AuthConsumer } from './../context/AuthContext'
import { FaLocationArrow } from 'react-icons/fa'

export class Profile extends Component {
    constructor(props) {
        super(props)
        this.state = {
            loading: true,
            customer: null
        }
    }

    componentDidMount() {
        this.populateProfileInformation()
    }

    populateProfileInformation() {
        fetch(`customers/profile`)
            .then(response => response.json())
            .then(data => {
                if (data.id > 0) {
                    this.setState({
                        customer: data,
                        loading: false
                    })
                }
            })
    }

    render() {
        return (
            <div>
                <AuthConsumer>
                    {({ authenticated }) => (
                        <div>
                            {!authenticated
                                ? <Redirect to='/sign-up' />
                                : this.renderProfileLayout()}
                        </div>
                    )}
                </AuthConsumer>
            </div>
            )
    }


    static renderProfile(customer) {
        return (
            <div>
                <div className="row">
                    <div className="col">
                        <label className="d-block font-weight-bold">
                            Name
                        </label>
                        {customer.name}
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
                    <div className="col">
                        <label className="d-block font-weight-bold">
                            Customer ID
                        </label>
                        {customer.id}
                    </div>
                </div>
            </div>
        )
    }

    renderProfileLayout() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Profile.renderProfile(this.state.customer);
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
                                <Link to="/" className="btn btn-success btn-lg mr-2">
                                    Recent Orders
                                </Link>
                                <Link to="/" className="btn btn-secondary btn-lg">
                                    See Merchants
                                </Link>
                            </div>
                        </div>
                    </div>
                </header>
                <div className="container">
                    <div className="mb-3">
                        <h3 className="mb-3">
                            Account Information
                        </h3>
                        <div className="mb-1">
                            {contents}
                        </div>
                        <small className="text-muted">
                            Is this information correct? 
                            If not, you can&nbsp;
                            <Link className="text-decoration-none" to="/edit-profile">
                                edit your account.
                            </Link>
                        </small>
                    </div>

                    <div>
                        <h3 className="mb-3">
                            More Settings
                        </h3>
                        <div className="row">
                            <div className="col-md-4 mb-4">
                                <Link className="text-decoration-none" to="/edit-profile">
                                    <div className="card h-100">
                                        <div className="card-body p-4">
                                            <div className="d-flex w-100 justify-content-between text-dark">
                                                <h5 className="mb-1">Edit Account</h5>
                                            </div>
                                            <p className="mb-1 text-muted">Need to change your account? Edit your account here.</p>
                                        </div>
                                    </div>
                                </Link>
                            </div>
                            <div className="col-md-4 mb-4">
                                <Link className="text-decoration-none" to="/saved-addresses">
                                    <div className={this.state.customer !== null && !this.state.customer.hasAddresses ? 'card h-100 border-danger' : 'card h-100'}>
                                        <div className="card-body p-4">
                                            <div className="d-flex w-100 justify-content-between text-dark">
                                                <h5 className="mb-1">Saved Addresses</h5>
                                            </div>
                                            <p className="mb-1 text-muted">
                                                {this.state.customer !== null && !this.state.customer.hasAddresses
                                                    ? <span className="text-danger">Set up your default saved addresses to skip at checkout.</span>
                                                    : 'Keep your default saved addresses up to date to ensure flawless services.'}
                                            </p>
                                        </div>
                                    </div>
                                </Link>
                            </div>
                            <div className="col-md-4 mb-4">
                                <Link className="text-decoration-none" to="/payment-methods">
                                    <div className={this.state.customer !== null && !this.state.customer.hasAddresses ? 'card h-100 border-danger' : 'card h-100'}>
                                        <div className="card-body p-4">
                                            <div className="d-flex w-100 justify-content-between text-dark">
                                                <h5 className="mb-1">Payment Methods</h5>
                                            </div>
                                            <p className="mb-1 text-muted">
                                                {this.state.customer !== null && !this.state.customer.hasPaymentMethods
                                                    ? <span className="text-danger">Add a secure stored payment method to skip card information at checkout.</span>
                                                    : 'Keep your default stored payment methods up to date to ensure flawless services.'}
                                            </p>
                                        </div>
                                    </div>
                                </Link>
                            </div>
                            <div className="col-md-4 mb-4">
                                <Link className="text-decoration-none" to="/orders">
                                    <div className="card h-100">
                                        <div className="card-bod p-4">
                                            <div className="d-flex w-100 justify-content-between text-dark">
                                                <h5 className="mb-1">Orders</h5>
                                            </div>
                                            <p className="mb-1 text-muted">Track progress of upcomming service requests and review past order history.</p>
                                        </div>
                                    </div>
                                </Link>
                            </div>
                            <div className="col-md-4 mb-4">
                                <Link className="text-decoration-none" to="/recurring-services">
                                    <div className="card h-100">
                                        <div className="card-body p-4">
                                            <div className="d-flex w-100 justify-content-between text-dark">
                                                <h5 className="mb-1">Recurring Services</h5>
                                            </div>
                                            <p className="mb-1 text-muted">Schedule recurring services for a automated service requests on the dates you choose.</p>
                                        </div>
                                    </div>
                                </Link>
                            </div>
                            <Col md="4" className="mb-4">
                                <Link className="text-decoration-none" to="/help">
                                    <Card className="h-100" to="/help">
                                        <CardBody className="p-4">
                                            <div className="d-flex w-100 justify-content-between text-dark">
                                                <h5 className="mb-1">Help</h5>
                                            </div>
                                            <p className="mb-1 text-muted">How can we help? Send a message to our support team.</p>
                                        </CardBody>
                                    </Card>
                                </Link>
                            </Col>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}