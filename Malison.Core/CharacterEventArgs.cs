using System;
using System.Collections.Generic;
using System.Text;

using Bramble.Core;

namespace Malison.Core
{
    public class CharacterEventArgs : EventArgs
    {
        public Vector2D Position { get { return mPos; } }
        public Character Character { get { return mCharacter; } }
        public int X { get { return mPos.X; } }
        public int Y { get { return mPos.Y; } }

        public CharacterEventArgs(Character character, Vector2D pos)
        {
            mCharacter = character;
            mPos = pos;
        }

        private Character mCharacter;
        private Vector2D mPos;
    }
}
