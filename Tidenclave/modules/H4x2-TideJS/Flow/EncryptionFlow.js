
export default class EncryptionFlow{
    /**
     * Config should include key/value pairs of: 
     * @example
     * {
     *  url: string[]
     * }
     * @example
     * @param {object} config 
     */
    constructor(config){
        if(!Object.hasOwn(config, 'url')){ throw Error("EntryFlow: Url has not been included in config")}
        
        /**
         * @type {string[]}
         */
        this.url = config.url
    }

    async AddUserEntry(encryptedCode, userID){

    }
}
