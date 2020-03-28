

export class Authentication {
    static getUserId() {
        return JSON.parse(localStorage.getItem('USER_ID') || 0)
    }
    static setUserId(id) {
        document.getElementById('nav_sign_in').hidden = true;
        document.getElementById('nav_sign_up').hidden = true;
        document.getElementById('nav_profile').hidden = false;
        document.getElementById('nav_sign_out').hidden = false;
        localStorage.setItem('USER_ID', id)
    }
    static clearLocalStorage() {
        this.setUserId(0)
    }
    static clearCustomerLocalStorage() {
        this.setCustomerId(0)
    }
    static getCustomerId() {
        return JSON.parse(localStorage.getItem('CUSTOMER_ID') || 0)
    }
    static setCustomerId(id) {
        document.getElementById('nav_customer_sign_in').hidden = true;
        document.getElementById('nav_customer_sign_up').hidden = true;
        document.getElementById('nav_customer_profile').hidden = false;
        document.getElementById('nav_customer_sign_out').hidden = false;
        localStorage.setItem('CUSTOMER_ID', id)
    }
}