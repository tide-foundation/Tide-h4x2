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

export default class SimulatorClient extends ClientBase {
    /**
     * @param {string} url
     */
    constructor(url){
        super(url)
    }

    /**
     * This method will query the simulator for all information about all ORKs and return
     * an array compromising of each ORK's name, url and public.
     * @returns {Promise<[string, string, string, Point][]>}
     */
    async GetAllORKs(){
        const response = await this._get('/orks'); // endpoint is at /
        const formattedResponse = JSON.parse(await response.text())
        const returnedResponse = formattedResponse.map(orkEntry => [orkEntry.orkID, orkEntry.orkName, orkEntry.orkUrl, Point.fromB64(orkEntry.orkPub)]);
        return returnedResponse;
    }

    async AddUserEntry(userID, signedEntry, orkUrls){
        var user = {
            userId: userID,
            orkUrls: orkUrls,
            signedUID: signedEntry
        }
        const response = await this._postJSON('', user);
        if(!response.ok){
            return Promise.reject("Adding user to simulator failed.")
        }
    }

    /**
     * 
     * @param {string} uid 
     * @returns {Promise<[string, Point][]>}
     */
    async GetUserORKs(uid){
        const response = await this._get(`users/orks/${uid}`);
        if(response.ok){
            const resp_obj = JSON.parse(await response.text());
            const pubs = resp_obj.orkPubs.map(pub => Point.fromB64(pub));
            return [resp_obj.orkUrls, pubs]
        }
        return Promise.reject("Simulator Client: Failed to get user's orks");
    }
}