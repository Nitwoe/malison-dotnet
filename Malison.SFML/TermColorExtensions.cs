using Malison.Core;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malison.SFML
{
    public static class TermColorExtensions
    {
        public static Color ToSFMLColor(this TermColor color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}
