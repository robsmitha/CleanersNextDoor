import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { AuthConsumer } from './../context/AuthContext'
import { IconContext } from 'react-icons'
import { FaCheckCircle, FaHome } from 'react-icons/fa'
import TextInput from './../helpers/TextInput';
import handleChange from './../helpers/HandleChange';
import { customerService } from '../services/customer.service';

export class SignUp extends Component {

    constructor(props) {
        super(props);
        this.state = {
            formIsValid: false,
            formControls: {
                name: {
                    value: '',
                    placeholder: '',
                    label: 'Full name',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 3
                    },
                    errors: []
                },
                email: {
                    value: '',
                    placeholder: '',
                    help: 'We use your email to send import information about your order.',
                    label: 'Email',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        isEmail: true
                    },
                    errors: []
                },
                phone: {
                    value: '',
                    placeholder: '',
                    help: 'Our drivers may need to contact you.',
                    label: 'Phone number',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 9
                    },
                    errors: []
                },
                password: {
                    value: '',
                    placeholder: '',
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
                    placeholder: '',
                    label: 'Confirm password',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 6
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
        customerService.checkEmailAvailability(this.state.formControls.email.value)
            .then(data => {
                if (!data.isAvailable) {
                    const updatedControls = {
                        ...this.state.formControls
                    };

                    updatedControls.email.valid = false;
                    updatedControls.email.errors = []
                    updatedControls.email.errors.push(`'${this.state.formControls.email.value}' is already taken.`)

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
                    {({ authenticated, signUp }) => (
                        <div>
                            {authenticated
                                ? <Redirect to='/account' />
                                : this.renderSignUp(signUp)}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        )
    }

    renderSignUp(signUp) {
        return (
            <div>
                <div className="container-fluid">
                    <div className="row no-gutter">
                        <div className="d-none d-md-block col-md-4 col-lg-6 bg-light border-right">
                            <div className="vh-100 d-flex align-items-stretch py-5">
                                <div className="container">
                                    <div className="row">
                                        <div className="col-md-9 col-lg-8 mx-auto">
                                            <h1 className="h3">
                                                <FaHome /> CleanersNextDoor
                                            </h1>
                                            <p>
                                                Convienent laundry pickup and delivery services.&nbsp;
                                                <Link to="/how-it-works">Learn more.</Link> 
                                            </p>
                                            <ol className="fa-ul list-unstyled">
                                                <li className="mb-3">
                                                    <span className="fa-li">
                                                        <IconContext.Provider
                                                            value={{ className: 'text-primary' }}>
                                                            <span>
                                                                <FaCheckCircle />
                                                            </span>
                                                        </IconContext.Provider>
                                                    </span>
                                                    <strong className="mb-2 ml-2">Quick and free sign‑up</strong>
                                                    <small className="d-block ml-4">Enter your basic information to create an account.</small>
                                                </li>
                                                <li className="mb-3">
                                                    <span className="fa-li">
                                                        <IconContext.Provider
                                                            value={{ className: 'text-primary' }}>
                                                            <span>
                                                                <FaCheckCircle />
                                                            </span>
                                                        </IconContext.Provider>
                                                    </span>
                                                    <strong className="mb-2 ml-2">Simple laundry services</strong>
                                                    <small className="d-block ml-4">Enter pick up and delivery information. It's that easy.</small>
                                                </li>
                                                <li className="mb-3">
                                                    <span className="fa-li">
                                                        <IconContext.Provider
                                                            value={{ className: 'text-primary' }}>
                                                            <span>
                                                                <FaCheckCircle />
                                                            </span>
                                                        </IconContext.Provider>
                                                    </span>
                                                    <strong className="mb-2 ml-2">Recurring services</strong>
                                                    <small className="d-block ml-4">Try our recurring services features for a set and forget automated experience.</small>
                                                </li>
                                            </ol>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="col-md-8 col-lg-6">
                            <div className="vh-100 d-flex align-items-stretch py-5">
                                <div className="container">
                                    <div className="row">
                                        <div className="col-md-9 col-lg-8 mx-auto">
                                            <h1 className="h3">Create your account</h1>
                                            <p>Already have an account? <Link to="/sign-in" className="text-decoration-none">Sign in.</Link></p>
                                            <form method="post" onSubmit={signUp}>

                                                <TextInput name="name"
                                                    placeholder={this.state.formControls.name.placeholder}
                                                    label={this.state.formControls.name.label}
                                                    value={this.state.formControls.name.value}
                                                    onChange={this.changeHandler}
                                                    touched={this.state.formControls.name.touched ? 1 : 0}
                                                    valid={this.state.formControls.name.valid ? 1 : 0}
                                                    errors={this.state.formControls.name.errors} />

                                                <TextInput name="phone"
                                                    type="tel"
                                                    placeholder={this.state.formControls.phone.placeholder}
                                                    label={this.state.formControls.phone.label}
                                                    value={this.state.formControls.phone.value}
                                                    onChange={this.changeHandler}
                                                    touched={this.state.formControls.phone.touched ? 1 : 0}
                                                    valid={this.state.formControls.phone.valid ? 1 : 0}
                                                    errors={this.state.formControls.phone.errors} />

                                                <TextInput name="email"
                                                    type="email"
                                                    placeholder={this.state.formControls.email.placeholder}
                                                    label={this.state.formControls.email.label}
                                                    value={this.state.formControls.email.value}
                                                    onChange={this.changeHandler}
                                                    onBlur={this.checkEmailBlur}
                                                    touched={this.state.formControls.email.touched ? 1 : 0}
                                                    valid={this.state.formControls.email.valid ? 1 : 0}
                                                    errors={this.state.formControls.email.errors} />

                                                <TextInput name="password"
                                                    type="password"
                                                    placeholder={this.state.formControls.password.placeholder}
                                                    label={this.state.formControls.password.label}
                                                    value={this.state.formControls.password.value}
                                                    onChange={this.changeHandler}
                                                    touched={this.state.formControls.password.touched ? 1 : 0}
                                                    valid={this.state.formControls.password.valid ? 1 : 0}
                                                    onBlur={this.checkPasswordMatch}
                                                    errors={this.state.formControls.password.errors} />

                                                <TextInput name="confirmPassword"
                                                    type="password"
                                                    placeholder={this.state.formControls.confirmPassword.placeholder}
                                                    label={this.state.formControls.confirmPassword.label}
                                                    value={this.state.formControls.confirmPassword.value}
                                                    onChange={this.changeHandler}
                                                    touched={this.state.formControls.confirmPassword.touched ? 1 : 0}
                                                    valid={this.state.formControls.confirmPassword.valid ? 1 : 0}
                                                    onBlur={this.checkPasswordMatch}
                                                    errors={this.state.formControls.confirmPassword.errors} />

                                                <button className="btn btn-primary btn-block" type="submit" disabled={!this.state.formIsValid}>
                                                    Create your account
                                                </button>
                                            </form>
                                            
                                        </div>
                                    </div>
                                </div>
                             </div>
                        </div>
                    </div>
                </div>
            </div>
            )
    }
}