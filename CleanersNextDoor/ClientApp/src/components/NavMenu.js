import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, Button } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { Authentication } from '../services/authentication'
import { GiHouse } from "react-icons/gi";

export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true,
            customerAuthenticated: Authentication.getCustomerId() > 0
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    componentDidMount() {
        document.addEventListener('DOMContentLoaded', function () {
            var navbar = document.querySelector('#main_nav').clientHeight;
            document.querySelector('body').style = 'padding-top: ' + navbar + 'px';
        }, false);
    }

    render() {
        return (
            <header>
                <Navbar id="main_nav" className="navbar-expand-sm navbar-toggleable-sm box-shadow fixed-top bg-white border-bottom shadow-sm navbar-light">
                    <Container>
                        <NavbarBrand tag={Link} to="/">
                            <GiHouse />
                            &nbsp;
                            CleanersNextDoor
                        </NavbarBrand>
                        <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                        <Collapse isOpen={!this.state.collapsed} navbar>
                            <ul className="navbar-nav">
                                <NavItem>
                                    <NavLink tag={Link} to="/">Home</NavLink>
                                </NavItem>
                            </ul>
                            <ul className="navbar-nav ml-auto">
                                <NavItem id="nav_customer_sign_in" hidden={this.state.customerAuthenticated}>
                                    <NavLink tag={Link} to="/customers/sign-in">Sign in</NavLink>
                                </NavItem>
                                <NavItem id="nav_customer_sign_up" hidden={this.state.customerAuthenticated}>
                                    <NavLink tag={Link} to="/customers/sign-up">Sign up</NavLink>
                                </NavItem>
                                <NavItem id="nav_customer_profile" hidden={!this.state.customerAuthenticated}>
                                    <NavLink tag={Link} to="/customers/profile">Profile</NavLink>
                                </NavItem>
                                <NavItem id="nav_customer_sign_out" hidden={!this.state.customerAuthenticated}>
                                    <NavLink tag={Link} to="/customers/sign-out">Sign out</NavLink>
                                </NavItem>
                            </ul>
                        </Collapse>
                    </Container>
                </Navbar>
            </header>
        );
    }
}
