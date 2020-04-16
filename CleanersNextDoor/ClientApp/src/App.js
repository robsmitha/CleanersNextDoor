import React, { Component } from 'react';

//Layouts
import LayoutRoute from './components/Layout';
import NoNavLayoutRoute from './components/NoNavLayout';

//Standard
import { Home } from './components/Home';
import { HowItWorks } from './components/HowItWorks';

//Merchant services
import { Merchant } from './components/Merchant';
import { RequestService } from './components/RequestService';
import { Payment } from './components/Payment';

//Customers
import { Profile } from './components/Profile'
import { SignIn } from './components/SignIn'
import { SignOut } from './components/SignOut'
import { SignUp } from './components/SignUp'
import { SavedAddresses } from './components/SavedAddresses'
import { NewAddress } from './components/NewAddress'

import { AuthProvider } from './context/AuthContext'

import './custom.css'
export default class App extends Component {
    static displayName = App.name;
    render() {
        return (
            <div>
                <AuthProvider>
                    <LayoutRoute exact path='/' component={Home} />
                    <LayoutRoute path='/profile' component={Profile} />
                    <LayoutRoute path='/saved-addresses' component={SavedAddresses} />
                    <LayoutRoute path='/new-address' component={NewAddress} />
                    <LayoutRoute path='/merchant/:id' component={Merchant} />
                    <LayoutRoute path='/request-service/:id' component={RequestService} />
                    <LayoutRoute path='/how-it-works' component={HowItWorks} />

                    <NoNavLayoutRoute path='/payment/:id' component={Payment} />
                    <NoNavLayoutRoute path='/sign-in' component={SignIn} />
                    <NoNavLayoutRoute path='/sign-up' component={SignUp} />
                    <NoNavLayoutRoute path='/sign-out' component={SignOut} />

                </AuthProvider>
            </div>
        );
    }
}
