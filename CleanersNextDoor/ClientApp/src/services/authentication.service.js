
import { BehaviorSubject } from 'rxjs';

import { handleResponse } from './../helpers/handle-response';

const _appUserKey = 'appUser';

const appUserSubject = new BehaviorSubject(JSON.parse(localStorage.getItem(_appUserKey)));

export const authenticationService = {
    authenticateUser,
    authenticateCustomer,
    createUser,
    createCustomer,
    customerLogout,
    appUser: appUserSubject.asObservable(),
    get appUserValue() { return appUserSubject.value }
};


function createUser(user) {
    const requestOptions = {
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    };

    return fetch('users/signup', requestOptions)
        .then(handleResponse)
        .then(user => {
            localStorage.setItem(_appUserKey, JSON.stringify(user));
            appUserSubject.next(user);
            return user;
        });
}


function authenticateUser(username, password) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
    };

    return fetch(`users/signin`, requestOptions)
        .then(handleResponse)
        .then(user => {
            localStorage.setItem(_appUserKey, JSON.stringify(user));
            appUserSubject.next(user);

            return user;
        });
}

function authenticateCustomer(email, password) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
    };

    return fetch(`customers/signin`, requestOptions)
        .then(handleResponse)
        .then(customer => {
            localStorage.setItem(_appUserKey, JSON.stringify(customer));
            appUserSubject.next(customer);
            return customer;
        });
}

function createCustomer(customer) {
    const requestOptions = {
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(customer)
    };

    return fetch('customers/signup', requestOptions)
        .then(handleResponse)
        .then(customer => {
            localStorage.setItem(_appUserKey, JSON.stringify(customer));
            appUserSubject.next(customer);
            return customer;
        });
}

function customerLogout() {
    const requestOptions = {
        method: 'post'
    };
    fetch(`customers/signout`, requestOptions)
        .then(customer => {
            // remove user from local storage to log user out
            localStorage.removeItem(_appUserKey);
            appUserSubject.next(null);
        });
}