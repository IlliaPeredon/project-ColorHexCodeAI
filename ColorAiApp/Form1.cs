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
        public Form1()
        {
            InitializeComponent();
            _service = new ColorService("*API*");

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
    }

}
