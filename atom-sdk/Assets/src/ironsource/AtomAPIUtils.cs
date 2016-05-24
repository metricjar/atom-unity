using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace ironsource {
    public class AtomAPIUtils {        
        
        public static string DictionaryToJson(Dictionary<string, string> dictData) {
            var entries = dictData.Select(entryObject =>
            string.Format("\"{0}\": \"{1}\"", entryObject.Key, entryObject.Value));

            return "{" + entries.Aggregate((i, j) => i + "," + j) + "}";
        }

        public static string ListToJson(List<string> listData) {
            return "[" + listData.Aggregate((i, j) => i + "," + j) + "]";
        }

        public static string EncodeHmac(string input, byte[] key) {
            using (HMACSHA256 hmac = new HMACSHA256(key)) {
            byte[] byteArray = Encoding.ASCII.GetBytes(input);
            return hmac.ComputeHash(byteArray).Aggregate(String.Empty, (s, e) => s + String.Format("{0:x2}",e), s => s );
            }
        }

        public static string Base64Encode(string data) {
            var dataBytes = System.Text.Encoding.UTF8.GetBytes(data);
            return System.Convert.ToBase64String(dataBytes);
        }

        public static string EscapeStringValue(string value) {
            const char BACK_SLASH = '\\';
            const char SLASH = '/';
            const char DBL_QUOTE = '"';

            var output = new StringBuilder(value.Length);
            foreach (char c in value) {
                switch (c) {
                    case SLASH:
                        output.AppendFormat("{0}{1}", BACK_SLASH, SLASH);
                        break;

                    case BACK_SLASH:
                        output.AppendFormat("{0}{0}", BACK_SLASH);
                        break;

                    case DBL_QUOTE:
                        output.AppendFormat("{0}{1}",BACK_SLASH,DBL_QUOTE);
                        break;

                    default:
                        output.Append(c);
                        break;
                }
            }

            return output.ToString();
        }
    }
}

