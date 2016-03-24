using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Bramble.Core;

using Malison.Core;

namespace Malison.WinForms
{
    public static class TermColorExtensions
    {
        /// <summary>
        /// Converts the Termcolor to System.Drawing.Color
        /// </summary>
        public static Color ToSystemColor(this TermColor color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);            
        }
    }
}
