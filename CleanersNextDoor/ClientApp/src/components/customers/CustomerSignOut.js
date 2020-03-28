import React, { Component } from 'react';
import { Authentication } from '../../services/authentication'

export class CustomerSignOut extends Component {
    constructor(props) {
        super(props)
    }

    componentDidMount() {
        this.signOut()
    }

    signOut = () => {
        Authentication.clearCustomerLocalStorage();
        document.getElementById('nav_customer_sign_in').hidden = false;
        document.getElementById('nav_customer_sign_up').hidden = false;
        document.getElementById('nav_customer_profile').hidden = true;
        document.getElementById('nav_customer_sign_out').hidden = true;
        this.props.history.push('/')
    }

    render() {
        return (
            <div className="container">
                <h1>Signing you out..</h1>
            </div>
        )
    }
}