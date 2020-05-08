import React, { Component } from 'react';

//Layouts
import LayoutRoute from './components/Layout';
import NoNavLayoutRoute from './components/NoNavLayout';

//General
import { Home } from './components/Home';
import { HowItWorks } from './components/HowItWorks';

//Identity
import { SignIn } from './components/SignIn'
import { SignOut } from './components/SignOut'
import { SignUp } from './components/SignUp'

//Merchant services
import { Merchant } from './components/merchants/Merchant';
import { Cart } from './components/merchants/Cart';
import { Payment } from './components/merchants/Payment';
import { Item } from './components/merchants/Item';

//Customer account
import { MyAccount } from './components/account/MyAccount'
import { SavedAddresses } from './components/account/SavedAddresses'
import { NewAddress } from './components/account/NewAddress'
import { PaymentMethods } from './components/account/PaymentMethods'
import { NewPaymentMethod } from './components/account/NewPaymentMethod'
import { OrderDetails } from './components/account/OrderDetails'
import { OrderHistory } from './components/account/OrderHistory'

//Context
import { AuthProvider } from './context/AuthContext'

import './custom.css'
export default class App extends Component {
    static displayName = App.name;
    render() {
        return (
            <div>
                <AuthProvider>
                    <LayoutRoute exact path='/' component={Home} />
                    <LayoutRoute path='/how-it-works' component={HowItWorks} />
                    <NoNavLayoutRoute path='/sign-in' component={SignIn} />
                    <NoNavLayoutRoute path='/sign-up' component={SignUp} />
                    <NoNavLayoutRoute path='/sign-out' component={SignOut} />

                    <LayoutRoute path='/account' component={MyAccount} />
                    <LayoutRoute path='/saved-addresses' component={SavedAddresses} />
                    <LayoutRoute path='/new-address' component={NewAddress} />
                    <LayoutRoute path='/payment-methods' component={PaymentMethods} />
                    <LayoutRoute path='/new-payment-method' component={NewPaymentMethod} />
                    <LayoutRoute exact path='/order-details/:id' component={OrderDetails} />
                    <LayoutRoute path='/order-history' component={OrderHistory} />

                    <LayoutRoute exact path='/merchant/:id' component={Merchant} />
                    <LayoutRoute exact path='/cart/:id' component={Cart} />
                    <LayoutRoute exact path='/item/:id' component={Item} />
                    <NoNavLayoutRoute exact path='/payment/:id' component={Payment} />

                </AuthProvider>
            </div>
        );
    }
}
