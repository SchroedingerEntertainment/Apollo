// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using SE.Config;
using SE.Json;

namespace SE.Apollo.Package
{
    /// <summary>
    /// Property converter from complex JSON to T
    /// </summary>
    public class ComplexConverter<T> : ITypeConverter where T : class, IAutoObject, new()
    {
        /// <summary>
        /// Creates a new converter instance
        /// </summary>
        public ComplexConverter()
        { }

        public bool TryParseValue(Type targetType, object value, out object result)
        {
            JsonNode node = (value as JsonNode);
            if (node != null && node.Type == JsonNodeType.Object)
            {
                JsonDocument settings = new JsonDocument();
                settings.AddAppend(null, node);

                result = new T();
                PropertyMapper.Assign(result, settings, true, true);
                
                return (result as T).IsValid;
            }
            else
            {
                result = null;
                return false;
            }
        }
    }
}
