using System;
using System.Collections.Generic;
using System.Text;

using Bramble.Core;

namespace Malison.Core
{
    public abstract class TerminalBase : ITerminal
    {
        public TerminalBase() : this(TermColor.White, TermColor.Black)
        {
        }

        public TerminalBase(TermColor foreColor, TermColor backColor)
        {
            ForeColor = foreColor;
            BackColor = backColor;
            Encoding = Encoding.Unicode;
        }

        #region IReadableTerminal Members

        public event EventHandler<CharacterEventArgs> CharacterChanged;

        public abstract Vector2D Size { get; }

        public TermColor ForeColor { get; private set; }
        public TermColor BackColor { get; private set; }

        public Character Get(Vector2D pos)
        {
            return GetValueCore(FlipNegativePosition(pos));
        }

        public Character Get(int x, int y)
        {
            return Get(new Vector2D(x, y));
        }

        #endregion

        #region ITerminal Members

        public Encoding Encoding
        {
            get;
            protected set;
        }


        public void Set(Vector2D pos, Character value)
        {
            SetInternal(FlipNegativePosition(pos), value);
        }

        public void Set(int x, int y, Character value)
        {
            Set(new Vector2D(x, y), value);
        }

        public ITerminal this[Vector2D pos]
        {
            // if we aren't given a size, go all the way to the bottom-right corner of the terminal
            get { return this[pos, Size - pos]; }
        }

        public ITerminal this[int x, int y]
        {
            get { return this[new Vector2D(x, y)]; }
        }

        public ITerminal this[Rect rect]
        {
            get
            {
                return CreateWindowCore(ForeColor, BackColor, new Rect(FlipNegativePosition(rect.Position), rect.Size));
            }
        }

        public ITerminal this[Vector2D pos, Vector2D size]
        {
            get { return this[new Rect(pos, size)]; }
        }

        public ITerminal this[int x, int y, int width, int height]
        {
            get { return this[new Rect(x, y, width, height)]; }
        }

        public ITerminal this[TermColor foreColor, TermColor backColor]
        {
            get
            {
                return CreateWindowCore(foreColor, backColor, new Rect(Size));
            }
        }

        public ITerminal this[ColorPair color]
        {
            get { return this[color.Fore, color.Back]; }
        }

        public ITerminal this[TermColor foreColor]
        {
            get { return this[foreColor, BackColor]; }
        }

        public void Write(char ascii)
        {
            Write(new Character(Character.Encode(ascii, Encoding), ForeColor, BackColor));
        }

        public void Write(int code)
        {
            Write(new Character(code, ForeColor, BackColor));
        }

        public void Write(Character character)
        {
            Set(Vector2D.Zero, character);
        }

        public void Write(string text)
        {
            Write(new CharacterString(text, ForeColor, BackColor, Encoding));
        }

        public void Write(CharacterString text)
        {
            Vector2D pos = Vector2D.Zero;

            CheckBounds(pos.X, pos.Y);

            foreach (Character c in text)
            {
                Set(pos, c);
                pos += new Vector2D(1, 0);

                // don't run past edge
                if (pos.X >= Size.X) break;
            }
        }

        public void Scroll(Vector2D offset, Func<Vector2D, Character> scrollOnCallback)
        {
            int xStart = 0;
            int xEnd = Size.X;
            int xStep = 1;

            int yStart = 0;
            int yEnd = Size.Y;
            int yStep = 1;

            if (offset.X > 0)
            {
                xStep = -1;

                Obj.Swap(ref xStart, ref xEnd);

                // shift the bounds back. the loops below
                // are half-inclusive, so when we flip the
                // range, we need to make it inclusive on the
                // other end of the range.
                // in other words, if the bounds are 10-50,
                // when we flip, we need to iterate from 9-49
                xStart--;
                xEnd--;
            }

            if (offset.Y > 0)
            {
                yStep = -1;

                Obj.Swap(ref yStart, ref yEnd);

                // shift the bounds back. the loops below
                // are half-inclusive, so when we flip the
                // range, we need to make it inclusive on the
                // other end of the range.
                // in other words, if the bounds are 10-50,
                // when we flip, we need to iterate from 9-49
                yStart--;
                yEnd--;
            }

            Rect bounds = new Rect(Size);

            for (int y = yStart; y != yEnd; y += yStep)
            {
                for (int x = xStart; x != xEnd; x += xStep)
                {
                    Vector2D to = new Vector2D(x, y);
                    Vector2D from = to - offset;

                    if (bounds.Contains(from))
                    {
                        // can be scrolled from
                        Set(to, Get(from));
                    }
                    else
                    {
                        // nothing to scroll onto this char, so clear it
                        Set(to, scrollOnCallback(to));
                    }
                }
            }
        }

        public void Scroll(int x, int y, Func<Vector2D, Character> scrollOnCallback)
        {
            Scroll(new Vector2D(x, y), scrollOnCallback);
        }

        public void Clear()
        {
            Fill(0);
        }

        public void Fill(int code)
        {
            Character character = new Character(code, ForeColor, BackColor);
            foreach (Vector2D pos in new Rect(Size))
            {
                Set(pos, character);
            }
        }

        public void DrawBox()
        {
            DrawBox(DrawBoxOptions.Default);
        }

