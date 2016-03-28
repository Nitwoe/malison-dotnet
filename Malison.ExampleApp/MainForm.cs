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

            // Initialize the terminal with a custom glyph sheet
            TerminalControl.GlyphSheet = new GlyphSheet(Properties.Resources.cp437_18x18, 16, 16);

            // Initialize terminal with proper character encoding
            Terminal = new Terminal(80, 30, Encoding.GetEncoding(437));
            m8x8ToolStripMenuItem.Click += m8x8ToolStripMenuItem_Click;
            m10x10ToolStripMenuItem.Click += m10x10ToolStripMenuItem_Click;
            m12x12ToolStripMenuItem.Click += m12x12ToolStripMenuItem_Click;
            m14x14ToolStripMenuItem.Click += m14x14ToolStripMenuItem_Click;
            m16x16ToolStripMenuItem.Click += m16x16ToolStripMenuItem_Click;
            m18x18ToolStripMenuItem.Click += m18x18ToolStripMenuItem_Click;
            m20x20ToolStripMenuItem.Click += m20x20ToolStripMenuItem_Click;
        }

        protected override void FontToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            m8x8ToolStripMenuItem.Checked = TerminalControl.GlyphSheet.Bitmap.Size == Properties.Resources.cp437_8x8.Size;
            m10x10ToolStripMenuItem.Checked = TerminalControl.GlyphSheet.Bitmap.Size == Properties.Resources.cp437_10x10.Size;
            m12x12ToolStripMenuItem.Checked = TerminalControl.GlyphSheet.Bitmap.Size == Properties.Resources.cp437_12x12.Size;
            m14x14ToolStripMenuItem.Checked = TerminalControl.GlyphSheet.Bitmap.Size == Properties.Resources.cp437_14x14.Size;
            m16x16ToolStripMenuItem.Checked = TerminalControl.GlyphSheet.Bitmap.Size == Properties.Resources.cp437_16x16.Size;
            m18x18ToolStripMenuItem.Checked = TerminalControl.GlyphSheet.Bitmap.Size == Properties.Resources.cp437_18x18.Size;
            m20x20ToolStripMenuItem.Checked = TerminalControl.GlyphSheet.Bitmap.Size == Properties.Resources.cp437_20x20.Size;
        }

        void m20x20ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerminalControl.GlyphSheet = new GlyphSheet(Properties.Resources.cp437_20x20, 16, 16);
        }

        void m18x18ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerminalControl.GlyphSheet = new GlyphSheet(Properties.Resources.cp437_18x18, 16, 16);
        }

        void m16x16ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerminalControl.GlyphSheet = new GlyphSheet(Properties.Resources.cp437_16x16, 16, 16);
        }

        void m14x14ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerminalControl.GlyphSheet = new GlyphSheet(Properties.Resources.cp437_14x14, 16, 16);
        }

        void m12x12ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerminalControl.GlyphSheet = new GlyphSheet(Properties.Resources.cp437_12x12, 16, 16);
        }

        void m10x10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerminalControl.GlyphSheet = new GlyphSheet(Properties.Resources.cp437_10x10, 16, 16);
        }

        void m8x8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerminalControl.GlyphSheet = new GlyphSheet(Properties.Resources.cp437_8x8, 16, 16);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Terminal[2, 2].Write("Hi, this is an example app ☺");

            Terminal[2, 4].Write("Here are some lines and boxes:");
            
            Terminal[2, 6, 10, 3][TermColor.White].DrawBox();

            Terminal[13, 6, 10, 3][TermColor.White].DrawBox(DrawBoxOptions.DoubleLines);

            Terminal[2, 10].Write("Here's a test of sample characters:");

            for (int i = 0; i < 20; i++)
            {
                Terminal[2 + i, 12][TermColor.Yellow].Write(i);
            }

            Terminal[2, 14].Write("Background and foreground colors are supported:");

            TermColor[] colors = TermColor.Analogous(TermColor.Red, 20).ToArray();

            for (int i = 0; i < colors.Length; i++)
            {
                Terminal[i + 3, 16][colors[i], TermColor.Black].Write('a');
                Terminal[i + 3, 17][TermColor.Black, colors[i]].Write('b');
            }

            Terminal[2, 20].Write("Alpha channel for colors is supported:");

            for (int i = 0; i < 20; i++)
            {
                // Also supporting parsing from a string
                Terminal[i + 3, 22][TermColor.Parse(String.Format("#c0ffee{0:X2}", 255/(i+1))), TermColor.Parse("#c0ffee").Negative()].Write('☻');
            }

            Terminal[2, 24].Write("Lightening and darkening functions:");

            for (int i = 0; i < 20; i++)
            {
                Terminal[i + 3, 26][TermColor.Black.Lighter(1 / 20f * (i + 1))].Write('☻');
                Terminal[i + 3, 27][TermColor.White.Darker(1 / 20f * (i + 1))].Write('☻');
            }

            TermColor source = TermColor.Blue.Lighter(0.2);
            TermColor gradientDest = TermColor.Yellow.Lighter(0.2);

            Terminal[39, 1, 40, 8].DrawBox();

            Terminal[40, 2].Write("Source:");
            Terminal[48, 2][TermColor.White, source].Write(' ');

            Terminal[40, 4].Write("Triad example:");
            colors = TermColor.Triad(source).ToArray();
            for (int i = 0; i < colors.Length; i++)
            {
                Terminal[40 + i, 5][TermColor.White, colors[i]].Write(' ');
            } 
            
            Terminal[40, 6].Write("Tetrad example:");
            colors = TermColor.Tetrad(source).ToArray();
            for (int i = 0; i < colors.Length; i++)
            {
                Terminal[40 + i, 7][TermColor.White, colors[i]].Write(' ');
            }

            Terminal[60, 4].Write("Analogous example:");
            colors = TermColor.Analogous(source, 15).ToArray();
            for (int i = 0; i < colors.Length; i++)
            {
                Terminal[60 + i, 5][TermColor.White, colors[i]].Write(' ');
            }

            Terminal[60, 6].Write("Gradient example:");
            colors = TermColor.Gradient(source, gradientDest, 15).ToArray();
            for (int i = 0; i < colors.Length; i++)
            {
                Terminal[60 + i, 7][TermColor.White, colors[i]].Write(' ');
            }
        }
    }
}
