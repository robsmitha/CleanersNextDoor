import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { AuthConsumer } from './../context/AuthContext';
import { merchantService } from '../services/merchant.service';
import { Badge } from 'reactstrap';

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
        merchantService.getMerchants()
            .then(data => this.setState({ merchants: data, loading: false }))
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
                                <h1 className="text-white display-4 mt-5 mb-2">
                                    Welcome!
                                </h1>
                                <p className="lead text-white-50">
                                    Choose local services such as laundry, alterations and more.
                                </p>
                                <AuthConsumer>
                                    {({ authenticated }) => (
                                        <div>
                                            <div hidden={authenticated}>
                                                <Link to="/sign-in" className="btn btn-success btn-lg mr-2">Sign in</Link>
                                                <Link to="/sign-up" className="btn btn-secondary btn-lg">Sign up</Link>
                                            </div>
                                            <div hidden={!authenticated}>
                                                <Link to="/account" className="btn btn-success btn-lg">My Account</Link>
                                            </div>
                                        </div>
                                    )}
                                </AuthConsumer>
                            </div>
                        </div>
                    </div>
                </header>
                <div className="container">
                    <h2 className="mb-2">How this Works</h2>
                    <div className="row mb-3">
                        <div className="col-md-8">
                            <p>
                                Choose a Merchant.&nbsp;
                                Enter pick up / drop off address.&nbsp;
                                <strong>It's that easy.</strong>
                            </p>
                        </div>
                        <div className="col-md-4">
                            <Link to="/how-it-works" className="btn btn-primary btn-block">Learn more</Link>
                        </div>
                    </div>
                </div>
                <div className="container">
                    <h2 className="mb-2">Available Merchants</h2>
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
                <div className="row text-center">
                    {merchants.map(m =>
                        <div key={m.id} className="col-lg-4 col-md-6 mb-4">
                            <Link className="text-decoration-none" to={'merchant/:id'.replace(':id', m.id)}>
                                <div className="card h-100">
                                    <div className="card-body text-dark">
                                        <h5 className="card-title">{m.name}</h5>
                                        <small className="text-muted">
                                            {m.itemTypes.map((i, index) =>
                                                <span key={i.id}>
                                                    {i.name}
                                                    {index < m.itemTypes.length - 1 ? ', ' : ''}
                                                </span>
                                            )}
                                        </small>
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