using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ironsource {
    internal class IronSourceCoroutineHandler : MonoBehaviour {
    }

    public class IronSourceAtom {
        private static string API_VERSION_ = "V1.0.0";

        private string endpoint_ = "https://track.atom-data.io/";
        private string authKey_ = "";

        private Dictionary<string, string> headers_ = new Dictionary<string, string>();
        private GameObject parentGameObject_ = null;
        private MonoBehaviour coroutineHandler_ = null;

        /// <summary>
        /// API constructor
        /// </summary>
        /// <param name="gameObject">
        /// A <see cref="GameObject"/> for coroutine method call.
        /// </param>
        public IronSourceAtom(GameObject gameObject) {
            parentGameObject_ = gameObject;

            coroutineHandler_ = gameObject.GetComponent<MonoBehaviour>();
            if (coroutineHandler_ == null) {
                coroutineHandler_ = parentGameObject_.AddComponent<IronSourceCoroutineHandler>();
            }

            headers_.Add("x-ironsource-atom-sdk-type", "unity");
            headers_.Add("x-ironsource-atom-sdk-version", IronSourceAtom.API_VERSION_);
        }

        /// <summary>
        /// API destructor - clear craeted IronSourceCoroutineHandler
        /// </summary>       
        ~IronSourceAtom() {
            if (coroutineHandler_ != null) {
                UnityEngine.Object.Destroy(coroutineHandler_);
            }
        }

        /// <summary>
        /// Set Auth Key for stream
        /// </summary>  
        /// <param name="authKey">
        /// A <see cref="string"/> for secret key of stream.
        /// </param>
        public void SetAuth(string authKey) {
            authKey_ = authKey;
        }

        /// <summary>
        /// Set endpoint for send data
        /// </summary>
        /// <param name="endpoint">
        /// A <see cref="string"/> for address of server
        /// </param>
        public void SetEndpoint(string endpoint) {
            endpoint_ = endpoint;
        }

        /**
         * 
         * @api {get/post} https://track.atom-data.io/ PutEvent
         * @apiVersion 1.0.0
         * @apiGroup IronSourceAtom
         * @apiDescription Send single data to Atom server 
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
         **/
       
        /// <summary>
        /// Send single data to Atom server .
        /// </summary>
        /// <param name="stream">
        /// Stream name for saving data in db table
        /// </param>
        /// <param name="data">
        /// Stream name for saving data in db table
        /// </param>
        /// <param name="method">
        ///  A <see cref="HttpMethod"/> for POST or GET method for do request
        /// </param>
        /// <param name="callback">
        /// A <see cref="string"/> for response data
        /// </param>/
        public void PutEvent(string stream, string data, HttpMethod method = HttpMethod.POST, 
                             Action<Response> callback = null) {
            string jsonEvent = GetRequestData(stream, data);
            SendEventCoroutine(endpoint_, method, headers_, jsonEvent, callback);
        }

        /// <summary>
        /// Puts the event.
        /// </summary>
        /// <param name="stream">
        /// A <see cref="string"/> for name of stream
        /// </param>
        /// <param name="data">
        /// A <see cref="string"/> for request data
        /// </param>
        /// <param name="method">
        /// A <see cref="HttpMethod"/> for type of request
        /// </param>
        /// <param name="callback">
        /// A <see cref="string"/> for reponse data
        /// </param>
        /// <param name="parrentGameObject">
        /// A <see cref="GameObject"/> for callback call.
        /// </param>
        public void PutEvent(string stream, string data, HttpMethod method = HttpMethod.POST, 
                             string callback = null, GameObject parrentGameObject = null) {
            string jsonEvent = GetRequestData(stream, data);
            SendEventCoroutine(endpoint_, method, headers_, jsonEvent, callback, parrentGameObject);
        }

        /**
         *
         * @api {get/post} https://track.atom-data.io/bulk PutEvents
         * @apiVersion 1.0.0
         * @apiGroup IronSourceAtom
         * @apiDescription Send multiple events data to Atom server
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
         **/

        /// <summary>
        /// Send multiple events data to Atom server
        /// </summary>
        /// <param name="stream">
        /// A <see cref="string"/> for name of stream
        /// </param>
        /// <param name="data">
        /// A <see cref="string"/> for request data
        /// </param>
        /// <param name="method">
        /// A <see cref="HttpMethod"/> for type of request
        /// </param>
        /// <param name="callback">
        /// A <see cref="Action<Response>"/> for reponse data
        /// </param>
        public void PutEvents(string stream, List<string> data, HttpMethod method = HttpMethod.POST, 
                              Action<Response> callback = null) {
            string json = IronSourceAtomUtils.ListToJson(data);
            Debug.Log ("Key: " + authKey_);

            string jsonEvent = GetRequestData(stream, json);

            SendEventCoroutine(endpoint_ + "bulk", method, headers_, jsonEvent, callback);
        }

        /// <summary>
        /// Puts the events.
        /// </summary>
        /// <param name="stream">
        /// A <see cref="string"/> for name of stream
        /// </param>
        /// <param name="data">
        /// A <see cref="string"/> for request data
        /// </param>
        /// <param name="method">
        /// A <see cref="HttpMethod"/> for type of request
        /// </param>
        /// <param name="callback">
        /// A <see cref="string"/> for reponse data
        /// </param>
        /// <param name="parrentGameObject">
        /// A <see cref="GameObject"/> for callback calling
        /// </param>
        public void PutEvents(string stream, List<string> data, HttpMethod method = HttpMethod.POST, 
                              string callback = null, GameObject parrentGameObject = null) {
            string json = IronSourceAtomUtils.ListToJson(data);
            Debug.Log ("Key: " + authKey_);

            string jsonEvent = GetRequestData(stream, json);

            SendEventCoroutine(endpoint_ + "bulk", method, headers_, jsonEvent, callback, parrentGameObject);
        }

        /// <summary>
        /// Create request json data
        /// </summary>
        /// <returns>The request data.</returns>
        /// <param name="stream">
        /// A <see cref="string"/> for request stream
        /// </param>
        /// <param name="data">
        /// A <see cref="string"/> for request data
        /// </param>
        private string GetRequestData(string stream, string data) {
            string hash = IronSourceAtomUtils.EncodeHmac(data, Encoding.ASCII.GetBytes(authKey_));

            var eventObject = new Dictionary<string, string>();
            eventObject ["table"] = stream;
            eventObject["data"] = IronSourceAtomUtils.EscapeStringValue(data);
            eventObject["auth"] = hash;
            string jsonEvent = IronSourceAtomUtils.DictionaryToJson(eventObject);

            Debug.Log("Request body: " + jsonEvent);

            return jsonEvent;
        }

        /// <summary>
        /// Check health of server
        /// </summary>
        /// <param name="callback">
        /// A <see cref="Action<Response>"/> for receive response from server
        /// </param>      
        public void Health(Action<Response> callback = null) {
            var eventObject = new Dictionary<string, string>();
            eventObject ["table"] = "helth_check";
            eventObject["data"] = null;
            string jsonEvent = IronSourceAtomUtils.DictionaryToJson(eventObject);

            SendEventCoroutine(endpoint_, HttpMethod.GET, headers_, jsonEvent, callback);
        }

        /**
         * Sending async data
         * 
         * @param {string} url
         * @param {HttpMethod} method - POST or GET method 
         * @param {Dictionary<string, string>} headers 
         * @param {string} data - request data
         * @param {Action<Response>} callback - receive response from server
         * 
         **/

        /// <summary>
        /// Check health of server
        /// </summary>
        /// <param name="url">
        /// A <see cref="string"/> for server address
        /// </param>
        /// <param name="method">
        /// A <see cref="HttpMethod"/> for POST or GET method 
        /// </param> 
        /// <param name="headers">
        /// A <see cref="Dictionary<string, string>"/>
        /// </param> 
        /// <param name="data">
        /// A <see cref="string"/> for request data
        /// </param> 
        /// <param name="callback">
        /// A <see cref="Action<Response>"/> for receive response from server
        /// </param> 
        private void SendEventCoroutine(string url, HttpMethod method, Dictionary<string, string> headers,
                                        string data, Action<Response> callback) {

            Request request = new Request(url, data, headers, callback);
            if (method == HttpMethod.GET) {
                coroutineHandler_.StartCoroutine(request.Get());
            } else {
                coroutineHandler_.StartCoroutine(request.Post());
            }
        }

        /// <summary>
        /// Check health of server
        /// </summary>
        /// <param name="url">
        /// A <see cref="string"/> for server address
        /// </param>
        /// <param name="method">
        /// A <see cref="HttpMethod"/> for POST or GET method 
        /// </param> 
        /// <param name="headers">
        /// A <see cref="Dictionary<string, string>"/>
        /// </param> 
        /// <param name="data">
        /// A <see cref="string"/> for request data
        /// </param> 
        /// <param name="callback">
        /// A <see cref="string"/> for receive response from server
        /// </param> 
        /// <param name="parrentGameObject">
        /// A <see cref="GameObject"/> for calling callback
        /// </param>
        private void SendEventCoroutine(string url, HttpMethod method, Dictionary<string, string> headers,
                                        string data, string callback, GameObject parrentGameObject) {
            if (parrentGameObject == null) {
                parrentGameObject = parentGameObject_;
            }

            Request request = new Request(url, data, headers, callback, parrentGameObject);
            if (method == HttpMethod.GET) {
                coroutineHandler_.StartCoroutine(request.Get());
            } else {
                coroutineHandler_.StartCoroutine(request.Post());
            }   
        }
    }
}
