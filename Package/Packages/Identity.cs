// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SE.Config;
using SE.Json;

namespace SE.Apollo.Package
{
    /// <summary>
    /// Provides information about a user identity
    /// </summary>
    public sealed class Identity : IAutoObject
    {
        [NamedProperty("name")]
        string name = null;
        /// <summary>
        /// The username
        /// </summary>
        public string Name
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return name; }
        }

        [NamedProperty("url", TypeConverter = typeof(UriConverter))]
        Uri homepage = null;
        /// <summary>
        /// An optional remote location owned by the user
        /// </summary>
        public Uri Homepage
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return homepage; }
        }

        [NamedProperty("email")]
        string email = null;
        /// <summary>
        /// An optional email address used to contact the user
        /// </summary>
        public string Email
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return email; }
        }

        public bool IsValid
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return !string.IsNullOrEmpty(name); }
        }

        /// <summary>
        /// Creates a new class instance
        /// </summary>
        public Identity()
        { }

        /// <summary>
        /// Serializes the information to JSON
        /// </summary>
        public void Serialize(JsonDocument doc, JsonNode root)
        {
            #region Name
            if (!string.IsNullOrWhiteSpace(name))
            {
                JsonNode node = doc.AddNode(root, JsonNodeType.String);
                node.RawValue = name;
                node.Name = "name";
            }
            #endregion

            #region Url
            if (homepage != null)
            {
                JsonNode node = doc.AddNode(root, JsonNodeType.String);
                node.RawValue = homepage.AbsoluteUri;
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

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public override int GetHashCode()
        {
            return name.GetHashCode();
        }
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public override bool Equals(object obj)
        {
            Identity settings = (obj as Identity);
            if (settings != null)
            {
                return name.Equals(settings.name);
            }
            else return false;
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public override string ToString()
        {
            return name.ToString();
        }
    }
}
