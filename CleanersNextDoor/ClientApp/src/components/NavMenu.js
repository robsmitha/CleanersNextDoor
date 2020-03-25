import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, Button } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { Authentication } from '../services/authentication'

export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true,
            authenticated: Authentication.getUserId() > 0
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    render() {
        return (
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow" dark color="dark">
                    <Container>
                        <NavbarBrand tag={Link} to="/">CleanersNextDoor</NavbarBrand>
                        <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                        <Collapse isOpen={!this.state.collapsed} navbar>
                            <ul className="navbar-nav">
                                <NavItem>
                                    <NavLink tag={Link} to="/">Home</NavLink>
                                </NavItem>
                            </ul>
                            <ul className="navbar-nav ml-auto">
                                <NavItem id="nav_sign_in" hidden={this.state.authenticated}>
                                    <NavLink tag={Link} to="/sign-in">Sign in</NavLink>
                                </NavItem>
                                <NavItem id="nav_sign_up" hidden={this.state.authenticated}>
                                    <NavLink tag={Link} to="/sign-up">Sign up</NavLink>
                                </NavItem>
                                <NavItem id="nav_profile" hidden={!this.state.authenticated}>
                                    <NavLink tag={Link} to="/profile">Profile</NavLink>
                                </NavItem>
                                <NavItem id="nav_sign_out" hidden={!this.state.authenticated}>
                                    <NavLink tag={Link} to="/sign-out">Sign out</NavLink>
                                </NavItem>
                            </ul>
                        </Collapse>
                    </Container>
                </Navbar>
            </header>
        );
    }
}
