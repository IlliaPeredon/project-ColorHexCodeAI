using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace ColorAiApp
{

    public partial class HistoryForm : Form
    {
        public HistoryForm()
        {
            InitializeComponent();
            LoadHistory();
        }

        public Action<string, List<string>> OnItemSelected;

        private List<string> _rawLines = new List<string>();

        void LoadHistory()
        {
            string path = "C:\\Users\\minep\\Desktop\\univ_2\\programming with ai\\project\\ColorAiApp\\history.txt";
            if (!File.Exists(path)) return;

            var lines = File.ReadAllLines(path);

            _rawLines = lines.ToList();
            listBox1.Items.Clear();

            foreach (var line in lines)
            {
                var parts = line.Split('|');
                listBox1.Items.Add(parts[0]);
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index < 0) return;

            var line = _rawLines[index];

            var parts = line.Split('|');
            var description = parts[0];
            var colors = parts[1].Split(',').ToList();

            OnItemSelected?.Invoke(description, colors);

        }
    }
}
