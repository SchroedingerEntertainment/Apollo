﻿// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using SE.Config;
using SE.Json;

namespace SE.Apollo.Package
{
    /// <summary>
    /// Provides information about a remote bug tracker
    /// </summary>
    public sealed class BugTracker : IAutoObject
    {
        [NamedProperty("url", TypeConverter = typeof(UriConverter))]
        Uri address = null;
        /// <summary>
        /// The base address of the bug tracker
        /// </summary>
        public Uri Address
        {
            get { return address; }
        }

        [NamedProperty("email")]
        string email = null;
        /// <summary>
        /// An email address to send bugs to
        /// </summary>
        public string Email
        {
            get { return email; }
        }

        public bool IsValid
        {
            get { return (address != null || !string.IsNullOrEmpty(email)); }
        }

        /// <summary>
        /// Creates a new class instance
        /// </summary>
        public BugTracker()
        { }

        /// <summary>
        /// Serializes the information to JSON
        /// </summary>
        public void Serialize(JsonDocument doc, JsonNode root)
        {
            #region Url
            if (address != null)
            {
                JsonNode node = doc.AddNode(root, JsonNodeType.String);
                node.RawValue = address.AbsoluteUri;
                node.Name = "url";
            }
            #endregion

            #region Email
            if (!string.IsNullOrWhiteSpace(email))
            {
                JsonNode node = doc.AddNode(root, JsonNodeType.String);
                node.RawValue = email;
                node.Name = "email";
            }
            #endregion
        }

        public override int GetHashCode()
        {
            return address.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            BugTracker settings = (obj as BugTracker);
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
