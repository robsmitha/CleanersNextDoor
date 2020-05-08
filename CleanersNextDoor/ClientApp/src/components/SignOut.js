import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { AuthConsumer } from './../context/AuthContext'
import { FaHome, FaCompass } from 'react-icons/fa'

export class SignOut extends Component {
    render() {
        return (
            <div>
                <AuthConsumer>
                    {({ authenticated, signOut }) => (
                        <div>
                            {!authenticated
                                ? <Redirect to='/' />
                                : this.renderSignOut(signOut)}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        )
    }
    renderSignOut(signOut) {
        return (
            <div className="vh-100 d-flex align-items-stretch py-5">
                <div className="container">
                    <div className="row">
                        <div className="col-sm-9 col-md-7 col-lg-5 mx-auto">
                            <h1 className="h3 mb-4 text-center">
                                <FaCompass /> CleanersNextDoor
                            </h1>
                            <h2 className="h4 mb-2">Are you sure you want to sign out?</h2>
                            <button type="button" onClick={signOut} className="btn btn-primary btn-block my-3">
                                Yes, sign me out
                            </button>
                            <Link to='/account' className="btn btn-link btn-block text-decoration-none">
                                No, keep me signed in
                            </Link>
                        </div>
                    </div>
                </div>
            </div>
            )
    }
}