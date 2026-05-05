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
            var colors = await _service.GetColors(txtInput.Text);

            if (colors.Count < 3) return;

            panel1.BackColor = ColorTranslator.FromHtml(colors[0]);
            panel2.BackColor = ColorTranslator.FromHtml(colors[1]);
            panel3.BackColor = ColorTranslator.FromHtml(colors[2]);

            label1.Text = colors[0];
            label2.Text = colors[1];
            label3.Text = colors[2];
        }
    }
    
}
