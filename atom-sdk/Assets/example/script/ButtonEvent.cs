using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ButtonEvent : MonoBehaviour {
    private ironsource.IronSourceAtom api_ = null;
    private ironsource.IronSourceAtomTracker tracker_ = null;

    void Start() {
        api_ = new ironsource.IronSourceAtom(gameObject);   
        api_.EnableDebug(true);
        api_.SetAuth("");

        tracker_ = new ironsource.IronSourceAtomTracker(gameObject); 
        tracker_.EnableDebug(true);
    }

    void Update() {
        tracker_.Update();
    }

    public void OnPostClick(){
        api_.PutEvent("ibtest", "{\"test\": \"data 1\"}", 
            ironsource.HttpMethod.POST, "ApiCallbackStr");
    }

    public void ApiCallbackStr(ironsource.Response response) {
        Debug.Log("response code from str: " + response.status);    
    }

    public void OnGetClick(){
        api_.PutEvent("ibtest", "{\"event_name\": \"test get\"}", 
                      ironsource.HttpMethod.GET, ButtonEvent.ApiCallback);
    }

    // antoher way of using api callback
    public static void ApiCallback(ironsource.Response response) {
        Debug.Log("from static callback: status = " + response.status); 
        Text text = GameObject.Find("response_data").GetComponent<Text>();

        text.text = response.ToString();
    }

    public void OnPostBulkClick() {
        List<string> events = new List<string>(); 
        events.Add("{\"event\": \"test post 1\"}");
        events.Add("{\"event\": \"test post 2\"}");
        events.Add("{\"event\": \"test post 3\"}");

        api_.PutEvents("ibtest", events, ButtonEvent.ApiCallback);
    }

    public void OnPostBulkClickDelegate() {
        Action<ironsource.Response> callback = delegate(ironsource.Response response) {
            Debug.Log("from callback: status = " + response.status); 
            Text text = GameObject.Find("response_data").GetComponent<Text>();

            text.text = response.ToString();
        };

        List<string> events = new List<string>(); 
        events.Add("{\"event\": \"test get 1\"}");
        events.Add("{\"event\": \"test get 2\"}");
        events.Add("{\"event\": \"test get 3\"}");

        api_.PutEvents("ibtest", events, callback);
    }

    public void OnTrackClick() {
        Action<string, string, Dictionary<string, ironsource.BulkData>> errorCallback = 
            delegate(string errorStr, string stream, Dictionary<string, ironsource.BulkData> data) {

            };
            
        tracker_.Track("ibtest", "{\"event\": \"test get 3\"}", errorCallback); 
    }

    public void OnFlushClick() {
        tracker_.Flush();
    }
}
