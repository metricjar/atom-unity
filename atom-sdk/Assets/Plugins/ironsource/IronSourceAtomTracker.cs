using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ironsource {
    public class IronSourceAtomTracker {
        protected float flushIntervals_;
        protected float bulkSize_;
        protected float bulkBytesSize_;

        protected IronSourceAtom api_;
        protected bool isDebug_;

		protected Dictionary<string, BulkData> bulkDataMap_;

		/// <summary>
		/// Initializes a new instance of the <see cref="ironsource.IronSourceAtomTracker"/> class.
		/// </summary>
		/// <param name="gameObject">
		/// Game object.
		/// </param>
        public IronSourceAtomTracker(GameObject gameObject) {
            api_ = new IronSourceAtom(gameObject);

			bulkDataMap_ = new Dictionary<string, BulkData>();

            isDebug_ = false;
        }

		/// <summary>
		/// Enables the debug mode.
		/// </summary>
		/// <param name="isDebug">
		/// If set to <c>true</c> is debug.
		/// </param>
        public void EnableDebug(bool isDebug) {
            isDebug_ = isDebug;
        }

		/// <summary>
		/// Prints data to unity log.
		/// </summary>
		/// <param name="data">
		/// Data for logger.
		/// </param>
        protected void printLog(string data) {
            if (isDebug_) {
                Debug.Log(data);
            }
        }

		/// <summary>
		/// Sets the auth.
		/// </summary>
		/// <param name="authKey">
		/// Auth key.
		/// </param>
		public void SetAuth(string authKey) {
			api_.SetAuth(authKey);
		}

		/// <summary>
		/// Sets the endpoint.
		/// </summary>
		/// <param name="endpoint">
		/// Atom server address.
		/// </param>
		public void SetEndpoint(string endpoint) {
			api_.SetEndpoint(endpoint);
		}

		/// <summary>
		/// Track the specified stream and data.
		/// </summary>
		/// <param name="stream">
		/// Name of stream.
		/// </param>
		/// <param name="data">
		/// Data.
		/// </param>
        public void track(string stream, string data) {
			if (!bulkDataMap_.ContainsKey(stream)) {
				bulkDataMap_[stream] = new BulkData();
			}

			BulkData bulkData = bulkDataMap_[stream];
            bulkData.AddData(data);
            if (bulkData.GetSize() >= bulkSize_) {
                flush(stream);        
            }
        }

        /// <summary>
        /// Sends the data.
        /// </summary>
        /// <param name="stream">
        /// Stream.
        /// </param>
        /// <param name="data">
        /// Data.
        /// </param>
        /// <param name="dataSize">
        /// Data size.
        /// </param>
        /// <param name="method">
        /// Method.
        /// </param>
        /// <param name="callback">
        /// Callback.
        /// </param>
        protected void SendData(string stream, string data, int dataSize = 1, 
                                HttpMethod method = HttpMethod.POST, 
                                Action<ironsource.Response> callback = null) {
            if (dataSize == 1) {
                api_.PutEvent(stream, data, method, callback);
            } else if (dataSize > 1){
                api_.PutEvents(stream, data, callback);
            }
        }

        /// <summary>
        /// Flushs the data.
        /// </summary>
        /// <param name="stream">
        /// Stream.
        /// </param>
        /// <param name="bulkData">
        /// Bulk data.
        /// </param>
        protected void flushData(string stream, BulkData bulkData) {
            string bulkStrData = bulkData.GetStringData();
            int bulkSize = bulkData.GetSize();
            bulkData.ClearData();

			Action<ironsource.Response> callback = null;
			callback = delegate(ironsource.Response response) {
                printLog("from callback: status = " + response.status);   

                if (response.status != 200) {
                    SendData(stream, bulkStrData, bulkSize, HttpMethod.POST, callback);
                }             
            };

            SendData(stream, bulkStrData, bulkSize, HttpMethod.POST, callback);
        }

		/// <summary>
        /// Flush the specified stream.
        /// </summary>
        /// <param name="stream">
        /// Stream.
        /// </param>
        public void flush(string stream) {
			if (stream.Length > 0) {
                if (bulkDataMap_.ContainsKey(stream)) {
                    BulkData bulkData = bulkDataMap_[stream];
                    flushData(stream, bulkData);
                }
            } else {
                printLog("Wrong stream name!");
            }                  
		}

        /// <summary>
        /// Flush this instance.
        /// </summary>
        public void flush() {
            foreach (var bulkDataEntry in bulkDataMap_) {
				flushData(bulkDataEntry.Key, bulkDataEntry.Value);
            }
        }
    }
}