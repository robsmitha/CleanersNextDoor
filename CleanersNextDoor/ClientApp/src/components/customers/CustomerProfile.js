import React, { Component } from 'react';
import { Authentication } from '../../services/authentication'

export class CustomerProfile extends Component {
    constructor(props) {
        super(props)
        this.state = {
            customer: null,
            loading: true
        }
    }

    componentDidMount() {
        this.populateProfileInformation()
    }
    static renderProfile(customer) {
        return (
            <div>
                {customer.email}<br />
                {customer.phone}
            </div>
        )
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : CustomerProfile.renderProfile(this.state.customer);
        return (
            <div className="container">
                Profile
                {contents}
            </div>
        )
    }

    async populateProfileInformation() {
        const customerId = Authentication.getCustomerId()
        const response = await fetch(`customers/${customerId}`);
        const data = await response.json();
        this.setState({ customer: data, loading: false });
    }
}