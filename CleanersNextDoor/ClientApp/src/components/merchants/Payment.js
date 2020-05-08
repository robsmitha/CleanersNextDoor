import React, { Component } from 'react';
import { Link, Redirect  } from 'react-router-dom'
import TextInput from './../../helpers/TextInput';
import handleChange from './../../helpers/HandleChange';
import { AuthConsumer } from './../../context/AuthContext'
import { customerService } from '../../services/customer.service'
import { loadStripe } from '@stripe/stripe-js';
import { Elements } from '@stripe/react-stripe-js';
import CheckoutForm from '../../helpers/CheckoutForm';
import { merchantService } from '../../services/merchant.service'
import AddressForm from '../../helpers/AddressForm';

export class Payment extends Component {

    constructor(props) {
        super(props);
        this.state = {
            merchantId: this.props.match.params.id,
            orderId: 0,
            workflowId: 0,
            cart: null,
            merchantName: null,
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
                addresses: []
            }
        };


        this.stripePromise = customerService.stripePublicKey()
            .then(data => loadStripe(data.key)) 
    }

    componentDidMount() {
        this.populateCustomerCart()
        this.populateWorkflow()
    }

    populateCustomerCart() {
        customerService.getCart(this.state.merchantId, true)
            .then(data => {
                this.setState({
                    cart: data,
                    orderId: data && data.cartItems.length > 0
                        ? data.cartItems[0].orderID
                        : 0,
                    clientSecret: data.clientSecret
                });
            })
    }

    populateWorkflow() {
        merchantService.getMerchantWorkflow(this.state.merchantId)
            .then(data => {
                let addresses = []
                data.steps.forEach(s => {
                    addresses.push(
                        {
                            step: s.step,
                            correspondenceTypeID: s.correspondenceTypeID,
                            customerConfigures: s.correspondenceTypeCustomerConfigures,
                            correspondenceTypeName: s.correspondenceTypeName,
                            correspondenceTypeDescription: s.correspondenceTypeDescription,
                            location: s.address.location,
                            street1: {
                                value: s.address.street1 !== null ? s.address.street1 : '',
                                placeholder: 'Street 1',
                                label: 'Street Address',
                                valid: !s.correspondenceTypeCustomerConfigures,
                                touched: false,
                                validationRules: {
                                    isRequired: s.correspondenceTypeCustomerConfigures,
                                    minLength: 2
                                },
                                errors: []
                            },
                            street2: {
                                value: s.address.street2 !== null ? s.address.street2 : '',
                                placeholder: 'Street 2',
                                label: 'Apt/Suite (Optional)',
                                valid: true,
                                touched: false,
                                validationRules: {
                                    isRequired: false
                                },
                                errors: []
                            },
                            city: {
                                value: s.address.city !== null ? s.address.city : '',
                                placeholder: 'City',
                                label: 'City',
                                valid: !s.correspondenceTypeCustomerConfigures,
                                touched: false,
                                validationRules: {
                                    isRequired: s.correspondenceTypeCustomerConfigures,
                                    minLength: 2
                                },
                                errors: []
                            },
                            stateAbbreviation: {
                                value: s.address.stateAbbreviation !== null ? s.address.stateAbbreviation : '',
                                placeholder: 'State',
                                label: 'State',
                                valid: !s.correspondenceTypeCustomerConfigures,
                                touched: false,
                                validationRules: {
                                    isRequired: s.correspondenceTypeCustomerConfigures,
                                    minLength: 2
                                },
                                errors: []
                            },
                            zip: {
                                value: s.address.zip !== null ? s.address.zip : '',
                                placeholder: 'Zip',
                                label: 'Zip',
                                valid: !s.correspondenceTypeCustomerConfigures,
                                touched: false,
                                validationRules: {
                                    isRequired: s.correspondenceTypeCustomerConfigures,
                                    minLength: 5
                                },
                                errors: []
                            },
                            scheduledAt: {
                                value: s.scheduledAt,
                                placeholder: 'Scheduled At',
                                label: 'Scheduled At',
                                valid: !s.correspondenceTypeCustomerConfigures,
                                touched: false,
                                validationRules: {
                                    isRequired: s.correspondenceTypeCustomerConfigures
                                },
                                errors: []
                            },
                            note: {
                                value: s.address.note !== null ? s.address.note : '',
                                placeholder: 'Note',
                                label: 'Note (Optional)',
                                valid: true,
                                touched: false,
                                validationRules: {
                                    isRequired: false
                                },
                                errors: []
                            }
                        }
                    )
                })
                const updateFormControls = {
                    ...this.state.formControls
                }
                updateFormControls.name.value = data.customer.name
                updateFormControls.phone.value = data.customer.phone
                updateFormControls.email.value = data.customer.email
                updateFormControls.addresses = addresses
                this.setState({
                    formControls: updateFormControls,
                    workflowId: data.workflowID,
                    merchantName: data.merchantName
                });
            })
    }

    stripeTokenHandler = async (paymentIntent) => {
        let payment = {
            stripePaymentMethodId: paymentIntent.payment_method,
            centAmount: paymentIntent.amount,
            currency: paymentIntent.currency,
            chargedTimestamp: paymentIntent.created
        }
        let serviceRequest = {
            name: this.state.formControls.name.value,
            phone: this.state.formControls.phone.value,
            email: this.state.formControls.email.value,
            workflowID: this.state.workflowId
        }
        let correspondenceAddresses = []
        this.state.formControls.addresses.map((a, index) =>
            correspondenceAddresses.push({
                street1: a.street1.value,
                street2: a.street2.value,
                city: a.city.value,
                stateAbbreviation: a.stateAbbreviation.value,
                zip: a.zip.value,
                correspondenceTypeID: a.correspondenceTypeID,
                scheduledAt: a.scheduledAt.value,
                note: a.note.value
            })
        )
        let data = {
            orderID: this.state.orderId,
            payment,
            serviceRequest,
            correspondenceAddresses
        }
        customerService.createServiceRequest(data)
            .then(data => {
                if (data !== null) {
                    if (data > 0) {
                        this.props.history.push(`/order-details/:id`.replace(':id', data))
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

    changeHandler = (event, index) => {
        const name = event.target.name;
        const value = event.target.value;
        this.setState(handleChange(name, value, this.state.formControls, index));
    }

    render() {
        return (
            <div>
                <AuthConsumer>
                    {({ authenticated }) => (
                        <div>
                            {!authenticated
                                ? <Redirect to='/sign-in' />
                                : this.renderLayout()}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        )
    }

    renderLayout() {
        const { cart, merchantId } = this.state
        return (
            <div>
                <div className="container">
                    <div className="py-3">
                        <div className="row">
                            <div className="col-md-4 order-md-2 mb-4">
                                {cart == null
                                    ? <p><em>Loading cart...</em></p>
                                    : Payment.renderCart(cart, merchantId)}
                            </div>
                            <div className="col-md-8 order-md-1">
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
                                <div className="row">
                                    <div className="col-md-6">
                                        <TextInput name="email"
                                            placeholder={this.state.formControls.email.placeholder}
                                            label={this.state.formControls.email.label}
                                            value={this.state.formControls.email.value}
                                            onChange={this.changeHandler}
                                            touched={this.state.formControls.email.touched ? 1 : 0}
                                            valid={this.state.formControls.email.valid ? 1 : 0}
                                            errors={this.state.formControls.email.errors} />
                                    </div>
                                    <div className="col-md-6">
                                        <TextInput name="phone"
                                            placeholder={this.state.formControls.phone.placeholder}
                                            label={this.state.formControls.phone.label}
                                            value={this.state.formControls.phone.value}
                                            onChange={this.changeHandler}
                                            touched={this.state.formControls.phone.touched ? 1 : 0}
                                            valid={this.state.formControls.phone.valid ? 1 : 0}
                                            errors={this.state.formControls.phone.errors} />
                                    </div>
                                </div>
                                <h3>
                                    Address Details
                                </h3>
                                <div className="list-group list-group-flush">
                                    {this.state.formControls.addresses.map((a, index) =>
                                        <div className={'list-group-item px-0 '.concat(!a.customerConfigures ? 'list-group-item-light' : '')} key={index}>
                                            <p className="mb-0">Step {a.step}: {a.correspondenceTypeName}</p>
                                            <small className="text-muted">
                                                {a.correspondenceTypeDescription}
                                            </small>
                                            <div hidden={a.customerConfigures}>
                                                {a.location}
                                            </div>
                                            <AddressForm hidden={!a.customerConfigures}
                                                street1={a.street1}
                                                street2={a.street2}
                                                city={a.city}
                                                stateAbbreviation={a.stateAbbreviation}
                                                zip={a.zip}
                                                note={a.note}
                                                scheduledAt={a.scheduledAt}
                                                changeHandler={(event) => this.changeHandler(event, index)}
                                            />
                                        </div>)}
                                </div>
                                <h3>
                                    Payment Details
                                </h3>
                                <Elements stripe={this.stripePromise}>
                                    <CheckoutForm
                                        stripeTokenHandler={this.stripeTokenHandler}
                                        /*disabled={!this.state.formIsValid}*/
                                        name={this.state.formControls.name.value}
                                        clientSecret={this.state.clientSecret}
                                    />
                                </Elements>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            )
    }


    static renderCart(cart, merchantId) {
        let cartItems = cart.cartItems;
        return (
            <div className="card border-0 shadow mb-3">
                <div className="list-group list-group-flush">
                    {cartItems.map(c =>
                        <div className="list-group-item" key={c.id}>
                            <div className="form-row">
                                <div className="col-md-7">
                                    <small className="text-muted"> {c.itemName}</small>
                                </div>
                                <div className="col-2 text-right">
                                    <small className="text-muted">X {c.currentQuantity}</small>
                                </div>
                                <div className="col-3 text-right">
                                    <small className="text-muted">{c.displayPrice}</small>
                                </div>
                            </div>
                        </div>
                    )}
                    <div className="list-group-item">
                        <strong>Total (USD):</strong>
                        <span className="float-right">
                            {cart.displayPrice}
                        </span>
                    </div>
                    <Link to={'/cart/:id'.replace(':id', merchantId)} className="list-group-item list-group-item-action text-center">
                        Modify order
                    </Link>
                </div>
            </div>
        )
    }
}