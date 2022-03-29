using System;
using System.Windows.Forms;


namespace Notepad {
    internal class TextField : RichTextBox{

        public bool IsSaved { get; set; }

        public TextField(TabPage parent) {
            this.Parent = parent;
            this.Dock = DockStyle.Fill;
            this.BorderStyle = BorderStyle.None;
            this.SelectionFont = new System.Drawing.Font("Calibri", 14);
            this.AcceptsTab = true;
        }
    }
}
