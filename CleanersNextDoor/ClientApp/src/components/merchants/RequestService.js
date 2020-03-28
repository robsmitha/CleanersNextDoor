import React, { Component } from 'react';
import { Authentication } from '../../services/authentication';
import TextInput from '../TextInput';
import PasswordInput from '../PasswordInput';
import validate from '../Validate'
import { GiDrippingKnife } from 'react-icons/gi';

export class RequestService extends Component {

    constructor(props) {
        super(props);
        this.state = {
            merchantId: this.props.match.params.id,
            formIsValid: false,
            formControls: {
                firstName: {
                    value: '',
                    placeholder: 'Enter your first name',
                    label: 'First Name',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 2
                    },
                    errors: []
                },
                lastName: {
                    value: '',
                    placeholder: 'Enter your last name',
                    label: 'Last Name',
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
                    placeholder: 'Enter your email',
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
                phone: {
                    value: '',
                    placeholder: 'Enter your phone',
                    label: 'Phone',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 9
                    },
                    errors: []
                }
            }
        };
    }

    changeHandler = event => {
        const name = event.target.name;
        const value = event.target.value;

        const updatedControls = {
            ...this.state.formControls
        };

        const updatedFormElement = {
            ...updatedControls[name]
        };

        updatedFormElement.value = value;
        updatedFormElement.touched = true;
        var validation = validate(value, updatedFormElement.validationRules, updatedFormElement.label);
        updatedFormElement.valid = validation.isValid;
        updatedFormElement.errors = validation.errorMessages;

        updatedControls[name] = updatedFormElement;

        let formIsValid = true;
        for (let inputIdentifier in updatedControls) {
            formIsValid = updatedControls[inputIdentifier].valid && formIsValid;
        }

        this.setState({
            formControls: updatedControls,
            formIsValid: formIsValid
        });
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
            this.props.history.push(`/merchant/${this.state.merchantId}/service-request/${data.id}`)
        }
        else {
            alert('The username or password was incorrect.')
            this.setState({
                formIsValid: true
            });
        }
    }

    render() {
        return (
            <div className="container">
                <div className="row">
                    <div className="col-md-4">
                        <h1>Request Pick Up Service</h1>
                        <form method="post" onSubmit={this.requestCreateServiceRequest}>
                            <TextInput name="firstName"
                                placeholder={this.state.formControls.firstName.placeholder}
                                label={this.state.formControls.firstName.label}
                                value={this.state.formControls.firstName.value}
                                onChange={this.changeHandler}
                                touched={this.state.formControls.firstName.touched ? 1 : 0}
                                valid={this.state.formControls.firstName.valid ? 1 : 0}
                                errors={this.state.formControls.firstName.errors} />

                            <TextInput name="lastName"
                                placeholder={this.state.formControls.lastName.placeholder}
                                label={this.state.formControls.lastName.label}
                                value={this.state.formControls.lastName.value}
                                onChange={this.changeHandler}
                                touched={this.state.formControls.lastName.touched ? 1 : 0}
                                valid={this.state.formControls.lastName.valid ? 1 : 0}
                                errors={this.state.formControls.lastName.errors} />

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

                            <button className="btn btn-primary" type="submit" disabled={!this.state.formIsValid}>
                                Request Services
                            </button>
                        </form>
                    </div>
                </div>
            </div>
        )
    }
}
