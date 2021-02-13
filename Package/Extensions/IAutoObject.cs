// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;

namespace SE.Apollo.Package
{
    /// <summary>
    /// An interface used on auto initialize an object from complex JSON
    /// </summary>
    public interface IAutoObject
    {
        /// <summary>
        /// Determines if this object is properly initialized
        /// </summary>
        bool IsValid { get; }
    }
}
