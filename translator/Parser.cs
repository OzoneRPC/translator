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
    private const int NASTING_OF_BRACES = 3;
    private int countOfNasting = 0;

    public string result = "";
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
        this.result = currentVar + " = " + this.varsArray[currentVar];
      } else {
        this.makeException("Ожидалось \"=\"");
      }
    }
    private int rightPart() {
      int result = 0;
      if( this.isInt(this.lexer.currentWord)              || 
          this.inVarsArray(this.lexer.currentWord)        || 
          this.isAdditiveOperator(this.lexer.currentWord) ||
          this.isBrace(this.lexer.currentWord)            ||
          this.lexer.currentWord == "not" ) {
   
        if(this.lexer.currentWord == "-") {
          this.lexer.nextWord();
          result = 0 - this.multiplicativeBlock();
        }else if(this.lexer.currentWord == "+") {
          this.lexer.nextWord();
          result = this.multiplicativeBlock();
        }else if (this.isBrace(this.lexer.currentWord)) {
          result = this.typeBlock();
        }else if (this.isInt(this.lexer.currentWord)) {
          result = this.multiplicativeBlock();
        }else if (this.lexer.currentWord == "not") {
          result = this.logicalNotBlock();
        }
        //this.lexer.nextWord();
        while (true) {
          if(this.lexer.currentWord != "+" && this.lexer.currentWord != "-") {
            break;
          }else if(this.lexer.currentWord == "+") {
            this.lexer.nextWord();
            if (this.isInt(this.lexer.currentWord) || isBrace(this.lexer.currentWord)) {
              result = result + this.multiplicativeBlock();
            } else {
              this.makeException("Ожидалось число");
            }
            
          }else if(this.lexer.currentWord == "-") {
            this.lexer.nextWord();
            if (this.isInt(this.lexer.currentWord) || isBrace(this.lexer.currentWord)) {
              result = result - this.multiplicativeBlock();
            } else {
              this.makeException("Ожидалось число");
            }
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
          if (this.isInt(this.lexer.currentWord) || isBrace(this.lexer.currentWord)) {
            result = this.boolToInt(this.intToBool(result) || this.intToBool(this.logicalNotBlock()));
          } else {
            this.makeException("Ожидалось число");
          }
        } else if((this.lexer.currentWord == "and")){
          this.lexer.nextWord();
          if (this.isInt(this.lexer.currentWord) || isBrace(this.lexer.currentWord)) {
            result = this.boolToInt(this.intToBool(result) && this.intToBool(this.logicalNotBlock()));
          } else {
            this.makeException("Ожидалось число");
          }
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
      int result = 0;
      if (this.inVarsArray(this.lexer.currentWord)) {
        return this.varsArray[this.lexer.currentWord];
      } else if(this.isInt(this.lexer.currentWord)){
        result = this.strToInt(this.lexer.currentWord);
        this.lexer.nextWord();
        return result;
      } else if(this.lexer.currentWord == "[") {
        int braceStartPos = this.lexer.startPos + 1;
        this.countOfNasting = this.countOfNasting + 1;
        if(this.countOfNasting < NASTING_OF_BRACES) {
          this.lexer.nextWord();
          if(this.lexer.currentWord == "]") {
            this.makeException("Ожидалась правая часть", braceStartPos, this.lexer.endPos-1);
          }
          result = this.rightPart();
          if(this.lexer.currentWord == "]") {
            this.countOfNasting = this.countOfNasting - 1;
          } else {
            this.makeException("Ожидалась скобка");
          }
          this.lexer.nextWord();
          return result;
        } else {
          this.lexer.nextWord();
          this.makeException("Вложеность скобок должна быть не больше 3");
        }
      }
      return 0;
    }
    private void makeException(string message, int startPos = 0, int endPos = 0) {
      if(startPos == 0 && endPos == 0) {
        throw new TException(message, this.lexer.startPos, this.lexer.endPos);
      } else {
        throw new TException(message, startPos, endPos);
      }

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
    private bool isBrace(string str) {
      return (str == "[" || str == "]");
    }
  }
}
