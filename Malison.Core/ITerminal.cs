using System;
using System.Collections.Generic;
using System.Text;

using Bramble.Core;

namespace Malison.Core
{
    public enum DrawBoxOptions
    {
        None          = 0x0,
        DoubleLines   = 0x1,
        ContinueLines = 0x2,

        Default = ContinueLines
    }

    public interface ITerminal : IReadableTerminal
    {
        void Write(char ascii);
        void Write(int code);
        void Write(Character character);
        void Write(string text);
        void Write(CharacterString text);

        void Scroll(Vector2D offset, Func<Vector2D, Character> scrollOnCallback);
        void Scroll(int x, int y, Func<Vector2D, Character> scrollOnCallback);

        void Clear();

        void Fill(int code);

        ITerminal this[TermColor foreColor] { get; }
        ITerminal this[TermColor foreColor, TermColor backColor] { get; }
        ITerminal this[ColorPair color] { get; }

        ITerminal this[Vector2D pos] { get; }
        ITerminal this[int x, int y] { get; }
        ITerminal this[Rect rect] { get; }
        ITerminal this[Vector2D pos, Vector2D size] { get; }
        ITerminal this[int x, int y, int width, int height] { get; }

        void Set(Vector2D pos, Character value);
        void Set(int x, int y, Character value);
    }
}
