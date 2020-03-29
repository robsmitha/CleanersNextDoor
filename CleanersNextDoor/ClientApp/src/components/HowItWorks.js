import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Authentication } from '../services/authentication';

export class HowItWorks extends Component {
    constructor(props) {
        super(props)
        this.state = {
            authenticated: Authentication.getCustomerId() > 0
        }
    }
    render() {
        return (
            <div>
                <h1>How It Works</h1>
            </div>
            )
    }
}