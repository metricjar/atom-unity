using UnityEngine;

using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ironsource {
	namespace test {  
		[TestFixture]
		public class BulkDataTest {
			[Test]
			public void TestDataSize() {
				BulkData data = new BulkData();

				int expectedSize = 3;

				data.AddData("1");
				data.AddData("2");
				data.AddData("3");

				Assert.AreEqual(expectedSize, data.GetSize());
			}

			[Test]
			public void TestGetData() {
				BulkData data = new BulkData();

				data.AddData("1");
				data.AddData("2");
				data.AddData("3");

				string expectedStr = "[1,2,3]";

				Assert.AreEqual(expectedStr, data.GetStringData());
			}

            [Test]
            public void TestClearData() {
                BulkData data = new BulkData();

                data.AddData("1");
                data.AddData("2");
                data.AddData("3");

                int expectedSize = 0;
                data.ClearData();

                Assert.AreEqual(expectedSize, data.GetSize());
            }

            [Test]
            public void TestGetData() {
                BulkData data = new BulkData();

                data.AddData("1");
                data.AddData("2");
                data.AddData("3");

                List<string> expectedList = new List<string>();
                expectedList.Add("1");
                expectedList.Add("2");
                expectedList.Add("3");

                List<string> compareList = data.GetData();

                for (int i = 0; i < expectedList.Count; ++i) {
                    Assert.AreEqual(expectedList[i], compareList[i]);
                }
            }
		}
	}
}
