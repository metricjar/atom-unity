#pragma strict
import UnityEngine;
import System;
import System.Collections;
import System.Collections.Generic;

private var api_ : ironsource.IronSourceAtom;

function Start() {
	api_ = GameObject.Find("test_scene_gui").GetComponent(ironsource.IronSourceAtom);
}

function ApiCallback(response : ironsource.Response) {
 	Debug.Log("from callback: status = " + response.status); 	
}

function onPostClick(){
    api_.PutEvent("g8y3eironsrc_g8y3e_test.public.atom_demo_events", "{\"event_name\": \"test post\"}", 
                  ironsource.HttpMethod.POST, ApiCallback);
}

function onGetClick(){
	Debug.Log("Test");

    api_.PutEvent("g8y3eironsrc_g8y3e_test.public.atom_demo_events", "{\"event_name\": \"test get\"}", 
                  ironsource.HttpMethod.GET, ApiCallback);
}

function onPostBulkClick() {
	var events :List.<String> = new List.<String>();
    events.Add("{\"event\": \"test post 1\"}");
    events.Add("{\"event\": \"test post 2\"}");
    events.Add("{\"event\": \"test post 3\"}");

    api_.PutEvents("g8y3eironsrc_g8y3e_test.public.g8y3etest", events, ironsource.HttpMethod.POST, ApiCallback);
}

function onGetBulkClick() {
    var events :List.<String> = new List.<String>();
    events.Add("{\"event\": \"test get 1\"}");
    events.Add("{\"event\": \"test get 2\"}");
    events.Add("{\"event\": \"test get 3\"}");

    api_.PutEvents("g8y3eironsrc_g8y3e_test.public.g8y3etest", events, ironsource.HttpMethod.GET, ApiCallback);
}