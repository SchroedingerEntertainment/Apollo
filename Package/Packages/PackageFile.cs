// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using SE.Config;
using SE.Json;

namespace SE.Apollo.Package
{
    /// <summary>
    /// Provides information about the package data
    /// </summary>
    public sealed class PackageContent : IAutoObject
    {
        [NamedProperty("tarball", TypeConverter = typeof(UriConverter))]
        Uri remoteLocation = null;
        /// <summary>
        /// The absolute remote location of the data
        /// </summary>
        public Uri RemoteLocation
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return remoteLocation; }
        }

        FileSystemDescriptor location;
        /// <summary>
        /// A local file system path to the package
        /// </summary>
        public FileSystemDescriptor Location
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return location; }
            [MethodImpl(OptimizationExtensions.ForceInline)]
            set { location = value; }
        }

        [NamedProperty("shasum")]
        string checksum = null;
        /// <summary>
        /// An SHA1 checksum to verify the package data
        /// </summary>
        public string Checksum
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return checksum; }
        }

        public bool IsValid
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return remoteLocation != null; }
        }

        /// <summary>
        /// Creates a new class instance
        /// </summary>
        public PackageContent()
        { }

        /// <summary>
        /// Serializes the information to JSON
        /// </summary>
        public void Serialize(JsonDocument doc, JsonNode root)
        {
            #region Tarball
            if (remoteLocation != null)
            {
                JsonNode node = doc.AddNode(root, JsonNodeType.String);
                node.RawValue = remoteLocation.AbsoluteUri;
                node.Name = "tarball";
            }
            #endregion

            #region Shasum
            if (!string.IsNullOrWhiteSpace(checksum))
            {
                JsonNode node = doc.AddNode(root, JsonNodeType.String);
                node.RawValue = checksum;
                node.Name = "shasum";
            }
            #endregion
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public override int GetHashCode()
        {
            return remoteLocation.GetHashCode();
        }
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public override bool Equals(object obj)
        {
            PackageContent settings = (obj as PackageContent);
            if (settings != null)
            {
                return remoteLocation.Equals(settings.remoteLocation);
            }
            else return false;
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public override string ToString()
        {
            return remoteLocation.ToString();
        }
    }
}