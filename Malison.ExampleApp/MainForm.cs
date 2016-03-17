using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Malison.Core;
using Malison.WinForms;

namespace Malison.ExampleApp
{
    public partial class MainForm : TerminalForm
    {
        public MainForm()
        {
            InitializeComponent();

            TerminalControl.GlyphSheet = new GlyphSheet(Properties.Resources.cp437_16x16, 16, 16);
            Terminal = new Terminal(80, 30, Encoding.GetEncoding(437));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Terminal[2, 2].Write("Hi, this is an example app.");

            Terminal[2, 4].Write("Here are some lines and boxes:");

            ITerminal blueTerm = Terminal[TermColor.Blue];

            Terminal[2, 6, 10, 3][TermColor.White].DrawBox();

            Terminal[13, 6, 10, 3][TermColor.White].DrawBox(DrawBoxOptions.DoubleLines);

            Terminal[2, 10].Write("Because this is tailored for games, there's some fun glyphs in here:");
            int[] glyphs = new int[]
            {
                0,
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8,
                9,
                10
            };

            int x = 4;
            for (int i = 0; i < glyphs.Length; i++)
            {
                Terminal[x, 12][TermColor.Orange].Write(glyphs[i]);
                x += 2;
            }

            Terminal[2, 14].Write("Background and foreground colors are supported:");

            TermColor[] colors = (TermColor[])Enum.GetValues(typeof(TermColor));
            for (int i = 0; i < colors.Length; i++)
            {
                Terminal[i + 3, 16][colors[i], TermColor.Black].Write('a');
                Terminal[i + 3, 17][TermColor.Black, colors[i]].Write('b');
            }
        }
    }
}
