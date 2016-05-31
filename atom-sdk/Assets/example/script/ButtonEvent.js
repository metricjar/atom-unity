#pragma strict

import UnityEngine.UI;

import System;
import System.Collections.Generic;

private var api_ : ironsource.IronSourceAtom;

function Start() {
	api_ = new ironsource.IronSourceAtom(gameObject);  
	api_.EnableDebug(true);     
    api_.SetAuth("");
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

    api_.PutEvent("ibtest", "{\"event_name\": \"test post\"}", 
                  	ironsource.HttpMethod.POST, ApiCallback);
}

function OnGetClick(){
	api_.Health(ApiHealthCallback);

    api_.PutEvent("ibtest", "{\"event_name\": \"test get\"}", 
                  	ironsource.HttpMethod.GET, ApiCallback);
}

function OnPostBulkClick() {
	var events :List.<String> = new List.<String>();
    events.Add("{\"event\": \"test post 1\"}");
    events.Add("{\"event\": \"test post 2\"}");
    events.Add("{\"event\": \"test post 3\"}");

    api_.PutEvents("ibtest", events, ApiCallback);
}
