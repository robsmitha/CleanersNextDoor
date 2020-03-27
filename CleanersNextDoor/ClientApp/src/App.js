import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Profile } from './components/account/Profile';
import { SignIn } from './components/account/SignIn';
import { SignUp } from './components/account/SignUp';
import { SignOut } from './components/account/SignOut';

import './custom.css'
import { MerchantDetails } from './components/merchants/MerchantDetails';
import { RequestService } from './components/merchants/RequestService';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
            <Route exact path='/' component={Home} />
            <Route path='/sign-in' component={SignIn} />
            <Route path='/sign-up' component={SignUp} />
            <Route path='/profile' component={Profile} />
            <Route path='/sign-out' component={SignOut} />
            <Route path='/merchant/:id' component={MerchantDetails} />
            <Route path='/request-service/:id' component={RequestService} />
      </Layout>
    );
  }
}
