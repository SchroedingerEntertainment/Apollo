// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Security;
using SE.Config;

namespace SE.Apollo.Package
{
    /// <summary>
    /// Property converter from string to SecureString
    /// </summary>
    public class TokenConverter : ITypeConverter
    {
        /// <summary>
        /// Creates a new converter instance
        /// </summary>
        public TokenConverter()
        { }

        public bool TryParseValue(Type targetType, object value, out object result)
        {
            result = new SecureString();
            foreach (char c in (value as string))
                (result as SecureString).AppendChar(c);

            return true;
        }
    }
}
