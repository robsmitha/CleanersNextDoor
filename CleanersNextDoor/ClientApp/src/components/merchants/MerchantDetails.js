import React, { Component } from 'react';

export class MerchantDetails extends Component {
    constructor(props) {
        super(props)
        this.state = {
            merchant: null,
            loading: true
        }
    }

    componentDidMount() {
        this.populateMerchantInformation()
    }
    static renderMerchant(merchant) {
        return (
            <div>
                {merchant.name}
            </div>
        )
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : MerchantDetails.renderMerchant(this.state.merchant);
        return (
            <div className="container">
                <h1>Merchant</h1>
                {contents}
            </div>
        )
    }

    async populateMerchantInformation() {
        const merchantId = this.props.match.params.id
        const response = await fetch(`merchants/${merchantId}`);
        const data = await response.json();
        this.setState({ merchant: data, loading: false });
    }
}