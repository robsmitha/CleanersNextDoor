

export class Authentication {
    static getUserId() {
        return JSON.parse(localStorage.getItem('USER_ID') || 0)
    }
    static setUserId(id) {
        localStorage.setItem('USER_ID', id)
    }
    static clearLocalStorage() {
        this.setUserId(0)
    }
}