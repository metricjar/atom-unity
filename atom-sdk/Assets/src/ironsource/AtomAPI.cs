using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ironsource {
    public class AtomAPI : MonoBehaviour {
        private static string API_VERSION_ = "V1";

        public string endpoint = "https://track.atom-data.io/";
        public string authKey = "";

        private Dictionary<string, string> headers_ = new Dictionary<string, string>();

        public void Awake() {
            headers_.Add("x-ironsource-atom-sdk-type", "unity");
            headers_.Add("x-ironsource-atom-sdk-version", AtomAPI.API_VERSION_);
        }

        public void PutEvent(string stream, string data, string method = "post") {
            string hash = AtomAPIUtils.EncodeHmac(data, Encoding.ASCII.GetBytes(authKey));

            var eventObject = new Dictionary<string, string>();
            eventObject ["table"] = stream;
            eventObject["data"] = AtomAPIUtils.EscapeStringValue(data);
            eventObject["auth"] = hash;
            string jsonEvent = AtomAPIUtils.DictionaryToJson(eventObject);

            Debug.Log("Request body: " + jsonEvent);

            this.StartCoroutine(SendEventCoroutine(endpoint, method, headers_, jsonEvent));
        }

        public void PutEvents(string stream, List<string> data, string method = "post") {
            string json = AtomAPIUtils.ListToJson(data);
            Debug.Log ("Key: " + authKey);

            string hash = AtomAPIUtils.EncodeHmac(json, Encoding.ASCII.GetBytes(authKey));

            var eventObject = new Dictionary<string, string>();
            eventObject ["table"] = stream;
            eventObject["data"] = AtomAPIUtils.EscapeStringValue(json);
            eventObject["auth"] = hash;
            string jsonEvent = AtomAPIUtils.DictionaryToJson(eventObject);

            Debug.Log("Request body: " + jsonEvent);

            this.StartCoroutine(SendEventCoroutine(endpoint + "bulk", method, headers_, jsonEvent));
        }

        private static IEnumerator SendEventCoroutine(string url, string method, Dictionary<string, string> headers,
        									  string jsonEvent) {
            WWW www = null;
            if (method.ToLower() == "get") {
                url = url + "?data=" + AtomAPIUtils.Base64Encode(jsonEvent);
                Debug.Log("Request URL: " + url);

                www = new WWW(url, null, headers);
            } else {
                Debug.Log("Request URL: " + url);
                www = new WWW(url, Encoding.ASCII.GetBytes(jsonEvent), headers);
            }
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
                Debug.Log(www.error);   
            else {

                Debug.Log("Response: " + www.text);
            }
        }
    }
}
