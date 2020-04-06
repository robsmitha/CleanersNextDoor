
import { BehaviorSubject } from 'rxjs';

import { handleResponse } from './../helpers/handle-response';

const currentUserSubject = new BehaviorSubject(JSON.parse(localStorage.getItem('currentUser')));

export const authenticationService = {
    authenticateUser,
    authenticateCustomer,
    createUser,
    createCustomer,
    customerLogout,
    currentUser: currentUserSubject.asObservable(),
    get currentUserValue() { return currentUserSubject.value }
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
            localStorage.setItem('currentUser', JSON.stringify(user));
            currentUserSubject.next(user);
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
            localStorage.setItem('currentUser', JSON.stringify(user));
            currentUserSubject.next(user);

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
            localStorage.setItem('currentUser', JSON.stringify(customer));
            currentUserSubject.next(customer);
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
            localStorage.setItem('currentUser', JSON.stringify(customer));
            currentUserSubject.next(customer);
            return customer;
        });
}

function customerLogout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    currentUserSubject.next(null);
}