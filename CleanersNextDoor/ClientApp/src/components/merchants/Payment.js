import React, { Component, useState } from 'react';
import { Link, Redirect } from 'react-router-dom'
import TextInput from './../../helpers/TextInput';
import handleChange from './../../helpers/HandleChange';
import { AuthConsumer } from './../../context/AuthContext'
import { FaTimes } from 'react-icons/fa'
import { customerService } from '../../services/customer.service'


import { loadStripe } from '@stripe/stripe-js';
import { Elements } from '@stripe/react-stripe-js';
import CheckoutForm from '../../helpers/CheckoutForm';
import './Payment.css'

export class Payment extends Component {

    constructor(props) {
        super(props);
        this.state = {
            merchantId: this.props.match.params.id,
            formIsValid: false,
            formControls: {
                name: {
                    value: '',
                    placeholder: 'Full name',
                    label: 'Full name',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 2
                    },
                    errors: []
                },
                email: {
                    value: '',
                    placeholder: 'Email',
                    label: 'Email Address',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 2,
                        isEmail: true
                    },
                    errors: []
                },
                phone: {
                    value: '',
                    placeholder: 'Phone',
                    label: 'Phone',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 9
                    },
                    errors: []
                },
                street1: {
                    value: '',
                    placeholder: 'Street 1',
                    label: 'Street Address',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 2
                    },
                    errors: []
                },
                street2: {
                    value: '',
                    placeholder: 'Street 2',
                    label: 'Apt/Suite (Optional)',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: false
                    },
                    errors: []
                },
                city: {
                    value: '',
                    placeholder: 'City',
                    label: 'City',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 2
                    },
                    errors: []
                },
                stateAbbreviation: {
                    value: '',
                    placeholder: 'State',
                    label: 'State',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 2
                    },
                    errors: []
                },
                zip: {
                    value: '',
                    placeholder: 'Zip',
                    label: 'Zip',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 5
                    },
                    errors: []
                }
            }
        };


        this.stripePromise = customerService.stripePublicKey()
            .then(data => loadStripe(data.key)) 
    }

    async stripeTokenHandler(token) {
        let data = { token: token.id }
        console.log(token)
        return;
    }
    changeHandler = event => {
        const name = event.target.name;
        const value = event.target.value;
        this.setState(handleChange(name, value, this.state.formControls));
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
            <div className="d-flex" id="wrapper">

                <div className="bg-light border-right" id="sidebar-wrapper">
                    <div className="sidebar-heading">
                        <Link to="/" className="text-muted text-decoration-none" hidden={true}>
                            <FaTimes />
                        </Link>
                        Checkout
                    </div>
                    <nav className="nav flex-column checkout-steps">
                        <Link className="nav-link border-left ml-4 border-primary" to={'contact-info'}>Contact Information</Link>
                        <Link className="nav-link border-left ml-4 text-secondary" to={'address-info'}>Address Information</Link>
                        <Link className="nav-link border-left ml-4 text-secondary" to={'payment-method'}>Payment method</Link>
                        <Link className="nav-link border-left ml-4 text-secondary" to={'review-and-confirm'}>Review and confirm</Link>
                    </nav>
                </div>
                <div id="page-content-wrapper">

                    <div className="container-fluid">
                        <div className="row">
                            <div className="col-md-4">

                                <h3>
                                    Contact Information
                                </h3>
                                <TextInput name="name"
                                    placeholder={this.state.formControls.name.placeholder}
                                    label={this.state.formControls.name.label}
                                    value={this.state.formControls.name.value}
                                    onChange={this.changeHandler}
                                    touched={this.state.formControls.name.touched ? 1 : 0}
                                    valid={this.state.formControls.name.valid ? 1 : 0}
                                    errors={this.state.formControls.name.errors} />
                                <TextInput name="email"
                                    placeholder={this.state.formControls.email.placeholder}
                                    label={this.state.formControls.email.label}
                                    value={this.state.formControls.email.value}
                                    onChange={this.changeHandler}
                                    touched={this.state.formControls.email.touched ? 1 : 0}
                                    valid={this.state.formControls.email.valid ? 1 : 0}
                                    errors={this.state.formControls.email.errors} />
                                <TextInput name="phone"
                                    placeholder={this.state.formControls.phone.placeholder}
                                    label={this.state.formControls.phone.label}
                                    value={this.state.formControls.phone.value}
                                    onChange={this.changeHandler}
                                    touched={this.state.formControls.phone.touched ? 1 : 0}
                                    valid={this.state.formControls.phone.valid ? 1 : 0}
                                    errors={this.state.formControls.phone.errors} />


                                <h3>
                                    Address Details
                                </h3>
                                <TextInput name="street1"
                                    placeholder={this.state.formControls.street1.placeholder}
                                    label={this.state.formControls.street1.label}
                                    value={this.state.formControls.street1.value}
                                    onChange={this.changeHandler}
                                    touched={this.state.formControls.street1.touched ? 1 : 0}
                                    valid={this.state.formControls.street1.valid ? 1 : 0}
                                    errors={this.state.formControls.street1.errors} />
                                <TextInput name="street2"
                                    placeholder={this.state.formControls.street2.placeholder}
                                    label={this.state.formControls.street2.label}
                                    value={this.state.formControls.street2.value}
                                    onChange={this.changeHandler}
                                    touched={this.state.formControls.street2.touched ? 1 : 0}
                                    valid={this.state.formControls.street2.valid ? 1 : 0}
                                    errors={this.state.formControls.street2.errors} />
                                <TextInput name="city"
                                    placeholder={this.state.formControls.city.placeholder}
                                    label={this.state.formControls.city.label}
                                    value={this.state.formControls.city.value}
                                    onChange={this.changeHandler}
                                    touched={this.state.formControls.city.touched ? 1 : 0}
                                    valid={this.state.formControls.city.valid ? 1 : 0}
                                    errors={this.state.formControls.city.errors} />
                                <div className="form-row">
                                    <div className="col-md-8">
                                        <div className="form-group">
                                            <label className="font-weight-bold">State</label>
                                            <select className="form-control" name="stateAbbreviation" value={this.state.formControls.stateAbbreviation.value} onChange={this.changeHandler}>
                                                <option value="">Select One</option>
                                                <option value="FL">Florida</option>
                                                <option value="NY">New York</option>
                                                <option value="CA">California</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div className="col-md-4">
                                        <TextInput name="zip"
                                            placeholder={this.state.formControls.zip.placeholder}
                                            label={this.state.formControls.zip.label}
                                            value={this.state.formControls.zip.value}
                                            onChange={this.changeHandler}
                                            touched={this.state.formControls.zip.touched ? 1 : 0}
                                            valid={this.state.formControls.zip.valid ? 1 : 0}
                                            errors={this.state.formControls.zip.errors} />
                                    </div>
                                </div>

                                <h3>
                                    Payment Details
                                </h3>
                                <Elements stripe={this.stripePromise}>
                                    <CheckoutForm stripeTokenHandler={this.stripeTokenHandler} disabled={!this.state.formIsValid} />
                                </Elements>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            )
    }
}