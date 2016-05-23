using UnityEngine;
using System.Collections;

public class ButtonEvent : MonoBehaviour {
	private AtomAPI api = null;

	// Use this for initialization
	void Start() {
		api = GameObject.Find("test_scene_gui").GetComponent<AtomAPI> ();
	}

	public void onPostClick(){
		// Save game data

		api.PutEvent("g8y3eironsrc_g8y3e_test.public.atom_demo_events", "{\"event_name\": \"test post\"}");
	}

	public void onGetClick(){
		// Save game data

		api.PutEvent("g8y3eironsrc_g8y3e_test.public.atom_demo_events", "{\"event_name\": \"test get\"}", "get");
	}
}
