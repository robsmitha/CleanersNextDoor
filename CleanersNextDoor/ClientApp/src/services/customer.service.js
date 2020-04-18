import { post, get } from "./api.service";

const _CONTROLLER = 'customers'

export const customerService = {
    getCustomer,
    checkEmailAvailability,
    getPaymentMethods,
    addPaymentMethod,
    removePaymentMethod,
    setDefaultPaymentMethod,
    getAddresses,
    addAddress,
    removeAddress,
    setDefaultAddress,
    getCart,
    removeCartItem,
    cartTransaction
};
//Customer
function getCustomer() {
    return get(`${_CONTROLLER}/account`)
}
function checkEmailAvailability() {
    return get(`customers/CheckEmailAvailability/${this.state.formControls.email.value}`)
}

//Payment Methods
function getPaymentMethods() {
    return get(`${_CONTROLLER}/getPaymentMethods`)
}
function addPaymentMethod(data) {
    return post(`${_CONTROLLER}/AddPaymentMethod`, data)
}
function removePaymentMethod(data) {
    return post(`${_CONTROLLER}/removePaymentMethod`, data)
}
function setDefaultPaymentMethod(data) {
    return post(`${_CONTROLLER}/setDefaultPaymentMethod`, data)
}
//Addresses
function getAddresses() {
    return get(`${_CONTROLLER}/getAddresses`)
}
function addAddress(data) {
    return post(`${_CONTROLLER}/AddAddress`, data)
}
function removeAddress(data) {
    return post(`${_CONTROLLER}/removeAddress`, data)
}
function setDefaultAddress(data) {
    return post(`${_CONTROLLER}/setDefaultAddress`, data)
}
//Cart
function getCart(merchantId) {
    return get(`${_CONTROLLER}/cart/${merchantId}`)
}
function removeCartItem(data) {
    return post(`${_CONTROLLER}/removeCartItem`, data)
}
function cartTransaction(data) {
    return post(`${_CONTROLLER}/addtocart`, data)
}
