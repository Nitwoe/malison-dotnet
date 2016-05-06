using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using Malison.Core;
using SFML.Window;

namespace Malison.SFML
{
    public class GlyphSheet
    {
        private Dictionary<Character, Sprite> spriteCache;

        public Texture SpriteSheet
        {
            get;
            private set;
        }

        public int GlyphsPerRow
        {
            get;
            private set;
        }

        public int GlyphsRows
        {
            get;
            private set;
        }

        public int Width { get { return (int)SpriteSheet.Size.X / GlyphsPerRow; } }
        public int Height { get { return (int)SpriteSheet.Size.Y / GlyphsRows; } }

        public GlyphSheet(Texture spritesheet, int perRow, int rows)
        {
            SpriteSheet = spritesheet;
            GlyphsPerRow = perRow;
            GlyphsRows = rows;

            spriteCache = new Dictionary<Character, Sprite>();
        }

        public void Draw(RenderWindow window, int x, int y, Character character)
        {
            if (character.Code == 0)
                return;

            Sprite sprite = GetSprite(character);

            sprite.Position = new Vector2f(x * Width, y * Height);
            
            RectangleShape rs = new RectangleShape(new Vector2f(Width, Height));
            rs.Position = new Vector2f(x * Width, y * Height);
            rs.FillColor = character.BackColor.ToSFMLColor();
            window.Draw(rs);

            window.Draw(sprite);
        }

        private Sprite GetSprite(Character character)
        {
            Sprite sprite = null;

            //If sprite was cached, return it
            if(spriteCache.TryGetValue(character, out sprite))
            {
                return sprite;
            }

            int column = character.Code % GlyphsPerRow;
            int row = character.Code / GlyphsPerRow;

            IntRect srcRect = new IntRect(column * Width, row * Height, Width, Height);

            sprite = new Sprite(SpriteSheet);
            sprite.TextureRect = srcRect;
            sprite.Color = character.ForeColor.ToSFMLColor();

            // cache it
            spriteCache[character] = sprite;

            return sprite;
        }
    }
}
