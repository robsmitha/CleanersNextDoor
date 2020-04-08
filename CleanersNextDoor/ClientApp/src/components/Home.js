import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { AuthConsumer } from './../context/AuthContext';

export class Home extends Component {
    constructor(props) {
        super(props)
        this.state = {
            merchants: [],
            loading: true
        }
    }

    componentDidMount() {
        this.fetchMerchants()
    }

    fetchMerchants() {
        fetch('merchants/GetMerchants')
            .then(response => response.json())
            .then(data => this.setState({ merchants: data, loading: false }))
            .catch(() => { console.log('oops') })
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading Merchants...</em></p>
            : Home.renderMerchantList(this.state.merchants)
        return (
            <div>
                <header className="bg-primary py-3 mb-5 shadow">
                    <div className="container h-100">
                        <div className="row h-100 align-items-center">
                            <div className="col-lg-12">
                                <h1 className="display-4 text-white mt-5 mb-2">
                                    A Warm Welcome!
                                </h1>
                                <p className="lead text-white-50">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ipsa, ipsam, eligendi, in quo sunt possimus non incidunt odit vero aliquid similique quaerat nam nobis illo aspernatur vitae fugiat numquam repellat.
                                </p>
                                <AuthConsumer>
                                    {({ authenticated }) => (
                                        <div>
                                            <div hidden={authenticated}>
                                                <Link to="/customer/sign-in" className="btn btn-success btn-lg mr-2">Sign in</Link>
                                                <Link to="/customer/sign-up" className="btn btn-secondary btn-lg">Sign up</Link>
                                            </div>
                                            <div hidden={!authenticated}>
                                                <Link to="/customer/profile" className="btn btn-success btn-lg">My Account</Link>
                                            </div>
                                        </div>
                                    )}
                                </AuthConsumer>
                                
                            </div>
                        </div>
                    </div>
                </header>
                <div className="container">
                    <h2 className="border-bottom mb-2">How This Works</h2>
                    <div className="row mb-3">
                        <div className="col-md-8">
                            <p>
                                Select from many merchant services such as laundry, alterations and more. <br />
                                Enter pick up and delivery information. <strong>It's that easy.</strong>
                            </p>
                        </div>
                        <div className="col-md-4">
                            <Link to="/how-it-works" className="btn btn-lg btn-primary btn-block">Learn more</Link>
                        </div>
                    </div>
                </div>
                <div className="container">
                    <h2 className="border-bottom mb-2">Available Merchants</h2>
                </div>
                <section>
                    <div className="container">
                        {contents}
                    </div>
                </section>
            </div>
        )
    }

    static renderMerchantList(merchants) {
        return (
            <div>
                <p>
                    <strong>Select a Merchant</strong> to browse and select services.
                </p>
                <div className="row text-center">
                    {merchants.map(m =>
                        <div key={m.id} className="col-lg-4 col-md-6 mb-4">
                            <Link className="text-decoration-none" to={'merchant/:id'.replace(':id', m.id)}>
                                <div className="card h-100 shadow rounded-0">
                                    <div className="card-body text-dark">
                                        <h4 className="card-title">{m.name}</h4>
                                        <small className="text-muted">
                                            {m.street1}
                                            <br />
                                            {m.city}, {m.stateAbbreviation}. {m.zip}
                                        </small>
                                        <hr className="w-25" />
                                        <p className="card-text text-muted mb-1">
                                            {m.shortDescription}
                                        </p>
                                    </div>
                                </div>
                            </Link>
                        </div>
                    )}
                </div>
            </div>
        )
    }
}