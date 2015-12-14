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
      this.bnf.Text = "Язык = \"Начало\"\nМножество...Множество Звено Опер\nМножество = \"Первое\" Цел...Цел !\"Второе\" Вещ\",\"...Вещ \"Конец второго\"! \"Третье\" Перем\",\"...Перем\nЗвено = \"Сочетаемое\" Цел...Цел\nОпер = </Метка\":\"/>Перем \"=\"Прав.часть\nМетка = Цел\nПрав.часть = Адд.блок Знач1...Адд.блок\nЗнач1 = \"+\" ! \"-\"\nАдд.блок = Мульт.блок Знач2 Мульт.блок\nЗнач2 = \"*\" ! \"/\"\nЛог.блок = ЛогНе.блок Знач3 ЛогНе.блок\nЗнач3 = \"or\" ! \"and\"\nЛогНе.блок = </not/>Тип.блок\nТип.блок = Перем ! Цел ! \"[\" Прав.часть \"]\"K < 4(глубина вложенности)\nПерем = Буква</Символ...Символ/>\nСимвол = Буква ! Цифра\nБуква = \"A\" ! \"B\" ! \"C\" ! ... ! \"Z\" ! \"А\" ! \"Б\" ! \"В\" !...! \"Я\"\nЦифра = \"0\" ! \"1\" ! \"2\" ! ... ! \"9\"\nЦел = Цифра...Цифра\nВещ = Цел\".\"Цел";
      this.textbox.Text = "Начало\nПервое 1 2 3\nВторое 1.2, 1.3 Конец второго\nТретье пер1, пер2, per\nСочетаемое 5 4 3\nпер1 = (1-)";
    }

    private void button1_Click(object sender, EventArgs e) {
        
      this.parser = new Parser(this.textbox.Text);
      try {
        this.parser.language();
        this.printMessage(this.parser.result);
      }
      catch (TException exc) {
        this.printMessage("Ошибка: " + exc.Message);        
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

    private void groupBox1_Enter(object sender, EventArgs e) {

    }
  }
}
