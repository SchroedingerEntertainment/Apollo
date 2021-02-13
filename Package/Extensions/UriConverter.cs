// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using SE.Config;

namespace SE.Apollo.Package
{
    /// <summary>
    /// Property converter from string to Uri
    /// </summary>
    public class UriConverter : ITypeConverter
    {
        /// <summary>
        /// Creates a new converter instance
        /// </summary>
        public UriConverter()
        { }

        public bool TryParseValue(Type targetType, object value, out object result)
        {
            string url = value as string; if (!string.IsNullOrWhiteSpace(url))
            {
                try
                {
                    result = new Uri(url);
                    return true;
                }
                catch { }
            }
            result = null;
            return false;
        }
    }
}
