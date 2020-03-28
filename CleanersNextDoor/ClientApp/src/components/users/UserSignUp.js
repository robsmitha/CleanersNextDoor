import React, { Component } from 'react';
import { Authentication } from '../../services/authentication'
import TextInput from '../TextInput';
import PasswordInput from '../PasswordInput';
import validate from '../Validate'

export class UserSignUp extends Component {

    constructor(props) {
        super(props);
        this.state = {
            formIsValid: false, 
            formControls: {
                username: {
                    value: '',
                    placeholder: 'Enter your username',
                    label: 'Username',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 3
                    },
                    errors: []
                },
                password: {
                    value: '',
                    placeholder: 'Enter your password',
                    label: 'Password',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 6
                    },
                    errors: []
                },
                confirmPassword: {
                    value: '',
                    placeholder: 'Enter the confirm password',
                    label: 'Confirm Password',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 6
                    },
                    errors: []
                },
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
        var validationResult = validate(value, updatedFormElement.validationRules, updatedFormElement.label);
        updatedFormElement.valid = validationResult.isValid;
        updatedFormElement.errors = validationResult.errorMessages;

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

    requestSignUp = (event) => {
        event.preventDefault();
        this.setState({
            formIsValid: true
        });
        var user = {
            username: this.state.formControls.username.value,
            password: this.state.formControls.password.value,
            firstName: this.state.formControls.firstName.value,
            lastName: this.state.formControls.lastName.value,
            email: this.state.formControls.email.value
        };
        this.trySignUp(user);
    }

    async trySignUp(user) {
        const response = await fetch('users/signup', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)
        });
        const data = await response.json();
        if (data && data.id > 0) {
            Authentication.setUserId(data.id);
            this.props.history.push('/profile')
        }
        else {
            alert('An error occurred. Please try again.')
            this.setState({
                formIsValid: true
            });
        }
    }

    checkUsernameBlur = event => {
        if (this.state.formControls.username.valid) {
            this.checkUsername()
        }
    }

    checkPasswordMatch = event => {
        if (this.state.formControls.password.value.length > 0 && this.state.formControls.confirmPassword.value.length > 0) {
            if (this.state.formControls.password.value !== this.state.formControls.confirmPassword.value) {
                const updatedControls = {
                    ...this.state.formControls
                };

                updatedControls.confirmPassword.valid = false;
                updatedControls.confirmPassword.errors = []
                updatedControls.confirmPassword.errors.push(`The passwords must match`)
                this.setState({
                    formControls: updatedControls,
                    formIsValid: false
                });
            }
        }
    }

    checkUsername = async () => {
        const response = await fetch(`users/CheckUsernameAvailability/${this.state.formControls.username.value}`);
        const data = await response.json();
        if (!data.isAvailable) {
            const updatedControls = {
                ...this.state.formControls
            };

            updatedControls.username.valid = false;
            updatedControls.username.errors = []
            updatedControls.username.errors.push(`The username ${this.state.formControls.username.value} is already taken`)

            this.setState({
                formControls: updatedControls,
                formIsValid: false
            });
        }
    }

    render() {
        return (
            <div className="container">
                <div className="row">
                    <div className="col-md-4">
                        <h1>Sign Up</h1>
                        <form method="post" onSubmit={this.requestSignUp}>

                            <TextInput name="username"
                                placeholder={this.state.formControls.username.placeholder}
                                label={this.state.formControls.username.label}
                                value={this.state.formControls.username.value}
                                onChange={this.changeHandler}
                                touched={this.state.formControls.username.touched ? 1 : 0}
                                valid={this.state.formControls.username.valid ? 1 : 0}
                                onBlur={this.checkUsernameBlur}
                                errors={this.state.formControls.username.errors} />

                            <PasswordInput name="password"
                                placeholder={this.state.formControls.password.placeholder}
                                label={this.state.formControls.password.label}
                                value={this.state.formControls.password.value}
                                onChange={this.changeHandler}
                                touched={this.state.formControls.password.touched ? 1 : 0}
                                valid={this.state.formControls.password.valid ? 1 : 0}
                                onBlur={this.checkPasswordMatch}
                                errors={this.state.formControls.password.errors} />

                            <PasswordInput name="confirmPassword"
                                placeholder={this.state.formControls.confirmPassword.placeholder}
                                label={this.state.formControls.confirmPassword.label}
                                value={this.state.formControls.confirmPassword.value}
                                onChange={this.changeHandler}
                                touched={this.state.formControls.confirmPassword.touched ? 1 : 0}
                                valid={this.state.formControls.confirmPassword.valid ? 1 : 0}
                                onBlur={this.checkPasswordMatch}
                                errors={this.state.formControls.confirmPassword.errors} />

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

                            <button className="btn btn-primary" type="submit" disabled={!this.state.formIsValid}>Sign in</button>
                        </form>
                    </div>
                </div>
            </div>
        )
    }
}