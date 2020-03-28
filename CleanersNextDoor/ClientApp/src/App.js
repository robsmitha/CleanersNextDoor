import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';

import './custom.css'
import { MerchantDetails } from './components/merchants/MerchantDetails';
import { RequestService } from './components/merchants/RequestService';

import { UserProfile } from './components/users/UserProfile';
import { UserSignIn } from './components/users/UserSignIn';
import { UserSignUp } from './components/users/UserSignUp';
import { UserSignOut } from './components/users/UserSignOut';

import { CustomerProfile } from './components/customers/CustomerProfile'
import { CustomerSignIn } from './components/customers/CustomerSignIn'
import { CustomerSignOut } from './components/customers/CustomerSignOut'
import { CustomerSignUp } from './components/customers/CustomerSignUp'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
            <Route exact path='/' component={Home} />
            <Route path='/users/sign-in' component={UserSignIn} />
            <Route path='/users/sign-up' component={UserSignUp} />
            <Route path='/users/profile' component={UserProfile} />
            <Route path='/users/sign-out' component={UserSignOut} />
            <Route path='/customers/sign-in' component={CustomerSignIn} />
            <Route path='/customers/sign-up' component={CustomerSignUp} />
            <Route path='/customers/profile' component={CustomerProfile} />
            <Route path='/customers/sign-out' component={CustomerSignOut} />
            <Route path='/merchant/:id' component={MerchantDetails} />
            <Route path='/request-service/:id' component={RequestService} />
      </Layout>
    );
  }
}
