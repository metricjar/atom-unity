using UnityEngine;

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace ironsource {
    internal class Request {
        private string url_;
        private string data_;
        private Dictionary<string, string> headers_;

        Action<Response> callbackAction_ = null;

        string callbackStr_ = null;
        GameObject parrentGameObject_ = null;

        /// <summary>
        /// Constructor for Reqeuest
        /// </summary>
        /// <param name="url">
        /// <see cref="string"/> for server address.
        /// </param>
        /// <param name="data">
        /// <see cref="string"/> for sending data.
        /// </param> 
        /// <param name="headers">
        /// <see cref="Dictionary<string, string>"/> for sending headers.
        /// </param> 
        /// <param name="callback">
        /// <see cref="Action<Response>"/> for get response data.
        /// </param>        
        public Request(string url, string data, Dictionary<string, string> headers, 
                        Action<Response> callback) {
            url_ = url;
            data_ = data;
            headers_ = headers;

            callbackAction_ = callback;
        }

        /// <summary>
        /// Constructor for Reqeuest
        /// </summary>
        /// <param name="url">
        /// <see cref="string"/> for server address.
        /// </param>
        /// <param name="data">
        /// <see cref="string"/> for sending data.
        /// </param> 
        /// <param name="headers">
        /// <see cref="Dictionary<string, string>"/> for sending headers.
        /// </param> 
        /// <param name="callback">
        /// <see cref="string"/> for get response data.
        /// </param>    
        /// <param name="parrentGameObject">
        /// <see cref="GameObject"/> for parrent GameObject for callback.
        /// </param>           
        public Request(string url, string data, Dictionary<string, string> headers, 
                       string callback, GameObject parrentGameObject) {
            url_ = url;
            data_ = data;
            headers_ = headers;

            callbackStr_ = callback;
            parrentGameObject_ = parrentGameObject;
        }

        /// <summary>
        /// GET request to server
        /// </summary>
        public IEnumerator Get() {
            string url = url_ + "?data=" + IronSourceAtomUtils.Base64Encode(data_);
            Debug.Log("Request URL: " + url);

            WWW www = new WWW(url, null, headers_);
            yield return www;

            ReadResponse(www);
        }

        /// <summary>
        /// POST request to server
        /// </summary>
        public IEnumerator Post() {
            Debug.Log("Request URL: " + url_);
            WWW www = new WWW(url_, Encoding.ASCII.GetBytes(data_), headers_);
            yield return www;

            ReadResponse(www);
        }


        /// <summary>
        /// Read response from WWW object
        /// </summary>
        /// <param name="www">
        /// <see cref="WWW"/> object with response information.
        /// </param>    
        private void ReadResponse(WWW www) {
            string error = null;
            string data = null;
            int status = -1;

            if (!string.IsNullOrEmpty(www.error)) {
                try {
                    string[] errors = www.error.Split(' ');
                    status = Convert.ToInt32(errors[0]);

                    error = www.text;
                } catch(Exception) {
                    error = www.error;
                }


            } else {
                status = 200;
                data = www.text;
            }

            if (callbackAction_ != null) {
                callbackAction_(new Response(error, data, status));
            }

            if (callbackStr_ != null) {
                parrentGameObject_.SendMessage(callbackStr_, new Response(error, data, status));
            }
        }
    }
}
