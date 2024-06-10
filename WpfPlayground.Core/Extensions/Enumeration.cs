using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace WpfPlayground.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEnumeration">The type of the enumeration.</typeparam>
    [DebuggerDisplay("{DisplayName} - {Value}")]
    [Serializable]
    public abstract class Enumeration<TEnumeration> : Enumeration<TEnumeration, int>
      where TEnumeration : Enumeration<TEnumeration>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Enumeration`1" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="displayName">The display name.</param>
        protected Enumeration(int value, string displayName)
          : base(value, displayName)
        {
        }

        /// <summary>Froms the int32.</summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static TEnumeration FromInt32(int value)
        {
            return Enumeration<TEnumeration, int>.FromValue(value);
        }

        /// <summary>Tries from int32.</summary>
        /// <param name="listItemValue">The list item value.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool TryFromInt32(int listItemValue, out TEnumeration result)
        {
            return Enumeration<TEnumeration, int>.TryParse(listItemValue, out result);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEnumeration">The type of the enumeration.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [DebuggerDisplay("{DisplayName} - {Value}")]
    [Serializable]
    public abstract class Enumeration<TEnumeration, TValue> : IComparable<TEnumeration>, IEquatable<TEnumeration>
      where TEnumeration : Enumeration<TEnumeration, TValue>
      where TValue : IComparable
    {
        private static Lazy<TEnumeration[]> _enumerations = new Lazy<TEnumeration[]>(new Func<TEnumeration[]>(Enumeration<TEnumeration, TValue>.GetEnumerations));
        private readonly string _displayName;
        private readonly TValue _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Enumeration`2" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="displayName">The display name.</param>
        protected Enumeration(TValue value, string displayName)
        {
            this._value = value;
            this._displayName = displayName;
        }

        /// <summary>Gets the value.</summary>
        /// <value>The value.</value>
        public TValue Value
        {
            get
            {
                return this._value;
            }
        }

        /// <summary>Gets the display name.</summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get
            {
                return this._displayName;
            }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" /> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the sort order.
        /// </returns>
        public int CompareTo(TEnumeration other)
        {
            return this.Value.CompareTo((object)other.Value);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String" /> that represents this instance.
        /// </returns>
        public override sealed string ToString()
        {
            return this.DisplayName;
        }

        /// <summary>Gets all.</summary>
        /// <returns></returns>
        public static TEnumeration[] GetAll()
        {
            return Enumeration<TEnumeration, TValue>._enumerations.Value;
        }

        /// <summary>Gets all.</summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static TEnumeration[] GetAll(Func<TEnumeration, bool> predicate)
        {
            return ((IEnumerable<TEnumeration>)Enumeration<TEnumeration, TValue>.GetAll()).Where<TEnumeration>(predicate).ToArray<TEnumeration>();
        }

        private static TEnumeration[] GetEnumerations()
        {
            Type enumerationType = typeof(TEnumeration);
            return ((IEnumerable<FieldInfo>)enumerationType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public)).Where<FieldInfo>((Func<FieldInfo, bool>)(info => enumerationType.IsAssignableFrom(info.FieldType))).Select<FieldInfo, object>((Func<FieldInfo, object>)(info => info.GetValue((object)null))).Cast<TEnumeration>().ToArray<TEnumeration>();
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="T:System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as TEnumeration);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(TEnumeration other)
        {
            if ((Enumeration<TEnumeration, TValue>)other != (Enumeration<TEnumeration, TValue>)null)
                return this.Value.Equals((object)other.Value);
            return false;
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>Implements the operator ==.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(
          Enumeration<TEnumeration, TValue> left,
          Enumeration<TEnumeration, TValue> right)
        {
            return object.Equals((object)left, (object)right);
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(
          Enumeration<TEnumeration, TValue> left,
          Enumeration<TEnumeration, TValue> right)
        {
            return !object.Equals((object)left, (object)right);
        }

        /// <summary>Froms the value.</summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static TEnumeration FromValue(TValue value)
        {
            return Enumeration<TEnumeration, TValue>.Parse((object)value, nameof(value), (Func<TEnumeration, bool>)(item => item.Value.Equals((object)value)));
        }

        /// <summary>Parses the specified display name.</summary>
        /// <param name="displayName">The display name.</param>
        /// <returns></returns>
        public static TEnumeration Parse(string displayName)
        {
            return Enumeration<TEnumeration, TValue>.Parse((object)displayName, "display name", (Func<TEnumeration, bool>)(item => item.DisplayName == displayName));
        }

        private static bool TryParse(Func<TEnumeration, bool> predicate, out TEnumeration result)
        {
            result = ((IEnumerable<TEnumeration>)Enumeration<TEnumeration, TValue>.GetAll()).FirstOrDefault<TEnumeration>(predicate);
            return (Enumeration<TEnumeration, TValue>)result != (Enumeration<TEnumeration, TValue>)null;
        }

        private static TEnumeration Parse(
          object value,
          string description,
          Func<TEnumeration, bool> predicate)
        {
            TEnumeration result;
            if (!Enumeration<TEnumeration, TValue>.TryParse(predicate, out result))
                throw new ArgumentException(string.Format("'{0}' is not a valid {1} in {2}", value, (object)description, (object)typeof(TEnumeration)), nameof(value));
            return result;
        }

        /// <summary>Tries the parse.</summary>
        /// <param name="value">The value.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool TryParse(TValue value, out TEnumeration result)
        {
            return Enumeration<TEnumeration, TValue>.TryParse((Func<TEnumeration, bool>)(e => e.Value.Equals((object)value)), out result);
        }

        /// <summary>Tries the parse.</summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool TryParse(string displayName, out TEnumeration result)
        {
            return Enumeration<TEnumeration, TValue>.TryParse((Func<TEnumeration, bool>)(e => e.DisplayName == displayName), out result);
        }
    }

}

