# ironSource.atom SDK for Unity 3D

[![License][license-image]][license-url]
[![Docs][docs-image]][docs-url]
[![Build status][travis-image]][travis-url]
[![Coverage Status][coverage-image]][coverage-url]

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
        api_ = new ironsource.IronSourceAtom(gameObject);  
        // enabling print logs
        api_.EnableDebug(true);   
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

        api_.PutEvents("<YOUR_STREAM_NAME>", events, ButtonEvent.ApiCallback);
    }
}
```

Example of sending an event in JavaScript:
```js
private var api_ : ironsource.IronSourceAtom;

function Start() {
	api_ = new ironsource.IronSourceAtom(gameObject);  
    // enabling print logs
    api_.EnableDebug(true);        
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

    api_.PutEvents("<YOUR_STREAM_NAME>", events, ApiCallback);
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
[travis-image]: https://travis-ci.org/ironSource/atom-unity.svg?branch=feature%2FISA-226
[travis-url]: https://travis-ci.org/ironSource/atom-unity
[coverage-image]: https://coveralls.io/repos/github/ironSource/atom-unity/badge.svg?branch=feature%2FISA-226
[coverage-url]: https://coveralls.io/github/ironSource/atom-unity?branch=feature%2FISA-226
[example]: https://cloud.githubusercontent.com/assets/1713228/15683093/590c2386-2769-11e6-8429-14f41e937f27.png "example"
