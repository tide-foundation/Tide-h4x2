// 
// Tide Protocol - Infrastructure for a TRUE Zero-Trust paradigm
// Copyright (C) 2022 Tide Foundation Ltd
// 
// This program is free software and is subject to the terms of 
// the Tide Community Open Code License as published by the 
// Tide Foundation Limited. You may modify it and redistribute 
// it in accordance with and subject to the terms of that License.
// This program is distributed WITHOUT WARRANTY of any kind, 
// including without any implied warranty of MERCHANTABILITY or 
// FITNESS FOR A PARTICULAR PURPOSE.
// See the Tide Community Open Code License for more details.
// You should have received a copy of the Tide Community Open 
// Code License along with this program.
// If not, see https://tide.org/licenses_tcoc2-0-0-en
//

import ClientBase from "./ClientBase.js"

export default class VendorClient extends ClientBase {
    /**
     * @param {string} url 
     * @param {string} keyID
     */
    constructor(url, keyID){
        super(url)
        this.keyID = keyID
    }

    /**
     * @param {string} secret
     * @param {string} uid 
     * @returns {Promise<string>}
     */
    async AddToVendor(uid, secret){
        const data = this._createFormData({'secret' : secret})
        const response = await this._post(`/users/${uid}` ,data)
        if(response.ok){
            return await response.text();
        }
        return Promise.reject(); // should never get here
    }
}