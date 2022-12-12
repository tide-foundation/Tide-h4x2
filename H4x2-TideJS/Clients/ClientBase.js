export default class ClientBase {
     /**
     * @param {string} url 
     */
      constructor(url){
        this.url = url
    }

    /**
     * @param {Object} form
     * @returns {FormData}
     */
    _createFormData(form){
        const formData = new FormData();

        Object.entries(form).forEach(([key, value]) => {
            formData.append(key, value)
        });

        return formData
    }

    /** 
     * @param {string} endpoint 
     * @returns {Promise<Response>}
     */
     _get(endpoint){
        return fetch(this.url + endpoint, {
            method: 'GET'
        });
    }

    /** 
     * @param {string} endpoint 
     * @param {FormData} data
     * @returns {Promise<Response>}
     */
    async _post(endpoint, data){
       return fetch(this.url + endpoint, {
            method: 'POST',
            body: data
        });
    }
}