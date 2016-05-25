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

        public Request(string url, string data, Dictionary<string, string> headers, 
                Action<Response> callback) {
            url_ = url;
            data_ = data;
            headers_ = headers;

            callback_ = callback;
        }

        public IEnumerator Get() {
            string url = url_ + "?data=" + IronSourceAtomUtils.Base64Encode(data_);
            Debug.Log("Request URL: " + url);

            WWW www = new WWW(url, null, headers_);
            yield return www;

            ReadResponse(www);
        }

        public IEnumerator Post() {
            Debug.Log("Request URL: " + url_);
            WWW www = new WWW(url_, Encoding.ASCII.GetBytes(data_), headers_);
            yield return www;

            ReadResponse(www);
        }

        private void ReadResponse(WWW www) {
            string error = null;
            string data = null;
            int status = -1;

            if (!string.IsNullOrEmpty(www.error)) {
                try {
                    status = Convert.ToInt32(www.error);
                } catch(Exception) {
                }

                switch (status) {
                case 400:
                    error = "Invalid JSON / No data / Missing data or table";
                    break;
                case 401:
                    error = "Auth Error";
                    break;
                default:
                    error = "Unknown error";
                    break;
                }
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