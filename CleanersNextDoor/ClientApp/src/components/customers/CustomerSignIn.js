import React, { Component } from 'react';
import { Authentication } from '../../services/authentication';
import TextInput from '../TextInput';
import PasswordInput from '../PasswordInput';
import validate from '../Validate'

export class CustomerSignIn extends Component {

    constructor(props) {
        super(props);
        this.state = {
            formIsValid: false, //we will use this to track the overall form validity
            formControls: {
                email: {
                    value: '',
                    placeholder: 'Email',
                    label: 'Email',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        isEmail: true
                    },
                    errors: []
                },
                password: {
                    value: '',
                    placeholder: 'Password',
                    label: 'Password',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true
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

    requestSignIn = (event) => {
        event.preventDefault();
        this.setState({
            formIsValid: false
        });
        var customer = {
            email: this.state.formControls.email.value,
            password: this.state.formControls.password.value
        };
        this.trySignIn(customer);
    }

    async trySignIn(customer) {
        const response = await fetch('customers/signin', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(customer)
        });
        const data = await response.json();
        if (data && data.id > 0) {
            Authentication.setCustomerId(data.id);
            this.props.history.push('/customers/profile')
        }
        else {
            alert('The email or password was incorrect.')
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
                        <h1>Sign In</h1>
                        <form method="post" onSubmit={this.requestSignIn}>
                            <TextInput name="email"
                                placeholder={this.state.formControls.email.placeholder}
                                label={this.state.formControls.email.label}
                                value={this.state.formControls.email.value}
                                onChange={this.changeHandler}
                                touched={this.state.formControls.email.touched ? 1 : 0}
                                valid={this.state.formControls.email.valid ? 1 : 0}
                                errors={this.state.formControls.email.errors} />

                            <PasswordInput name="password"
                                placeholder={this.state.formControls.password.placeholder}
                                label={this.state.formControls.password.label}
                                value={this.state.formControls.password.value}
                                onChange={this.changeHandler}
                                touched={this.state.formControls.password.touched ? 1 : 0}
                                valid={this.state.formControls.password.valid ? 1 : 0}
                                errors={this.state.formControls.password.errors} />
                            <button className="btn btn-primary" type="submit" disabled={!this.state.formIsValid}>Sign in</button>
                        </form>
                    </div>
                </div>
            </div>
        )
    }
}
