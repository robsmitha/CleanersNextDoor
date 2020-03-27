import React, { Component } from 'react';
import { Link } from 'react-router-dom';

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
            .then(data => this.setState({ members: data, loading: false }))
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading Merchants...</em></p>
            : Home.renderMerchantList(this.state.members);
        return (
            <div className="container">
                <h1>
                    Merchants
                </h1>
                <hr />
                {contents}
            </div>
        )
    }

    static renderMerchantList(merchants) {
        return (
            <div>
                <div className="row">
                    {merchants.map(m =>
                        <div key={m.id} className="col-md-3">
                            <Link className="text-decoration-none" to={'merchant/:id'.replace(':id', m.id)}>
                                <div className="card h-100">
                                    <img className="card-img-top img-fluid" src="http://placehold.it/350x300" />
                                    <div className="card-body p-2">
                                        <h5 className="display-5 text-dark font-weight-light">
                                            {m.name}
                                        </h5>
                                        <p className="lead text-muted">
                                            {m.description}
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