
import { get, post } from './api.service';


export const merchantService = {
    getMerchants,
    getMerchant,
    getItems,
    makePayment
};
function getMerchants() {
    return get(`merchants/GetMerchants`)
}

function getMerchant(merchantId) {
    return get(`merchants/${merchantId}`)
}

function getItems(merchantId) {
    return get(`merchants/${merchantId}/items`)
}

function makePayment(data) {
    return post('/merchants/charge', data)
}