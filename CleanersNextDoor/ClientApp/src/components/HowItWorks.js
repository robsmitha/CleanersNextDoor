import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export class HowItWorks extends Component {
    constructor(props) {
        super(props)
    }

    componentDidMount() {

    }

    render() {
        return (
            <div>
                <header className="bg-primary py-3 mb-5">
                    <div className="container h-100">
                        <div className="row h-100 align-items-center">
                            <div className="col-lg-12">
                                <h1 className="display-4 text-white mt-5 mb-2">
                                    How it Works
                                </h1>
                                <p className="lead text-white-50">
                                    Select from many merchant services such as laundry, alterations and more. <br />
                                    Enter pick up and delivery information. <strong>It's that easy.</strong>
                                </p>
                                <Link to="/" className="btn btn-success btn-lg">Try it out</Link>
                            </div>
                        </div>
                    </div>
                </header>
                <div className="container">
                    <div className="my-3">
                        <h3 className="border-bottom">You Need a Laundry Service</h3>
                        <p>
                            Laundry errands can be a hassle.
                            Our pick up and delivery services make it easy.
                        </p>
                    </div>
                    <div className="my-3">

                        <h3 className="border-bottom">Merchants Provide Services</h3>
                        <p>
                            Dry cleaning, alterations, suit cleanings and more.
                        </p>
                        <Link to="/" className="text-decoration-none">
                            Browse Merchants
                        </Link>

                    </div>
                    <div className="my-3">

                        <h3 className="border-bottom">We Bring the Service to You</h3>
                        <p>
                            Schedule a pick up and delivery that works for your schedule. We will make it happen.
                        </p>
                    </div>
                </div>
            </div>
            )
    }
}