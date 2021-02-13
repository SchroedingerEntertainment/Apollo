// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SE.Apollo.Package
{
    /// <summary>
    /// A unique 3-component package ID
    /// </summary>
    [TypeConverter(typeof(PackageIdConverter))]
    public struct PackageId
    {
        string scope;
        /// <summary>
        /// An optional scope used to address the package
        /// </summary>
        public string Scope
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(scope))
                {
                    return string.Empty;
                }
                else return scope;
            }
        }

        string owner;
        /// <summary>
        /// The shortcut of the package owner
        /// </summary>
        public string Owner
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(owner))
                {
                    return string.Empty;
                }
                else return owner;
            }
        }

        string @namespace;
        /// <summary>
        /// The namespace this package is located in
        /// </summary>
        public string Namespace
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(@namespace))
                {
                    return string.Empty;
                }
                else return @namespace;
            }
        }

        string name;
        /// <summary>
        /// The name of this package
        /// </summary>
        public string Name
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return string.Empty;
                }
                else return name;
            }
        }

        /// <summary>
        /// Determines if this ID is a valid 3-component package ID
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(owner) || string.IsNullOrWhiteSpace(@namespace) || string.IsNullOrWhiteSpace(name))
                {
                    return false;
                }
                else return true;
            }
        }

        /// <summary>
        /// Creates a new ID instance from the provided components
        /// </summary>
        public PackageId(string owner, string @namespace, string name)
        {
            this.scope = string.Empty;
            this.owner = owner;
            this.@namespace = @namespace;
            this.name = name;
        }
        /// <summary>
        /// Creates a new ID instance from the provided components
        /// </summary>
        public PackageId(string scope, string owner, string @namespace, string name)
        {
            this.scope = scope;
            this.owner = owner;
            this.@namespace = @namespace;
            this.name = name;
        }
        /// <summary>
        /// Creates a new id instance from the provided root id and a scope
        /// </summary>
        /// <param name="scope">The name of a scope</param>
        /// <param name="id">The root id to scope</param>
        public PackageId(string scope, PackageId id)
        {
            if (scope.StartsWith("@"))
            {
                scope = scope.Substring(1);
            }
            this.scope = scope;
            this.owner = id.owner;
            this.@namespace = id.@namespace;
            this.name = id.name;
        }

        /// <summary>
        /// Creates a formatted package ID string
        /// </summary>
        /// <param name="scoped">A flag to also append the package scope to the string</param>
        /// <returns>The formatted package ID string</returns>
        public string FriendlyName(bool scoped)
        {
            if (IsValid)
            {
                string id; if (scoped && !string.IsNullOrWhiteSpace(scope))
                {
                    id = string.Concat(scope, "/");
                }
                else id = string.Empty;
                return string.Concat(id, owner, ".", @namespace, ".", name);
            }
            else return string.Empty;
        }

        /// <summary>
        /// Determines if this package ID equals the provided one
        /// </summary>
        /// <returns>True if both IDs are equal, false otherwise</returns>
        public bool Match(PackageId id)
        {
            return
            (
                Scope.Equals(id.scope) &&
                Owner.Equals(id.Owner) &&
                Namespace.Equals(id.Namespace) &&
                Name.Equals(id.Name)
            );
        }

        public override bool Equals(object obj)
        {
            if (obj is PackageId)
            {
                PackageId id = (PackageId)obj;
                return (Owner.Equals(id.Owner) && Namespace.Equals(id.Namespace) && Name.Equals(id.Name));
            }
            else return false;
        }
        public override int GetHashCode()
        {
            HashCombiner hash = new HashCombiner();
            hash.Add(Owner);
            hash.Add(Namespace);
            hash.Add(Name);

            return hash.Value;
        }
        public override string ToString()
        {
            if (IsValid)
            {
                string id; if (!string.IsNullOrWhiteSpace(scope))
                {
                    id = string.Concat("@", scope, "/");
                }
                else id = string.Empty;
                return string.Concat(id, owner, ".", @namespace, ".", name);
            }
            else return string.Empty;
        }

        /// <summary>
        /// Tries to convert a given string into a valid id instance
        /// </summary>
        /// <param name="value">A string in <owner>.<namespace>.<name> format</param>
        /// <returns>True if parsing the string was successful, false otherwise</returns>
        public static bool TryParse(string value, out PackageId result)
        {
            result = new PackageId();

            int index = value.IndexOf('/');
            if (index >= 0)
            {
                result.scope = value.Substring(0, index)
                                    .Trim()
                                    .ToLowerInvariant();

                value = value.Substring(index + 1);
                if (result.scope.StartsWith("@"))
                {
                    result.scope = result.scope.Substring(1);
                }
            }
            index = 0;
            for (int i = 0; i <= value.Length; i++)
            {
                if (i == value.Length)
                {
                    if(value.Length > 0)
                        switch (index)
                        {
                            case 2:
                                {
                                    result.name = value.Trim()
                                                       .ToLowerInvariant();
                                }
                                break;
                            default: return false;
                        }
                }
                else switch (value[i])
                {
                    #region Component Separator
                    case '.':
                        {
                            if (i > 0)
                            {
                                switch (index)
                                {
                                    case 0:
                                        {
                                            result.owner = value.Substring(0, i)
                                                                .Trim()
                                                                .ToLowerInvariant();
                                        }
                                        break;
                                    case 1:
                                        {
                                            result.@namespace = value.Substring(0, i)
                                                                     .Trim()
                                                                     .ToLowerInvariant();
                                        }
                                        break;
                                    case 2:
                                        {
                                            result.name = value.Substring(0, i)
                                                               .Trim()
                                                               .ToLowerInvariant();
                                        }
                                        break;
                                }
                                value = value.Substring(i + 1);
                                index++;
                                i = -1;
                            }
                            else return false;
                            if (index > 2)
                            {
                                return false;
                            }
                        }
                        break;
                    #endregion

                    #region Trailing Underscore
                    case '_':
                        {
                            if (i == 0)
                            {
                                return false;
                            }
                        }
                        break;
                    #endregion

                    #region URL Reserved
                    case '!': 
                    case '*':
                    case '\'':
                    case '(':
                    case ')':
                    case ';':
                    case ':':
                    case '@':
                    case '&':
                    case '=':
                    case '+':
                    case '$':
                    case ',':
                    case '/':
                    case '?':
                    case '%':
                    case '#':
                    case '[':
                    case ']': return false;
                    #endregion
                }
            }
            return (index == 2 && !string.IsNullOrWhiteSpace(result.name));
        }
    }
}
