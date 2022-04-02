using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad {
    public partial class Find : Form {
        public Find() {
            InitializeComponent();
        }

        public string FindText { get; private set; }

        private void button1_Click(object sender, EventArgs e) {
            FindText = textBox1.Text;
            this.Close();
        }

        
    }
}
