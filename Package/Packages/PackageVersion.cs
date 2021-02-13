﻿// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using SE.Parsing;

namespace SE.Apollo.Package
{
    /// <summary>
    /// A unified version number with flags
    /// </summary>
    [TypeConverter(typeof(PackageVersionConverter))]
    public struct PackageVersion
    {
        private readonly UInt32 value;

        /// <summary>
        /// The major version component
        /// </summary>
        public UInt32 Major
        {
            get { return (UInt32)((value >> 22) & 0x3FF); }
        }
        /// <summary>
        /// The minor version component
        /// </summary>
        public UInt32 Minor
        {
            get { return (UInt32)((value >> 12) & 0x3FF); }
        }
        /// <summary>
        /// The revision component
        /// </summary>
        public UInt32 Revision
        {
            get { return (UInt32)((value >> 2) & 0x3FF); }
        }

        /// <summary>
        /// Determines if this version is a compatibility version up to the next major release
        /// </summary>
        public bool IsCompatibilityVersion
        {
            get { return ((value & 1) == 1); }
        }
        /// <summary>
        /// Determines if this version number is valid
        /// </summary>
        public bool IsValid
        {
            get { return (value != 0); }
        }

        /// <summary>
        /// Converts an integer into a new version number instance
        /// </summary>
        public PackageVersion(UInt32 value)
        {
            this.value = value;
        }

        public static implicit operator UInt32(PackageVersion version)
        {
            return version.value;
        }
        public static implicit operator PackageVersion(UInt32 version)
        {
            return new PackageVersion(version);
        }

        /// <summary>
        /// Compares this version and returns if this version number matches the provided
        /// second version number
        /// </summary>
        /// <returns>True if versions are equal or considered to be equal</returns>
        public bool Match(PackageVersion version)
        {
            bool result = (Major == version.Major);
            result &= ((IsCompatibilityVersion && Minor <= version.Minor) || Minor == version.Minor);
            result &= (IsCompatibilityVersion || Revision == version.Revision);

            return result;
        }

        /// <summary>
        /// Compares this version number to a second one and returns the result
        /// </summary>
        public int CompareTo(PackageVersion other)
        {
            return value.CompareTo(other.value);
        }
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
        public override string ToString()
        {
            if (value != 0)
            {
                return string.Format("{0}{1}.{2}.{3}", (IsCompatibilityVersion) ? "^" : string.Empty, Major, Minor, Revision);
            }
            else return "latest";
        }

        /// <summary>
        /// Creates a new version number instance from the provided version components
        /// </summary>
        public static PackageVersion Create(UInt32 major, UInt32 minor, UInt32 revision, bool compatibilityVersion)
        {
            return (UInt32)
            (
                (UInt32)((major & 0x3FF) << 22) |
                (UInt32)((minor & 0x3FF) << 12) |
                (UInt32)((revision & 0x3FF) << 2) |
                (UInt32)((compatibilityVersion ? 1 : 0))
            );
        }
        /// <summary>
        /// Parses a new version number instance from the provided version string
        /// </summary>
        public static PackageVersion Create(string versionString)
        {
            if (string.IsNullOrWhiteSpace(versionString))
                return new PackageVersion();

            int segmentIndex = 0;
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(versionString));
            UInt32[] segments = new UInt32[3];
            bool[] flags = new bool[1];
            while (!stream.Eof())
            {
                switch ((char)stream.Peek())
                {
                    #region Whitespace
                    case '\r':
                    case Char32.NewLineGroup.LineFeed:
                    case Char32.NewLineGroup.NextLine:
                    case Char32.NewLineGroup.LineSeparator:
                    case Char32.NewLineGroup.ParagraphSeparator:
                    case Char32.WhiteSpaceGroup.Space:
                    case Char32.WhiteSpaceGroup.HorizontalTab:
                    case Char32.WhiteSpaceGroup.VerticalTab:
                    case Char32.WhiteSpaceGroup.FormFeed:
                    case Char32.WhiteSpaceGroup.NoBreakingSpace:
                    case Char32.WhiteSpaceGroup.OghamSpace:
                    case Char32.WhiteSpaceGroup.MongolianVowelSeparator:
                    case Char32.WhiteSpaceGroup.EnQuad:
                    case Char32.WhiteSpaceGroup.EmQuad:
                    case Char32.WhiteSpaceGroup.EnSpace:
                    case Char32.WhiteSpaceGroup.EmSpace:
                    case Char32.WhiteSpaceGroup.ThreePerEmSpace:
                    case Char32.WhiteSpaceGroup.FourPerEmSpace:
                    case Char32.WhiteSpaceGroup.SixPerEmSpace:
                    case Char32.WhiteSpaceGroup.PunctuationSpace:
                    case Char32.WhiteSpaceGroup.ThinSpace:
                    case Char32.WhiteSpaceGroup.HairSpace:
                    case Char32.WhiteSpaceGroup.NarrowSpace:
                    case Char32.WhiteSpaceGroup.IdeographicSpace:
                    case Char32.WhiteSpaceGroup.MediumMathematicalSpace: stream.Get(); break;
                    #endregion
                    case '^':
                        {
                            flags[0] = true;
                            stream.Get();
                        }
                        break;
                    case '.':
                        {
                            segmentIndex++;
                            stream.Get();
                        }
                        break;
                    default:
                        {
                            long streamPos = stream.Position;
                            while (!stream.Eof())
                            {
                                char c = (char)stream.Peek();
                                if (c < '0' || c > '9')
                                    break;

                                stream.Get();
                            }
                            long length = stream.Position - streamPos;
                            if (length == 0)
                                return new PackageVersion(0);

                            stream.Position = streamPos;
                            UInt32 result; if(!UInt32.TryParse(stream.Read(length), out result))
                                return new PackageVersion(0);

                            segments[segmentIndex] = result;
                        }
                        break;
                }
            }
            return Create(segments[0], segments[1], segments[2], flags[0]);
        }
    }
}
