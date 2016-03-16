using System;
using System.Collections.Generic;
using System.Text;

namespace Malison.Core
{
    public struct Character : IEquatable<Character>
    {
        //### bob: move these out of here
        /// <summary>
        /// Gets the default foreground <see cref="Color"/> for a Character.
        /// </summary>
        public static TermColor DefaultForeColor { get { return TermColor.White; } }

        /// <summary>
        /// Gets the default background <see cref="Color"/> for a Character.
        /// </summary>
        public static TermColor DefaultBackColor { get { return TermColor.Black; } }

        /// <summary>
        /// Gets the code represented by the given ASCII character.
        /// </summary>
        /// <param name="ascii"></param>
        /// <returns></returns>
        public static int ToCode(char ascii)
        {
            return ToCode(ascii, Encoding.Unicode);
        }

        /// <summary>
        /// Gets the code represented by the given ASCII character.
        /// </summary>
        /// <param name="ascii"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static int ToCode(char ascii, Encoding encoding)
        {
            return (int)(encoding.GetBytes(ascii.ToString())[0]);
        }

        /// <summary>
        /// Gets the code used to draw this Character.
        /// </summary>
        public int Code { get { return mCode; } }

        /// <summary>
        /// Gets the foreground <see cref="Color"/> of this Character.
        /// </summary>
        public TermColor ForeColor
        {
            get
            {
                /*
                // default if empty
                if (mForeColor == Color.Empty) mForeColor = DefaultForeColor;
                */

                return mForeColor;
            }
        }

        /// <summary>
        /// Gets the background <see cref="Color"/> of this Character.
        /// </summary>
        public TermColor BackColor
        {
            get
            {
                /*
                // default if empty
                if (mBackColor == Color.Empty) mBackColor = DefaultBackColor;
                */

                return mBackColor;
            }
        }

        /// <summary>
        /// Returns true if the <see cref="Glyph"/> for this Character is a non-visible
        /// whitespace Glyph.
        /// </summary>
        public bool IsWhitespace { get { return mCode == 0; } }

        /// <summary>
        /// Initializes a new Character.
        /// </summary>
        /// <param name="glyph">Glyph used to draw the Character.</param>
        /// <param name="foreColor">Foreground <see cref="TermColor"/> of the Character.</param>
        /// <param name="backColor">Background <see cref="TermColor"/> of the Character.</param>
        public Character(int code, TermColor foreColor, TermColor backColor)
        {
            mCode = code;
            mBackColor = backColor;
            mForeColor = foreColor;
        }

        /// <summary>
        /// Initializes a new Character using the default background <see cref="TermColor"/>.
        /// </summary>
        /// <param name="glyph">Glyph used to draw the Character.</param>
        /// <param name="foreColor">Foreground <see cref="TermColor"/> of the Character.</param>
        public Character(int code, TermColor foreColor)
            : this(code, foreColor, DefaultBackColor)
        {
        }

        /// <summary>
        /// Initializes a new Character using the default background and foreground
        /// <see cref="TermColor"/>.
        /// </summary>
        /// <param name="glyph">Glyph used to draw the Character.</param>
        public Character(int code)
            : this(code, DefaultForeColor)
        {
        }

        /// <summary>
        /// Initializes a new Character.
        /// </summary>
        /// <param name="ascii">ASCII representation of the <see cref="Glyph"/> used
        /// to draw the Character.</param>
        /// <param name="foreColor">Foreground <see cref="TermColor"/> of the Character.</param>
        /// <param name="backColor">Background <see cref="TermColor"/> of the Character.</param>
        public Character(char ascii, TermColor foreColor, TermColor backColor)
            : this(Character.ToCode(ascii), foreColor, backColor)
        {
        }

        /// <summary>
        /// Initializes a new Character using the default background <see cref="Color"/>.
        /// </summary>
        /// <param name="ascii">ASCII representation of the <see cref="Glyph"/> used
        /// to draw the Character.</param>
        /// <param name="foreColor">Foreground <see cref="TermColor"/> of the Character.</param>
        public Character(char ascii, TermColor foreColor)
            : this(Character.ToCode(ascii), foreColor, DefaultBackColor)
        {
        }

        /// <summary>
        /// Initializes a new Character using the default background and foreground
        /// <see cref="Color"/>.
        /// </summary>
        /// <param name="ascii">ASCII representation of the <see cref="Glyph"/> used
        /// to draw the Character.</param>
        public Character(char ascii)
            : this(Character.ToCode(ascii), DefaultForeColor)
        {
        }

        /// <summary>
        /// Gets a string representation of this Character.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return mCode.ToString();
        }

        /// <summary>
        /// Determines whether the specified object equals this <see cref="Character"/>.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns><c>true</c> if <c>obj</c> is a Character equivalent to this Character; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Character) return Equals((Character)obj);

            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this <see cref="Character"/>.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this Character.</returns>
        public override int GetHashCode()
        {
            return mCode.GetHashCode() + mBackColor.GetHashCode() + mForeColor.GetHashCode();
        }

        #region IEquatable<Character> Members

        /// <summary>
        /// Determines whether the specified <see cref="Character"/> equals this one.
        /// </summary>
        /// <param name="other">The <see cref="Character"/> to test.</param>
        /// <returns><c>true</c> if <c>other</c> is equivalent to this Character; otherwise, <c>false</c>.</returns>
        public bool Equals(Character other)
        {
            return (mCode == other.mCode) && mBackColor.Equals(other.mBackColor) && mForeColor.Equals(other.mForeColor);
        }

        #endregion

        private int mCode;
        private TermColor mForeColor;
        private TermColor mBackColor;
    }
}
