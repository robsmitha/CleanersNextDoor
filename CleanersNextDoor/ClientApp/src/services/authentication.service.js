
import { BehaviorSubject } from 'rxjs';
import { handleResponse } from './../helpers/handle-response';

const _appUserKey = 'appUser';

const appUserSubject = new BehaviorSubject(JSON.parse(localStorage.getItem(_appUserKey)));

export const authenticationService = {
    signIn,
    signUp,
    signOut,
    clearAppUser,
    appUser: appUserSubject.asObservable(),
    get appUserValue() { return appUserSubject.value }
};


function signIn(email, password) {
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

function signUp(customer) {
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

function signOut() {
    const requestOptions = {
        method: 'post'
    };
    fetch(`customers/signout`, requestOptions)
        .then(response => {
            if (response.ok) {
                clearAppUser();
            }
        });
}

function clearAppUser() {
    // remove user from local storage to log user out
    localStorage.removeItem(_appUserKey);
    appUserSubject.next(null);
}