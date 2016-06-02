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

				data.AddData ("1");
				data.AddData ("2");
				data.AddData ("3");

				string expectedStr = "[1,2,3]";

				Assert.AreEqual(expectedStr, data.GetStringData());
			}
		}
	}
}
