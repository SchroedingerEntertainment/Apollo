// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime;
using System.Threading.Tasks;
using SE.Json;
using SE.Web;

namespace SE.Apollo.Package
{
    public static partial class HttpClientExtension
    {
        /// <summary>
        /// Loads a metadata object from this http host
        /// </summary>
        /// <param name="name">A full qualified package name to obtain metadata for</param>
        /// <param name="logger">A logging interface to write parser errors to</param>
        public static async Task<Any<JsonDocument>> GetPackageMeta(this HttpClient http, string name, ILogSystem logger)
        {
            HttpResponseMessage response = await http.GetAsync(name);
            if (response.IsSuccessStatusCode)
            {
                using (StreamBuffer buffer = new StreamBuffer(await response.Content.ReadAsStreamAsync(), 128))
                {
                    JsonDocument document = new JsonDocument();
                    if (document.Load(buffer))
                    {
                        return new Any<JsonDocument>(document);
                    }
                    else foreach (string error in document.Errors)
                            logger.Error(error);
                }
            }
            return Any<JsonDocument>.Empty;
        }
    }
}
