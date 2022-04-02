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
            selectAllToolStripMenuItem.Enabled = false;
            cutToolStripMenuItem.Enabled = false;
            copyToolStripMenuItem.Enabled = false;
            pasteToolStripMenuItem.Enabled = false;
            findToolStripMenuItem.Enabled = false;
        }

        private List<TextField> textFields = new List<TextField>();

        private void menuCreateNew_Click(object sender, EventArgs e) {
            var tab = new TabPage("tabPage ");
            tab.Font = new System.Drawing.Font("Calibri", 10);
            tabControl1.SelectedTab = tab;
            tab.BorderStyle = BorderStyle.FixedSingle;
            var textBox = new TextField(tab);
            tabControl1.TabPages.Add(tab);

            menuSaveAs.Enabled = true;
            selectAllToolStripMenuItem.Enabled = true;
            cutToolStripMenuItem.Enabled = true;
            copyToolStripMenuItem.Enabled = true;
            pasteToolStripMenuItem.Enabled = true;
            findToolStripMenuItem.Enabled = true;

            textBox.IsSaved = false;
            textBox.TextChanged += new EventHandler(textBox_TextChanged);

            textFields.Add(textBox);
        }

        private void menuClose_Click(object sender, EventArgs e) {
            if (!textFields[tabControl1.SelectedIndex].IsSaved)
               
                if (MessageBox.Show($"{tabControl1.SelectedTab.Text.Remove(tabControl1.SelectedTab.Text.LastIndexOf(" "))} " +
                    "has unsaved changes.\nSave it?",
                    "Close",
                    MessageBoxButtons.OKCancel) == DialogResult.OK) {
                    menuSaveAs_Click(sender, e);
                }
                else return;
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

            var tab = new TabPage(filePath + " ");
            tab.Font = new System.Drawing.Font("Calibri", 10);


            tabControl1.SelectedTab = tab;
            tabControl1.TabPages.Add(tab);
            tab.BorderStyle = BorderStyle.FixedSingle;

            StreamReader sr = new StreamReader(filePath);

            var textBox = new TextField(tab);
            textBox.TextChanged += new EventHandler(textBox_TextChanged);

            textBox.Text = sr.ReadToEnd();

            menuSave.Enabled = true;
            menuSaveAs.Enabled = true;
            selectAllToolStripMenuItem.Enabled = true;
            cutToolStripMenuItem.Enabled = true;
            copyToolStripMenuItem.Enabled = true;
            pasteToolStripMenuItem.Enabled = true;
            findToolStripMenuItem.Enabled = true;

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
            var path = tabControl1.SelectedTab.Text.Remove(tabControl1.SelectedTab.Text.LastIndexOf(" "));
            Save(path);
            tabControl1.SelectedTab.Text = path;
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
        
        private void textBox_TextChanged(object sender, EventArgs e){
            textFields[tabControl1.SelectedIndex].IsSaved = false;
            if (textFields[tabControl1.SelectedIndex].Parent.Text.Contains("*"))
                return;

            textFields[tabControl1.SelectedIndex].Parent.Text += " *";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            foreach(var field in textFields) {
                if (!field.IsSaved) {
                    if (MessageBox.Show($"{field.Parent.Text.Remove(field.Parent.Text.LastIndexOf(" "))} has unsaved changes.\nSave it?",
                    "Close",
                    MessageBoxButtons.OKCancel) == DialogResult.OK) {
                        menuSaveAs_Click(sender, e);
                        tabControl1.TabPages.Remove((TabPage)field.Parent);
                    }
                    else {
                        tabControl1.TabPages.Remove((TabPage)field.Parent);
                        continue;
                    }
                }
            }
        }

        private string buffer = "";

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            textFields[tabControl1.SelectedIndex].SelectAll();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
            buffer = textFields[tabControl1.SelectedIndex].SelectedText;
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e) {
            textFields[tabControl1.SelectedIndex].SelectedText = buffer;
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e) {
            buffer = textFields[tabControl1.SelectedIndex].SelectedText;
            textFields[tabControl1.SelectedIndex].SelectedText = "";
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e) {
            var find = new Find();
            find.ShowDialog();
            string findingText = find.FindText;
            if(findingText != null) {
                if (textFields[tabControl1.SelectedIndex].Text.Contains(findingText)) {
                    int start = textFields[tabControl1.SelectedIndex].Text.IndexOf(findingText);
                    textFields[tabControl1.SelectedIndex].Select(start, findingText.Length);
                }
                else 
                    MessageBox.Show("Word don't found...");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            new About().ShowDialog();
        }
    }
}
