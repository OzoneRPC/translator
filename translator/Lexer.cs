using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections;
using System.Windows.Forms;



namespace translator {
  class Lexer {
    private string inputText = "";
    private char[] arrayChar;
    private int currentPos = 0;
    private string[] terms = { "Начало", "Первое", "Второе", "Конец", "второго", "Третье", "Сочетаемое" };
    private string word = "";

    public int startPos = 0;
    public int endPos = 0;

    public string currentWord;

    public Lexer(string text) {
      //Получаем текст и преобразовываем его в символьный массив
      //this.inputText = text.Trim(' ');
      this.arrayChar = text.ToCharArray();
    }
    public void nextWord() {
      this.word = "";
      this.startPos = this.currentPos;
      if (this.currentPos < this.arrayChar.Length) {
        if (this.currentSymbol() == ' ' || this.currentSymbol() == '\n') {
          this.skipWhitespaces();
        }
        if (this.currentPos < this.arrayChar.Length) {
          if (this.isLiteral(this.currentSymbol())) { //Текущий символ - буква
            this.processLiteral();
          } else if (this.isOperator(this.currentSymbol())) {//Текущий символ - оператор
            this.processOperator();
          } else if (this.isNumericChar(this.currentSymbol())) {//Текущий символ - число
            this.processNumber();
          } else {
            this.endPos = this.currentPos+1;
            throw new TException("Не удалось распознать \"" + this.currentSymbol()+"\"", this.startPos, this.endPos);
          }
        } else {
          this.endPos = this.currentPos;
        }
      } else {
        this.endPos = this.currentPos;
      }
      this.currentWord = this.word;   
    }

    private char currentSymbol() {
      return this.arrayChar[this.currentPos];
    }

    private void processLiteral() {
      while (true) {
        if (this.currentPos == this.arrayChar.Length) {
          this.endPos = this.currentPos;
          if (this.isTerminal(this.word) || this.isVar(this.word) || this.isLogicalOperator(this.word)) {
            break;
          } else {
            throw new TException("Не удалось распознать " + this.word, this.startPos, this.endPos);
          }
        }
        if (this.currentSymbol() == ' ' || this.currentSymbol() == ',' || this.currentSymbol() == '\n') {
          this.endPos = this.currentPos;
          if (this.isTerminal(this.word) || this.isVar(this.word) || this.isLogicalOperator(this.word)) {
            break;
          } else {
            throw new TException("Не удалось распознать " + this.word, this.startPos, this.endPos);
          }
        }
        this.word += this.currentSymbol();
        if (this.isLogicalOperator(this.word)) {//Может быть неправильно
          this.currentPos++;// Допустим, получилось слово and, текущая позиция указывает на букву "d", поэтому смещаем указатель, чтобы нормально выделить
          this.endPos = this.currentPos;
          break;
        }
        this.currentPos++;
      }
    }
    private void processOperator() {
      this.endPos = this.currentPos+1;
      this.word += this.currentSymbol();
      this.currentPos++;
    }
    private void processNumber() {
      while (true) {
        if (this.currentPos == this.arrayChar.Length) {
          this.endPos = this.currentPos;
          if (this.isNumericString(this.word)) {
            break;
          } else {
            throw new TException("Не удалось распознать " + this.word, this.startPos, this.endPos);
          }
        }
        if ( this.isWhiteSpace(this.currentSymbol()) || this.isLiteral(this.currentSymbol()) || this.isOperator(this.currentSymbol()) ) {
          if (this.isNumericString(this.word)) {
            this.endPos = this.currentPos;
            break;
          } else {
            throw new TException("Не удалось распознать " + this.word, this.startPos, this.endPos);
          }
        }

        this.word += this.currentSymbol();
        this.currentPos++;
      }
    }

    public bool isTerminal(string str) {
      if (Array.IndexOf(this.terms, str) == -1) {
        return false;
      } else {
        return true;
      }
    }

    private bool isWhiteSpace(char symbol) {
      return (symbol == ' ' || symbol == '\n');
    }
    private bool isLiteral(char symbol) {
      Regex regex = new Regex("[a-zA-Zа-яА-Я]");
      return regex.IsMatch(symbol.ToString());
    }
    private bool isOperator(char symbol) {
      if (symbol == '(' || symbol == ')') {
        throw new TException("Недопустимо использование круглых скобок", this.startPos, this.currentPos + 1);
      } else if (symbol == '{' || symbol == '}') {
        throw new TException("Недопустимо использование фигурных скобок", this.startPos, this.currentPos + 1);
      }
      Regex regex = new Regex("\\,|\\+|\\-|\\*|\\/|\\[|\\]|\\:|=");
      return regex.IsMatch(symbol.ToString());
    }
    private bool isNumericChar(char symbol) {
      Regex regex = new Regex("[0-9]");
      return regex.IsMatch(symbol.ToString());
    }
    private bool isNumericString(string str) {
      Regex regex = new Regex("^\\d{1,}(.\\d{1,})?$");
      return regex.IsMatch(str);
    }
    private bool isLogicalOperator(string str) {
      return (str == "or" || str == "and" || str == "not");
    }
    private bool isVar(string str) {
      Regex regex = new Regex("^[a-zA-Zа-яА-Я]{1,}([0-9]*|[a-zA-Zа-яА-Я]*)*$");
      return regex.IsMatch(str);
    }
    private bool isBrace(char symbol) {
      return (symbol == '[' || symbol == ']');
    }
    private void skipWhitespaces() {
      if(this.currentSymbol() == ' ' || this.currentSymbol() == '\n') {
        while (true) {
          if(this.currentPos < this.arrayChar.Length) {
            if (this.currentSymbol() == ' ' || this.currentSymbol() == '\n') {
              this.currentPos++;
              continue;
            } else {
              break;
            }
          } else {
            break;
          }              
        }
      }
      this.startPos = this.currentPos;
    } 

  }
}
