using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ironsource {
	public class BulkData {
		protected List<string> bulkData_;

       // #if CODE_COVERAGE 
        [ExcludeFromCodeCoverage]
       // #endif
        public BulkData() {
            bulkData_ = new List<string>();
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
