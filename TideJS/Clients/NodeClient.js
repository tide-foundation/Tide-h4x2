// Tide Protocol - Infrastructure for the Personal Data economy
// Copyright (C) 2019 Tide Foundation Ltd
// 
// This program is free software and is subject to the terms of 
// the Tide Community Open Source License as published by the 
// Tide Foundation Limited. You may modify it and redistribute 
// it in accordance with and subject to the terms of that License.
// This program is distributed WITHOUT WARRANTY of any kind, 
// including without any implied warranty of MERCHANTABILITY or 
// FITNESS FOR A PARTICULAR PURPOSE.
// See the Tide Community Open Source License for more details.
// You should have received a copy of the Tide Community Open 
// Source License along with this program.
// If not, see https://tide.org/licenses_tcosl-1-0-en

import Point from "../Ed25519/point.js"
import ClientBase from "./ClientBase.js"

export default class NodeClient extends ClientBase {
    /**
     * @param {string} url 
     * @param {string} keyID
     */
    constructor(url, keyID){
        super(url)
        this.keyID = keyID
    }

    /**
     * @param {Point} point 
     * @returns {Promise<Point>}
     */
    async Apply(point){
        const data = this._createFormData({'point': point.toBase64()})
        const response = await this._post(`/Apply/${this.keyID}`, data)
        return Point.fromB64(await response.text());
    }
}