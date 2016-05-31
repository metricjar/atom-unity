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

            protected override void initCoroutineHandler() {
            }

            public MonoBehaviour getCoroutineHandler() {
                return this.coroutineHandler_;
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
        }
    }
}

