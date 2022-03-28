using System;
using System.Windows.Forms;


namespace Notepad {
    internal class TextField : RichTextBox{
        private RichTextBox textBox;

        public bool IsSaved { get; set; }

        public TextField(TabPage parent) {
            textBox = new RichTextBox();
            textBox.Parent = parent;
            textBox.Dock = DockStyle.Fill;
            textBox.BorderStyle = BorderStyle.None;
            textBox.SelectionFont = new System.Drawing.Font("Calibri", 14);
            textBox.AcceptsTab = true;
        }
    }
}
