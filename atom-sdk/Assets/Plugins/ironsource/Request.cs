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

        Action<Response> callback_;

        /**
         * Constructor for Reqeuest
         * 
         * @param {string} url
         * @param {string} data
         * @param {Dictionary<string, string>} headers
         * @param {Action<Response>} callback
         * 
         **/
        public Request(string url, string data, Dictionary<string, string> headers, 
                        Action<Response> callback) {
            url_ = url;
            data_ = data;
            headers_ = headers;

            callback_ = callback;
        }

        /**
         * GET request to server
         * 
         **/
        public IEnumerator Get() {
            string url = url_ + "?data=" + IronSourceAtomUtils.Base64Encode(data_);
            Debug.Log("Request URL: " + url);

            WWW www = new WWW(url, null, headers_);
            yield return www;

            ReadResponse(www);
        }

        /**
         * POST request to server
         * 
         **/
        public IEnumerator Post() {
            Debug.Log("Request URL: " + url_);
            WWW www = new WWW(url_, Encoding.ASCII.GetBytes(data_), headers_);
            yield return www;

            ReadResponse(www);
        }


        /**
         * Read response from WWW object
         * 
         * @param {WWW} www - object with response information
         * 
         **/
        private void ReadResponse(WWW www) {
            string error = null;
            string data = null;
            int status = -1;

            if (!string.IsNullOrEmpty(www.error)) {
                try {
                    string[] errors = www.error.Split(' ');
                    status = Convert.ToInt32(errors[0]);
                } catch(Exception) {
                }

                error = www.text;
            } else {
                status = 200;
                data = www.text;
            }

            if (callback_ != null) {
                callback_(new Response(error, data, status));
            }
        }
    }
}