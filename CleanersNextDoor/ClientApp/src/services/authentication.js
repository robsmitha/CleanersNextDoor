

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
}