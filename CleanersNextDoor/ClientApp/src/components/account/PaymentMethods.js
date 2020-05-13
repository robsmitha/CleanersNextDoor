import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { AuthConsumer } from './../../context/AuthContext'
import { Row, Col, Container, Badge, Card, CardBody } from 'reactstrap'
import { customerService } from '../../services/customer.service'
import Loading from '../../helpers/Loading';
import { FaPlusCircle, FaTimesCircle } from 'react-icons/fa';


export class PaymentMethods extends Component {

    constructor(props) {
        super(props);
        this.state = {
            paymentMethods: null
        }
    }

    componentDidMount() {
        this.populatePaymentMethods()
    }

    async populatePaymentMethods() {
        const data = await customerService.getPaymentMethods()
        if (data) {
            this.setState({
                paymentMethods: data
            })
        }
    }

    removePaymentMethod = event => {
        if (window.confirm(`Are you sure you want to delete this payment method ${event.target.value}?`)) {
            customerService.removePaymentMethod({ id: Number(event.target.value) })
                .then(data => data ? this.populatePaymentMethods() : console.log(data))
        }
    }

    checkHandler = event => {
        customerService.setDefaultPaymentMethod({ id: Number(event.target.value) })
            .then(data => data ? this.populatePaymentMethods() : console.log(data))
    }

    render() {
        const { paymentMethods } = this.state
        return (
            <AuthConsumer>
                {({ authenticated }) => (
                    <div>
                        {!authenticated
                            ? <Redirect to='/' />
                            : PaymentMethods.renderPaymentMethods(paymentMethods, this.checkHandler, this.removePaymentMethod)}
                    </div>
                )}
            </AuthConsumer>
            )
    }

    static renderPaymentMethods(paymentMethods, checkHandler, removePaymentMethod) {
        return (
            <Container className="mt-3 mb-5">
                <Link to={'/'}>Home</Link>&nbsp;&minus;&nbsp;
                <Link to={'/account'}>Account</Link>&nbsp;&minus;&nbsp;Your payment methods
                <div className="my-md-5 my-4">
                    <h1 className="h3">
                        Payment Methods
                            </h1>
                    <p className="text-muted">
                        Payment methods are stored using secure payment services to tokenize and protect card information.
                    </p>
                </div>
                {paymentMethods === null
                    ? <Loading message="Loading payment methods, please wait" />
                    : <div>
                        <Row>
                            {paymentMethods.map(pm =>
                                <Col md="4" key={pm.id} className="mb-4">
                                    <Card className="h-100">
                                        <CardBody>
                                            <div className="custom-control custom-radio">
                                                <input type="radio"
                                                    id={'isDefault' + pm.id}
                                                    name="isDefault"
                                                    className="custom-control-input"
                                                    value={pm.id}
                                                    id={'isDefault' + pm.id}
                                                    checked={pm.isDefault}
                                                    onChange={checkHandler}
                                                />
                                                <label className={'custom-control-label'} htmlFor={'isDefault' + pm.id}>
                                                    <span className="sr-only">Default payment method</span>
                                                    <h5 className={'mb-0 '.concat(pm.isDefault ? 'text-primary' : '')}>
                                                        {pm.cardBrand} - {pm.last4}
                                                    </h5>
                                                    <small className="d-block text-muted">{pm.expMonth}/{pm.expYear}</small>
                                                </label>
                                            </div>
                                            <div className="my-2" hidden={true}>
                                                <Badge hidden={pm.nameOnCard === null || pm.nameOnCard.length === 0} color="light" className="border" pill>{pm.nameOnCard !== null ? pm.nameOnCard.toUpperCase() : ''}</Badge>
                                            </div>
                                            <div className="mt-2">
                                                <button type="button" className="btn btn-link btn-sm pl-0 text-danger" value={pm.id} onClick={removePaymentMethod}>
                                                    <FaTimesCircle /> REMOVE
                                                </button>
                                            </div>
                                        </CardBody>
                                    </Card>
                                </Col>
                            )}

                            <Col md="4" className="mb-4">
                                <Link to='/new-payment-method' className="text-decoration-none">
                                    <Card className="h-100">
                                        <CardBody>
                                            <span className="sr-only" hidden={paymentMethods.length > 0}>
                                                You have no saved payment methods.
                                            </span>
                                            <h5 className={'mb-1 '}>
                                                <FaPlusCircle /> New payment method
                                            </h5>
                                            <small className={'d-block '.concat(paymentMethods.length === 0 ? 'text-danger' : 'text-muted')}>Add a new payment method</small>
                                        </CardBody>
                                    </Card>
                                </Link>
                            </Col>

                        </Row>
                    </div>
                }
            </Container>
        )
    }
}