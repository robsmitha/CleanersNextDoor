import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Profile } from './components/account/Profile';
import { SignIn } from './components/account/SignIn';
import { SignUp } from './components/account/SignUp';
import { SignOut } from './components/account/SignOut';

import './custom.css'
import { MerchantList } from './components/merchants/MerchantList';
import { MerchantDetails } from './components/merchants/MerchantDetails';

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
            <Route path='/merchants' component={MerchantList} />
            <Route path='/merchant/:id' component={MerchantDetails} />
      </Layout>
    );
  }
}
