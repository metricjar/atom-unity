using System;

namespace ironsource {
    public class Response {
        public string error;
        public string data;
        public int status;

        /**
         * Constructor for Response
         * 
         * @param {string} error
         * @param {string} data
         * @param {int} status
         * s
         **/
        public Response(string error, string data, int status) {
            this.error = error;
            this.data = data;
            this.status = status;
        }
    }
}
