// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Reflection;
using SE.Config;
using SE.Json;

namespace SE.Apollo.Package
{
    /// <summary>
    /// Property converter from JSON property to KeyValuePair<T>
    /// </summary>
    public class KeyValuePairConverter : ITypeConverter
    {
        /// <summary>
        /// Creates a new converter instance
        /// </summary>
        public KeyValuePairConverter()
        { }

        public bool TryParseValue(Type targetType, object value, out object result)
        {
            JsonNode node = (value as JsonNode);
            if (node != null && node.Type == JsonNodeType.Object)
            {
                Type[] genericArgs = targetType.GetGenericArguments();
                Type functorType = typeof(Func<,,>).MakeGenericType(genericArgs[0], genericArgs[1], targetType);
                Delegate creator = functorType.GetCreator(targetType);

                List<object> items = CollectionPool<List<object>, object>.Get();
                try
                {
                    node = node.Child;
                    while (node != null)
                    {
                        if (node.Type > JsonNodeType.Array)
                        {
                            object k, v; if (node.Name.TryCast(genericArgs[0], out k) && k != null && node.RawValue.TryCast(genericArgs[1], out v) && v != null)
                            {
                                items.Add(creator.DynamicInvoke(k, v));
                            }
                        }
                        node = node.Next;
                    }
                    result = items.ToArray();
                    return true;
                }
                finally
                {
                    CollectionPool<List<object>, object>.Return(items);
                }
            }
            else
            {
                result = null;
                return false;
            }
        }
    }
}
