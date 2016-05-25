using System;

namespace ironsource {
    public class Response {
        public string error;
        public string data;
        public int status;

        public Response(string error, string data, int status) {
            this.error = error;
            this.data = data;
            this.status = status;
        }
    }
}
