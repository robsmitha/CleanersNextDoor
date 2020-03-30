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
        Authentication.clearSession();
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