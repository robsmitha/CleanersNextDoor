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
import {Row, Col } from 'reactstrap'
import Loading from '../../helpers/Loading';

export class Payment extends Component {

    constructor(props) {
        super(props);
        this.state = {
            merchantId: this.props.match.params.id,
            orderId: 0,
            workflowId: 0,
            cart: null,
            merchantName: null,
            merchantDefaultImageUrl: null,
            formIsValid: false,
            steps: null,
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
                    label: 'Email address',
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
                    label: 'Street address',
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
                    label: 'Apt / suite',
                    valid: true,
                    touched: false,
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
                    valid: true,
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
                },
                scheduledAt: {
                    value: '',
                    placeholder: 'Pickup date',
                    label: 'Pickup date',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true
                    },
                    errors: []
                },
                note: {
                    value: '',
                    placeholder: 'Note',
                    label: 'Note',
                    valid: true,
                    touched: false,
                    validationRules: {
                        isRequired: false
                    },
                    errors: []
                },
                cardHolderName: {
                    value: '',
                    placeholder: 'Full name displayed on card',
                    label: 'Name on card',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true
                    },
                    errors: []
                },
                stripePaymentMethodId: {
                    value: '',
                    placeholder: 'Payment methods',
                    label: 'Payment methods',
                    valid: true,
                    touched: false,
                    validationRules: {
                    },
                    errors: []
                }
            },
            paymentMethods: null
        };


        this.stripePromise = customerService.stripePublicKey()
            .then(data => loadStripe(data.key)) 
    }

    componentDidMount() {
        this.populateCustomerCart()
        this.populateWorkflow()
        this.populatePaymentMethods()
    }

    async populatePaymentMethods() {
        const data = await customerService.getPaymentMethods()
        if (data) {
            const defaultPaymentMethod = data.filter(pm => pm.isDefault === true)
            const stripePaymentMethodId = defaultPaymentMethod.length > 0
                ? defaultPaymentMethod[0].stripePaymentMethodID
                : '';
            const updateFormControls = {
                ...this.state.formControls
            }
            if (stripePaymentMethodId.length > 0) {
                updateFormControls.stripePaymentMethodId.valid = true;
                updateFormControls.stripePaymentMethodId.value = stripePaymentMethodId;
            }
            this.setState({
                paymentMethods: data,
                formControls: updateFormControls
            })
        }
    }
    populateCustomerCart() {
        customerService.getCart(this.state.merchantId, true)
            .then(data => {
                this.setState({
                    cart: data,
                    orderId: data.orderID,
                    clientSecret: data.clientSecret
                });
            })
    }

    populateWorkflow() {
        merchantService.getMerchantWorkflow(this.state.merchantId)
            .then(data => {
                const { workflow, customer, steps } = data

                const updateFormControls = {
                    ...this.state.formControls
                }
                if (data.customer !== null) {
                    updateFormControls.name.value = data.customer.name
                    updateFormControls.phone.value = data.customer.phone
                    updateFormControls.email.value = data.customer.email
                    updateFormControls.name.valid = true
                    updateFormControls.phone.valid = true
                    updateFormControls.email.valid = true
                }
                

                steps.forEach(s => {
                    if (s.correspondenceTypeCustomerConfigures === true) {
                        if (s.address.street1 !== null) {
                            updateFormControls.street1.value = s.address.street1
                            updateFormControls.street1.valid = true
                        }
                        if (s.address.street2 !== null) {
                            updateFormControls.street2.value = s.address.street2
                            updateFormControls.street2.valid = true
                        }
                        if (s.address.city !== null) {
                            updateFormControls.city.value = s.address.city
                            updateFormControls.city.valid = true
                        }
                        if (s.address.stateAbbreviation !== null) {
                            updateFormControls.stateAbbreviation.value = s.address.stateAbbreviation
                            updateFormControls.stateAbbreviation.valid = true
                        }
                        if (s.address.zip !== null) {
                            updateFormControls.zip.value = s.address.zip
                            updateFormControls.zip.valid = true
                        }
                    }
                })


                this.setState({
                    workflowId: workflow.workflowID,
                    merchantId: workflow.merchantID,
                    merchantName: workflow.merchantName,
                    merchantDefaultImageUrl: workflow.merchantDefaultImageUrl,
                    merchantCallToAction: workflow.merchantCallToAction,
                    customer: customer,
                    steps: steps
                });
            })
    }

    stripePaymentMethodHandler = () => {
        const { formControls } = this.state
        const payment = {
            stripePaymentMethodId: formControls.stripePaymentMethodId.value
        }
        const data = this.processServiceRequest(payment)
        //try charge
        customerService.tryPayWithPaymentMethod(data)
            .then(this.handleResponse)
    }

    stripeTokenHandler = async (paymentIntent) => {
        const payment = {
            stripePaymentMethodId: paymentIntent.payment_method
        }
        const data = this.processServiceRequest(payment)
        customerService.createServiceRequest(data)
            .then(this.handleResponse)
    }

    handleResponse = (data) => {
        if (data !== null) {
            if (data > 0) {
                this.props.history.push(`/order-details/:id`.replace(':id', data))
            } else {
                alert(data)
            }
        }
        else {
            //request failed
            alert('request failed')
        }

        this.setState({
            formIsValid: true
        });
    }
    processServiceRequest = (payment) => {

        const { formControls, workflowId } = this.state

        const serviceRequest = {
            name: formControls.name.value,
            phone: formControls.phone.value,
            email: formControls.email.value,
            workflowID: workflowId
        }
        const delivery = {
            street1: formControls.street1.value,
            street2: formControls.street2.value,
            city: formControls.city.value,
            stateAbbreviation: formControls.stateAbbreviation.value,
            zip: formControls.zip.value,
            correspondenceTypeID: formControls.correspondenceTypeID,
            scheduledAt: formControls.scheduledAt.value,
            note: formControls.note.value
        }
        let correspondenceAddresses = []
        this.state.steps.forEach((s, index) => {

            if (s.correspondenceTypeCustomerConfigures === true) {
                correspondenceAddresses.push({
                    street1: delivery.street1,
                    street2: delivery.street2,
                    city: delivery.city,
                    stateAbbreviation: delivery.stateAbbreviation,
                    zip: delivery.zip,
                    correspondenceTypeID: s.correspondenceTypeID,
                    scheduledAt: delivery.scheduledAt,
                    note: delivery.note

                })
            }
            else {
                correspondenceAddresses.push({
                    street1: s.address.street1,
                    street2: s.address.street2,
                    city: s.address.city,
                    stateAbbreviation: s.address.stateAbbreviation,
                    zip: s.address.zip,
                    correspondenceTypeID: s.correspondenceTypeID,
                    scheduledAt: s.address.scheduledAt
                })
            }
        })
        const data = {
            orderID: this.state.orderId,
            payment,
            serviceRequest,
            correspondenceAddresses,
            merchantId: this.state.merchantId
        }

        return data
    }
    changeHandler = event => {
        const name = event.target.name;
        const value = event.target.value;
        this.setState(handleChange(name, value, this.state.formControls))
        console.log(this.state.formControls)
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
        const { 
            cart, 
            merchantId, 
            merchantName,
            merchantDefaultImageUrl,
            merchantCallToAction,
            paymentMethods
        } = this.state
        return (
            <div>
                <div className="progress rounded-0 sticky-top">
                    <div className="progress-bar" role="progressbar" style={{ width: 75 + '%' }} aria-valuenow={75} aria-valuemin={75} aria-valuemax="100"></div>
                </div>
                <div className="container">
                    <div className="py-3">
                        <div className="row">
                            <div className="col-md-5 order-md-2 mb-4">
                                {cart == null
                                    ? <Loading />
                                    : Payment.renderCart(cart, merchantId, merchantName, merchantDefaultImageUrl, merchantCallToAction)}
                            </div>
                            <div className="col-md-7 order-md-1">
                                <h1 className="mb-1 h5 text-primary text-uppercase">
                                    Your Order Details
                                </h1>
                                <h3>
                                    Pickup & Delivery Information
                                </h3>
                                <Row>
                                    <Col md="6">
                                        <TextInput name="scheduledAt" type="date"
                                            placeholder={this.state.formControls.scheduledAt.placeholder}
                                            label={this.state.formControls.scheduledAt.label}
                                            value={this.state.formControls.scheduledAt.value}
                                            onChange={this.changeHandler}
                                            touched={this.state.formControls.scheduledAt.touched ? 1 : 0}
                                            valid={this.state.formControls.scheduledAt.valid ? 1 : 0}
                                            errors={this.state.formControls.scheduledAt.errors} />
                                    </Col>
                                    <Col md="6">
                                        <TextInput name="name"
                                            placeholder={this.state.formControls.name.placeholder}
                                            label={this.state.formControls.name.label}
                                            value={this.state.formControls.name.value}
                                            onChange={this.changeHandler}
                                            touched={this.state.formControls.name.touched ? 1 : 0}
                                            valid={this.state.formControls.name.valid ? 1 : 0}
                                            errors={this.state.formControls.name.errors} />
                                    </Col>
                                </Row>
                                

                                
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

                                <AddressForm
                                    street1={this.state.formControls.street1}
                                    street2={this.state.formControls.street2}
                                    city={this.state.formControls.city}
                                    stateAbbreviation={this.state.formControls.stateAbbreviation}
                                    zip={this.state.formControls.zip}
                                    note={this.state.formControls.note}
                                    changeHandler={this.changeHandler}
                                />

                                <hr />

                                <h3>
                                    Payment
                                </h3>
                                <div>
                                    {paymentMethods === null
                                        ? <Loading />
                                        : <div className="form-group">
                                            <label className="font-weight-bold">Saved payment methods</label>
                                            <select className="form-control"
                                                name="stripePaymentMethodId"
                                                value={this.state.formControls.stripePaymentMethodId.value}
                                                onChange={this.changeHandler}>
                                                <option value="">Manually enter card</option>
                                                {paymentMethods.map(pm =>
                                                    <option key={pm.id} value={pm.stripePaymentMethodID}>
                                                        {pm.cardBrand} - {pm.last4} - {pm.expMonth}/{pm.expYear}
                                                    </option>
                                                )}
                                            </select>
                                        </div>}

                                    <TextInput name="cardHolderName"
                                        placeholder={this.state.formControls.cardHolderName.placeholder}
                                        label={this.state.formControls.cardHolderName.label}
                                        value={this.state.formControls.cardHolderName.value}
                                        onChange={this.changeHandler}
                                        touched={this.state.formControls.cardHolderName.touched ? 1 : 0}
                                        valid={this.state.formControls.cardHolderName.valid ? 1 : 0}
                                        errors={this.state.formControls.cardHolderName.errors} />

                                    {this.state.formControls.stripePaymentMethodId.value.length === 0
                                        ? <Elements stripe={this.stripePromise}>
                                            <CheckoutForm
                                                stripeTokenHandler={this.stripeTokenHandler}
                                                disabled={!this.state.formIsValid}
                                                cardHolderName={this.state.formControls.cardHolderName.value}
                                                clientSecret={this.state.clientSecret}
                                            />
                                        </Elements>
                                        : <button type="button" className="btn btn-dark btn-block my-3" disabled={!this.state.formIsValid} onClick={this.stripePaymentMethodHandler}>
                                            Complete Checkout with Stored Payment
                                        </button>}
                                    

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            )
    }


    static renderCart(cart, merchantId, merchantName, merchantDefaultImageUrl, merchantCallToAction) {
        let cartItems = cart.cartItems;
        return (
            <div className="card border-0 shadow mb-3">
                <div className="card-body">
                    <div className="media">
                        <div className="media-body">
                            <h5>{merchantName}</h5>
                            <small className="text-muted">{merchantCallToAction}</small>
                        </div>
                        <img src={merchantDefaultImageUrl} className="ml-3 rounded" width={100} />
                    </div>
                </div>
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