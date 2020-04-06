import React, { Component } from 'react';
import { authenticationService } from './../services/authentication.service'

const AuthContext = React.createContext();

class AuthProvider extends Component {

    state = { isAuth: false }

    constructor(props) {
        super(props)
        this.customerLogin = this.customerLogin.bind(this)
        this.customerLogout = this.customerLogout.bind(this)
    }

    componentDidMount() {
        authenticationService.currentUser.subscribe(c => {
            if (c && c.token) {
                //todo: validate token
                this.setState({ isAuth: true })
            }
        })
    }

    customerLogin = (event) => {
        event.preventDefault();
        let email = event.target.elements["email"].value;
        let password = event.target.elements["password"].value;
        authenticationService.authenticateCustomer(email, password)
            .then(c => {
                let auth = c.token && c.token.length > 0;
                this.setState({
                    isAuth: auth,
                    msg: auth ? '' : 'The entered email or password is not valid.'
                })
            })
    }

    customerSignUp = (event) => {
        event.preventDefault();
        var data = {
            password: event.target.elements["email"].value,
            firstName: event.target.elements["password"].value,
            lastName: event.target.elements["firstName"].value,
            email: event.target.elements["lastName"].value,
            phone: event.target.elements["phone"].value
        };
        authenticationService.createCustomer(data)
            .then(c => {
                let auth = c.id > 0;
                this.setState({
                    isAuth: auth,
                    msg: auth ? '' : 'An error occurred.'
                })
            })
    }


    customerLogout() {
        authenticationService.customerLogout();
        this.setState({ isAuth: false })
    }

    checkAuth() {
        authenticationService.currentUser
            .subscribe(x => this.setState({ user: x, isAuth: x !== undefined }));
    }

    render() {
        return (
            <AuthContext.Provider value={{
                isAuth: this.state.isAuth,
                customerLogin: this.customerLogin,
                customerLogout: this.customerLogout,
                customerSignUp: this.customerSignUp,
                msg: this.state.msg
            }}>
                {this.props.children}
            </AuthContext.Provider>
        )
    }
}

const AuthConsumer = AuthContext.Consumer

export { AuthProvider, AuthConsumer }