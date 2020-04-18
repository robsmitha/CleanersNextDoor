
import { get } from './api.service';


export const merchantService = {
    getMerchants,
    getMerchant,
    getItems
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