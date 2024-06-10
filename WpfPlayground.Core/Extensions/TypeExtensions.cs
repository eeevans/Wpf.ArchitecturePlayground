using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace WpfPlayground.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>Ensures the type is nullable.</summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns></returns>
        public static Type EnsureTypeIsNullable(this Type propertyType)
        {
            if ((!propertyType.IsGenericType ? 0 : (propertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? 1 : 0)) != 0 || !propertyType.IsValueType)
                return propertyType;
            return typeof(Nullable<>).MakeGenericType(propertyType);
        }

        /// <summary>Determines whether this instance is nullable.</summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns></returns>
        public static bool IsNullable(this Type propertyType)
        {
            if (propertyType.IsGenericType)
                return propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            return false;
        }

        /// <summary>Tries the get interface.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <returns></returns>
        public static bool TryGetInterface<T>(this Type type, out Type interfaceType)
        {
            interfaceType = ((IEnumerable<Type>)type.GetInterfaces()).FirstOrDefault<Type>((Func<Type, bool>)(t => t == typeof(T)));
            return interfaceType != (Type)null;
        }

        /// <summary>Gets the custom attribute.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(this Type type)
        {
            object obj = ((IEnumerable<object>)type.GetCustomAttributes(typeof(T), false)).FirstOrDefault<object>();
            if (obj != null)
                return (T)obj;
            return default(T);
        }

        /// <summary>Determines whether [has custom attribute].</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool HasCustomAttribute<T>(this Type type)
        {
            return ((IEnumerable<object>)type.GetCustomAttributes(typeof(T), false)).FirstOrDefault<object>() != null;
        }

        /// <summary>Closes the generic with.</summary>
        /// <param name="genericType">Type of the generic.</param>
        /// <param name="innerTypes">The inner types.</param>
        /// <returns></returns>
        public static Type CloseGenericWith(this Type genericType, params Type[] innerTypes)
        {
            return genericType.MakeGenericType(innerTypes);
        }

        /// <summary>Determines whether [is assignable to].</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool IsAssignableTo<T>(this Type type)
        {
            return type.IsAssignableTo(typeof(T));
        }

        /// <summary>
        /// Determines whether [is assignable to] [the specified to type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="toType">To type.</param>
        /// <returns></returns>
        public static bool IsAssignableTo(this Type type, Type toType)
        {
            return toType.IsAssignableFrom(type);
        }

        /// <summary>Determines whether [is assignable from].</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool IsAssignableFrom<T>(this Type type)
        {
            return type.IsAssignableFrom(typeof(T));
        }

        /// <summary>
        /// Determines whether [is assignable from open generic] [the specified open generic type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="openGenericType">Type of the open generic.</param>
        /// <returns></returns>
        public static bool IsAssignableFromOpenGeneric(this Type type, Type openGenericType)
        {
            for (Type baseType = type.BaseType; baseType != typeof(object) && !(baseType == (Type)null); baseType = baseType.BaseType)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == openGenericType)
                    return true;
            }
            return false;
        }

        /// <summary>Finds the interfaces that close.</summary>
        /// <param name="type">The type.</param>
        /// <param name="openGeneric">The open generic.</param>
        /// <returns></returns>
        public static IEnumerable<Type> FindInterfacesThatClose(
          this Type type,
          Type openGeneric)
        {
            return ((IEnumerable<Type>)type.GetInterfaces()).Where<Type>((Func<Type, bool>)(x => x.ClosesGeneric(openGeneric)));
        }

        /// <summary>Closeses the generic.</summary>
        /// <param name="type">The type.</param>
        /// <param name="openGeneric">The open generic.</param>
        /// <returns></returns>
        public static bool ClosesGeneric(this Type type, Type openGeneric)
        {
            if (type.IsGenericType)
                return type.GetGenericTypeDefinition() == openGeneric;
            return false;
        }

        /// <summary>Gets the generic arguments for interface that closes.</summary>
        /// <param name="type">The type.</param>
        /// <param name="openGeneric">The open generic.</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetGenericArgumentsForInterfaceThatCloses(
          this Type type,
          Type openGeneric)
        {
            return (IEnumerable<Type>)type.FindInterfacesThatClose(openGeneric).Single<Type>().GetGenericArguments();
        }

        /// <summary>Gets the type of the type or underlying nullable.</summary>
        /// <param name="possibleNullableType">Type of the possible nullable.</param>
        /// <returns></returns>
        public static Type GetTypeOrUnderlyingNullableType(this Type possibleNullableType)
        {
            if (possibleNullableType.IsNull())
                return (Type)null;
            if (!possibleNullableType.IsGenericType || !(possibleNullableType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                return possibleNullableType;
            return Nullable.GetUnderlyingType(possibleNullableType);
        }

        /// <summary>Implementses the interface template.</summary>
        /// <param name="type">The type.</param>
        /// <param name="openGeneric">The open generic.</param>
        /// <returns></returns>
        public static bool ImplementsInterfaceTemplate(this Type type, Type openGeneric)
        {
            return ((IEnumerable<Type>)type.GetInterfaces()).Any<Type>((Func<Type, bool>)(x =>
            {
                if (x.IsGenericType)
                    return x.GetGenericTypeDefinition() == openGeneric;
                return false;
            }));
        }

        /// <summary>News the instance.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static T NewInstance<T>(this Type type, params object[] args)
        {
            return (T)Activator.CreateInstance(type, args);
        }

        /// <summary>Gets all interfaces properties.</summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetAllInterfacesProperties(
          this Type interfaceType)
        {
            return ((IEnumerable<PropertyInfo>)interfaceType.GetProperties()).Union<PropertyInfo>(((IEnumerable<Type>)interfaceType.GetInterfaces()).SelectMany<Type, PropertyInfo>((Func<Type, IEnumerable<PropertyInfo>>)(x => (IEnumerable<PropertyInfo>)x.GetProperties()))).Distinct<PropertyInfo>();
        }

        /// <summary>Gets the default value.</summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static object GetDefaultValue(this Type type)
        {
            return typeof(TypeExtensions).GetMethod("GetGenericDefaultValue").MakeGenericMethod(type).Invoke((object)null, new object[0]);
        }

        /// <summary>Calls the generic method.</summary>
        /// <param name="type">The type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="genericType">Type of the generic.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static object CallGenericMethod(
          this Type type,
          string methodName,
          Type genericType,
          object instance,
          params object[] args)
        {
            return type.GetMethod(methodName).MakeGenericMethod(genericType).Invoke(instance, args);
        }

        /// <summary>Gets the generic default value.</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <remarks>this method is called via reflection</remarks>
        public static T GetGenericDefaultValue<T>()
        {
            return default(T);
        }

        /// <summary>Clones the object based on properties.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original">The original.</param>
        /// <returns></returns>
        public static T CloneObjectBasedOnProperties<T>(this T original)
        {
            Type type = typeof(T);
            T clone = (T)Activator.CreateInstance(type);
            ((IEnumerable<PropertyInfo>)type.GetProperties()).ForEach<PropertyInfo>((Action<PropertyInfo>)(p => p.SetValue((object)clone, p.GetValue((object)(T)original, (object[])null), (object[])null)));
            return clone;
        }

        /// <summary>Tests to determine if a Type is an Anonymous.</summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsAnonymousType(this Type type)
        {
            if (type.Name[0] == '<' && type.Name[1] == '>')
                return type.Name.IndexOf("AnonymousType", StringComparison.OrdinalIgnoreCase) >= 0;
            return false;
        }

        public static T CloneObjectBasedOnProperties<T>(this T original, T clone)
        {
            Type type = typeof(T);
            type.GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetSetMethod() != null)
                .ToArray().ForEach(p => p.SetValue(clone, p.GetValue(original, null), null));
            return clone;
        }

        public static string NullToEmptyString(object value)
        {
            return value == null ? string.Empty : value is IConvertible ? ((IConvertible)value).ToString(CultureInfo.CreateSpecificCulture("en-US")) : value.ToString();
        }
    }
}
