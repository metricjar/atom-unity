﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ButtonEvent : MonoBehaviour {
    private ironsource.IronSourceAtom api_ = null;

    void Start() {
        api_ = new ironsource.IronSourceAtom(gameObject);   
        api_.EnableDebug(false);
        api_.SetAuth("");
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
        string errorStr = (response.error == null) ? "null" : "\"" + response.error + "\"";
        string dataStr = (response.data == null) ? "null" : "\"" + response.data + "\"";

        text.text = "{ \"err\": " + errorStr + ", \"data\": " + dataStr +
                        ", \"status\": " + response.status + "}";
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
            string errorStr = (response.error == null) ? "null" : "\"" + response.error + "\"";
            string dataStr = (response.data == null) ? "null" : "\"" + response.data + "\"";

            text.text = "{ \"err\": " + errorStr + ", \"data\": " + dataStr +
                        ", \"status\": " + response.status + "}";
        };

        List<string> events = new List<string>(); 
        events.Add("{\"event\": \"test get 1\"}");
        events.Add("{\"event\": \"test get 2\"}");
        events.Add("{\"event\": \"test get 3\"}");

        api_.PutEvents("ibtest", events, callback);
    }
}
