# ironSource.atom SDK for Unity 3D

[![License][license-image]][license-url]
[![Docs][docs-image]][docs-url]
[![Build status][travis-image]][travis-url]
[![Coverage Status][coverage-image]][coverage-url]

atom-unity is the official [ironSource.atom](http://www.ironsrc.com/data-flow-management) SDK for the Unity 3D engine.

- [Signup](https://atom.ironsrc.com/#/signup)
- [Documentation](https://ironsource.github.io/atom-unity/)
- [Installation](#installation)
- [Usage](#usage)
- [Sending an event](#Using-the-IronSource-API-to-send-events)

## Installation

## Usage

You may use the SDK in two different ways:

1. High level "Tracker" - contains in-memory storage and tracks events based on certain parameters.
2. Low level - contains 2 methods: PutEvent() and PutEvents() to send 1 event or a batch respectively.

### High Level SDK - "Tracker"

The Tracker process:

You can use Track() method in order to track the events to an Atom Stream.
The tracker accumulates events and flushes them when it meets one of the following conditions:

- Every 10 seconds (default)
- Number of accumulated events has reached 64 (default)
- Size of accumulated events has reached 64KB (default)

In case of failure the tracker will preform an exponential backoff.
In case when maximum of backlog size will be reached - then will be called error callback from Track method.
The tracker stores events in memory.

Example of using tracker in C#:
```c#
public class ButtonEvent : MonoBehaviour {
    private ironsource.IronSourceAtomTracker tracker_ = null;

    void Start() {
        tracker_ = new ironsource.IronSourceAtomTracker(gameObject); 
        // enable print logs
        tracker_.EnableDebug(true);
        tracker_.SetAuth("<YOUR_AUTH_KEY>");
    }

    void Update() {
        tracker_.Update();
    }

    public void OnTrackClick() {
        Action<string, string, Dictionary<string, ironsource.BulkData>> errorCallback = 
            delegate(string errorStr, string stream, Dictionary<string, ironsource.BulkData> data) {

        };
            
        tracker_.Track("<YOUR_STREAM_NAME>", "{\"event\": \"test get 3\"}", errorCallback); 
    }

    public void OnFlushClick() {
        tracker_.Flush();
    }
}
```

Example of using tracker in JavaScript:
```js
private var tracker_ : ironsource.IronSourceAtomTracker;

function Start() {
    tracker_ = new ironsource.IronSourceAtomTracker(gameObject);  
    tracker_.EnableDebug(true);     
    tracker_.SetAuth("<YOUR_AUTH_KEY>");
}

function Update() {
    tracker_.Update();
}

function ApiErrorCallback(errorStr: String, stream: String, 
                              data: Dictionary.<String, ironsource.BulkData>) {

}

function OnTrackClick(){
    tracker_.Track("<YOUR_STREAM_NAME>", "{\"event\": \"test get 3\"}", ApiErrorCallback));
}

function OnFlushClick() {
    tracker_.Flush();
}

```

### Low Level (Basic) SDK

The Low Level SDK has 2 methods:  

- PutEvent - Sends a single event to Atom  
- PutEvents - Sends a bulk (batch) of events to Atom

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


## Example
You can use our [example][example-url] for sending data to Atom:

![alt text][example]

## License
[MIT](LICENSE)

[license-image]: https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square
[license-url]: LICENSE
[docs-image]: https://img.shields.io/badge/docs-latest-blue.svg
[docs-url]: https://ironsource.github.io/atom-unity/
[travis-image]: https://travis-ci.org/ironSource/atom-unity.svg?branch=master
[travis-url]: https://travis-ci.org/ironSource/atom-unity
[coverage-image]: https://coveralls.io/repos/github/ironSource/atom-unity/badge.svg?branch=master
[coverage-url]: https://coveralls.io/github/ironSource/atom-unity?branch=master
[example]: https://cloud.githubusercontent.com/assets/1713228/22325892/126f2358-e3b9-11e6-9852-4339748a9ff8.png "example"
