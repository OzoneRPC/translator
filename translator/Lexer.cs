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

    private string[] terms = { "Начало", "Первое", "Второе", "Конец", "Второго", "Третье", "Сочетаемое" };

    public int startPos = 0;
    public int endPos = 0;

    public string currentWord;

    public Lexer(string text) {
      //Получаем текст и преобразовываем его в символьный массив
      this.inputText = text.Trim(' ');
      this.arrayChar = this.inputText.ToCharArray();
    }
    public void nextWord() {
      string word = "";
      if(this.currentPos < this.arrayChar.Length) {
        if(this.currentSymbol() == ' ' || this.currentSymbol() == '\n') {
          this.skipWhitespaces();
        }
        this.startPos = this.currentPos;
        //Текущий символ - буква
        if (this.isLiteral(this.currentSymbol())) {
          while (true) {
            if(this.currentPos == this.arrayChar.Length) {
              this.endPos = this.currentPos;
              if (Array.IndexOf(this.terms, word) != -1) {
                break;
              } else {               
                throw new TException("Не удалось распознать " + word, this.startPos, this.endPos);
              }
            }       
            if (this.currentSymbol() == ' ' || this.currentSymbol() == ',' ||this.currentSymbol() == '\n') {
              this.endPos = this.currentPos;          
              if (Array.IndexOf(this.terms, word) != -1) {
                break;
              } else {              
                throw new TException("Не удалось распознать " + word, this.startPos, this.endPos);
              }          
            }
            word += this.currentSymbol();
            this.currentPos++;
          }
        }else if (this.isOperator(this.currentSymbol())) {//Текущий символ - оператор
          this.endPos = this.currentPos;
          word += this.currentSymbol();
          this.currentPos++;
        }else if (this.isNumericChar(this.currentSymbol())) {
          while(true) {
            if(this.currentPos == this.arrayChar.Length) {
              this.endPos = this.currentPos;
              if (this.isNumericString(word)) {
                break;
              } else {
                throw new TException("Не удалось распознать " + word, this.startPos, this.endPos);
              }
            }
            if(this.currentSymbol() == ' ' || this.currentSymbol() == ',' || this.currentSymbol() == '\n') {
              if (this.isNumericString(word)) {
                this.endPos = this.currentPos - 1;
                break;
              } else {
                throw new TException("Не удалось распознать " + word, this.startPos, this.endPos);
              }
            }

            word += this.currentSymbol();
            this.currentPos++;
          }
        }
      }
      this.currentWord = word;   
    }

    private char currentSymbol() {
      return this.arrayChar[this.currentPos];
    }
    private bool isLiteral(char symbol) {
      Regex regex = new Regex("[a-zA-Zа-яА-Я]");
      return regex.IsMatch(symbol.ToString());
    }
    private bool isOperator(char symbol) {
      Regex regex = new Regex("\\,|\\+|\\-|\\*|\\|\\[|\\]");
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
    private void skipWhitespaces() {
      if(this.currentPos < this.arrayChar.Length) {
        if(this.currentSymbol() == ' ' || this.currentSymbol() == '\n') {
          while (true) {
            if(this.currentSymbol() == ' ' || this.currentSymbol() == '\n') {
              this.currentPos++;
              continue;
            }
            break;
          }
        }
      }
    }
  }
}
