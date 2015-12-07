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
      this.inputText = text.Trim(' ');
      this.arrayChar = this.inputText.ToCharArray();
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
          }
        }
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
          if (Array.IndexOf(this.terms, this.word) != -1 || this.isVar(this.word)) {
            break;
          } else {
            throw new TException("Не удалось распознать " + this.word, this.startPos, this.endPos);
          }
        }
        if (this.currentSymbol() == ' ' || this.currentSymbol() == ',' || this.currentSymbol() == '\n') {
          this.endPos = this.currentPos;
          if (Array.IndexOf(this.terms, this.word) != -1 || this.isVar(this.word)) {
            break;
          } else {
            throw new TException("Не удалось распознать " + this.word, this.startPos, this.endPos);
          }
        }
        this.word += this.currentSymbol();
        this.currentPos++;
      }
    }
    private void processOperator() {
      this.endPos = this.currentPos;
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
        if (this.currentSymbol() == ' ' || this.currentSymbol() == ',' || this.currentSymbol() == '\n' || this.currentSymbol() == ':') {
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

    private bool isLiteral(char symbol) {
      Regex regex = new Regex("[a-zA-Zа-яА-Я]");
      return regex.IsMatch(symbol.ToString());
    }
    private bool isOperator(char symbol) {
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
    private bool isVar(string str) {
      Regex regex = new Regex("^[a-zA-Zа-яА-Я]{1,}([0-9]*|[a-zA-Zа-яА-Я]*)*$");
      return regex.IsMatch(str);
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
    } 
  }
}
