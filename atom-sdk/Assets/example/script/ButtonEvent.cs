using UnityEngine;
using System.Collections;

public class ButtonEvent : MonoBehaviour {
    private ironsource.AtomAPI api_ = null;

    void Start() {
        api_ = GameObject.Find("test_scene_gui").GetComponent<ironsource.AtomAPI> ();
    }

    public void onPostClick(){
        api_.PutEvent("g8y3eironsrc_g8y3e_test.public.atom_demo_events", "{\"event_name\": \"test post\"}");
    }

    public void onGetClick(){
        api_.PutEvent("g8y3eironsrc_g8y3e_test.public.atom_demo_events", "{\"event_name\": \"test get\"}", "get");
    }
}
