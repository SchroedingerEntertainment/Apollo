// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SE.Apollo.Package
{
    public static partial class HttpClientExtension
    {
        /// <summary>
        /// Begins to stream from this http host
        /// </summary>
        /// <param name="content">The package location on remote host</param>
        public static async Task<Any<Stream>> GetPackage(this HttpClient http, PackageContent content)
        {
            if (content.IsValid)
            {
                HttpResponseMessage response = await http.GetAsync(content.RemoteLocation);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStreamAsync();
                }
            }
            return Any<Stream>.Empty;
        }
    }
}