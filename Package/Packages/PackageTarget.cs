// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Apollo.Package
{
    /// <summary>
    /// A compound object of package ID and version
    /// </summary>
    public struct PackageTarget
    {
        PackageId id;
        /// <summary>
        /// The package ID
        /// </summary>
        public PackageId Id
        {
            get { return id; }
        }

        PackageVersion version;
        /// <summary>
        /// The package version
        /// </summary>
        public PackageVersion Version
        {
            get { return version; }
        }

        bool isDependency;
        /// <summary>
        /// Determines if this package target is a package dependency
        /// </summary>
        public bool IsDependency
        {
            get { return isDependency; }
        }

        /// <summary>
        /// Creates a new instance from package ID and version
        /// </summary>
        public PackageTarget(PackageId id, PackageVersion version, bool isDependency = false)
        {
            this.id = id;
            this.version = version;
            this.isDependency = isDependency;
        }
        /// <summary>
        /// Creates a new instance from package ID and 'latest'
        /// </summary>
        public PackageTarget(PackageId id)
            : this(id, new PackageVersion())
        { }
        /// <summary>
        /// Creates a new dependency instance from the provided target
        /// </summary>
        public PackageTarget(PackageTarget target)
        {
            this.id = target.id;
            this.version = target.version;
            this.isDependency = true;
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

        public override string ToString()
        {
            return string.Concat(id.ToString(), "@", version.ToString());
        }

        /// <summary>
        /// Tries to convert a given string into a valid target
        /// </summary>
        /// <param name="value">A string in <owner>.<namespace>.<name>@<version> format</param>
        /// <returns>True if parsing the string was successful, false otherwise</returns>
        public static bool TryParse(string value, out PackageTarget result)
        {
            result = new PackageTarget();
            StringBuilder textBuffer = new StringBuilder(value);
            for (int i = 0; i <= textBuffer.Length; i++)
            {
                string buffer;
                if (i == textBuffer.Length)
                {
                    buffer = textBuffer.ToString().Trim();
                    textBuffer.Clear();
                }
                else if (textBuffer[i] == '@')
                {
                    buffer = textBuffer.ToString(0, i).Trim();
                    textBuffer.Remove(0, i + 1);
                }
                else continue;
                PackageId tmp; if (PackageId.TryParse(buffer, out tmp))
                {
                    result.id = tmp;
                    break;
                }
                else return false;
            }
            if (textBuffer.Length > 0)
            {
                result.version = PackageVersion.Create(textBuffer.ToString().Trim());
                if (!result.version.IsValid)
                {
                    return false;
                }
            }
            return (result.id.IsValid);
        }
    }
}
