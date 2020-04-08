import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { AuthConsumer } from './../../context/AuthContext'

export class MerchantDetails extends Component {
    constructor(props) {
        super(props)
        this.state = {
            merchantId: this.props.match.params.id,
            merchant: null,
            merchantLoading: true,
            itemsLoading: true,
            items: []
        }
    }

    componentDidMount() {
        this.populateMerchantInformation()
        this.populateMerchantItems()
    }

    populateMerchantInformation() {
        fetch(`merchants/${this.state.merchantId}`)
            .then(response => response.json())
            .then(data => this.setState({ merchant: data, merchantLoading: false }))
    }
    
    populateMerchantItems() {
        fetch(`merchants/${this.state.merchantId}/items`)
            .then(response => response.json())
            .then(data => this.setState({ items: data, itemsLoading: false }))
    }

    render() {
        let merchantContents = this.state.merchantLoading
            ? <p><em>Loading Merchant...</em></p>
            : this.renderMerchant(this.state.merchant);
        let itemContents = this.state.itemsLoading
            ? <p><em>Loading Services...</em></p>
            : this.renderMerchantItems(this.state.items);
        return (
            <div>
                {merchantContents}
                <section>
                    <div className="my-3">
                        {itemContents}
                    </div>
                </section>
            </div>
        )
    }
    renderMerchant(merchant) {
        return (
            <div>
                <header className="bg-primary py-3 mb-5">
                    <div className="container h-100">
                        <div className="row h-100 align-items-center">
                            <div className="col-lg-12">
                                <h1 className="display-4 text-white mt-5 mb-2">
                                    {merchant.name}
                                </h1>
                                <p className="lead text-white-50">{merchant.shortDescription}</p>

                                <AuthConsumer>
                                    {({ authenticated }) => (
                                        <div>
                                            {authenticated
                                                ? <Link to={'/request-service/:id'.replace(':id', merchant.id)} className="btn btn-success btn-lg mb-3" hidden={!authenticated}>
                                                        Start pickup request
                                                    </Link>
                                                : <Link to="/customer/sign-in" className="btn btn-success btn-lg mb-3" hidden={authenticated}>
                                                        Sign in to use service
                                                    </Link>}
                                        </div>
                                    )}
                                </AuthConsumer>
                                
                            </div>
                        </div>
                    </div>
                </header>
                <div className="container">
                    <div className="row">
                        <div className="col-md-8 mb-3">
                            <h2 className="border-bottom mb-2">About</h2>
                            <p className="lead">
                                {merchant.description}
                            </p>
                        </div>
                        <div className="col-md-4 mb-3">
                            <h2 className="border-bottom mb-2">Contact Us</h2>
                            <address>
                                <strong>{merchant.name}</strong>
                                <br />
                                {merchant.street1}
                                <br />
                                {merchant.city}, {merchant.stateAbbreviation}. {merchant.zip}
                            </address>
                            <address>
                                <abbr title="Hours">H:</abbr>
                                {merchant.operatingHours}
                                <br />
                                <abbr title="Phone">P:</abbr>
                                {merchant.phone}
                                <br />
                                <abbr title="Email">E:</abbr>
                                <a href={'mailto:{0}'.replace('{0}', merchant.contactEmail)}>{merchant.contactEmail}</a>
                            </address>
                        </div>
                    </div>
                    <div className="card text-white bg-secondary my-3 py-4 text-center">
                        <div className="card-body">
                            <p className="text-white m-0">
                                {merchant.callToAction}
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
    renderMerchantItems(items) {
        return (
            <div className="container">
                <h2 className="border-bottom mb-2">Available Services</h2>
                <AuthConsumer>
                    {({ authenticated }) => (
                        <div>
                            {authenticated
                                ? <p>Browse services offered by merchant. <Link to={'/request-service/:id'.replace(':id', this.state.merchantId)}>Start a pick up request!</Link></p>
                                : <p><strong>Please sign in</strong> to start a pick up request.</p>}
                            <div className="row">
                                {items.map(i =>
                                    <div key={i.id} className="col-md-4 mb-4">
                                        <Link className="text-decoration-none" to={authenticated ? '/request-service/:id'.replace(':id', this.state.merchantId) : '/customer/sign-in'}>
                                            <div className="card h-100 shadow">
                                                <div className="card-body">
                                                    <div className="d-flex w-100 justify-content-between text-dark">
                                                        <h5 className="mb-1"> {i.name}</h5>
                                                        <small>{i.displayPrice}</small>
                                                    </div>
                                                    <p className="mb-1 text-muted">{i.description}</p>
                                                    <small className="text-dark">Max Allowed: {i.maxAllowed}</small>
                                                </div>
                                            </div>
                                        </Link>
                                    </div>
                                )}
                            </div>
                        </div>
                    )}
                </AuthConsumer>
            </div>
        )
    }

}