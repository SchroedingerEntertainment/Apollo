// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using SE.Config;
using SE.Json;

namespace SE.Apollo.Package
{
    /// <summary>
    /// Provides information about the remote repository
    /// </summary>
    public sealed class PackageSource : IAutoObject
    {
        [NamedProperty("url", TypeConverter = typeof(UriConverter))]
        Uri baseAddress = null;
        /// <summary>
        /// The absolute address to the package repository
        /// </summary>
        public Uri BaseAddress
        {
            get { return baseAddress; }
        }

        [NamedProperty("type")]
        string protocol = null;
        /// <summary>
        /// The protocol used to access the repository
        /// </summary>
        public string Protocol
        {
            get { return protocol; }
        }

        [NamedProperty("directory")]
        string path = null;
        /// <summary>
        /// An optional sub-directory the package is located in
        /// </summary>
        public string Path
        {
            get { return path; }
        }

        public bool IsValid
        {
            get { return baseAddress != null; }
        }

        /// <summary>
        /// Creates a new class instance
        /// </summary>
        public PackageSource()
        { }

        /// <summary>
        /// Serializes the information to JSON
        /// </summary>
        public void Serialize(JsonDocument doc, JsonNode root)
        {
            #region Url
            if (baseAddress != null)
            {
                JsonNode node = doc.AddNode(root, JsonNodeType.String);
                node.RawValue = baseAddress.AbsoluteUri;
                node.Name = "url";
            }
            #endregion

            #region Type
            if (!string.IsNullOrWhiteSpace(protocol))
            {
                JsonNode node = doc.AddNode(root, JsonNodeType.String);
                node.RawValue = protocol;
                node.Name = "type";
            }
            #endregion

            #region Directory
            if (!string.IsNullOrWhiteSpace(path))
            {
                JsonNode node = doc.AddNode(root, JsonNodeType.String);
                node.RawValue = path;
                node.Name = "directory";
            }
            #endregion
        }

        public override int GetHashCode()
        {
            return baseAddress.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            PackageSource settings = (obj as PackageSource);
            if (settings != null)
            {
                return baseAddress.Equals(settings.baseAddress);
            }
            else return false;
        }

        public override string ToString()
        {
            return baseAddress.ToString();
        }
    }
}
