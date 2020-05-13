import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { Col, Card, CardBody, Container, Row } from 'reactstrap'
import { AuthConsumer } from './../../context/AuthContext'
import { customerService } from '../../services/customer.service'
import { FaAddressCard, FaHistory, FaMapMarker, FaCreditCard } from 'react-icons/fa';

export class MyAccount extends Component {
    constructor(props) {
        super(props)
        this.state = {
            loading: true,
            customer: null
        }
    }

    componentDidMount() {
        this.populateCustomer()
    }

    async populateCustomer() {
        const data = await customerService.getCustomer()
        if (data.id > 0) {
            this.setState({
                customer: data,
                loading: false
            })
        }
    }

    render() {
        const { customer } = this.state
        return (
            <AuthConsumer>
                {({ authenticated }) => (
                    <div>
                        {!authenticated
                            ? <Redirect to='/sign-up' />
                            : MyAccount.renderAccount(customer)}
                    </div>
                )}
            </AuthConsumer>
            )
    }

    static renderAccount(customer) {
        return (
            <Container className="mt-3 mb-5">

                <Link to={'/'}>Home</Link> &minus; Your account

                <div className="my-md-5 my-4">
                    <h1 className="h3">
                        Your account
                    </h1>
                    <p className="text-muted">Manage your account and settings here.</p>
                </div>

                <Row>

                    <Col md="4" className="mb-4">
                        <Link className="text-decoration-none" to={'order-history'}>
                            <Card className="h-100">
                                <CardBody>
                                    <h5 className="card-title mb-3">
                                        <FaHistory className="text-primary" /> Orders
                                    </h5>
                                    <p className="text-muted card-text text-sm">
                                        Track progress of upcomming service requests and review past order history.
                                    </p>
                                </CardBody>
                            </Card>
                        </Link>
                    </Col>

                    <Col md="4" className="mb-4">
                        <Link className="text-decoration-none" to={'saved-addresses'}>
                            <Card className="h-100">
                                <CardBody>
                                    <h5 className="card-title mb-3">
                                        <FaAddressCard className="text-primary" /> Saved Addresses
                                    </h5>
                                    <p className="text-muted card-text text-sm">
                                        {customer !== null && !customer.hasAddresses
                                            ? <span className="text-danger">Set up your default saved addresses to skip at checkout.</span>
                                            : 'Keep your default saved addresses up to date to skip address steps at checkout.'}
                                    </p>
                                </CardBody>
                            </Card>
                        </Link>
                    </Col>

                    <Col md="4" className="mb-4">
                        <Link className="text-decoration-none" to={'payment-methods'}>
                            <Card className="h-100">
                                <CardBody>
                                    <h5 className="card-title mb-3">
                                        <FaCreditCard className="text-primary" /> Payment Methods
                                    </h5>
                                    <p className="text-muted card-text text-sm">
                                        {customer !== null && !customer.hasPaymentMethods
                                            ? <span className="text-danger">Add a secure stored payment method to skip card information at checkout.</span>
                                            : 'Maintain stored payment methods to skip card information steps at checkout.'}
                                    </p>
                                </CardBody>
                            </Card>
                        </Link>
                    </Col>

                </Row>

            </Container>
            )
    }

    static renderCustomer(customer) {
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

    renderLayout() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : MyAccount.renderCustomer(this.state.customer);
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
                                <Link to="/order-history" className="btn btn-success btn-lg mr-2">
                                    Orders
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
                            <Link className="text-decoration-none" to="/edit-account">
                                edit your account.
                            </Link>
                        </small>
                    </div>
                </div>
            </div>
        )
    }
}