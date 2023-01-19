import SimulatorClient from "../Clients/SimulatorClient"

export default class EntryFlow{
    /**
     * @param {string} url
     */
    constructor(url){
        /**
         * @type {string}
         */
        this.url = url
    }

    async SubmitEntry(userID, signedEntry, ORKUrls){
        const client = new SimulatorClient(this.url);
        await client.AddUserEntry(userID, signedEntry, ORKUrls);
    }
}
