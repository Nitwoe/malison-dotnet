using System;
using System.Collections.Generic;
using System.Text;

using Bramble.Core;

namespace Malison.Core
{
    public interface IReadableTerminal
    {
        event EventHandler<CharacterEventArgs> CharacterChanged;

        Vector2D Size { get; }

        TermColor ForeColor { get; }
        TermColor BackColor { get; }

        Character Get(Vector2D pos);
        Character Get(int x, int y);
    }
}
