import React, { Component } from 'react';
import { authenticationService } from './../services/authentication.service'

const AuthContext = React.createContext();

class AuthProvider extends Component {

    state = {
        authenticated: false
    }

    constructor(props) {
        super(props)
        this.customerLogin = this.customerLogin.bind(this)
        this.customerLogout = this.customerLogout.bind(this)
    }

    componentDidMount() {
        authenticationService.appUser.subscribe(x => {
            if (x !== null && x.authenticated) {
                fetch('identity/authorize', { method: 'post' })
                    .then(response => response.json())
                    .then(data => {
                        this.setState({ authenticated: data !== null && data.authenticated })
                    })  
            }
        })
    }

    customerLogin = (event) => {
        event.preventDefault();
        let email = event.target.elements["email"].value;
        let password = event.target.elements["password"].value;
        authenticationService.authenticateCustomer(email, password)
            .then(x => {
                this.setState({
                    authenticated: x.authenticated
                })
                if (!this.state.authenticated)
                    alert('The entered email or password is not valid.')
            })
    }

    customerSignUp = (event) => {
        event.preventDefault();
        var data = {
            password: event.target.elements["password"].value,
            firstName: event.target.elements["firstName"].value,
            lastName: event.target.elements["lastName"].value,
            email: event.target.elements["email"].value,
            phone: event.target.elements["phone"].value
        };
        authenticationService.createCustomer(data)
            .then(x => {
                this.setState({
                    authenticated: x.authenticated
                })
                if (!this.state.authenticated)
                    alert('An error occurred.')
            })
    }


    customerLogout() {
        authenticationService.customerLogout();
        this.setState({ authenticated: false })
    }

    render() {
        return (
            <AuthContext.Provider value={{
                authenticated: this.state.authenticated,
                customerLogin: this.customerLogin,
                customerLogout: this.customerLogout,
                customerSignUp: this.customerSignUp
            }}>
                {this.props.children}
            </AuthContext.Provider>
        )
    }
}

const AuthConsumer = AuthContext.Consumer

export { AuthProvider, AuthConsumer }