import React, { Component } from 'react';
import { Authentication } from '../../services/authentication'

export class UserSignOut extends Component {
    constructor(props) {
        super(props)
    }

    componentDidMount() {
        this.signOut()
    }

    signOut = () => {
        Authentication.clearLocalStorage();
        document.getElementById('nav_sign_in').hidden = false;
        document.getElementById('nav_sign_up').hidden = false;
        document.getElementById('nav_profile').hidden = true;
        document.getElementById('nav_sign_out').hidden = true;
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