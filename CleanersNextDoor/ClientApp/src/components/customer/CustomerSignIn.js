import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import TextInput from '../TextInput';
import PasswordInput from '../PasswordInput';
import { AuthConsumer } from './../../context/AuthContext'
import handleChange from '../HandleChange';

export class CustomerSignIn extends Component {

    constructor(props) {
        super(props);

        this.state = {
            formIsValid: false,
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
        this.setState(handleChange(name, value, this.state.formControls));
    }

    render() {
        return (
            <div>
                <AuthConsumer>
                    {({ isAuth, customerLogin, msg }) => (
                        <div>
                            {isAuth
                                ? <Redirect to='/customer/profile' />
                                : this.renderLoginForm(customerLogin, msg)}
                        </div>
                    )}
                </AuthConsumer>
            </div>
            )
    }

    renderLoginForm(customerLogin, msg) {
        return (
            <div className="container my-md-5">
                <div className="row justify-content-center align-items-center h-100">
                    <div className="col-md-4">
                        <h1 className="text-center">Sign in.</h1>
                        <span className="text-danger">
                            {msg}
                        </span>
                        <form method="post" onSubmit={customerLogin}>
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
                            <button className="btn btn-primary btn-block" type="submit" disabled={!this.state.formIsValid}>Sign in</button>
                            <Link to="/customer/sign-up" className="btn btn-secondary btn-block">Sign up</Link>
                        </form>
                    </div>
                </div>
            </div>
            )
    }
}