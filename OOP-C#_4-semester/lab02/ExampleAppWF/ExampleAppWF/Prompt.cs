using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExampleAppWF
{
    public static class Prompt
    {
        private static Form prompt;
        public static string ShowDialogWindow(string text, string caption)
        {
            prompt = new Form();
            prompt.Width = 400;
            prompt.Height = 150;
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.Text = caption;
            prompt.StartPosition = FormStartPosition.CenterScreen;

            Label textLabel = new Label()
            {
                Left = 20,
                Top = 20,
                Text = text,
                AutoSize = true
            };

            TextBox inputBox = new TextBox()
            {
                Left = 20,
                Top = 50,
                Width = 340,
            };

            Button confirmation = new Button()
            {
                Text = "ОК",
                Left = 280,
                Width = 80,
                Top = 80,
                DialogResult = DialogResult.OK
            };

            //при простой подписке на события, предпочтительно использовать сокращённую запись
            //полная запись:
            EventHandler confirmationHandler = new EventHandler(Confirmation_Click);
            confirmation.Click += confirmationHandler;

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(inputBox);
            prompt.Controls.Add(confirmation);

            prompt.AcceptButton = confirmation; //Enter

            string result = prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : "";

            return result;
        }

        public static void Confirmation_Click(object sender, EventArgs e)
        {
            prompt.Close();
        }
    }
}
