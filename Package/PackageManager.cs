// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using SE.Config;

namespace SE.Apollo.Package
{
    /// <summary>
    /// Manages remote repositories and local package storage
    /// </summary>
    public sealed class PackageManager
    {
        /// <summary>
        /// The default package location
        /// </summary>
        public const string DefaultPackageLocation = "Packages";

        [NamedProperty("repositories", TypeConverter = typeof(ComplexConverter<Repository>))]
        private readonly static HashSet<Repository> repositories;
        /// <summary>
        /// A collection of package repositories to request
        /// </summary>
        public static HashSet<Repository> Repositories
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return repositories; }
        }

        [NamedProperty('a', "accept")]
        [PropertyDescription("Adds an SPDX license identifier to the collection of installable packages", Type = PropertyType.Optional)]
        private readonly static HashSet<string> acceptedLicenses;
        /// <summary>
        /// A collection of allowed SPDX license identifiers
        /// </summary>
        public static HashSet<string> AcceptedLicenses
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return acceptedLicenses; }
        }

        [NamedProperty("packagelocations", TypeConverter = typeof(KeyValuePairConverter))]
        private readonly static Dictionary<string, string> packageLocations;
        /// <summary>
        /// A filter based collection of package locations
        /// </summary>
        public static Dictionary<string, string> PackageLocations
        {
            [MethodImpl(OptimizationExtensions.ForceInline)]
            get { return packageLocations; }
        }

        static PackageManager()
        {
            repositories = new HashSet<Repository>();
            acceptedLicenses = new HashSet<string>();
            packageLocations = new Dictionary<string, string>();
        }
        private PackageManager()
        { }

        /// <summary>
        /// Resolves the provided package into a local file system directory path 
        /// </summary>
        /// <param name="package">The package to which the target should be located</param>
        /// <param name="basePath">The resolved file system directory path</param>
        /// <returns>True if the package could be resolved to a specific path, false otherwise</returns>
        public static bool GetLocation(PackageTarget package, ref PathDescriptor basePath)
        {
            if (packageLocations.Count > 0)
            {
                Filter filter = new Filter();
                foreach (KeyValuePair<string, string> location in packageLocations)
                {
                    FilterToken last = null;
                    filter.Clear();

                    string[] tiles = location.Key.Split('.');
                    foreach (string tile in tiles)
                    {
                        FilterToken current = null;
                        if (last != null)
                        {
                            current = last.GetChild(tile);
                        }
                        if (current == null)
                        {
                            if (last != null) current = filter.Add(last, tile);
                            else current = filter.Add(tile);
                        }
                        last = current;
                    }
                    if (filter.IsMatch(package.Id.Owner, package.Id.Namespace, package.Id.Name))
                    {
                        basePath = basePath.Combine
                        (
                            location.Value.Replace("[id]", package.FriendlyName(false))
                                          .Replace("[owner]", package.Id.Owner.ToUpper())
                                          .Replace("[namespace]", package.Id.Namespace.ToTitleCase())
                                          .Replace("[name]", package.Id.Name.FromPackageName())
                        );
                        return true;
                    }
                }
            }
            basePath = basePath.Combine(DefaultPackageLocation, package.FriendlyName(false));
            return false;
        }
        /// <summary>
        /// Resolves the provided package into a local file system directory path 
        /// </summary>
        /// <param name="package">The package to which the target should be located</param>
        /// <param name="basePath">The resolved file system directory path</param>
        /// <returns>True if the package could be resolved to a specific path, false otherwise</returns>
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static bool GetLocation(PackageMeta package, ref PathDescriptor basePath)
        {
            return GetLocation(new PackageTarget(package.Id, package.Version), ref basePath);
        }
    }
}
