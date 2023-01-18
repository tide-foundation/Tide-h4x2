import VendorClient from "../Clients/VendorClient"

export default class VendorFlow{
    /**
     * @param {string} url
     */
    constructor(url){
        /**
         * @type {string}
         */
        this.url = url
    }

    async AddUserEntry(encryptedCode, userID){
        const client = new VendorClient(this.url, userID);
        await client.AddToVendor(encryptedCode);
    }
}
