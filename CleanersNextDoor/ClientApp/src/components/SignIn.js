import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { AuthConsumer } from './../context/AuthContext'
import { FaHome } from 'react-icons/fa'
import TextInput from './../helpers/TextInput';
import handleChange from './../helpers/HandleChange';

export class SignIn extends Component {

    constructor(props) {
        super(props);

        this.state = {
            formIsValid: false,
            staySignedIn: true,
            signInWithPhone: false,
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
        this.checkHandler = this.checkHandler.bind(this);
    }

    changeHandler = event => {
        const name = event.target.name;
        const value = event.target.value;
        this.setState(handleChange(name, value, this.state.formControls));
    }

    checkHandler(event) {
        const name = event.target.name;
        const value = event.target.checked;
        this.setState({
            [name]: value
        });
    }

    toggleSignInWithPhone = (event) => {
        var signInWithPhone = !this.state.signInWithPhone;
        event.target.innerHTML = signInWithPhone ? 'Sign in with password' : 'Sign in with phone number instead';
        this.setState({ signInWithPhone: signInWithPhone })
    }

    render() {
        return (
            <div>
                <AuthConsumer>
                    {({ authenticated, signIn }) => (
                        <div>
                            {authenticated
                                ? <Redirect to='/account' />
                                : this.renderSignInForm(signIn)}
                        </div>
                    )}
                </AuthConsumer>
            </div>
            )
    }

    renderSignInForm(signIn) {
        return (

            <div className="vh-100 d-flex align-items-stretch py-5">
                <div className="container">
                    <div className="row">
                        <div className="col-sm-9 col-md-7 col-lg-5 mx-auto">
                            <h1 className="h3 mb-4 text-center">
                                <FaHome /> CleanersNextDoor
                            </h1>
                            <h2 className="h4 mb-2">Sign in to your account</h2>
                            <form method="post" onSubmit={signIn}>
                                <TextInput name="email"
                                    type="email"
                                    placeholder={this.state.formControls.email.placeholder}
                                    label={this.state.formControls.email.label}
                                    value={this.state.formControls.email.value}
                                    onChange={this.changeHandler}
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
                                    errors={this.state.formControls.password.errors}
                                    hidden={this.state.signInWithPhone} />


                                <div className="custom-control custom-checkbox" hidden={this.state.signInWithPhone}>
                                    <input type="checkbox" className="custom-control-input" id="staySignedIn" name="staySignedIn" onChange={this.checkHandler} checked={this.state.staySignedIn} />
                                    <label className="custom-control-label" htmlFor="staySignedIn">
                                        Stay signed in
                                    </label>
                                </div>

                                <button className="btn btn-primary btn-block my-3" type="submit" disabled={!this.state.formIsValid}>Continue</button>
                                <button type="button" className="btn btn-link btn-block text-decoration-none" onClick={this.toggleSignInWithPhone}>
                                    Sign in with phone number instead
                                </button>
                            </form>
                            <p className="text-center mt-3">
                                Don't have an account? <Link to="/sign-up" className="text-decoration-none">Sign up.</Link>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            )
    }
}