        public void DrawBox(DrawBoxOptions options)
        {
            Vector2D pos = Vector2D.Zero;

            if (Size.X == 1)
            {
                DrawVerticalLine(pos, Size.Y, options);
            }
            else if (Size.Y == 1)
            {
                DrawHorizontalLine(pos, Size.X, options);
            }
            else
            {
                int topLeft;
                int topRight;
                int bottomLeft;
                int bottomRight;
                int horizontal;
                int vertical;

                if ((options & DrawBoxOptions.DoubleLines) == DrawBoxOptions.DoubleLines)
                {
                    topLeft = Character.Encode('╔', Encoding);
                    topRight = Character.Encode('╗', Encoding);
                    bottomLeft = Character.Encode('╚', Encoding);
                    bottomRight = Character.Encode('╝', Encoding);
                    horizontal = Character.Encode('═', Encoding);
                    vertical = Character.Encode('║', Encoding);
                }
                else
                {                
                    topLeft = Character.Encode('┌', Encoding);
                    topRight = Character.Encode('┐', Encoding);
                    bottomLeft = Character.Encode('└', Encoding);
                    bottomRight = Character.Encode('┘', Encoding);
                    horizontal = Character.Encode('─', Encoding);
                    vertical = Character.Encode('│', Encoding);
                }

                // top left corner
                WriteLineChar(pos, topLeft);

                // top right corner
                WriteLineChar(pos.OffsetX(Size.X - 1), topRight);

                // bottom left corner
                WriteLineChar(pos.OffsetY(Size.Y - 1), bottomLeft);

                // bottom right corner
                WriteLineChar(pos + Size - 1, bottomRight);

                // top and bottom edges
                foreach (Vector2D iter in Rect.Row(pos.X + 1, pos.Y, Size.X - 2))
                {
                    WriteLineChar(iter, horizontal);
                    WriteLineChar(iter.OffsetY(Size.Y - 1), horizontal);
                }

                // left and right edges
                foreach (Vector2D iter in Rect.Column(pos.X, pos.Y + 1, Size.Y - 2))
                {
                    WriteLineChar(iter, vertical);
                    WriteLineChar(iter.OffsetX(Size.X - 1), vertical);
                }
            }
        }

        #endregion

        internal bool SetInternal(Vector2D pos, Character value)
        {
            if (SetValueCore(pos, value))
            {
                if (CharacterChanged != null) CharacterChanged(this, new CharacterEventArgs(value, pos));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Override this to get the <see cref="Character"/> at the given position in the terminal.
        /// </summary>
        /// <param name="pos">The position of the character to retrieve. Must be in bounds.</param>
        /// <returns>The character at that position.</returns>
        protected abstract Character GetValueCore(Vector2D pos);

        /// <summary>
        /// Override this to set the <see cref="Character"/> at the given position in the terminal.
        /// </summary>
        /// <param name="pos">The position of the character to write.</param>
        /// <param name="value">The character to write to the terminal.</param>
        /// <returns><c>true</c> if the character is different from what was already there.</returns>
        protected abstract bool SetValueCore(Vector2D pos, Character value);

        internal abstract ITerminal CreateWindowCore(TermColor foreColor, TermColor backColor, Rect bounds);

        private void DrawHorizontalLine(Vector2D pos, int length, DrawBoxOptions options)
        {
            // figure out which code to use
            int lineChar;

            if ((options & DrawBoxOptions.DoubleLines) == DrawBoxOptions.DoubleLines)
            {
                lineChar = Character.Encode('═', Encoding);
            }
            else
            {
                lineChar = Character.Encode('─', Encoding);
            }

            // middle
            foreach (Vector2D iter in Rect.Row(pos.X, pos.Y, length - 1))
            {
                WriteLineChar(iter, lineChar);
            }
        }

        private void DrawVerticalLine(Vector2D pos, int length, DrawBoxOptions options)
        {
            // figure out which code to use
            int lineChar;

            if ((options & DrawBoxOptions.DoubleLines) == DrawBoxOptions.DoubleLines)
            {
                lineChar = Character.Encode('║', Encoding);
            }
            else
            {
                lineChar = Character.Encode('│', Encoding);
            }

            // middle
            foreach (Vector2D iter in Rect.Column(pos.X, pos.Y, length - 1))
            {
                WriteLineChar(iter, lineChar);
            }
        }
        private Vector2D FlipNegativePosition(Vector2D pos)
        {
            // negative coordinates mean from the right/bottom edge
            if (pos.X < 0) pos.X = Size.X + pos.X;
            if (pos.Y < 0) pos.Y = Size.Y + pos.Y;

            return pos;
        }
        
        private void WriteLineChar(Vector2D pos, int code)
        {
            this[pos][ForeColor, BackColor].Write(code);
        }

        private void CheckBounds(int x, int y)
        {
            if (x >= Size.X) throw new ArgumentOutOfRangeException("x");
            if (y >= Size.Y) throw new ArgumentOutOfRangeException("y");

            // negative values are valid and mean "from the right or bottom", so apply and check range
            if ((x < 0) && (Size.X + x >= Size.X)) throw new ArgumentOutOfRangeException("x");
            if ((y < 0) && (Size.Y + y >= Size.Y)) throw new ArgumentOutOfRangeException("y");
        }

        private void CheckBounds(int x, int y, int width, int height)
        {
            //### bob: need to handle negative coords
            if (x < 0) throw new ArgumentOutOfRangeException("x");
            if (x >= Size.X) throw new ArgumentOutOfRangeException("x");
            if (y < 0) throw new ArgumentOutOfRangeException("y");
            if (y >= Size.Y) throw new ArgumentOutOfRangeException("y");
            if (width <= 0) throw new ArgumentException("width");
            if (x + width > Size.X) throw new ArgumentOutOfRangeException("width");
            if (height <= 0) throw new ArgumentException("height");
            if (y + height > Size.Y) throw new ArgumentOutOfRangeException("height");
        }

        private void CheckBounds(Vector2D pos, Vector2D size)
        {
            CheckBounds(pos.X, pos.Y, size.X, size.Y);
        }
    }
}
