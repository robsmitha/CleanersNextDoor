import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import TextInput from '../TextInput';
import handleChange from '../HandleChange';
import { AuthConsumer } from './../../context/AuthContext'

export class Payment extends Component {

    constructor(props) {
        super(props);
        this.state = {
            merchantId: this.props.match.params.id,
            formIsValid: false,
            formControls: {
                contactName: {
                    value: '',
                    placeholder: 'Enter the full contact name',
                    label: 'Name',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 2
                    },
                    errors: []
                },
                contactEmail: {
                    value: '',
                    placeholder: 'Enter the contact email',
                    label: 'Email',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        isEmail: true,
                        minLength: 5
                    },
                    errors: []
                },
                contactPhone: {
                    value: '',
                    placeholder: 'Enter the contact phone',
                    label: 'Phone',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 9
                    },
                    errors: []
                },
                pickUpStreet1: {
                    value: '',
                    placeholder: 'Enter the street address',
                    label: 'Street Address',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 2
                    },
                    errors: []
                }
            }
        };
    }
    changeHandler = event => {
        const name = event.target.name;
        const value = event.target.value;
        this.setState(handleChange(name, value, this.state.formControls));
    }

    requestCreateServiceRequest = (event) => {
        event.preventDefault();
        this.setState({
            formIsValid: false
        });
        var serviceRequest = {

        };
        this.CreateServiceRequest(serviceRequest);
    }

    async CreateServiceRequest(serviceRequest) {
        const response = await fetch('merchants/CreateServiceRequest', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(serviceRequest)
        });
        const data = await response.json();
        if (data && data.id > 0) {
            this.props.history.push(`/service-request/${data.id}`)
        }
        else {
            alert('An error occurred.')
            this.setState({
                formIsValid: true
            });
        }
    }

    render() {
        return (
            <div>
                <AuthConsumer>
                    {({ authenticated }) => (
                        <div>
                            {!authenticated
                                ? <Redirect to='/customer/sign-in' />
                                : this.renderContent()}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        )
    }
    renderContent() {
        return (
            <div className="container">
                <h1>Request Pick Up Service</h1>
                <h3>Payment Information</h3>
                <hr />
                <div className="row">

                    <div className="col-md-4">
                        <form method="post" onSubmit={this.requestCreateServiceRequest}>
                            <h1 className="border-bottom">
                                Contact Information
                            </h1>
                            <TextInput name="contactName"
                                placeholder={this.state.formControls.contactName.placeholder}
                                label={this.state.formControls.contactName.label}
                                value={this.state.formControls.contactName.value}
                                onChange={this.changeHandler}
                                touched={this.state.formControls.contactName.touched ? 1 : 0}
                                valid={this.state.formControls.contactName.valid ? 1 : 0}
                                errors={this.state.formControls.contactName.errors} />

                            <TextInput name="contactEmail"
                                placeholder={this.state.formControls.contactEmail.placeholder}
                                label={this.state.formControls.contactEmail.label}
                                value={this.state.formControls.contactEmail.value}
                                onChange={this.changeHandler}
                                touched={this.state.formControls.contactEmail.touched ? 1 : 0}
                                valid={this.state.formControls.contactEmail.valid ? 1 : 0}
                                errors={this.state.formControls.contactEmail.errors} />

                            <TextInput name="contactPhone"
                                placeholder={this.state.formControls.contactPhone.placeholder}
                                label={this.state.formControls.contactPhone.label}
                                value={this.state.formControls.contactPhone.value}
                                onChange={this.changeHandler}
                                touched={this.state.formControls.contactPhone.touched ? 1 : 0}
                                valid={this.state.formControls.contactPhone.valid ? 1 : 0}
                                errors={this.state.formControls.contactPhone.errors} />

                            <h1 className="border-bottom">
                                Pick up Information
                            </h1>

                            <button className="btn btn-primary btn-block" type="submit" disabled={!this.state.formIsValid}>
                                Continue to payment
                            </button>

                            <Link className="btn btn-secondary btn-block" to={'/request-service/:id'.replace(':id', this.state.merchantId)}>
                                Go Back
                            </Link>
                        </form>
                    </div>
                </div>
            </div>
            )
    }
}