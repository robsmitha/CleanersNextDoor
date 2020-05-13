import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { Container, Row, Col, FormGroup, Card, CardHeader, CardBody } from 'reactstrap';
import { FaLock, FaCreditCard } from 'react-icons/fa';
import { AuthConsumer } from './../../context/AuthContext'
import TextInput from './../../helpers/TextInput';
import handleChange from './../../helpers/HandleChange';
import { customerService } from '../../services/customer.service'

import { loadStripe } from '@stripe/stripe-js';
import { Elements } from '@stripe/react-stripe-js';
import PaymentMethodForm from './../../helpers/PaymentMethodForm';


export class NewPaymentMethod extends Component {

    constructor(props) {
        super(props);
        this.state = {
            formIsValid: false,
            isDefault: false,
            formControls: {
                nameOnCard: {
                    value: '',
                    placeholder: 'Name on card',
                    label: 'Name on card',
                    valid: true
                }
            }
        };

        this.stripePromise = customerService.stripePublicKey()
            .then(data => loadStripe(data.key)) 
    }


    changeHandler = event => {
        const name = event.target.name;
        const value = event.target.value;
        this.setState(handleChange(name, value, this.state.formControls));
    }

    checkHandler = event => {
        if (event.target instanceof HTMLInputElement && event.target.getAttribute('type') == 'checkbox') {
            const name = event.target.name;
            const value = event.target.checked;
            this.setState({
                [name]: value
            });
        }
    }

    stripePaymentMethodHandler = (payment_method) => {
        this.setState({
            formIsValid: false
        });

        const {
            nameOnCard
        } = this.state.formControls;

        let data = {
            nameOnCard: nameOnCard.value,
            isDefault: this.state.isDefault,
            stripePaymentMethodID: payment_method
        }
        customerService.addPaymentMethod(data)
            .then(data => {
                if (data !== null) {
                    if (data.id > 0) {
                        this.props.history.push(`/payment-methods`)
                    } else {
                        alert(data)
                    }
                }
                else {
                    //request failed
                }

                this.setState({
                    formIsValid: true
                });
            })
    }

    render() {
        return (
            <AuthConsumer>
                {({ authenticated }) => (
                    <div>
                        {!authenticated
                            ? <Redirect to='/sign-in' />
                            : NewPaymentMethod.renderCreatePaymentMethodForm(
                                this.state,
                                this.changeHandler,
                                this.checkHandler,
                                this.stripePromise,
                                this.stripePaymentMethodHandler
                            )}
                    </div>
                )}
            </AuthConsumer>
        )
    }
    static renderCreatePaymentMethodForm(state, changeHandler, checkHandler, stripePromise, stripePaymentMethodHandler) {
        const { formControls, isDefault, formIsValid } = state

        return (
            <Container className="mt-3 mb-5">
                <Link to={'/'}>Home</Link>&nbsp;&minus;&nbsp;
                <Link to={'/account'}>Account</Link>&nbsp;&minus;&nbsp;
                <Link to={'/payment-methods'}>Payment methods</Link>&nbsp;&minus;&nbsp;New payment method
                <div className="my-md-5 my-4">
                    <h1 className="h3">
                        New Payment Method
                    </h1>
                    <p className="text-muted">
                        By completing this form, you authorize CleanersNextDoor to send instructions to the financial institution that issued your card
                        to take payments from your card account in accordance with the terms of my agreement with CleanersNextDoor.
                    </p>
                </div>

                <Row>
                    <Col sm="9" md="7" lg="5">

                        <TextInput name="nameOnCard"
                            placeholder={formControls.nameOnCard.placeholder}
                            label={formControls.nameOnCard.label}
                            value={formControls.nameOnCard.value}
                            valid={formControls.nameOnCard.valid ? 1 : 0}
                            onChange={changeHandler} />


                        <div className="form-group">
                            <div className="custom-control custom-checkbox">
                                <input type="checkbox" className="custom-control-input" id="isDefault" name="isDefault" onChange={checkHandler} checked={isDefault} />
                                <label className="custom-control-label" htmlFor="isDefault">
                                    Make this my default payment method
                                    </label>
                            </div>
                        </div>

                        <Elements stripe={stripePromise}>
                            <PaymentMethodForm
                                nameOnCard={formControls.nameOnCard.value}
                                stripePaymentMethodHandler={stripePaymentMethodHandler}
                                disabled={!formIsValid} />
                        </Elements>

                        <small className="text-muted d-block mt-3 mb-4">
                            Your data is securely tokenized by leading payment services.&nbsp;<FaLock />
                        </small>
                    </Col>

                    <Col sm="5" md="4" lg="4" className="ml-lg-auto">
                        <Card className="h-100 border-0 shadow">
                            <CardHeader className="bg-primary text-white border-bottom-0">
                                <Row>
                                    <Col><h5>Why do you need this info?</h5></Col>
                                    <Col xs="auto"><FaCreditCard /></Col>
                                </Row>
                            </CardHeader>
                            <CardBody>
                                <p>
                                    Saving default payment methods is a completely optional feature of our system.
                                    
                                    </p>
                                <p className="font-weight-bold">
                                    Your data is never shared with third parties.
                                    </p>
                                <p>
                                    The goal with default payment methods is to make checkout as few fields as possible.
                                    </p>
                                <p>
                                    Another great benefit of saving your default payment method is to make it easier to setup automatic subscriptions.
                                    </p>
                            </CardBody>
                        </Card>
                    </Col>
                </Row>
            </Container>
        )
    }
}