// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime;
using SE.Config;

namespace SE.Apollo.Package
{
    /// <summary>
    /// Code package meta data
    /// </summary>
    public partial class PackageMeta : IDisposable
    {
        [NamedProperty("name")]
        PackageId id = new PackageId();
        /// <summary>
        /// The unique package ID
        /// </summary>
        public PackageId Id
        {
            get { return id; }
            set
            {
                if (id.IsValid)
                {
                    throw new InvalidOperationException();
                }
                else id = value;
            }
        }

        [NamedProperty("description")]
        string description = null;
        /// <summary>
        /// A description string
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [NamedProperty("homepage", TypeConverter = typeof(UriConverter))]
        Uri projectHome = null;
        /// <summary>
        /// An optional remote location that provides information about this package
        /// </summary>
        public Uri ProjectHome
        {
            get { return projectHome; }
            set { projectHome = value; }
        }

        [NamedProperty("license")]
        string license = string.Empty;
        /// <summary>
        /// The SPDX license identifier this package was published to
        /// </summary>
        public string License
        {
            get { return license; }
            set { license = value; }
        }

        [NamedProperty("version")]
        PackageVersion version = new PackageVersion();
        /// <summary>
        /// The release version of this package
        /// </summary>
        public PackageVersion Version
        {
            get { return version; }
            set
            {
                if (version.IsValid)
                {
                    throw new InvalidOperationException();
                }
                else version = value;
            }
        }

        [NamedProperty("dependencies", TypeConverter = typeof(KeyValuePairConverter))]
        Dictionary<PackageId, PackageVersion> dependencies;
        /// <summary>
        /// Determines dependencies this package needs to build properly
        /// </summary>
        public Dictionary<PackageId, PackageVersion> Dependencies
        {
            get { return dependencies; }
        }

        [NamedProperty("peerdependencies", TypeConverter = typeof(KeyValuePairConverter))]
        Dictionary<PackageId, PackageVersion> references;
        /// <summary>
        /// Determines references this package is based on
        /// </summary>
        /// <remarks>Usually used for plugin packages that refer to the plugin host</remarks>
        public Dictionary<PackageId, PackageVersion> References
        {
            get { return references; }
        }

        [NamedProperty("os")]
        HashSet<string> platform;
        /// <summary>
        /// An optional set of platforms this package targets
        /// </summary>
        public HashSet<string> Platform
        {
            get { return platform; }
        }

        [NamedProperty("cpu")]
        HashSet<PlatformTarget> architecture;
        /// <summary>
        /// An optional set of architectures this package targets
        /// </summary>
        public HashSet<PlatformTarget> Architecture
        {
            get { return architecture; }
        }

        [NamedProperty("files")]
        HashSet<string> files;
        /// <summary>
        /// A collection of file patterns that describes the entries to be included
        /// </summary>
        public HashSet<string> Files
        {
            get { return files; }
        }

        [NamedProperty("bugs", TypeConverter = typeof(ComplexConverter<BugTracker>))]
        BugTracker bugTracker = null;
        /// <summary>
        /// Optional information about a bug tracker attached to the project
        /// </summary>
        public BugTracker BugTracker
        {
            get { return bugTracker; }
            set { bugTracker = value; }
        }

        [NamedProperty("keywords")]
        HashSet<string> tags;
        /// <summary>
        /// A set of tags the package was published to
        /// </summary>
        public HashSet<string> Tags
        {
            get { return tags; }
        }

        [NamedProperty("author", TypeConverter = typeof(ComplexConverter<Identity>))]
        HashSet<Identity> maintainer;
        /// <summary>
        /// A collection of users maintaining the package
        /// </summary>
        [NamedProperty("contributors", TypeConverter = typeof(ComplexConverter<Identity>))]
        public HashSet<Identity> Maintainer
        {
            get { return maintainer; }
        }

