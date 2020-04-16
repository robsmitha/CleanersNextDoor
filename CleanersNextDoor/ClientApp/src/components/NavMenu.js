import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { GiHouse } from "react-icons/gi";
import { FaSignOutAlt, FaUser, FaUserPlus, FaSignInAlt } from 'react-icons/fa'
import { AuthConsumer } from './../context/AuthContext'

export class NavMenu extends Component {
    static displayName = NavMenu.name;
    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);

        this.state = {
            collapsed: true
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    componentDidMount() {
        this.configureUI()
    }

    configureUI() {
        var navbar = document.querySelector('.main-nav').clientHeight;
        document.querySelector('.header').style = 'padding-top: ' + navbar + 'px';
    }
    render() {
        return (
            <header className="header">
                <AuthConsumer>
                    {({ authenticated }) => (
                        <header>
                            <Navbar className="main-nav navbar-expand-sm navbar-toggleable-sm box-shadow fixed-top bg-white border-bottom shadow-sm navbar-light">
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
                                            <NavItem>
                                                <NavLink tag={Link} to="/how-it-works">How it works</NavLink>
                                            </NavItem>
                                        </ul>
                                        <ul className="navbar-nav ml-auto">
                                            <NavItem id="nav_customer_sign_up" hidden={authenticated}>
                                                <NavLink tag={Link} to="/sign-up">
                                                    <FaUserPlus />&nbsp;Sign up
                                            </NavLink>
                                            </NavItem>
                                            <NavItem id="nav_customer_sign_in" hidden={authenticated}>
                                                <NavLink tag={Link} to="/sign-in">
                                                    <FaSignInAlt />&nbsp;Sign in
                                                </NavLink>
                                            </NavItem>
                                            <NavItem id="nav_customer_profile" hidden={!authenticated}>
                                                <NavLink tag={Link} to="/profile">
                                                    <FaUser />&nbsp;Account
                                                </NavLink>
                                            </NavItem>
                                            <NavItem id="nav_customer_sign_out" hidden={!authenticated}>
                                                <NavLink tag={Link} to="/sign-out">
                                                    <FaSignOutAlt />&nbsp;Sign out
                                                </NavLink>
                                            </NavItem>
                                        </ul>
                                    </Collapse>
                                </Container>
                            </Navbar>
                        </header>
                    )}
                </AuthConsumer>
            </header>
            )
    }
}
