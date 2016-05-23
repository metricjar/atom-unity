using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ironsource {
	public class AtomAPI : MonoBehaviour {
		private static string API_VERSION = "V1";

		public string endpoint = "https://track.atom-data.io/";
		public string auth = "";

		private Dictionary<string, string> headers = new Dictionary<string, string>();

		protected void Awake() {		
			headers.Add("x-ironsource-atom-sdk-type", "unity");
			headers.Add("x-ironsource-atom-sdk-version", AtomAPI.API_VERSION);
		}

		public void PutEvent(string stream, string data, string method = "post") {
			string hash = "";
			//if (auth.Length != 0) {
				hash = AtomAPIUtils.EncodeHmac(data, Encoding.ASCII.GetBytes(auth));
			//}

			var eventObject = new Dictionary<string, string>();
			eventObject ["table"] = stream;
			eventObject["data"] = AtomAPIUtils.EscapeStringValue(data);
			eventObject["auth"] = hash;
			string jsonEvent = AtomAPIUtils.DictionaryToJson(eventObject);

			Debug.Log("Request body: " + jsonEvent);

			this.StartCoroutine(SendEventCoroutine(endpoint, method, headers, jsonEvent));
		}

		public void PutEvents(string stream, List<string> data, string method = "post") {
			string json = AtomAPIUtils.ListToJson(data);
			string hash = "";
			//if (auth.Length != 0) {
			hash = AtomAPIUtils.EncodeHmac(json, Encoding.ASCII.GetBytes(auth));
			//}

			var eventObject = new Dictionary<string, string>();
			eventObject ["table"] = stream;
			eventObject["data"] = json;
			eventObject["auth"] = hash;
			string jsonEvent = AtomAPIUtils.DictionaryToJson(eventObject);

			this.StartCoroutine(SendEventCoroutine(endpoint + "bulk", method, headers, jsonEvent));
		}

		private static IEnumerator SendEventCoroutine(string url, string method, Dictionary<string, string> headers,
													  string jsonEvent) {
			WWW www = null;
			if (method.ToLower() == "get") {
				url = url + "?data=" + AtomAPIUtils.Base64Encode(jsonEvent);
				Debug.Log("Request URL: " + url);

				www = new WWW(url, null, headers);
			} else {
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
