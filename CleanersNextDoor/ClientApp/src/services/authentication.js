

export class Authentication {

    static async getClaimId() {
        const response = await fetch('/Authentication/GetClaimID');
        const claimId = await response.json();
        return claimId;
    }
    static async clearSession() {
        const response = await fetch('/Authentication/ClearSession');
        const success = await response.json();
        return success;
    }
}