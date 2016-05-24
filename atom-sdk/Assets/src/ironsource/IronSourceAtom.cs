using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ironsource {
    public class IronSourceAtom : MonoBehaviour {
        private static string API_VERSION_ = "V1";

        public string endpoint = "https://track.atom-data.io/";
        public string authKey = "";

        private Dictionary<string, string> headers_ = new Dictionary<string, string>();

        public void Awake() {
            headers_.Add("x-ironsource-atom-sdk-type", "unity");
            headers_.Add("x-ironsource-atom-sdk-version", IronSourceAtom.API_VERSION_);
        }

        /**
         *
         * @api {get/post} https://track.atom-data.io/ PutEvent Send single data to Atom server
         * @apiVersion 1.0.0
         * @apiGroup Atom
         * @apiParam {string} stream Stream name for saving data in db table
         * @apiParam {string} data Data for saving 
         * @apiParam {string} method POST or GET method for do request
         * 
         * @apiSuccess {null} err Server response error 
         * @apiSuccess {string} data Server response data
         * @apiSuccess {int} status Server response status
         * 
         * @apiError {string} err Server response error
         * @apiError {null} data Server response data
         * @apiError {int} status Server response status
         * 
         * @apiErrorExample Error-Response:
         *  HTTP 401 Permission Denied
         *  {
         *    "err": {"Target Stream": "Permission denied"},
         *    "data": null,
         *    "status": 401    
         *  }
         * 
         * @apiSuccessExample Response:
         * HTTP 200 OK
         * {
         *    "err": null,
         *    "data": "success"
         *    "status": 200
         * }
         *
         * @apiParamExample {json} Request-Example:
         * {
         *    "stream": "streamName",
         *    "data":  "{\"name\": \"iron\", \"last_name\": \"Source\"}"
         * }
         * 
         */
        public void PutEvent(string stream, string data, HttpMethod method = HttpMethod.POST, 
                             Action<Response> callback = null) {
            string hash = IronSourceAtomUtils.EncodeHmac(data, Encoding.ASCII.GetBytes(authKey));

            var eventObject = new Dictionary<string, string>();
            eventObject ["table"] = stream;
            eventObject["data"] = IronSourceAtomUtils.EscapeStringValue(data);
            eventObject["auth"] = hash;
            string jsonEvent = IronSourceAtomUtils.DictionaryToJson(eventObject);

            Debug.Log("Request body: " + jsonEvent);

            SendEventCoroutine (endpoint, method, headers_, jsonEvent, callback);
        }

        /**
         *
         * @api {get/post} https://track.atom-data.io/bulk PutEvents Send multiple events data to Atom server
         * @apiVersion 1.0.0
         * @apiGroup Atom
         * @apiParam {string} stream Stream name for saving data in db table
         * @apiParam {list} data Multiple event data for saving
         * @apiParam {string} method POST or GET method for do request
         *
         * @apiSuccess {null} err Server response error
         * @apiSuccess {string} data Server response data
         * @apiSuccess {int} status Server response status
         *
         * @apiError {string} err Server response error
         * @apiError {null} data Server response data
         * @apiError {int} status Server response status
         *
         * @apiErrorExample Error-Response:
         *  HTTP 401 Permission Denied
         *  {
         *    "err": {"Target Stream": "Permission denied",
         *    "data": null,
         *    "status": 401
         *  }
         *
         * @apiSuccessExample Response:
         * HTTP 200 OK
         * {
         *    "err": null,
         *    "data": "success"
         *    "status": 200
         * }
         * @apiParamExample {json} Request-Example:
         * {
         *    "stream": "streamName",
         *    "data":  ["{\"name\": \"iron\", \"last_name\": \"Source\"}",
         *            "{\"name\": \"iron2\", \"last_name\": \"Source2\"}"]
         *
         * }
         *
         */

        public void PutEvents(string stream, List<string> data, HttpMethod method = HttpMethod.POST, 
                              Action<Response> callback = null) {
            string json = IronSourceAtomUtils.ListToJson(data);
            Debug.Log ("Key: " + authKey);

            string hash = IronSourceAtomUtils.EncodeHmac(json, Encoding.ASCII.GetBytes(authKey));

            var eventObject = new Dictionary<string, string>();
            eventObject ["table"] = stream;
            eventObject["data"] = IronSourceAtomUtils.EscapeStringValue(json);
            eventObject["auth"] = hash;
            string jsonEvent = IronSourceAtomUtils.DictionaryToJson(eventObject);

            Debug.Log("Request body: " + jsonEvent);

            SendEventCoroutine(endpoint + "bulk", method, headers_, jsonEvent, callback);
        }

        private void SendEventCoroutine(string url, HttpMethod method, Dictionary<string, string> headers,
                                        string data, Action<Response> callback) {

            Request request = new Request(url, data, headers, callback);
            if (method == HttpMethod.GET) {
                this.StartCoroutine(request.Get());
            } else {
                this.StartCoroutine(request.Post());
            }
           
        }
    }
}
