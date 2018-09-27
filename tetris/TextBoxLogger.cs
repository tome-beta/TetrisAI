using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tetris
{
    class TextBoxLogger
    {
        private static TextBoxLogger _singleInstance = new TextBoxLogger();

        public static TextBoxLogger GetInstance()
        {
            return _singleInstance;
        }

        public void SetTextBox(TextBox textBox)
        {
            myConsole = textBox;
        }

        public void log(string line)
        {
            if (line != string.Empty)
            {
                if (myConsole != null)
                {
                    myConsole.AppendText(line + "\r\n");
                    myConsole.Invalidate();
                    System.Threading.Thread.Sleep(1);
                }
            }
        }
        public void clear()
        {
            myConsole.Clear();
            myConsole.Invalidate();
        }

        private static TextBox myConsole;
    }
}
