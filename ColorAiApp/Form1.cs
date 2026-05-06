using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace ColorAiApp
{
    public partial class Form1 : Form
    {
        private ColorService _service;
        private ContextMenuStrip _labelMenu;

        public Form1()
        {
            InitializeComponent();
            _service = new ColorService("*API*");

            InitializeLabelCopying();
        }

        string filePath = "C:\\Users\\minep\\Desktop\\univ_2\\programming with ai\\project\\ColorAiApp\\history.txt";

        private async void btnGenerate_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtInput.Text))
            {
                MessageBox.Show("Enter description");
                return;
            }

            var colors = await _service.GetColors(txtInput.Text);

            if (colors.Count < 3) return;

            ApplyColors(colors);

            SaveHistory(txtInput.Text, colors);
        }

        private void ApplyColors(List<string> colors)
        {
            panel1.BackColor = ColorTranslator.FromHtml(colors[0]);
            panel2.BackColor = ColorTranslator.FromHtml(colors[1]);
            panel3.BackColor = ColorTranslator.FromHtml(colors[2]);

            var c1 = ColorTranslator.FromHtml(colors[0]);
            var c2 = ColorTranslator.FromHtml(colors[1]);
            var c3 = ColorTranslator.FromHtml(colors[2]);

            label1.Text = $"{colors[0]}\n" +
                $"RR:{c1.R}\n" +
                $"GG:{c1.G}\n" +
                $"BB:{c1.B}";
            label2.Text = $"{colors[1]}\n" +
                $"RR:{c2.R}\n" +
                $"GG:{c2.G}\n" +
                $"BB:{c2.B}";
            label3.Text = $"{colors[2]}\n" +
                $"RR:{c3.R}\n" +
                $"GG:{c3.G}\n" +
                $"BB:{c3.B}";
        }

        void SaveHistory(string input, List<string> colors)
        {
            string line = $"{input}|{colors[0]},{colors[1]},{colors[2]}";
            File.AppendAllText(filePath, line + Environment.NewLine);
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            var form = new HistoryForm();

            form.OnItemSelected = (description, colors) =>
            {
                txtInput.Text = description;
                ApplyColors(colors);
            };

            form.Show();
        }

        private void InitializeLabelCopying()
        {
            _labelMenu = new ContextMenuStrip();
            var miCopyHex = new ToolStripMenuItem("Copy Hex");
            var miCopyRgb = new ToolStripMenuItem("Copy RGB");
            var miCopyAll = new ToolStripMenuItem("Copy All");

            miCopyHex.Click += CopyHex_Click;
            miCopyRgb.Click += CopyRGB_Click;
            miCopyAll.Click += CopyAll_Click;

            _labelMenu.Items.AddRange(new ToolStripItem[] { miCopyHex, miCopyRgb, miCopyAll });

            label1.MouseUp += Label_MouseUp;
            label2.MouseUp += Label_MouseUp;
            label3.MouseUp += Label_MouseUp;

            label1.DoubleClick += Label_DoubleClick;
            label2.DoubleClick += Label_DoubleClick;
            label3.DoubleClick += Label_DoubleClick;

            label1.Cursor = Cursors.Hand;
            label2.Cursor = Cursors.Hand;
            label3.Cursor = Cursors.Hand;
        }

        private void Label_MouseUp(object? sender, MouseEventArgs e)
        {
            if (sender is not Label lbl) return;

            if (e.Button == MouseButtons.Right)
            {
                _labelMenu.Tag = lbl;
                _labelMenu.Show(Cursor.Position);
            }
            else if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.Control)
            {
                CopyHex(lbl);
            }
        }

        private void Label_DoubleClick(object? sender, EventArgs e)
        {
            if (sender is not Label lbl) return;
            CopyHex(lbl);
        }

        private void CopyHex_Click(object? sender, EventArgs e)
        {
            if (_labelMenu.Tag is not Label lbl) return;
            CopyHex(lbl);
        }

        private void CopyRGB_Click(object? sender, EventArgs e)
        {
            if (_labelMenu.Tag is not Label lbl) return;
            var lines = GetLabelLines(lbl);
            if (lines.Length <= 1)
            {
                Clipboard.SetText(string.Empty);
                return;
            }

            var rgb = string.Join(Environment.NewLine, lines.Skip(1));
            Clipboard.SetText(rgb);
        }

        private void CopyAll_Click(object? sender, EventArgs e)
        {
            if (_labelMenu.Tag is not Label lbl) return;
            Clipboard.SetText(lbl.Text);
        }

        private static string[] GetLabelLines(Label lbl)
        {
            return lbl.Text
                      .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                      .Select(s => s.Trim())
                      .ToArray();
        }

        private static void CopyHex(Label lbl)
        {
            var lines = GetLabelLines(lbl);
            if (lines.Length == 0) return;
            var hex = lines[0];
            Clipboard.SetText(hex);
        }
    }

}
