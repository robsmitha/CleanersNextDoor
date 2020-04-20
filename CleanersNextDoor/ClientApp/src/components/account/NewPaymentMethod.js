import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { Container, Row, Col } from 'reactstrap';
import { FaLock } from 'react-icons/fa';
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
            <div>
                <AuthConsumer>
                    {({ authenticated }) => (
                        <div>
                            {!authenticated
                                ? <Redirect to='/sign-in' />
                                : this.renderContent()}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        )
    }
    renderContent() {
        return (
            <div>
                <header className="bg-primary py-3 mb-5 shadow">
                    <div className="container h-100">
                        <div className="row h-100 align-items-center">
                            <div className="col-lg-12">
                                <h1 className="display-4 text-white mt-5 mb-2">
                                    New Payment Method
                                </h1>
                                <p className="lead text-white-50">
                                    Create a new payment method by securely entering your card information below.
                                </p>
                                <Link to="/account" className="btn btn-success btn-lg mr-2">My Account</Link>
                                <Link to="/payment-methods" className="btn btn-secondary btn-lg">Payment Methods</Link>
                            </div>
                        </div>
                    </div>
                </header>
                <Container>

                    <h3>
                        Payment method details
                     </h3>
                    <p className="mb-1">
                        By completing this form, you authorize CleanersNextDoor to send instructions to the financial institution that issued your card
                        to take payments from your card account in accordance with the terms of my agreement with CleanersNextDoor.
                    </p>

                    <Row>
                        <Col sm="9" md="7" lg="5">

                            <TextInput name="nameOnCard"
                                placeholder={this.state.formControls.nameOnCard.placeholder}
                                label={this.state.formControls.nameOnCard.label}
                                value={this.state.formControls.nameOnCard.value}
                                valid={this.state.formControls.nameOnCard.valid ? 1 : 0}
                                onChange={this.changeHandler} />


                            <div className="form-group">
                                <div className="custom-control custom-checkbox">
                                    <input type="checkbox" className="custom-control-input" id="isDefault" name="isDefault" onChange={this.checkHandler} checked={this.state.isDefault} />
                                    <label className="custom-control-label" htmlFor="isDefault">
                                        Make this my default payment method
                                    </label>
                                </div>
                            </div>

                            <Elements stripe={this.stripePromise}>
                                <PaymentMethodForm
                                    nameOnCard={this.state.formControls.nameOnCard.value}
                                    stripePaymentMethodHandler={this.stripePaymentMethodHandler}
                                    disabled={!this.state.formIsValid} />
                            </Elements>
                        </Col>
                    </Row>
                    <small className="text-muted d-block mb-4">
                        <FaLock /> Your data is securely tokenized by leading payment services.
                    </small>
                </Container>
            </div>
        )
    }
}