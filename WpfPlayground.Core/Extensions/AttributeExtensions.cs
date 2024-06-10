// Decompiled with JetBrains decompiler
using System;
using System.Linq;

namespace WpfPlayground.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class AttributeExtensions
    {
        /// <summary>Gets the attribute.</summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static TAttribute GetAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            return (TAttribute)type.GetCustomAttributes(typeof(TAttribute), false).SingleOrDefault();
        }

        /// <summary>Gets the attribute value.</summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="getValueFunc">The get value function.</param>
        /// <param name="valueIfNoAttribute">The value if no attribute.</param>
        /// <returns></returns>
        public static TValue GetAttributeValue<TAttribute, TValue>(
          this Type type,
          Func<TAttribute, TValue> getValueFunc,
          TValue valueIfNoAttribute)
          where TAttribute : Attribute
        {
            TAttribute attribute = type.GetAttribute<TAttribute>();
            if (attribute != null)
                return getValueFunc(attribute);
            return valueIfNoAttribute;
        }
    }
}
