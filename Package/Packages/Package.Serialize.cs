// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime;
using SE.Json;

namespace SE.Apollo.Package
{
    public partial class PackageMeta
    {
        /// <summary>
        /// Serializes the information to the provides stream in JSON format
        /// </summary>
        /// <param name="formatted">Determines if serialization should create human readable text</param>
        public void Serialize(Stream stream, bool formatted = false)
        {
            JsonDocument doc = new JsonDocument();
            JsonNode root = doc.AddNode(JsonNodeType.Object);

            #region Name
            JsonNode node = doc.AddNode(root, JsonNodeType.String);
            node.RawValue = id.ToString();
            node.Name = "name";
            #endregion

            #region Description
            if (!string.IsNullOrWhiteSpace(description))
            {
                node = doc.AddNode(root, JsonNodeType.String);
                node.RawValue = description;
                node.Name = "description";
            }
            #endregion

            #region Homepage
            if (projectHome != null)
            {
                node = doc.AddNode(root, JsonNodeType.String);
                node.RawValue = projectHome.AbsoluteUri;
                node.Name = "homepage";
            }
            #endregion

            #region License
            if (!string.IsNullOrWhiteSpace(license))
            {
                node = doc.AddNode(root, JsonNodeType.String);
                node.RawValue = license;
                node.Name = "license";
            }
            #endregion

            #region Version
            node = doc.AddNode(root, JsonNodeType.String);
            node.RawValue = version.ToString();
            node.Name = "version";
            #endregion

            #region Dependencies
            if (dependencies.Count > 0)
            {
                JsonNode container = doc.AddNode(root, JsonNodeType.Object);
                foreach(KeyValuePair<PackageId, PackageVersion> item in dependencies)
                {
                    node = doc.AddNode(container, JsonNodeType.String);
                    node.RawValue = item.Value.ToString();
                    node.Name = item.Key.ToString();
                }
                container.Name = "dependencies";
            }
            #endregion

            #region PeerDependencies
            if (references.Count > 0)
            {
                JsonNode container = doc.AddNode(root, JsonNodeType.Object);
                foreach (KeyValuePair<PackageId, PackageVersion> item in references)
                {
                    node = doc.AddNode(container, JsonNodeType.String);
                    node.RawValue = item.Value.ToString();
                    node.Name = item.Key.ToString();
                }
                container.Name = "peerdependencies";
            }
            #endregion

            #region OS
            if (platform.Count > 0)
            {
                JsonNode container = doc.AddNode(root, JsonNodeType.Array);
                foreach (string item in platform)
                {
                    node = doc.AddNode(container, JsonNodeType.String);
                    node.RawValue = item;
                }
                container.Name = "os";
            }
            #endregion

            #region CPU
            if (architecture.Count > 0)
            {
                JsonNode container = doc.AddNode(root, JsonNodeType.Array);
                foreach (PlatformTarget item in architecture)
                {
                    node = doc.AddNode(container, JsonNodeType.String);
                    node.RawValue = item.ToString();
                }
                container.Name = "cpu";
            }
            #endregion

            #region Files
            if (files.Count > 0)
            {
                JsonNode container = doc.AddNode(root, JsonNodeType.Array);
                foreach (string item in files)
                {
                    node = doc.AddNode(container, JsonNodeType.String);
                    node.RawValue = item;
                }
                container.Name = "files";
            }
            #endregion

            #region Bugs
            if (bugTracker != null)
            {
                node = doc.AddNode(root, JsonNodeType.Object);
                bugTracker.Serialize(doc, node);
                node.Name = "bugs";
            }
            #endregion

            #region Keywords
            if (tags.Count > 0)
            {
                JsonNode container = doc.AddNode(root, JsonNodeType.Array);
                foreach (string item in tags)
                {
                    node = doc.AddNode(container, JsonNodeType.String);
                    node.RawValue = item;
                }
                container.Name = "keywords";
            }
            #endregion

            #region Author/ Contributors
            if (maintainer.Count > 0)
            {
                bool isAuthor = true;
                JsonNode contributors = null;
                foreach (Identity identity in maintainer)
                {
                    if (isAuthor)
                    {
                        node = doc.AddNode(root, JsonNodeType.Object);
                        identity.Serialize(doc, node);
                        node.Name = "author";
                        isAuthor = false;
                    }
                    else
                    {
                        if (contributors == null)
                        {
                            contributors = doc.AddNode(root, JsonNodeType.Object);
                        }
                        identity.Serialize(doc, contributors);
                    }
                }
                if (contributors != null)
                {
                    contributors.Name = "contributors";
                }
            }
            #endregion

            #region Repository
            if (source != null)
            {
                node = doc.AddNode(root, JsonNodeType.Object);
                source.Serialize(doc, node);
                node.Name = "repository";
            }
            #endregion

            #region Dist
            if (content != null)
            {
                node = doc.AddNode(root, JsonNodeType.Object);
                content.Serialize(doc, node);
                node.Name = "dist";
            }
            #endregion

            #region Scripts
            if (events.Count > 0)
            {
                JsonNode container = doc.AddNode(root, JsonNodeType.Object);
                foreach (KeyValuePair<string, string> item in events)
                {
                    node = doc.AddNode(container, JsonNodeType.String);
                    node.RawValue = item.Value;
                    node.Name = item.Key;
                }
                container.Name = "scripts";
            }
            #endregion

            #region Config
            if (parameter.Count > 0)
            {
                JsonNode container = doc.AddNode(root, JsonNodeType.Object);
                foreach (KeyValuePair<string, string> item in parameter)
                {
                    node = doc.AddNode(container, JsonNodeType.String);
                    node.RawValue = item.Value;
                    node.Name = item.Key;
                }
                container.Name = "config";
            }
            #endregion

            doc.Save(stream, formatted);
        }
    }
}
