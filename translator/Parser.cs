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
    private const int NESTING_OF_BRACES = 4;
    private int countOfNesting = 0;
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
        int numberCounter = 0;//Нужна, чтобы проверять, есть ли после Первого число или нет.Чтобы не было такого - Первое Второе
        while (true) {
          if (this.lexer.currentWord == "Сочетаемое" || this.lexer.currentWord == "Первое" || this.lexer.currentWord == "Второе" || this.lexer.currentWord == "Третье") {
            if(numberCounter == 1) {
              break;
            } else {
              this.makeException("Ожидалось целое число. Получено: \"" + this.lexer.currentWord + "\"");
            }
          }
          if (this.isInt(this.lexer.currentWord)) {
            this.integerNums.Add(this.strToInt(this.lexer.currentWord));
            this.lexer.nextWord();
            numberCounter = 1;
          }else if(this.lexer.currentWord == "") {
            this.makeException("Ожидалось целое число, либо \"Первое\", либо \"Второе\", либо \"Третье\".");
          } else {
            this.makeException("Доступны только целые числа. Получено: \"" + this.lexer.currentWord + "\"");
          }
        }
      } else if (this.lexer.currentWord == "Второе") {
        this.lexer.nextWord();
        int numberCounter = 0;
        while (true) {
          if (this.isDouble(this.lexer.currentWord)) {
            double currentValue = this.strToDouble(this.lexer.currentWord);
            int startPos = this.lexer.endPos; //Нужна для выделения отсутствующей запятой между двумя числами.
            this.lexer.nextWord();
            if (this.lexer.currentWord == ",") {
              this.doubleNums.Add(currentValue);
              this.lexer.nextWord();
              numberCounter = 1;
            } else if (this.lexer.currentWord == "Конец") {
              this.lexer.nextWord();
              if (this.lexer.currentWord != "второго") {
                this.makeException("Ожидалось \"Конец второго\". Получено: \"" + this.lexer.currentWord + "\"");
              } else {
                this.lexer.nextWord();
                break;
              }
            } else if(this.isDouble(this.lexer.currentWord)) {
              makeException("Пропущена запятая.", startPos, this.lexer.startPos);
            } else {
              this.makeException("Ожидалась запята, либо \"Конец второго\". Получено: \"" + this.lexer.currentWord + "\"");
            }
          } else {
            this.makeException("Доступны только вещественные числа. Получено: \"" + this.lexer.currentWord + "\"");
          }
        }
      } else if (this.lexer.currentWord == "Третье") {
        this.lexer.nextWord();
        while (true) {
          if (this.isVar(this.lexer.currentWord)) {
            string currentVar = this.lexer.currentWord;
            int startPos = this.lexer.endPos; //Нужна для выделения отсутствующей запятой между двумя переменными.
            this.lexer.nextWord();

            if (this.lexer.currentWord == ",") {
              this.varsArray[currentVar] = DEFAULT_VAR_VAlUE;
              this.lexer.nextWord();
              if(this.isVar(this.lexer.currentWord)) {//Переменная ли за запятой?
                continue;
              } else {
                makeException("Ожидалось переменная. Получено: \"" + this.lexer.currentWord + "\"");
              }

            } else if(this.isVar(this.lexer.currentWord)) {
              makeException("Пропущена запятая.", startPos, this.lexer.startPos);
            } else if(this.lexer.currentWord == "Сочетаемое" || this.lexer.currentWord == "Первое" || this.lexer.currentWord == "Второе" || this.lexer.currentWord == "Третье") {
              this.varsArray[currentVar] = DEFAULT_VAR_VAlUE;
              break;
            }
          } else {
            makeException("Доступны только переменные. Получено: \"" + this.lexer.currentWord + "\"");
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
          this.isLogicalNot(this.lexer.currentWord) ) {
   
        if(this.lexer.currentWord == "-") {
          this.lexer.nextWord();
          result = 0 - this.multiplicativeBlock();
        }else if(this.lexer.currentWord == "+") {
          this.lexer.nextWord();
          result = this.multiplicativeBlock();
        }else if (this.isBrace(this.lexer.currentWord)) {
          result = this.multiplicativeBlock();
        }else if (this.isInt(this.lexer.currentWord)) {
          result = this.multiplicativeBlock();
        }else if (this.isLogicalNot(this.lexer.currentWord)) {
          result = this.logicalNotBlock();
        }else if (this.inVarsArray(this.lexer.currentWord)) {
          result = this.multiplicativeBlock();
        } 
        
        if (this.lexer.currentWord == "]" && this.countOfNesting == 0) {
          this.makeException("Лишняя закрывающая скобка");
        }

        while (true) {
          if (this.isAdditiveOperator(this.lexer.currentWord)) {
            string oper = this.lexer.currentWord;
            this.lexer.nextWord();
            if( this.isInt(this.lexer.currentWord)        || 
                this.isBrace(this.lexer.currentWord)      || 
                this.isLogicalNot(this.lexer.currentWord) || 
                this.inVarsArray(this.lexer.currentWord)) {
              if(this.lexer.currentWord == "]") {
                this.makeException("Недопустимо использование знаков операции перед \"]\"");
              } else {
                switch (oper) {
                  case "+":
                    result = result + this.multiplicativeBlock();
                    break;
                  case "-":
                    result = result - this.multiplicativeBlock();
                    break;
                }
              }  
            } else {
              this.makeException("Ожидалось число, либо \"not\", либо переменная");
            }
          } else {
            break;
          }
        }
      } else {
        this.makeException("Ожидалось либо число, либо переменная, либо \"[\", либо \"not\". Получено: \"" + this.lexer.currentWord + "\"");
      }
      return result;
    }
    private int multiplicativeBlock() {
      int result = 0;
      result = this.logicalBlock();
      while (true) {
        if (this.isMultiplicativeOperator(this.lexer.currentWord)) {
          string oper = this.lexer.currentWord; // Сохраняем оператор
          this.lexer.nextWord();//Смещаемся на следующий токен и проверяем его
          if( 
            this.isInt(this.lexer.currentWord)        || 
            this.isBrace(this.lexer.currentWord)      || 
            this.isLogicalNot(this.lexer.currentWord) ||
            this.inVarsArray(this.lexer.currentWord)) {
            if (this.lexer.currentWord == "]") {
              this.makeException("Недопустимо использование знаков операции перед \"]\"");
            } else {
                switch (oper) {
                case "*":
                  result = result * this.logicalBlock();
                  break;
                case "/":
                  int startPos = this.lexer.startPos; // Запоминаем позицию для выделения в случае ошибки
                  int interimResult = this.logicalBlock(); // Промежуточный результат. Проверяем деление на ноль               
                  if (interimResult != 0) {
                    result = result / interimResult;
                  } else {
                    this.makeException("Деление на ноль запрещено", startPos, this.lexer.endPos);
                  }
                  break;
              }
            }
          } else {
            this.makeException("Ожидалось число");
          }
        } else {
          break;
        }
      }
      return result;
    }
    private int logicalBlock() {
      int result = 0;
      result = this.logicalNotBlock();
      //this.lexer.nextWord();
      while (true) {
        if (this.isLogicalOperator(this.lexer.currentWord)) {
          string oper = this.lexer.currentWord;
          this.lexer.nextWord();
          if( this.isInt(this.lexer.currentWord)        ||   
              this.isBrace(this.lexer.currentWord)      || 
              this.isLogicalNot(this.lexer.currentWord) ||
              this.inVarsArray(this.lexer.currentWord)) {
            if (this.lexer.currentWord == "]") {
              this.makeException("Недопустимо использование знаков операции перед \"]\"");
            } else {
              switch (oper) {
                case "or":
                  result = this.boolToInt(this.intToBool(result) || this.intToBool(this.logicalNotBlock()));
                  break;
                case "and":
                  result = this.boolToInt(this.intToBool(result) && this.intToBool(this.logicalNotBlock()));
                  break;
              }
            }
          } else {
            this.makeException("Ожидалось число");
          }
        } else {
          break;
        }
      }
      return result;
    }
    private int logicalNotBlock() {
      int result = 0;
      //this.lexer.nextWord();
      if(this.isLogicalNot(this.lexer.currentWord)) {
        this.lexer.nextWord();
        if (this.lexer.currentWord == "]") {
          this.makeException("Недопустимо использование знаков операции перед \"]\"");
        } else {
          bool typeBlockValue = !this.intToBool(this.typeBlock());
          result = this.boolToInt(typeBlockValue);
        }
      } else {
        result = this.typeBlock();
      }
      return result;
    }
    private int typeBlock() {
      int result = 0;
      if (this.inVarsArray(this.lexer.currentWord)) {
        result = this.varsArray[this.lexer.currentWord];
        this.lexer.nextWord();
        return result;
      } else if(this.isInt(this.lexer.currentWord)){
        int numberEndPos = this.lexer.endPos;
        result = this.strToInt(this.lexer.currentWord);
        this.lexer.nextWord();
        if(this.isInt(this.lexer.currentWord) || this.inVarsArray(this.lexer.currentWord)) {
          this.makeException("Пропущен знак операции.", numberEndPos, this.lexer.startPos);
        } else if (this.lexer.currentWord != "" && !this.isOperator(this.lexer.currentWord)) {
          this.makeException("Ожидался знак операции, получено: \"" + this.lexer.currentWord + "\"");
        }
        return result;
      } else if(this.lexer.currentWord == "[") {
        int braceStartPos = this.lexer.startPos + 1;
        this.countOfNesting = this.countOfNesting + 1;
        if(this.countOfNesting < NESTING_OF_BRACES) {
          this.lexer.nextWord();
          if(this.lexer.currentWord == "]") {
            this.makeException("Ожидалась правая часть", braceStartPos, this.lexer.endPos-1);
          }
          result = this.rightPart();
          if(this.lexer.currentWord == "]") {
            this.countOfNesting = this.countOfNesting - 1;
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

    private bool inVarsArray(string str) {
      return this.varsArray.ContainsKey(str);
    }

    private bool isVar(string str) {
      Regex regex = new Regex("^[a-zA-Zа-яА-Я]{1,}([0-9]*|[a-zA-Zа-яА-Я]*)*$");//Терминалы проходят через эту регулярку.
      return (regex.IsMatch(str) && !this.lexer.isTerminal(this.lexer.currentWord));
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
    private bool isMultiplicativeOperator(string str) {
      return (str == "*" || str == "/");
    }
    private bool isLogicalOperator(string str) {
      return (str == "or" || str == "and");
    }
    private bool isLogicalNot(string str) {
      return str == "not";
    }
    private bool isBrace(string str) {
      return (str == "[" || str == "]");
    }
    private bool isOperator(string str) {
      Regex regex = new Regex("\\,|\\+|\\-|\\*|\\/|\\[|\\]|\\:|and|or");
      return regex.IsMatch(str);
    }
  }
}
