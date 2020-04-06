import React, { Component } from 'react';
import { authenticationService } from '../../services/authentication.service'

export class UserSignOut extends Component {
    componentDidMount() {
        this.signOut()
    }

    signOut = () => {
        authenticationService.logout();
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