using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WpfPlayground.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class SortingExtensions
    {
        private static readonly char numberSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
        private static readonly KeyTokenFactory keyTokenFactory = new KeyTokenFactory();
        private static readonly KeyComparer comparer = new KeyComparer();

        /// <summary>Natural alphanumeric sorting.</summary>
        /// <remarks>
        /// Replaces the original implementation SortingExtensions.OrderByNumeric.
        /// </remarks>
        public static IOrderedEnumerable<TSource> OrderByNumeric<TSource>(
          this IEnumerable<TSource> source,
          Func<TSource, string> keySelector)
        {
            return source.OrderBy(item => SplitOnDigitSequencesInner(keySelector(item)), comparer);
        }

        /// <summary>Natural alphanumeric sorting.</summary>
        /// <remarks>
        /// Replaces the original implementation SortingExtensions.OrderByNumeric.
        /// </remarks>
        public static IOrderedEnumerable<TSource> ThenByNumeric<TSource>(
          this IOrderedEnumerable<TSource> source,
          Func<TSource, string> keySelector)
        {
            return source.ThenBy(item => SplitOnDigitSequencesInner(keySelector(item)), comparer);
        }

        /// <summary>Split the string into a sequence of substrings.</summary>
        /// <example>
        /// null: []
        /// "abc 123 def": ["abc", "123", "def"]
        /// "123 def":         ["", "123", "def"]
        /// "123 def 456": ["", "123", "def", "456"]
        /// </example>
        public static string[] SplitOnDigitSequences(string text)
        {
            return SplitOnDigitSequencesInner(text).Tokens.Select(x => x.Token).ToArray();
        }

        private static Key SplitOnDigitSequencesInner(
          string sortingKey)
        {
            return new Key(new KeyTokenBuilder(sortingKey).Build());
        }

        private class KeyComparer : IComparer<Key>
        {
            public int Compare(Key x, Key y)
            {
                if (x == null && y == null)
                    return 0;
                if (x == null && y != null)
                    return -1;
                if (x != null && y == null)
                    return 1;
                return x.CompareTo(y);
            }
        }

        private class Key : IComparable<Key>
        {
            private KeyToken[] tokens;

            public Key(KeyToken[] tokens)
            {
                Tokens = tokens;
            }

            public KeyToken[] Tokens
            {
                get
                {
                    return tokens;
                }
                set
                {
                    tokens = value;
                }
            }

            public int CompareTo(Key other)
            {
                if (other == null)
                    return 1;
                if (this == other)
                    return 0;
                for (int index = 0; index < tokens.Length; ++index)
                {
                    KeyToken token1 = tokens[index];
                    if (index >= other.tokens.Length)
                        return 1;
                    KeyToken token2 = other.tokens[index];
                    int num = token1.CompareTo(token2);
                    if (num != 0)
                        return num;
                }
                return tokens.Length < other.tokens.Length ? -1 : 0;
            }

            public override string ToString()
            {
                return string.Format("Tokens: [{0}]", string.Join(", ", tokens.Select(x => x.ToString())));
            }
        }

        private class KeyTokenBuilder
        {
            private List<KeyToken> tokens = new List<KeyToken>();
            private readonly string text;
            private KeyToken current;

            public KeyTokenBuilder(string text)
            {
                this.text = text;
            }

            public KeyToken[] Build()
            {
                Parse();
                return tokens.ToArray();
            }

            private void Parse()
            {
                if (!TryStartToken(0))
                    return;
                int startIndex = current.StartIndex;
                while (startIndex < text.Length)
                {
                    char c = text[startIndex];
                    bool flag1 = char.IsDigit(c);
                    bool flag2 = startIndex == text.Length - 1;
                    switch (current.Type)
                    {
                        case KeyTokenType.Int:
                            if (flag1)
                            {
                                ++startIndex;
                                continue;
                            }
                            if (c == numberSeparator && !flag2 && char.IsDigit(text[startIndex + 1]))
                            {
                                current = keyTokenFactory.Create(current.StartIndex, KeyTokenType.Decimal);
                                startIndex += 2;
                                continue;
                            }
                            break;
                        case KeyTokenType.Decimal:
                            if (flag1)
                            {
                                ++startIndex;
                                continue;
                            }
                            break;
                        case KeyTokenType.String:
                            if (!flag1)
                            {
                                ++startIndex;
                                continue;
                            }
                            break;
                    }
                    TryEndToken(startIndex - 1);
                    if (!TryStartToken(startIndex))
                        return;
                    startIndex = current.StartIndex;
                }
                TryEndToken(startIndex);
            }

            private bool TryStartToken(int startIndex)
            {
                if (text == null || startIndex < 0 || startIndex >= text.Length && text.Length != 0)
                    return false;
                if (startIndex == 0 && text.Length == 0)
                {
                    current = keyTokenFactory.Create(startIndex, KeyTokenType.String);
                    return true;
                }
                char c;
                for (c = text[startIndex]; char.IsWhiteSpace(c); c = text[startIndex])
                {
                    ++startIndex;
                    if (startIndex >= text.Length)
                        return false;
                }
                KeyTokenType type = char.IsDigit(c) ? KeyTokenType.Int : KeyTokenType.String;
                current = keyTokenFactory.Create(startIndex, type);
                return true;
            }

            private void TryEndToken(int endIndex)
            {
                if (current == null)
                    return;
                if (current.StartIndex == 0 && text.Length == 0)
                {
                    current = keyTokenFactory.Create(current.StartIndex, KeyTokenType.String);
                    current.EndToken(endIndex, "");
                    tokens.Add(current);
                    current = null;
                }
                else
                {
                    endIndex = endIndex < current.StartIndex ? current.StartIndex : endIndex;
                    endIndex = endIndex > text.Length - 1 ? text.Length - 1 : endIndex;
                    for (char c = text[endIndex]; char.IsWhiteSpace(c); c = text[endIndex])
                    {
                        --endIndex;
                        if (endIndex <= current.StartIndex)
                            break;
                    }
                    string token = text.Substring(current.StartIndex, endIndex + 1 - current.StartIndex);
                    current.EndToken(endIndex, token);
                    tokens.Add(current);
                    current = null;
                }
            }
        }

        private abstract class KeyToken : IComparable<KeyToken>
        {
            private string token;
            private readonly int startIndex;
            private int endIndex;
            private readonly KeyTokenType type;

            protected KeyToken(int startIndex, KeyTokenType type)
            {
                this.startIndex = startIndex;
                this.type = type;
            }

            public string Token
            {
                get
                {
                    return token;
                }
            }

            public KeyTokenType Type
            {
                get
                {
                    return type;
                }
            }

            public int StartIndex
            {
                get
                {
                    return startIndex;
                }
            }

            public int EndIndex
            {
                get
                {
                    return endIndex;
                }
            }

            public virtual void EndToken(int endIndex, string token)
            {
                this.endIndex = endIndex;
                this.token = token;
                ParseSucceeded = true;
            }

            public bool ParseSucceeded { get; protected set; }

            public int CompareTo(KeyToken other)
            {
                if (other == null)
                    return 1;
                if (this == other || token == null && other.token == null)
                    return 0;
                if (token == null && other.token != null)
                    return -1;
                if (token != null && other.token == null)
                    return 1;
                if (token == "" && other.Token == "")
                    return 0;
                if (token == "" && other.Token != "")
                    return -1;
                if (token != "" && other.Token == "")
                    return 1;
                if (!ParseSucceeded || !other.ParseSucceeded)
                    return string.Compare(token, other.token, StringComparison.CurrentCulture);
                return CompareParsed(other);
            }

            /// <summary>
            /// Implemenations should compare tokens/parsed values should not check for null values or reference equality
            /// as this will already have been performed.
            /// </summary>
            /// <param name="other">The other.</param>
            /// <returns></returns>
            protected virtual int CompareParsed(KeyToken other)
            {
                return string.Compare(token, other.token, StringComparison.CurrentCulture);
            }

            public override string ToString()
            {
                if (token != null)
                    return string.Format("{0} Token: '{1}'", type, token);
                return string.Format("{0} Token: null", type);
            }
        }

        private class StringKeyToken : KeyToken
        {
            public StringKeyToken(int startIndex, KeyTokenType type)
              : base(startIndex, type)
            {
            }

            protected override int CompareParsed(KeyToken other)
            {
                if (other.Type != KeyTokenType.String)
                    return 1;
                return base.CompareParsed(other);
            }
        }

        private class IntKeyToken : KeyToken
        {
            private int parsed;

            public IntKeyToken(int startIndex, KeyTokenType type)
              : base(startIndex, type)
            {
            }

            public int Parsed
            {
                get
                {
                    return parsed;
                }
            }

            public override void EndToken(int endIndex, string token)
            {
                base.EndToken(endIndex, token);
                int result;
                bool flag = int.TryParse(token, out result);
                parsed = result;
                ParseSucceeded = flag;
            }

            protected override int CompareParsed(KeyToken other)
            {
                if (other.Type == KeyTokenType.String)
                    return -1;
                if (Type == other.Type)
                    return parsed.CompareTo(((IntKeyToken)other).parsed);
                if (other.Type == KeyTokenType.Decimal)
                    return ((DecimalKeyToken)other).Parsed.CompareTo(parsed) * -1;
                return base.CompareParsed(other);
            }
        }

        private class DecimalKeyToken : KeyToken
        {
            private decimal parsed;

            public DecimalKeyToken(int startIndex, KeyTokenType type)
              : base(startIndex, type)
            {
            }

            public decimal Parsed
            {
                get
                {
                    return parsed;
                }
            }

            public override void EndToken(int endIndex, string token)
            {
                base.EndToken(endIndex, token);
                decimal result;
                bool flag = decimal.TryParse(token, out result);
                parsed = result;
                ParseSucceeded = flag;
            }

            protected override int CompareParsed(KeyToken other)
            {
                if (other.Type == KeyTokenType.String)
                    return -1;
                if (Type == other.Type)
                    return parsed.CompareTo(((DecimalKeyToken)other).parsed);
                if (other.Type == KeyTokenType.Int)
                    return parsed.CompareTo(((IntKeyToken)other).Parsed);
                return base.CompareParsed(other);
            }
        }

        private class KeyTokenFactory
        {
            public KeyToken Create(
              int startIndex,
              KeyTokenType type)
            {
                if (type == KeyTokenType.Int)
                    return new IntKeyToken(startIndex, type);
                if (type == KeyTokenType.Decimal)
                    return new DecimalKeyToken(startIndex, type);
                return new StringKeyToken(startIndex, type);
            }
        }

        private enum KeyTokenType
        {
            Int,
            Decimal,
            String,
        }
    }
}
