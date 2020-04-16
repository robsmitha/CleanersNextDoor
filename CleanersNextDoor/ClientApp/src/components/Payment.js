import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import TextInput from './TextInput';
import handleChange from './HandleChange';
import { AuthConsumer } from './../context/AuthContext'

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
                state: {
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
            <div className="container">
                <h1>Service Request Information</h1>
                <hr />
                <div className="row">
                    <div className="col-md-4 order-md-2 mb-4">

                    </div>
                    <div className="col-md-8 order-md-1">
                        <form method="post" onSubmit={this.requestCreateServiceRequest}>
                            <div className="row mb-2">
                                <div className="col-md-6 border-right">
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
                                </div>
                                <div className="col-md-6">
                                    <h3>
                                        Pick up Information
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
                                            <label>State</label>
                                            <select className="form-control">
                                                <option value="">Select one</option>
                                            </select>
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
                                </div>
                            </div>

                            <div className="row">
                                <div className="col-md-6 offset-md-3">
                                    <button className="btn btn-primary btn-block" type="submit" disabled={!this.state.formIsValid}>
                                        Confirm payment
                                    </button>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>

            </div>
            )
    }
}