import React, { Component } from 'react';
import { authenticationService } from './../services/authentication.service'

const AuthContext = React.createContext();

class AuthProvider extends Component {

    state = {
        authenticated: authenticationService.appUserValue !== null
            && authenticationService.appUserValue.authenticated
    }

    constructor(props) {
        super(props)
        this.signIn = this.signIn.bind(this)
        this.signOut = this.signOut.bind(this)
    }

    componentDidMount() {
        authenticationService.appUser.subscribe(x => {
            if (x !== null && x.authenticated) {
                authenticationService.authorize()
                    .then(data => {
                        let authenticated = data !== null && data.authenticated;
                        if (!authenticated) {
                            //client appUser was conflicting with server appUser, force remove
                            authenticationService.clearAppUser(); 
                        }
                        this.setState({ authenticated: authenticated })
                    })  
            }
        })
    }

    signIn = (event) => {
        event.preventDefault();
        //TODO: check sign in with phone is enabled

        let email = event.target.elements["email"].value;
        let password = event.target.elements["password"].value;

        authenticationService.signIn(email, password)
            .then(x => {
                this.setState({
                    authenticated: x.authenticated
                })
                if (!this.state.authenticated)
                    alert('The entered email or password is not valid.')
            })
    }

    signUp = (event) => {
        event.preventDefault();
        var data = {
            password: event.target.elements["password"].value,
            name: event.target.elements["name"].value,
            email: event.target.elements["email"].value,
            phone: event.target.elements["phone"].value
        };
        authenticationService.signUp(data)
            .then(x => {
                this.setState({
                    authenticated: x.authenticated
                })
                if (!this.state.authenticated)
                    alert('An error occurred.')
            })
    }


    signOut() {
        authenticationService.signOut();
        this.setState({ authenticated: false })
    }

    render() {
        return (
            <AuthContext.Provider value={{
                authenticated: this.state.authenticated,
                signIn: this.signIn,
                signOut: this.signOut,
                signUp: this.signUp
            }}>
                {this.props.children}
            </AuthContext.Provider>
        )
    }
}

const AuthConsumer = AuthContext.Consumer

export { AuthProvider, AuthConsumer }