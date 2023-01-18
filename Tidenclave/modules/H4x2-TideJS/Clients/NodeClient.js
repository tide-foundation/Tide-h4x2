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

import Point from "../Ed25519/point.js"
import ClientBase from "./ClientBase.js"

export default class NodeClient extends ClientBase {
    /**
     * @param {string} url
     */
    constructor(url){
        super(url)
    }

    /**
     * @param {Point} point
     * @param {string} uid 
     * @returns {Promise<Point>}
     */
    async Apply(uid, point){
        const data = this._createFormData({'point': point.toBase64()})
        const response = await this._post(`/Apply/Prism/${uid}`, data)
        if(response.ok){
            return Point.fromB64(await response.text());
        }
        if(response.status == 429){
            return Promise.reject("Too many requests")
        }
        return Promise.reject("Node client: " + response.status); // should never get here
    }

    /**
     * @param {Point} point
     * @param {string} uid 
     * @returns {Promise<[string, Point]>}
     */
    async CreatePRISM(uid, point){
        const data = this._createFormData({'point': point.toBase64()})
        const response = await this._post(`/Create/Prism/${uid}`, data)
        if(response.ok){
            const resp_obj = JSON.parse(await response.text());
            return [resp_obj.encryptedState, Point.fromB64(resp_obj.point)];
        }
        return Promise.reject("Node client: " + response.status);
    }

    /**
     * @param {Point} prismPub 
     * @param {string} encryptedState 
     * @returns {Promise<[string, string]>}
     */
    async CreateAccount(prismPub, encryptedState){
        const data = this._createFormData({'prismPub': prismPub.toBase64(), 'encryptedState': encryptedState})
        const response = await this._post('/Create/Account', data);
        if(response.ok){
            const resp_obj = JSON.parse(await response.text());
            return [resp_obj.encryptedCVK, resp_obj.signedUID]
        }
        return Promise.reject("Node client: " + response.status);
    }
}