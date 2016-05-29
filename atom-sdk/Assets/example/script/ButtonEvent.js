#pragma strict

import UnityEngine.UI;

import System;
import System.Collections.Generic;

private var api_ : ironsource.IronSourceAtom;

function Start() {
	api_ = new ironsource.IronSourceAtom(gameObject);       
    api_.SetAuth("yYFxqzZj2AYO2ytya5hsPAwTbyY40b");
}

function ApiCallback(response : ironsource.Response) {
 	Debug.Log("from callback: status = " + response.status); 

	var text : Text = GameObject.Find("response_data").GetComponent.<Text>();

	var errorStr = (response.error == null) ? "null" : "\"" + response.error + "\"";
    var dataStr = (response.data == null) ? "null" : "\"" + response.data + "\"";

    text.text = "{ \"err\": " + errorStr + ", \"data\": " + dataStr +
        			", \"status\": " + response.status + "}";	
}

function ApiHealthCallback(response : ironsource.Response) {
	Debug.Log("from health callback: status = " + response.status); 
}

function OnPostClick(){
	api_.Health(ApiHealthCallback);

    api_.PutEvent("g8y3eironsrc_g8y3e_test.public.atom_demo_events", "{\"event_name\": \"test post\"}", 
                  	ironsource.HttpMethod.POST, ApiCallback);
}

function OnGetClick(){
	Debug.Log("Test");
	api_.Health(ApiHealthCallback);

    api_.PutEvent("g8y3eironsrc_g8y3e_test.public.atom_demo_events", "{\"event_name\": \"test get\"}", 
                  	ironsource.HttpMethod.GET, ApiCallback);
}

function OnPostBulkClick() {
	var events :List.<String> = new List.<String>();
    events.Add("{\"event\": \"test post 1\"}");
    events.Add("{\"event\": \"test post 2\"}");
    events.Add("{\"event\": \"test post 3\"}");

    api_.PutEvents("g8y3eironsrc_g8y3e_test.public.g8y3etest", events, ironsource.HttpMethod.POST, ApiCallback);
}

function OnGetBulkClick() {
    var events :List.<String> = new List.<String>();
    events.Add("{\"event\": \"test get 1\"}");
    events.Add("{\"event\": \"test get 2\"}");
    events.Add("{\"event\": \"test get 3\"}");

    api_.PutEvents("g8y3eironsrc_g8y3e_test.public.g8y3etest", events, ironsource.HttpMethod.GET, ApiCallback);
}