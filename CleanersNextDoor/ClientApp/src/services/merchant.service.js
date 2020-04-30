
import { get, post } from './api.service';


export const merchantService = {
    getMerchants,
    getMerchant,
    getItems,
    getMerchantWorkflow
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

function getMerchantWorkflow(merchantId) {
    return get(`merchants/GetMerchantWorkflow/${merchantId}`)
}
