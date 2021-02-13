// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using SE.Config;

namespace SE.Apollo.Package
{
    /// <summary>
    /// A remote package storage
    /// </summary>
    public sealed class Repository : IAutoObject
    {
        const string JsonMimeType = "application/json";

        HttpClient http;

        [NamedProperty("url", TypeConverter = typeof(UriConverter))]
        Uri address = null;
        /// <summary>
        /// The basic remote location
        /// </summary>
        public Uri Address
        {
            get { return address; }
        }

        [NamedProperty("auth", TypeConverter = typeof(TokenConverter))]
        [PropertyDescription("Sets an authentication token for this operation", Type = PropertyType.Optional)]
        SecureString token = null;

        [NamedProperty("prefixes", TypeConverter = typeof(KeyValuePairConverter))]
        readonly Dictionary<string, string> prefixes;
        /// <summary>
        /// A collection of Package-ID owner shortcuts used to expand package identifiers
        /// conditionally by an NPM scope prefix
        /// </summary>
        public Dictionary<string, string> Prefixes
        {
            get { return prefixes; }
        }

        public bool IsValid
        {
            get { return (address != null); }
        }

        /// <summary>
        /// Creates a new repository instance
        /// </summary>
        public Repository()
        {
            this.prefixes = new Dictionary<string, string>();
        }

        /// <summary>
        /// Obtains an HTTP interface instance to this repository
        /// </summary>
        public HttpClient GetClient()
        {
            switch (address.Scheme.ToLowerInvariant())
            {
                case "http": break;
                case "https": if (!SslContext.Enabled)
                    {
                        goto default;
                    }
                    break;
                default: throw new UriFormatException();
            }
            HttpClientHandler handler = new HttpClientHandler { AllowAutoRedirect = true };
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }
            http = new HttpClient(handler);
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.npm.install-v1+json"));
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMimeType));
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", new NetworkCredential(string.Empty, token).Password);
            http.BaseAddress = address;
            return http;
        }

        public override int GetHashCode()
        {
            return address.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            Repository settings = (obj as Repository);
            if (settings != null)
            {
                return address.Equals(settings.address);
            }
            else return false;
        }

        public override string ToString()
        {
            return address.ToString();
        }
    }
}
