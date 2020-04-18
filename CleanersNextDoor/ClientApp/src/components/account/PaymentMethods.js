import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { AuthConsumer } from './../../context/AuthContext'
import { Row, Col, Container, Badge, Card, CardBody, CardFooter } from 'reactstrap'


export class PaymentMethods extends Component {

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            paymentMethods: null
        }
    }

    componentDidMount() {
        this.populatePaymentMethods()
    }

    populatePaymentMethods() {
        fetch(`customers/getPaymentMethods`)
            .then(response => response.json())
            .then(data => {
                this.setState({
                    paymentMethods: data,
                    loading: false
                })
            })
    }

    removePaymentMethod = event => {
        if (window.confirm(`Are you sure you want to delete this payment method ${event.target.value}?`)) {

            const request = {
                method: 'post',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ id: Number(event.target.value) })
            }

            fetch(`customers/removePaymentMethod`, request)
                .then(reponse => reponse.json())
                .then(data => data ? this.populatePaymentMethods() : console.log(data))
        }
    }

    checkHandler = event => {

        const request = {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ id: Number(event.target.value) })
        }

        fetch(`customers/setDefaultPaymentMethod`, request)
            .then(reponse => reponse.json())
            .then(data => data ? this.populatePaymentMethods() : console.log(data))
    }

    render() {
        return (
            <div>
                <AuthConsumer>
                    {({ authenticated }) => (
                        <div>
                            {!authenticated
                                ? <Redirect to='/' />
                                : this.renderLayout()}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        )
    }

    renderLayout() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderPaymentMethods(this.state.paymentMethods);
        return (
            <div>
                <header className="bg-primary py-3 mb-5">
                    <Container className="h-100">
                        <Row className="h-100 align-items-center">
                            <Col>
                                <h1 className="display-4 text-white mt-5 mb-2">
                                    Payment Method
                                </h1>
                                <p className="lead text-white-50">
                                    Set up your default payment methods for a speedy checkout process.
                                </p>
                                <Link to="/new-payment-method" className="btn btn-success btn-lg mr-2">
                                    New Payment
                                </Link>
                                <Link to="/account" className="btn btn-secondary btn-lg">
                                    My Account
                                </Link>
                            </Col>
                        </Row>
                    </Container>
                </header>
                {contents}
            </div>
        )
    }

    renderPaymentMethods(paymentMethods) {
        return (
            <div>
                <Container>

                    <h3>
                        My Payment Methods
                    </h3>
                    <p>
                        Payment methods are stored using secure payment services to tokenize and protect card information.
                    </p>

                    <Row hidden={paymentMethods.length === 0}>
                        {paymentMethods.map(pm =>
                            <Col md="4" key={pm.id} className="mb-4">
                                <Card className={pm.isDefault ? 'h-100 border-primary' : 'h-100'}>
                                    <CardBody className="pb-0 border-bottom-0">
                                        <Row>
                                            <Col>
                                                <p className="mb-1 font-weight-bold">
                                                    {pm.cardTypeName} - {pm.last4}
                                                </p>
                                                <small className="d-block text-muted">{new Date(pm.expirationDate).toLocaleDateString()}</small>
                                                <Badge hidden={pm.nameOnCard === null  || pm.nameOnCard.length === 0} color="light" className="border" pill>{pm.nameOnCard !== null ? pm.nameOnCard.toUpperCase() : ''}</Badge>
                                            </Col>
                                            <Col xs="auto">
                                                <div className="custom-control custom-radio">
                                                    <input type="radio" id={'isDefault' + pm.id} name="isDefault" className="custom-control-input" value={pm.id} id={'isDefault' + pm.id} checked={pm.isDefault} onChange={this.checkHandler} />
                                                    <label className={'custom-control-label ' + (pm.isDefault ? 'font-weight-bold' : '')} htmlFor={'isDefault' + pm.id}>DEFAULT</label>
                                                </div>
                                            </Col>
                                        </Row>
                                    </CardBody>
                                    <CardFooter className="bg-white pt-0 border-top-0">
                                        <button type="button" className="btn btn-link btn-sm text-danger pl-0" value={pm.id} onClick={this.removePaymentMethod}>
                                            REMOVE
                                        </button>
                                    </CardFooter>
                                </Card>
                            </Col>
                        )}
                    </Row>
                    <div hidden={paymentMethods.length > 0} className="mb-4">
                        <p className="lead mb-1">
                            You have no saved payment methods.
                        </p>
                        <small className="text-muted">
                            <Link to='/new-payment-method' className="text-decoration-none">
                                Add a new payment method&nbsp;
                            </Link>
                            to speed up the checkout process.
                        </small>
                    </div>
                </Container>
            </div>
        )
    }
}