using UnityEngine;

using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ironsource {
	namespace test {  
		[TestFixture]
		public class BulkData_Tests {
			[Test]
			public void GetSize_Test() {
				BulkData data = new BulkData();

				int expectedSize = 3;

				data.AddData("1");
				data.AddData("2");
				data.AddData("3");

				Assert.AreEqual(expectedSize, data.GetSize());
			}

			[Test]
			public void GetStringData_Test() {
				BulkData data = new BulkData();

				data.AddData("1");
				data.AddData("2");
				data.AddData("3");

				string expectedStr = "[1,2,3]";

				Assert.AreEqual(expectedStr, data.GetStringData());
			}

            [Test]
            public void ClearData_Test() {
                BulkData data = new BulkData();

                int expectedSize = 0;

                Assert.AreEqual(expectedSize, data.GetSize());
            }

            [Test]
            public void GetData_Test() {
                BulkData data = new BulkData();

                data.AddData("1");
                data.AddData("2");
                data.AddData("3");

                List<string> expectedList = new List<string>();
                expectedList.Add("1");
                expectedList.Add("2");
                expectedList.Add("3");

                //List<string> compareList = data.GetData();

                for (int i = 0; i < expectedList.Count; ++i) {
                    Assert.AreEqual(expectedList[i], data.GetData()[i]);
                }
            }
		}
	} 
}
