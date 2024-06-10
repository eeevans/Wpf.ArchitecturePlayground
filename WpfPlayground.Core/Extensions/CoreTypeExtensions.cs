using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WpfPlayground.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class CoreTypeExtensions
    {

        /// <summary>Determines whether this instance is enumeration.</summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool IsEnumeration(this Type type)
        {
            return type.IsAssignableFromOpenGeneric(typeof(Enumeration<,>));
        }

        /// <summary>
        /// Determines whether [is castable to] [the specified to].
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="implicitly">if set to <c>true</c> [implicitly].</param>
        /// <returns></returns>
        public static bool IsCastableTo(this Type from, Type to, bool implicitly = false)
        {
            if (!to.IsAssignableFrom(from))
                return from.HasCastDefined(to, implicitly);
            return true;
        }

        /// <summary>
        /// Determines whether [has cast defined] [the specified to].
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="implicitly">if set to <c>true</c> [implicitly].</param>
        /// <returns></returns>
        private static bool HasCastDefined(this Type from, Type to, bool implicitly)
        {
            if ((from.IsPrimitive || from.IsEnum) && (to.IsPrimitive || to.IsEnum))
            {
                if (!implicitly)
                {
                    if (from == to)
                        return true;
                    if (from != typeof(bool))
                        return to != typeof(bool);
                    return false;
                }
                Type[][] typeArray1 = new Type[6][]
                {
          new Type[3]{ typeof (byte), typeof (sbyte), typeof (char) },
          new Type[2]{ typeof (short), typeof (ushort) },
          new Type[2]{ typeof (int), typeof (uint) },
          new Type[2]{ typeof (long), typeof (ulong) },
          new Type[1]{ typeof (float) },
          new Type[1]{ typeof (double) }
                };
                IEnumerable<Type> types = Enumerable.Empty<Type>();
                foreach (Type[] typeArray2 in typeArray1)
                {
                    if (typeArray2.Any(t => t == to))
                        return types.Any(t => t == from);
                    types = types.Concat(typeArray2);
                }
                return false;
            }
            if (!IsCastDefined(to, m => m.GetParameters()[0].ParameterType, _ => from, implicitly, false))
                return IsCastDefined(from, _ => to, m => m.ReturnType, implicitly, true);
            return true;
        }

        private static bool IsCastDefined(
          Type type,
          Func<MethodInfo, Type> baseType,
          Func<MethodInfo, Type> derivedType,
          bool implicitly,
          bool lookInBase)
        {
            BindingFlags bindingAttr = (BindingFlags)(24 | (lookInBase ? 64 : 2));
            return type.GetMethods(bindingAttr).Any(m =>
            {
                if (m.Name == "op_Implicit" || !implicitly && m.Name == "op_Explicit")
                    return baseType(m).IsAssignableFrom(derivedType(m));
                return false;
            });
        }
    }

}

