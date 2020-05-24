import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, UncontrolledDropdown, DropdownMenu, DropdownItem, DropdownToggle } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { FaSignOutAlt, FaUser, FaUserPlus, FaSignInAlt, FaCompass, FaAddressCard, FaHistory, FaShoppingCart } from 'react-icons/fa'
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
                                <Container fluid>
                                    <NavbarBrand tag={Link} to="/">
                                        <span className="h4">
                                            <FaCompass />
                                        </span>
                                    </NavbarBrand>
                                    <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                                    <Collapse isOpen={!this.state.collapsed} navbar>
                                        <ul className="navbar-nav">
                                            <NavItem>
                                                <NavLink tag={Link} to="/">Merchants</NavLink>
                                            </NavItem>
                                            <NavItem>
                                                <NavLink tag={Link} to="/how-it-works">How it works</NavLink>
                                            </NavItem>
                                        </ul>
                                        <ul className="navbar-nav ml-auto">
                                            <NavItem hidden={authenticated}>
                                                <NavLink tag={Link} to="/sign-up">
                                                    <FaUserPlus />&nbsp;Sign up
                                            </NavLink>
                                            </NavItem>
                                            <NavItem hidden={authenticated}>
                                                <NavLink tag={Link} to="/sign-in">
                                                    <FaSignInAlt />&nbsp;Sign in
                                                </NavLink>
                                            </NavItem>
                                                    
                                            <UncontrolledDropdown nav inNavbar hidden={!authenticated}>
                                                <DropdownToggle nav caret>
                                                    <FaUser /> Account
                                                </DropdownToggle>
                                                <DropdownMenu right className="animate slideIn">
                                                    <DropdownItem tag={Link} to="/account">
                                                        <FaAddressCard />&nbsp;My account
                                                    </DropdownItem>
                                                    <DropdownItem tag={Link} to="/order-history">
                                                        <FaHistory />&nbsp;Orders
                                                    </DropdownItem>
                                                    <DropdownItem divider />
                                                    <DropdownItem tag={Link} to="/sign-out">
                                                        <FaSignOutAlt />&nbsp;Sign out
                                                    </DropdownItem>
                                                </DropdownMenu>
                                            </UncontrolledDropdown>
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
