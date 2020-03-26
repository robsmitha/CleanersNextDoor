import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export class MerchantList extends Component {
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
            : MerchantList.renderMerchantList(this.state.members);
        return (
            <div className="container">
                <h1>
                    Merchants
                </h1>
                {contents}
            </div>
        )
    }

    static renderMerchantList(merchants) {
        return (
            <div>
                {merchants.map(m =>
                    <div key={m.id}>
                        <Link to={'merchant/:id'.replace(':id', m.id)}>
                            {m.name}
                        </Link>
                    </div>
                )}
            </div>
        )
    }
}