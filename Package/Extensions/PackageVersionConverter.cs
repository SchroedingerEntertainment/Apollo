// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace SE.Apollo.Package
{
    /// <summary>
    /// Type converter from string to PackageVersion
    /// </summary>
    internal class PackageVersionConverter : TypeConverter
    {
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string) || base.CanConvertFrom(context, sourceType));
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string version = (value as string);
            return ((version != null) ? PackageVersion.Create(version) : base.ConvertFrom(context, culture, value));
        }
        [MethodImpl(OptimizationExtensions.ForceInline)]
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return ((destinationType == typeof(string)) ? value.ToStringNoExcept() : base.ConvertTo(context, culture, value, destinationType));
        }
    }
}
