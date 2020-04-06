import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';

import './custom.css'
import { MerchantDetails } from './components/merchant/MerchantDetails';
import { RequestService } from './components/merchant/RequestService';
import { Payment } from './components/merchant/Payment';

import { UserProfile } from './components/user/UserProfile';
import { UserSignIn } from './components/user/UserSignIn';
import { UserSignUp } from './components/user/UserSignUp';
import { UserSignOut } from './components/user/UserSignOut';

import { CustomerProfile } from './components/customer/CustomerProfile'
import { CustomerSignIn } from './components/customer/CustomerSignIn'
import { CustomerSignOut } from './components/customer/CustomerSignOut'
import { CustomerSignUp } from './components/customer/CustomerSignUp'
import { HowItWorks } from './components/HowItWorks';
import { AuthProvider } from './context/AuthContext'

export default class App extends Component {
    static displayName = App.name;
    render() {
        return (
            <div>
                <AuthProvider>
                    <Layout>
                        <Route exact path='/' component={Home} />
                        <Route path='/user/sign-in' component={UserSignIn} />
                        <Route path='/user/sign-up' component={UserSignUp} />
                        <Route path='/user/profile' component={UserProfile} />
                        <Route path='/user/sign-out' component={UserSignOut} />
                        <Route path='/customer/sign-in' component={CustomerSignIn} />
                        <Route path='/customer/sign-up' component={CustomerSignUp} />
                        <Route path='/customer/profile' component={CustomerProfile} />
                        <Route path='/customer/sign-out' component={CustomerSignOut} />
                        <Route path='/merchant/:id' component={MerchantDetails} />
                        <Route path='/request-service/:id' component={RequestService} />
                        <Route path='/payment/:id' component={Payment} />
                        <Route path='/how-it-works' component={HowItWorks} />
                    </Layout>
                </AuthProvider>
            </div>
        );
    }
}
