import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { AuthConsumer } from '../../context/AuthContext'

export class CustomerSignOut extends Component {
    render() {
        return (
            <div>
                <AuthConsumer>
                    {({ isAuth, customerLogout }) => (
                        <div>
                            {!isAuth
                                ? <Redirect to='/' />
                                : this.renderSignOut(customerLogout)}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        )
    }
    renderSignOut(customerLogout) {
        return (
            <div className="container">
                <h1>Are you sure?</h1>
                <button type="button" onClick={customerLogout} className="btn btn-primary">
                    Yes
                </button>
                <Link to='/customer/profile' className="btn btn-secondary">
                    No
                </Link>
            </div>
            )
    }
}