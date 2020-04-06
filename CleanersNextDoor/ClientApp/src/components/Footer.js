import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { GiHouse } from "react-icons/gi";

export class Footer extends Component {
    static displayName = Footer.name;

    constructor(props) {
        super(props);
        this.state = {
            authenticated: props.currentUser != null
        }
    }
    componentDidMount() {

    }
    render() {
        return (
            <div>
                <footer className="bg-light pt-4 my-md-5 pt-md-5 border-top">
                    <div className="container">
                        <div className="row">
                            <div className="col-12 col-md">
                                <GiHouse />
                                <small className="mb-3 text-muted">
                                    &nbsp;CleanersNextDoor
                                &copy;{new Date().getFullYear()}
                                </small>
                            </div>
                            <div className="col-6 col-md">
                                <h5>Services</h5>
                                <ul className="list-unstyled text-small">
                                    <li><Link className="text-muted" to="/">Laundry</Link></li>
                                    <li><Link className="text-muted" to="/">Alteration Services</Link></li>
                                    <li><Link className="text-muted" to="/">Suit & Jacket Cleaning</Link></li>
                                    <li><Link className="text-muted" to="/">Dress Cleaning</Link></li>
                                </ul>
                            </div>
                            <div className="col-6 col-md">
                                <h5>About</h5>
                                <ul className="list-unstyled text-small">
                                    <li><Link className="text-muted" to="/">How it Works</Link></li>
                                    <li><Link className="text-muted" to="/">Become a Driver</Link></li>
                                </ul>
                            </div>
                            <div className="col-6 col-md">
                                <h5>Businesses</h5>
                                <ul className="list-unstyled text-small">
                                    <li><Link className="text-muted" to="/users/sign-in" id="nav_sign_in" hidden={this.state.userAuthenticated}>Merchant Sign In</Link></li>
                                    <li><Link className="text-muted" to="/users/sign-up" id="nav_sign_up" hidden={this.state.userAuthenticated}>Become a Merchant</Link></li>
                                    <li><Link className="text-muted" to="/users/profile" id="nav_profile" hidden={!this.state.userAuthenticated}>Profile</Link></li>
                                    <li><Link className="text-muted" to="/users/sign-out" id="nav_sign_out" hidden={!this.state.userAuthenticated}>Sign out</Link></li>
                                    <li><Link className="text-muted" to="/">Locations</Link></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </footer>
            </div>
        )
    }
}