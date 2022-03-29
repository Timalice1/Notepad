using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace Notepad {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            menuSave.Enabled = false;
            menuSaveAs.Enabled = false;
        }

        private List<TextField> textFields = new List<TextField>();

        private void menuCreateNew_Click(object sender, EventArgs e) {
            var tab = new TabPage("tabPage");
            tabControl1.SelectedTab = tab;
            tab.BorderStyle = BorderStyle.FixedSingle;
            var textBox = new TextField(tab);
            tabControl1.TabPages.Add(tab);
            menuSaveAs.Enabled = true;
            textBox.IsSaved = false;

            textFields.Add(textBox);
        }

        private void menuClose_Click(object sender, EventArgs e) {
            textFields.RemoveAt(tabControl1.SelectedIndex);
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
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
            tabControl1.SelectedTab = tab;
            tabControl1.TabPages.Add(tab);
            tab.BorderStyle = BorderStyle.FixedSingle;
            var textBox = new TextField(tab);

            StreamReader sr = new StreamReader(filePath);
            textBox.Text = sr.ReadToEnd();

            menuSave.Enabled = true;
            menuSaveAs.Enabled = true;

            sr.Close();

            textBox.IsSaved = false;
            textFields.Add(textBox); 

        }

        private void Save(string filePath) {
            StreamWriter sw = new StreamWriter(filePath);
            var textField = textFields[tabControl1.SelectedIndex];

            textField.IsSaved = true;

            sw.WriteLine(textField.Text);
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
