import React, { Component } from 'react';
import { Link } from 'react-router-dom';

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
                <div className="container">

                    <div className="row align-items-center my-5">
                        <div className="col-lg-7">
                            <img className="img-fluid rounded mb-4 mb-lg-0" src="http://placehold.it/900x400" alt="" />
                        </div>

                        <div className="col-lg-5">
                            <h1 className="font-weight-light">{merchant.name}</h1>
                            <p>{merchant.description}</p>
                            <Link to={'/request-service/:id'.replace(':id', merchant.id)} className="btn btn-primary">
                                Request Service
                            </Link>
                        </div>
                    </div>
                    <div className="card text-white bg-secondary my-5 py-4 text-center">
                        <div className="card-body">
                            <p className="text-white m-0">
                                Experienced neighborhood laundry and alteration services.
                            </p>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md-4 mb-5">
                            <div className="card h-100">
                                <div className="card-body">
                                    <h2 className="card-title">Laundry</h2>
                                    <p className="card-text">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Rem magni quas ex numquam, maxime minus quam molestias corporis quod, ea minima accusamus.</p>
                                </div>
                            </div>
                        </div>
                        <div className="col-md-4 mb-5">
                            <div className="card h-100">
                                <div className="card-body">
                                    <h2 className="card-title">Alterations</h2>
                                    <p className="card-text">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Quod tenetur ex natus at dolorem enim! Nesciunt pariatur voluptatem sunt quam eaque, vel, non in id dolore voluptates quos eligendi labore.</p>
                                </div>
                            </div>
                        </div>
                        <div className="col-md-4 mb-5">
                            <div className="card h-100">
                                <div className="card-body">
                                    <h2 className="card-title">Other</h2>
                                    <p className="card-text">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Rem magni quas ex numquam, maxime minus quam molestias corporis quod, ea minima accusamus.</p>
                                </div>
                            </div>
                        </div>

                    </div>


                </div>

               
            </div>
        )
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : MerchantDetails.renderMerchant(this.state.merchant);
        return (
            <div>
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