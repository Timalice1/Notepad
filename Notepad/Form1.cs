using System;
using System.Windows.Forms;
using System.IO;

namespace Notepad {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            menuSave.Enabled = false;
            menuSaveAs.Enabled = false;
        }

        private int count = 0;


        private RichTextBox initializeTextBox(TabPage parent) {
            var text = new RichTextBox();
            text.Parent = parent;
            text.Dock = DockStyle.Fill;
            text.BorderStyle = BorderStyle.None;
            text.SelectionFont = new System.Drawing.Font("Calibri", 14);
            text.AcceptsTab = true;
            return text;
        }

        private void menuCreateNew_Click(object sender, EventArgs e) {
            var tab = new TabPage($"tabPage{++count}");
            tab.BorderStyle = BorderStyle.FixedSingle;
            var textBox = initializeTextBox(tab);
            tabControl1.TabPages.Add(tab);
            menuSaveAs.Enabled = true;
        }

        private void menuClose_Click(object sender, EventArgs e) {
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            count--;
        }

        private void menuOpen_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog() {
                Filter = "| *.txt; *.cs; *.java; *.json; *.c; *.cpp; *.html; *.css; *.xml"
            };

            var res = dialog.ShowDialog();

            if (res != DialogResult.OK)
                return;

            string filePath = dialog.FileName;

            var tab = new TabPage(filePath);
            tabControl1.TabPages.Add(tab);
            tab.BorderStyle = BorderStyle.FixedSingle;
            var textBox = initializeTextBox(tab);

            StreamReader sr = new StreamReader(filePath);
            textBox.Text = sr.ReadToEnd();

            textBox.SelectionFont = new System.Drawing.Font("Calibri", 14);
            menuSave.Enabled = true;
            menuSaveAs.Enabled = true;

            sr.Close();

            count++;
        }

        private void Save(string filePath) {
            StreamWriter sw = new StreamWriter(filePath);
            var controls = tabControl1.SelectedTab.Controls;

            sw.WriteLine(controls[0].Text);
            sw.Close();
        }

        private void menuSave_Click(object sender, EventArgs e) {
            Save(tabControl1.SelectedTab.Text);
        }

        private void menuSaveAs_Click(object sender, EventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();
            var res = sfd.ShowDialog();
            if (res == DialogResult.OK) {
                Save(sfd.FileName);
                tabControl1.SelectedTab.Text = sfd.FileName;
            }
            menuSave.Enabled = true;

        }
    }
}
