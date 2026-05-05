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

        private async void btnGenerate_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtInput.Text))
            {
                MessageBox.Show("Enter description");
                return;
            }
             
            var colors = await _service.GetColors(txtInput.Text);

            if (colors.Count < 3) return;

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

       
    }

}
