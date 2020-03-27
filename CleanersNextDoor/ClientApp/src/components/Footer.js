import React, { Component } from 'react';
import { Link } from 'react-router-dom';
export class Footer extends Component {
    static displayName = Footer.name;

    constructor(props) {
        super(props);
    }
    render() {
        return (
            <div className="container">
                <footer className="pt-4 my-md-5 pt-md-5 border-top">
                    <div className="row">
                        <div className="col-12 col-md">
                            <img className="mb-2" src="/docs/4.4/assets/brand/bootstrap-solid.svg" alt="" width="24" height="24" />
                            <small className="d-block mb-3 text-muted">&copy; 2017-2019</small>
                        </div>
                        <div className="col-6 col-md">
                            <h5>Services</h5>
                            <ul className="list-unstyled text-small">
                                <li><a className="text-muted" href="#">Laundry</a></li>
                                <li><a className="text-muted" href="#">Alteration Services</a></li>
                                <li><a className="text-muted" href="#">Suit & Jacket Cleaning</a></li>
                                <li><a className="text-muted" href="#">Dress Cleaning</a></li>
                            </ul>
                        </div>
                        <div className="col-6 col-md">
                            <h5>About</h5>
                            <ul className="list-unstyled text-small">
                                <li><a className="text-muted" href="#">How it Works</a></li>
                            </ul>
                        </div>
                        <div className="col-6 col-md">
                            <h5>About</h5>
                            <ul className="list-unstyled text-small">
                                <li><a className="text-muted" href="#">Sign In</a></li>
                                <li><a className="text-muted" href="#">Sign Up</a></li>
                                <li><a className="text-muted" href="#">Locations</a></li>
                            </ul>
                        </div>
                    </div>
                </footer>
            </div>
        )
    }
}