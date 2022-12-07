import NodeClient from "../Clients/NodeClient.js"
import Point from "../Ed25519/point.js"
import { decryptData } from "../Tools/AES.js"
import { RandomBigInt, mod, mod_inv } from "../Tools/Utils.js"

export default class PrismFlow{
    /**
     * Config should include key/value pairs of: 
     * @example
     * {
     *  pass: string
     *  urls: string[]
     *  encryptedData: string
     * }
     * @example
     * @param {object} config 
     */
    constructor(config){
        if(!Object.hasOwn(config, 'pass')){ throw Error("Password has not been included in config")}
        if(!Object.hasOwn(config, 'urls')){ throw Error("Urls has not been included in config")}
        if(!Object.hasOwn(config, 'encryptedData')){ throw Error("EncryptedData has not been included in config")}
        
        /**
         * @type {string}
         */
        this.pass = config.pass
        /**
         * @type {string[]}
         */
        this.urls = config.urls
        /**
         * @type {string}
         */
        this.encryptedData = config.encryptedData
    }

    /**
     * Starts the Prism Flow to attempt to decrypt the supplied data with the given password.
     * @returns {Promise<string>}
     */
    async start(){
        const keyIds = ['Test', 'Prize'] // doing this because a toggle on the main page would look bad                                                                                                                                                                                                                                                                                     no it wouldn't. just add it
        var i;
        var success = false;
        var decrypted;
        for(i = 0; i < 2 && success == false; i++){ // twice so the password is tested on both endpoints
            const random = RandomBigInt();
            const passwordPoint_R = (await Point.fromString(this.pass)).times(random); // password point * random
            const clients = this.urls.map(url => new NodeClient(url, keyIds[i])) // create node clients
            const appliedPoints = clients.map(client => client.Apply(passwordPoint_R)); // get the applied points from clients
            const authPoint_R = (await Promise.all(appliedPoints)).reduce((sum, next) => sum.add(next)); // sum all points returned from nodes
            const authPoint = authPoint_R.times(mod_inv(random)); // remove the random to get the authentication point

            decrypted = await decryptData(this.encryptedData, authPoint.toBase64()); // Attempt to decrypt the data with the authPoint as a base64 string (acting as password)
            success = decrypted === "Decryption Failed" ? false : true; // determine if decryption worked
        }
        return decrypted;
    }

    /**
     * @returns {Promise<string>}
     */
    async getAuthPoint(){ // Exclusively for Prize Key
        const random = RandomBigInt();
        const passwordPoint_R = (await Point.fromString(this.pass)).times(random); // password point * random
        const clients = this.urls.map(url => new NodeClient(url, 'Prize')) // create node clients
        const appliedPoints = clients.map(client => client.Apply(passwordPoint_R)); // get the applied points from clients
        const authPoint_R = (await Promise.all(appliedPoints)).reduce((sum, next) => sum.add(next)); // sum all points returned from nodes
        const authPoint = authPoint_R.times(mod_inv(random)); // remove the random to get the authentication point

        return authPoint.toBase64();
    }
}