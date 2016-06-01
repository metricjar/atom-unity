using UnityEngine;

using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ironsource {
    namespace test {   
        public class IronSourceAtomOpened : IronSourceAtom {

        	public string currentData;

            public IronSourceAtomOpened(GameObject gameObject) : base(gameObject) {
            }

            public Dictionary<string, string> getHeaders() {
                return this.headers_;
            }

            public string getVersion() {
                return IronSourceAtom.API_VERSION_;
            }

            public string getAuth() {
                return this.authKey_;
            }

            public string getEndpoint() {
                return this.endpoint_;
            }

			public override void SetAuth(string authKey) {
				base.SetAuth(authKey);
			}

            protected override void initCoroutineHandler() {
            }

            public MonoBehaviour getCoroutineHandler() {
                return this.coroutineHandler_;
            }

            public string GetRequestDataOpened(string stream, string data) {
                return base.GetRequestData(stream, data);
            }

            protected override void SendEventCoroutine(string url, HttpMethod method, 
            							Dictionary<string, string> headers,
                                        string data, Action<Response> callback) {
            	currentData = data;
            }

            protected override void SendEventCoroutine(string url, HttpMethod method, 
                Dictionary<string, string> headers,
                string data, string callback, GameObject parrentGameObject) {
                currentData = data;
            }
        }
           
        [TestFixture()]
        public class IronSourceAtomTest {
            [Test()]
            public void TestCreateApi() {
				IronSourceAtomOpened api = new IronSourceAtomOpened(null);
                Dictionary<string, string> expectHeaders = new Dictionary<string, string>();
                expectHeaders.Add("x-ironsource-atom-sdk-type", "unity");
				expectHeaders.Add("x-ironsource-atom-sdk-version", api.getVersion());

				Dictionary<string, string> resultHeaders = api.getHeaders();
                foreach (var entry in expectHeaders) {
                    Assert.IsTrue(resultHeaders.ContainsKey(entry.Key));
                    Assert.AreEqual(resultHeaders[entry.Key], entry.Value);
                }
            } 

            [Test()]
            public void TestSetAuth() {
				IronSourceAtom api = new IronSourceAtomOpened(null);
                string expectedAuth = "test_auth_key";
				api.SetAuth(expectedAuth);

				Assert.AreEqual(expectedAuth, ((IronSourceAtomOpened)api).getAuth());
            }


            [Test()]
            public void TestSetEndpoint() {
				IronSourceAtomOpened api = new IronSourceAtomOpened(null);
                string expectedEndpoint = "test_endpoint";
				api.SetEndpoint(expectedEndpoint);

				Assert.AreEqual(expectedEndpoint, api.getEndpoint());
            }

            [Test()]
            public void TestGetRequestData() {
				IronSourceAtomOpened api = new IronSourceAtomOpened(null);
                string expectedData = "{\"test\": \"data 1\"}";
                string expectedStream = "test_stream";

                string expectedStr = "{\"table\": \"" + expectedStream + "\",\"data\": \"" +
                    IronSourceAtomUtils.EscapeStringValue(expectedData) + "\",\"auth\": \"a2f9cfd6b52071018a90502b6db66e45a78cb29c36ab40f13938243e011ab901\"}";

				Assert.AreEqual(expectedStr, api.GetRequestDataOpened(expectedStream, expectedData));
            }

            [Test()]
            public void TestHealth() {
                api_.Health();

                string expectedData = "";

                Assert.AreEqual(expectedData, api_.currentData);
            }

            [Test()]
            public void TestPutEvent() {
                string expectedData = "{\"test\": \"data 1\"}";
                string expectedStream = "test_stream";

                string expectedStr = "{\"table\": \"" + expectedStream + "\",\"data\": \"" +
                    IronSourceAtomUtils.EscapeStringValue(expectedData) + "\",\"auth\": \"a2f9cfd6b52071018a90502b6db66e45a78cb29c36ab40f13938243e011ab901\"}";
                

                // test post method
                api_.PutEvent(expectedStream, expectedData, HttpMethod.POST, null);
                Assert.AreEqual(expectedStr, api_.currentData);
            }

            [Test()]
            public void TestPutEvents() {
                List<string> events = new List<string>(); 
		        events.Add("{\"event\": \"test post 1\"}");
		        events.Add("{\"event\": \"test post 2\"}");
		        events.Add("{\"event\": \"test post 3\"}");

                string expectedStream = "test_stream";

                string expectedStr = "{\"table\": \"" +
                               expectedStream + "\",\"data\": \"" +
                               IronSourceAtomUtils.EscapeStringValue(IronSourceAtomUtils.ListToJson(events)) +
                               "\",\"auth\": \"9630f4c8049d06f27a8c53be3eee8974bc35355b778f3cb5a8af20b7de2380ab\"}";

                api_.PutEvents(expectedStream, events, "", null);
                Assert.AreEqual(expectedStr, api_.currentData);

                api_.PutEvents(expectedStream, events, null);
                Assert.AreEqual(expectedStr, api_.currentData);
            }
        }
    }
}

