# ironSource.atom SDK for Unity 3D

[![License][license-image]][license-url]
[![Docs][docs-image]][docs-url]

atom-unity is the official [ironSource.atom](http://www.ironsrc.com/data-flow-management) SDK for the Unity 3D engine.

- [Signup](https://atom.ironsrc.com/#/signup)
- [Documentation](https://ironsource.github.io/atom-unity/)
- [Installation](#Installation)
- [Sending an event](#Using-the-IronSource-API-to-send-events)

#### Installation

#### Using the IronSource API to send events
Example of sending an event in C#:
```c#
public class ButtonEvent : MonoBehaviour {
    private ironsource.IronSourceAtom api_ = null;

    void Start() {
        api_ = new ironsource.IronSourceAtom(transform);       
        api_.SetAuth("<YOUR_AUTH_KEY>");
    }

    // send single event
    public void OnPostClick(){
    	// delegate callback type
        Action<ironsource.Response> callback = delegate(ironsource.Response response) {
            Debug.Log("from callback: status = " + response.status); 
        };

        api_.PutEvent("<YOUR_STREAM_NAME>", "{\"name\": \"iron\"}", 
                      ironsource.HttpMethod.POST, callback);
    }

    // antoher way of using api callback
    public static void ApiCallback(ironsource.Response response) {
    	Debug.Log("from callback: status = " + response.status); 
    }

    // send list of events
    public void OnPostBulkClick() {
        List<string> events = new List<string>(); 
        events.Add("{\"name\": \"iron 1\"}");
        events.Add("{\"name\": \"iron 2\"}");
        events.Add("{\"name\": \"iron 3\"}");

        api_.PutEvents("<YOUR_STREAM_NAME>", events, 
        			   ironsource.HttpMethod.POST, ButtonEvent.ApiCallback);
    }
}
```

Example of sending an event in JavaScript:
```js
private var api_ : ironsource.IronSourceAtom;

function Start() {
	api_ = new ironsource.IronSourceAtom(transform);       
    api_.SetAuth("<YOUR_AUTH_KEY>");
}

function ApiCallback(response : ironsource.Response) {
 	Debug.Log("from callback: status = " + response.status); 	
}

// send single event
function OnPostClick(){
    api_.PutEvent("<YOUR_STREAM_NAME>", "{\"event_name\": \"test post\"}", 
                  ironsource.HttpMethod.POST, ApiCallback);
}

//send list of events
function OnPostBulkClick() {
    var events :List.<String> = new List.<String>();
    events.Add("{\"name\": \"iron 1\"}");
    events.Add("{\"name\": \"iron 2\"}");
    events.Add("{\"name\": \"iron 3\"}");

    api_.PutEvents("<YOUR_STREAM_NAME>", events, 
                   ironsource.HttpMethod.POST, ApiCallback);
}
```


### Example

![alt text][example]

### License
MIT

[license-image]: https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square
[license-url]: LICENSE
[docs-image]: https://img.shields.io/badge/docs-latest-blue.svg
[docs-url]: https://ironsource.github.io/atom-unity/
[example]: https://cloud.githubusercontent.com/assets/1713228/15546177/954a8b6a-22a7-11e6-8017-1a27bde809a5.png "example"
