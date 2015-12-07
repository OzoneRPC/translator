using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace translator {
  public partial class Translator : Form {
    private Parser parser;

    public Translator() {
      InitializeComponent();
      this.textbox.Text = "Начало\nПервое 1 2 3\nВторое 1.2, 1.3 Конец второго\nТретье пер1, пер2, per\nСочетаемое 5 4 3\nпер1 = ";
    }

    private void button1_Click(object sender, EventArgs e) {

      this.parser = new Parser(this.textbox.Text);
      try {
        this.parser.language();
        this.printMessage("OK");
      }
      catch (TException exc) {
        this.printMessage("Exception: " + exc.Message);        
        this.textbox.Focus();
        this.textbox.SelectionStart = exc.startPositon;
        this.textbox.SelectionLength = exc.endPosition - exc.startPositon;
      }
      
    }

    private void richTextBox1_TextChanged(object sender, EventArgs e) {
    }

    private void Form1_Load(object sender, EventArgs e) {
      
    }
    public void printMessage(string text) {
      this.outputLabel.Text = text;
      
    }
  }
}
