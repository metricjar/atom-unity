using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonEvent : MonoBehaviour {
    private ironsource.AtomAPI api_ = null;

    void Start() {
        api_ = GameObject.Find("test_scene_gui").GetComponent<ironsource.AtomAPI>();
    }

    public void onPostClick(){
        api_.PutEvent("g8y3eironsrc_g8y3e_test.public.atom_demo_events", "{\"event_name\": \"test post\"}");
    }

    public void onGetClick(){
        api_.PutEvent("g8y3eironsrc_g8y3e_test.public.atom_demo_events", "{\"event_name\": \"test get\"}", "get");
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

        api_.PutEvents("g8y3eironsrc_g8y3e_test.public.g8y3etest", events, "get");
    }
}
