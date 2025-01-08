using System;
using System.Windows.Forms;

namespace PasswordChecker
{
    public partial class MainForm : Form
    {
        private TextBox passwordTextBox;
        private Button checkButton;
        private Button generateButton;
        private Label resultLabel;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.passwordTextBox = new TextBox();
            this.checkButton = new Button();
            this.generateButton = new Button();
            this.resultLabel = new Label();

            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(12, 12);
            this.passwordTextBox.Size = new System.Drawing.Size(260, 20);

            // 
            // checkButton
            // 
            this.checkButton.Location = new System.Drawing.Point(12, 38);
            this.checkButton.Size = new System.Drawing.Size(75, 23);
            this.checkButton.Text = "Vérifier";
            this.checkButton.Click += new EventHandler(this.CheckButton_Click);

            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(197, 38);
            this.generateButton.Size = new System.Drawing.Size(75, 23);
            this.generateButton.Text = "Générer";
            this.generateButton.Click += new EventHandler(this.GenerateButton_Click);

            // 
            // resultLabel
            // 
            this.resultLabel.Location = new System.Drawing.Point(12, 64);
            this.resultLabel.Size = new System.Drawing.Size(260, 23);

            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 101);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.checkButton);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.resultLabel);
            this.Text = "Password Checker";
        }

        private async void CheckButton_Click(object sender, EventArgs e)
        {
            string password = this.passwordTextBox.Text;
            string message;

            if (PasswordUtils.IsSecure(password, out message))
            {
                this.resultLabel.Text = "✅ " + message;
            }
            else
            {
                this.resultLabel.Text = "❌ " + message;
            }

            bool isPwned = await PasswordUtils.IsPwnedAsync(password);
            if (isPwned)
            {
                MessageBox.Show("⚠️ Ce mot de passe a été compromis !", "Alerte");
            }
            else
            {
                MessageBox.Show("✅ Ce mot de passe n'a pas été trouvé dans les bases compromises.", "Sécurité");
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            string securePassword = PasswordUtils.GenerateSecurePassword();
            MessageBox.Show("💡 Mot de passe généré : " + securePassword, "Générateur de mot de passe");
        }
    }
}