        [NamedProperty("repository", TypeConverter = typeof(ComplexConverter<PackageSource>))]
        PackageSource source = null;
        /// <summary>
        /// The remote location of the project
        /// </summary>
        public PackageSource Source
        {
            get { return source; }
            set { source = value; }
        }

        [NamedProperty("dist", TypeConverter = typeof(ComplexConverter<PackageContent>))]
        PackageContent content = null;
        /// <summary>
        /// The remote location of this packages original content
        /// </summary>
        public PackageContent Content
        {
            get { return content; }
        }

        [NamedProperty("scripts", TypeConverter = typeof(KeyValuePairConverter))]
        Dictionary<string, string> events;
        /// <summary>
        /// A collection of actions that should be performed on certain occurrence
        /// </summary>
        /// <remarks>The execution depends on the host processing the package meta data</remarks>
        public Dictionary<string, string> Events
        {
            get { return events; }
        }

        [NamedProperty("config", TypeConverter = typeof(KeyValuePairConverter))]
        Dictionary<string, string> parameter;
        /// <summary>
        /// A collection of additional parameters
        /// </summary>
        /// <remarks>The usage depends on the host processing the package meta data</remarks>
        public Dictionary<string, string> Parameter
        {
            get { return parameter; }
        }

        /// <summary>
        /// Creates a new package instance
        /// </summary>
        public PackageMeta()
        {
            this.dependencies = CollectionPool<Dictionary<PackageId, PackageVersion>, PackageId, PackageVersion>.Get();
            this.references = CollectionPool<Dictionary<PackageId, PackageVersion>, PackageId, PackageVersion>.Get();
            this.platform = CollectionPool<HashSet<string>, string>.Get();
            this.architecture = CollectionPool<HashSet<PlatformTarget>, PlatformTarget>.Get();
            this.files = CollectionPool<HashSet<string>, string>.Get();
            this.tags = CollectionPool<HashSet<string>, string>.Get();
            this.maintainer = CollectionPool<HashSet<Identity>, Identity>.Get();
            this.events = CollectionPool<Dictionary<string, string>, string, string>.Get();
            this.parameter = CollectionPool<Dictionary<string, string>, string, string>.Get();
        }
        public void Dispose()
        {
            CollectionPool<Dictionary<PackageId, PackageVersion>, PackageId, PackageVersion>.Return(dependencies);
            dependencies = null;

            CollectionPool<Dictionary<PackageId, PackageVersion>, PackageId, PackageVersion>.Return(references);
            references = null;

            CollectionPool<HashSet<string>, string>.Return(platform);
            platform = null;

            CollectionPool<HashSet<PlatformTarget>, PlatformTarget>.Return(architecture);
            architecture = null;

            CollectionPool<HashSet<string>, string>.Return(files);
            files = null;

            CollectionPool<HashSet<string>, string>.Return(tags);
            tags = null;

            CollectionPool<HashSet<Identity>, Identity>.Return(maintainer);
            maintainer = null;

            CollectionPool<Dictionary<string, string>, string, string>.Return(events);
            events = null;

            CollectionPool<Dictionary<string, string>, string, string>.Return(parameter);
            parameter = null;
        }

        /// <summary>
        /// Creates a formatted string
        /// </summary>
        /// <param name="scoped">A flag to also append the package scope to the string</param>
        /// <returns>The formatted string</returns>
        public string FriendlyName(bool scope)
        {
            return string.Concat(id.FriendlyName(scope), "@", version.ToString());
        }

        public override int GetHashCode()
        {
            HashCombiner hash = new HashCombiner();
            hash.Add(id);
            hash.Add(version);

            return hash.Value;
        }
        public override bool Equals(object obj)
        {
            PackageMeta pkg = (obj as PackageMeta);
            if (pkg != null)
            {
                return (id.Equals(pkg.id) && version.Equals(pkg.version));
            }
            else return false;
        }

        public override string ToString()
        {
            return string.Concat(id.ToString(), "@", version.ToString());
        }
    }
}
