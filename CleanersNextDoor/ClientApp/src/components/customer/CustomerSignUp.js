import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import TextInput from '../TextInput';
import PasswordInput from '../PasswordInput';
import handleChange from '../HandleChange';
import { AuthConsumer } from '../../context/AuthContext'

export class CustomerSignUp extends Component {

    constructor(props) {
        super(props);
        this.state = {
            formIsValid: false,
            formControls: {
                email: {
                    value: '',
                    placeholder: 'Enter your email',
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
                phone: {
                    value: '',
                    placeholder: 'Enter your phone number',
                    label: 'Phone Number',
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
        this.setState(handleChange(name, value, this.state.formControls));
    }

    checkEmailBlur = event => {
        if (this.state.formControls.email.valid) {
            this.checkEmail()
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

    checkEmail() {

        fetch(`customers/CheckEmailAvailability/${this.state.formControls.email.value}`)
            .then(response => response.json())
            .then(data => {
                if (!data.isAvailable) {
                    const updatedControls = {
                        ...this.state.formControls
                    };

                    updatedControls.email.valid = false;
                    updatedControls.email.errors = []
                    updatedControls.email.errors.push(`The email ${this.state.formControls.email.value} is already in use.`)

                    this.setState({
                        formControls: updatedControls,
                        formIsValid: false
                    });
                }

            })
            .catch((ex) => { console.log(ex) })
    }

    render() {
        return (
            <div>
                <AuthConsumer>
                    {({ authenticated, customerSignUp }) => (
                        <div>
                            {authenticated
                                ? <Redirect to='/customer/profile' />
                                : this.renderSignUp(customerSignUp)}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        )
    }

    renderSignUp(customerSignUp) {
        return (
            <div className="container my-md-5">
                <div className="row justify-content-center align-items-center h-100">
                    <div className="col-md-4">
                        <h1 className="text-center">Sign up.</h1>
                        <form method="post" onSubmit={customerSignUp}>

                            <TextInput name="email"
                                placeholder={this.state.formControls.email.placeholder}
                                label={this.state.formControls.email.label}
                                value={this.state.formControls.email.value}
                                onChange={this.changeHandler}
                                onBlur={this.checkEmailBlur}
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

                            <TextInput name="phone"
                                placeholder={this.state.formControls.phone.placeholder}
                                label={this.state.formControls.phone.label}
                                value={this.state.formControls.phone.value}
                                onChange={this.changeHandler}
                                touched={this.state.formControls.phone.touched ? 1 : 0}
                                valid={this.state.formControls.phone.valid ? 1 : 0}
                                errors={this.state.formControls.phone.errors} />

                            <button className="btn btn-primary btn-block" type="submit" disabled={!this.state.formIsValid}>Sign up</button>
                            <Link to="/customer/sign-in" className="btn btn-secondary btn-block">Sign in</Link>
                        </form>
                    </div>
                </div>
            </div>
            )
    }
}