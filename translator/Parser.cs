using System;
using System.Collections;
using System.Collections.Generic;
using AssocArray = System.Collections.Generic.Dictionary<string, int>;
using AssocElem = System.Collections.Generic.KeyValuePair<string, int>;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace translator {
  class Parser {
    private Lexer lexer;
    private ArrayList integerNums = new ArrayList();
    private ArrayList doubleNums = new ArrayList();
    private ArrayList zvenoNums = new ArrayList();
    private AssocArray varsArray = new AssocArray();
    private const int DEFAULT_VAR_VAlUE = 0;

    public Parser(string text) {
      this.lexer = new Lexer(text);
    }

    public void language() {
      this.lexer.nextWord();
      if (lexer.currentWord != "Начало") {
        this.makeException("Ожидалось \"Начало\"");
      } else {
        this.lexer.nextWord();
        if (this.lexer.currentWord == "Первое" || this.lexer.currentWord == "Второе" || this.lexer.currentWord == "Третье") {
          while (this.lexer.currentWord == "Первое" || this.lexer.currentWord == "Второе" || this.lexer.currentWord == "Третье") {
            this.multiplicity();
          }
        } else {
          this.makeException("Ожидалось либо \"Первое\", либо \"Второе\", либо \"Третье\"");
        }
        if (this.lexer.currentWord == "Сочетаемое") {
          this.zveno();
        } else {
          this.makeException("Ожидалось \"Сочетаемое\"");
        }      
        this.oper();
        this.printArrays();
      }
    }

    private void multiplicity() {
      if (this.lexer.currentWord == "Первое") {
        this.lexer.nextWord();
        while (true) {
          if (this.lexer.currentWord == "Второе" || this.lexer.currentWord == "Третье" || this.lexer.currentWord == "") {
            break;
          }
          this.integerNums.Add(this.strToInt(this.lexer.currentWord));
          this.lexer.nextWord();
        }
      } else if (this.lexer.currentWord == "Второе") {
        this.lexer.nextWord();
        while (true) {
          if (this.isDouble(this.lexer.currentWord)) {
            this.doubleNums.Add(this.strToDouble(this.lexer.currentWord));
            this.lexer.nextWord();
            if(this.lexer.currentWord == ",") {
              this.lexer.nextWord();
            }else if(this.lexer.currentWord == "Конец") {
              this.lexer.nextWord();
              if(this.lexer.currentWord != "второго") {
                this.makeException("Ожидалось \"Конец второго\". Получено: \"" + this.lexer.currentWord + "\"");
              } else {
                this.lexer.nextWord();
                break;
              }
            } else {
              this.makeException("Ожидалась запята, либо \"Конец второго\". Получено: \"" + this.lexer.currentWord + "\"");
            } 
          } else {
            this.makeException("Ожидалось дробное число. Получено: \"" + this.lexer.currentWord + "\"");
          }
        }
      } else if (this.lexer.currentWord == "Третье") {
        this.lexer.nextWord();
        while (true) {
          if (this.isVar(this.lexer.currentWord)) {
            this.varsArray[this.lexer.currentWord] = DEFAULT_VAR_VAlUE;
            this.lexer.nextWord();
            if (this.lexer.currentWord == ",") {
              this.lexer.nextWord();
              continue;
            } else {
              break;
            }
          } else {
            this.makeException("Ожидалась переменная");
          }
        }
      }

    }
    private void zveno() {
      this.lexer.nextWord();
      int k = 0;
      while (true) {
        if (this.isInt(this.lexer.currentWord)) {
          this.zvenoNums.Add(this.lexer.currentWord);
          this.lexer.nextWord();
          if (this.lexer.currentWord == ":" && k == 0) {
            makeException("Ожидалось целое число, получена метка");
          }else if(this.lexer.currentWord == ":" && k != 0) {
            break;
          }else if (this.inVarsArray(this.lexer.currentWord)) {
            break;
          }
        } else {
          makeException("Ожидалось целое число, либо метка, либо переменная. Получено: \"" + this.lexer.currentWord + "\"");
        }
        k = 1;
      }

    }
    private void oper() {
      string currentVar = this.lexer.currentWord;
      this.lexer.nextWord();
      if (this.lexer.currentWord == "=") {
        this.lexer.nextWord();
        this.varsArray[currentVar] = this.rightPart();
      } else {
        this.makeException("Ожидалось \"=\"");
      }
    }
    private int rightPart() {
      int result = 0;
      if(this.isInt(this.lexer.currentWord) || this.inVarsArray(this.lexer.currentWord) || this.isAdditiveOperator(this.lexer.currentWord)) {
        if(this.lexer.currentWord == "-") {
          this.lexer.nextWord();
          result = 0 - this.typeBlock();
        } else if (this.isInt(this.lexer.currentWord)) {
          result = this.multiplicativeBlock();
        }
        //this.lexer.nextWord();
        while (true) {
          if(this.lexer.currentWord != "+" && this.lexer.currentWord != "-") {
            break;
          }else if(this.lexer.currentWord == "+") {
            this.lexer.nextWord();
            result = result + this.multiplicativeBlock();
          }else if(this.lexer.currentWord == "-") {
            this.lexer.nextWord();
            result = result - this.multiplicativeBlock();
          }
        }
      }
      return result;
    }
    private int multiplicativeBlock() {
      int result = 0;
      result = this.logicalBlock();
      //this.lexer.nextWord();//Переходим от числа к оператору
      while (true) {
        if (this.lexer.currentWord != "/" && this.lexer.currentWord != "*") {
          break;
        }
        if (this.lexer.currentWord == "*") {
          this.lexer.nextWord();
          result = result * this.logicalBlock();
        }else if(this.lexer.currentWord == "/") {
          this.lexer.nextWord();
          result = result / this.logicalBlock();
        }
      }
      return result;
    }
    private int logicalBlock() {
      int result = 0;
      result = this.logicalNotBlock();
      //this.lexer.nextWord();
      while (true) {
        if(this.lexer.currentWord != "or" && this.lexer.currentWord != "and") {
          break;
        }else if(this.lexer.currentWord == "or") {
          this.lexer.nextWord();
          result = this.boolToInt(this.intToBool(result) || this.intToBool(this.logicalNotBlock()));
        }else if((this.lexer.currentWord == "and")){
          this.lexer.nextWord();
          result = this.boolToInt(this.intToBool(result) && this.intToBool(this.logicalNotBlock()));
        }
      }
      return result;
    }
    private int logicalNotBlock() {
      int result = 0;
      //this.lexer.nextWord();
      if(this.lexer.currentWord == "not") {
        this.lexer.nextWord();
        bool typeBlockValue = !this.intToBool(this.typeBlock());
        result = this.boolToInt(typeBlockValue);
      } else {
        result = this.typeBlock();
      }
      return result;
    }
    private int typeBlock() {
      if (this.inVarsArray(this.lexer.currentWord)) {
        return this.varsArray[this.lexer.currentWord];
      } else if(this.isInt(this.lexer.currentWord)){
        int result = this.strToInt(this.lexer.currentWord);
        this.lexer.nextWord();
        return result;
      }
      return 0;
    }
    private void makeException(string message) {
      throw new TException(message, this.lexer.startPos, this.lexer.endPos);
    }

    private int strToInt(string str) {      
      return Convert.ToInt32(str);
    }
    private double strToDouble(string str) {
      string newStr = str.Replace('.', ',');
      return Convert.ToDouble(newStr);
    }
    private bool intToBool(int value) {
      return Convert.ToBoolean(value);
    }
    private int boolToInt(bool value) {
      return Convert.ToInt32(value);
    }

    private bool isVar(string str) {
      Regex regex = new Regex("^[a-zA-Zа-яА-Я]{1,}([0-9]*|[a-zA-Zа-яА-Я]*)*$");
      return regex.IsMatch(str);
    }
    private bool inVarsArray(string str) {
      return this.varsArray.ContainsKey(str);
    }
    private bool isInt(string str) {
      Regex regex = new Regex("^[0-9]+$");
      return regex.IsMatch(str);
    }
    private bool isDouble(string str) {
      Regex regex = new Regex("^[0-9]+.[0-9]+$");
      return regex.IsMatch(str);
    }
    private bool isAdditiveOperator(string str) {
      return (str == "+" || str == "-");
    }


    private void printArrays() {
      Console.WriteLine("Int:");
      foreach(int elem in this.integerNums) {
        Console.WriteLine(elem);
      }
      Console.WriteLine("Double:");
      foreach (double elem in this.doubleNums) {
        Console.WriteLine(elem);
      }
      Console.WriteLine("Vars:");
      foreach(AssocElem elem in this.varsArray) {
        if(elem.Value !=  0) {
          Console.WriteLine(elem.Key.ToString() + " " + elem.Value);
        }      
      }
      
    }

    public string getResult() {
      string result = "";
      foreach (AssocElem elem in this.varsArray) {
        if (elem.Value != 0) {
          result += elem.Key.ToString() + " = " + elem.Value + "\n";
        }
      }
      return result;
    }
  }
}
