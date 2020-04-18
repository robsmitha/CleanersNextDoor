
import { BehaviorSubject } from 'rxjs';
import { post } from './api.service';

const _appUserKey = 'appUser';

const appUserSubject = new BehaviorSubject(JSON.parse(localStorage.getItem(_appUserKey)));

export const authenticationService = {
    signIn,
    signUp,
    signOut,
    clearAppUser,
    authorize,
    appUser: appUserSubject.asObservable(),
    get appUserValue() { return appUserSubject.value }
};


function signIn(email, password) {
    return post(`authentication/signin`, { email, password })
        .then(customer => {
            localStorage.setItem(_appUserKey, JSON.stringify(customer));
            appUserSubject.next(customer);
            return customer;
        });
}

function signUp(customer) {
    return post('authentication/signup', customer)
        .then(customer => {
            localStorage.setItem(_appUserKey, JSON.stringify(customer));
            appUserSubject.next(customer);
            return customer;
        });
}

function signOut() {
    post(`authentication/signout`)
        .then(data => {
            if (data) clearAppUser();
        });
}
function authorize() {
    return post(`authentication/authorize`)
}
function clearAppUser() {
    // remove user from local storage to log user out
    localStorage.removeItem(_appUserKey);
    appUserSubject.next(null);
}