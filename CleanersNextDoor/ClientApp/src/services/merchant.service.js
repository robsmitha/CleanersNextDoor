
import { get, post } from './api.service';


export const merchantService = {
    searchMerchants,
    getMerchant,
    getItems,
    getMerchantWorkflow,
    getItem
};
function searchMerchants(data) {
    return post(`merchants/searchMerchants`, data)
}

function getMerchant(merchantId) {
    return get(`merchants/${merchantId}`)
}

function getItems(merchantId, itemTypeId) {
    return get(`merchants/${merchantId}/items/${itemTypeId && itemTypeId != null ? itemTypeId : 0}`)
}

function getMerchantWorkflow(merchantId) {
    return get(`merchants/GetMerchantWorkflow/${merchantId}`)
}

function getItem(itemId) {
    return get(`merchants/getItem/${itemId}`)
}