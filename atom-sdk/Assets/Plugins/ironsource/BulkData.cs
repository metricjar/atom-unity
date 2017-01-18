using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ironsource {
	public class BulkData {
		protected List<string> bulkData_;

        public BulkData() {
            bulkData_ = new List<string>();
        }

        public BulkData(BulkData bulkData):
            this() {
            List<String> bulkDataList = bulkData.GetData();
            foreach(var data in bulkDataList) {
                AddData(data);
            }
        }

        /// <summary>
        /// Adds the data.
        /// </summary>
        /// <param name="data">
        /// Data.
        /// </param>
		public void AddData(string data) {
			bulkData_.Add(data);
		}


        /// <summary>
        /// Adds the bulk data.
        /// </summary>
        /// <param name="data">
        /// Data.
        /// </param>
        public void AddBulkData(BulkData bulkData) {
            List<String> bulkDataList = bulkData.GetData();
            foreach(var data in bulkDataList) {
                AddData(data);
            }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <returns>
        /// The size.
        /// </returns>
		public int GetSize() {
			return bulkData_.Count;
		}

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>
        /// The data.
        /// </returns>
		public List<string> GetData() {
			return bulkData_;
		}

        /// <summary>
        /// Gets the string data.
        /// </summary>
        /// <returns>
        /// The string data.
        /// </returns>
		public string GetStringData() {
			return IronSourceAtomUtils.ListToJson(bulkData_);
		}

        /// <summary>
        /// Clears the data.
        /// </summary>
		public void ClearData() {
			bulkData_.Clear();
		}
 	}
}
