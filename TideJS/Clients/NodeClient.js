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