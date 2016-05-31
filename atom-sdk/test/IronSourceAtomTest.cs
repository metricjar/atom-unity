using UnityEngine;

using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ironsource {
    namespace test {   
        public class IronSourceAtomOpened : IronSourceAtom {

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

            protected override void initCoroutineHandler() {
            }

            public MonoBehaviour getCoroutineHandler() {
                return this.coroutineHandler_;
            }

            public string GetRequestDataOpened(string stream, string data) {
                return this.GetRequestData(stream, data);
            }
        }
           
        [TestFixture()]
        public class IronSourceAtomTest {
            IronSourceAtomOpened api_;

            [TestFixtureSetUp()] 
            public void Init() { 
                api_ = new IronSourceAtomOpened(null);
            }

            [Test()]
            public void TestCreateApi() {
                Dictionary<string, string> expectHeaders = new Dictionary<string, string>();
                expectHeaders.Add("x-ironsource-atom-sdk-type", "unity");
                expectHeaders.Add("x-ironsource-atom-sdk-version", api_.getVersion());

                Dictionary<string, string> resultHeaders = api_.getHeaders();
                foreach (var entry in expectHeaders) {
                    Assert.IsTrue(resultHeaders.ContainsKey(entry.Key));
                    Assert.AreEqual(resultHeaders[entry.Key], entry.Value);
                }
            } 

            [Test()]
            public void TestSetAuth() {
                string expectedAuth = "test_auth_key";
                api_.SetAuth(expectedAuth);

                Assert.AreEqual(expectedAuth, api_.getAuth());
            }


            [Test()]
            public void TestSetEndpoint() {
                string expectedEndpoint = "test_endpoint";
                api_.SetEndpoint(expectedEndpoint);

                Assert.AreEqual(expectedEndpoint, api_.getEndpoint());
            }

            [Test()]
            public void TestGetRequestData() {
                string expectedData = "{\"test\": \"data 1\"}";
                string expectedStream = "test_stream";

                string expectedStr = "{\"table\": \"" + expectedStream + "\",\"data\": \"" +
                    IronSourceAtomUtils.EscapeStringValue(expectedData) + "\",\"auth\": \"a2f9cfd6b52071018a90502b6db66e45a78cb29c36ab40f13938243e011ab901\"}";

                Assert.AreEqual(expectedStr, api_.GetRequestDataOpened(expectedStream, expectedData));
            }
        }
    }
}

