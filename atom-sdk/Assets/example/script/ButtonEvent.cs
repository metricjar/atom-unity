﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ButtonEvent : MonoBehaviour {
    private ironsource.IronSourceAtom api_ = null;

    void Start() {
        api_ = GameObject.Find("test_scene_gui").GetComponent<ironsource.IronSourceAtom>();
    }

    public void onPostClick(){
        Action<ironsource.Response> callback = delegate(ironsource.Response response) {
            Debug.Log("from callback: status = " + response.status); 
        };

        api_.PutEvent("g8y3eironsrc_g8y3e_test.public.atom_demo_events", "{\"event_name\": \"test post\"}", 
                      ironsource.HttpMethod.POST, callback);
    }

    public void onGetClick(){
        api_.PutEvent("g8y3eironsrc_g8y3e_test.public.atom_demo_events", "{\"event_name\": \"test get\"}", 
                      ironsource.HttpMethod.GET);
    }

    public void onPostBulkClick() {
        List<string> events = new List<string>(); 
        events.Add("{\"event\": \"test post 1\"}");
        events.Add("{\"event\": \"test post 2\"}");
        events.Add("{\"event\": \"test post 3\"}");

        api_.PutEvents("g8y3eironsrc_g8y3e_test.public.g8y3etest", events);
    }

    public void onGetBulkClick() {
        List<string> events = new List<string>(); 
        events.Add("{\"event\": \"test get 1\"}");
        events.Add("{\"event\": \"test get 2\"}");
        events.Add("{\"event\": \"test get 3\"}");

        api_.PutEvents("g8y3eironsrc_g8y3e_test.public.g8y3etest", events, ironsource.HttpMethod.GET);
    }
}
