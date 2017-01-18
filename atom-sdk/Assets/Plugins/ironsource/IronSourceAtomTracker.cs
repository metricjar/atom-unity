using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ironsource {
    public class IronSourceAtomTracker {
        protected float deltaTime_ = 0;

        protected float flushInterval_ = 10;
        protected float bulkSize_ = 64;
        protected float bulkBytesSize_ = 64 * 1024;

        protected float retryTimeout_ = 1;
        protected int backlogSize_ = 500;

        protected IronSourceAtom api_;
        protected bool isDebug_ = false;

        protected Dictionary<string, BulkData> bulkDataMap_ = new Dictionary<string, BulkData>();

        protected Dictionary<string, bool> isStreamFlush_ = new Dictionary<string, bool>();
        protected Dictionary<string, bool> queueFlush_ = new Dictionary<string, bool>();

		/// <summary>
		/// Initializes a new instance of the <see cref="ironsource.IronSourceAtomTracker"/> class.
		/// </summary>
		/// <param name="gameObject">
		/// Game object.
		/// </param>
        public IronSourceAtomTracker(GameObject gameObject) {
            api_ = new IronSourceAtom(gameObject);
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
        /// Check is Debug mode enabled
        /// </summary>
        /// <returns>
        /// Is Debug Mode
        /// </returns>
        public bool IsDebug() {
            return isDebug_;
        }

		/// <summary>
		/// Prints data to unity log.
		/// </summary>
		/// <param name="data">
		/// Data for logger.
		/// </param>
        protected void PrintLog(string data) {
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
        /// Sets flush interval.
        /// </summary>
        /// <param name="flushInterval">
        /// Timer interval for flushing data.
        /// </param>
        public void SetFlusInterval(float flushInterval) {
            flushInterval_ = flushInterval;
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
        public void Track(string stream, string data, 
            Action<string, string, Dictionary<string, ironsource.BulkData>> errorCallback = null) {
			if (!bulkDataMap_.ContainsKey(stream)) {
				bulkDataMap_[stream] = new BulkData();
			}

            if (bulkDataMap_[stream].GetSize() >= backlogSize_) {
                string errorStr = "Message store for stream: '" + stream + "' has reached its maximum size!";
                PrintLog (errorStr);
                if (errorCallback != null) {
                    errorCallback (errorStr , stream, bulkDataMap_);
                }
                return;
            }

			BulkData bulkData = bulkDataMap_[stream];
            bulkData.AddData(data);
            if (bulkData.GetSize() >= bulkSize_) {
                Flush(stream);        
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
        protected IEnumerator SendData(string stream, string data, int dataSize = 1, 
                                       HttpMethod method = HttpMethod.POST, 
                                       Action<ironsource.Response> callback = null,
                                       bool isRetry = false, float timeout = 0) {
            if (isRetry) {
                yield return new WaitForSeconds(timeout);
            }

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
        protected void FlushData(string stream, BulkData bulkData) {
            if (bulkData.GetSize() == 0) {
                return;
            }

            if (isStreamFlush_.ContainsKey(stream) && isStreamFlush_[stream]) {
                queueFlush_[stream] = true;
                return;
            }

            isStreamFlush_[stream] = true;

            string bulkStrData = bulkData.GetStringData();
            BulkData bulkDataTemp = new BulkData(bulkData);

            int bulkSize = bulkData.GetSize();
            bulkData.ClearData();

            float timeout = retryTimeout_;

			Action<ironsource.Response> callback = null;
			callback = delegate(ironsource.Response response) {
                PrintLog("from callback: status = " + response.status);   

                if (response.status <= -1 || response.status >= 500) {
                    if (timeout < 20 * 60) {
                        api_.getCoruotinehandler().StartCoroutine(
                            SendData(stream, bulkStrData, bulkSize, HttpMethod.POST, callback, true, timeout));
                        timeout = timeout * 2;
                        return;
                    } else {
                        BulkData currentBulkData = bulkDataMap_[stream];
                        currentBulkData.AddBulkData(bulkDataTemp);

                        callback(new Response("Timeout - No reponse from server", null, 408));
                        return;
                    }
                }

                isStreamFlush_[stream] = false;
                if (queueFlush_.ContainsKey(stream) && queueFlush_[stream]) {
                    queueFlush_[stream] = false;
                    Flush();
                }
            };

            api_.getCoruotinehandler().StartCoroutine(SendData(stream, bulkStrData, bulkSize, HttpMethod.POST, callback));
        }

		/// <summary>
        /// Flush the specified stream.
        /// </summary>
        /// <param name="stream">
        /// Stream.
        /// </param>
        public void Flush(string stream) {
			if (stream.Length > 0) {
                if (bulkDataMap_.ContainsKey(stream)) {
                    BulkData bulkData = bulkDataMap_[stream];
                    FlushData(stream, bulkData);
                }
            } else {
                PrintLog("Wrong stream name!");
            }                  
		}

        /// <summary>
        /// Flush this instance.
        /// </summary>
        public void Flush() {
            foreach (var bulkDataEntry in bulkDataMap_) {
                FlushData(bulkDataEntry.Key, bulkDataEntry.Value);
            }
        }

        /// <summary>
        /// Update timer for Flush Data
        /// </summary>
        public void Update() {
            deltaTime_ += Time.deltaTime;
            if (deltaTime_ >= flushInterval_) {
                deltaTime_ = 0;
                Flush();
            }
        }
    }
}