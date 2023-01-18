import Point from "../Ed25519/point"
import EncryptionFlow from "../Flow/VendorFlow"
import EntryFlow from "../Flow/EntryFlow"
import PrismFlow from "../Flow/Prism"
import { SHA256_Digest } from "../Tools/Hash"
import { BigIntFromByteArray } from "../Tools/Utils"
import VendorFlow from "../Flow/VendorFlow"

export default class SignUp{
    /**
     * Config should include key/value pairs of: 
     * @example
     * {
     *  orkUrls: string[]
     *  simulatorUrl: string
     *  vendorUrl: string
     * }
     * @example
     * @param {object} config 
     */
    constructor(config){
        if(!Object.hasOwn(config, 'orkUrls')){ throw Error("Ork Urls has not been included in config")}
        if(!Object.hasOwn(config, 'simulatorUrl')){ throw Error("Simulator Url has not been included in config")}
        if(!Object.hasOwn(config, 'vendorUrl')){ throw Error("Vendor Url has not been included in config")}
        
        /**
         * @type {string[]}
         */
        this.orkUrls = config.orkUrls
        /**
         * @type {string}
         */
        this.simulatorUrl = config.simulatorUrl
        /**
         * @type {string}
         */
        this.vendorUrl = config.vendorUrl
    }

    async start(username, password, secretCode){
        //hash username
        const uid = BigIntFromByteArray(await SHA256_Digest(username)).toString();
        //convert password to point
        const passwordPoint = (await Point.fromString(password));
        
        const prismFlow = new PrismFlow(this.orkUrls);
        const [encryptedCode, signedEntry] = await prismFlow.SetUp(uid, passwordPoint, secretCode);
        
        const entryFlow = new EntryFlow(this.simulatorUrl);
        await entryFlow.SubmitEntry(uid, signedEntry, this.orkUrls)

        const encryptionFlow = new VendorFlow(this.vendorUrl);
        await encryptionFlow.AddUserEntry(uid, encryptedCode);
    }
}
