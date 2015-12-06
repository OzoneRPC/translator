using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace translator {
  class Parser {
    private Lexer lexer;
    private ArrayList integerNums;
    private ArrayList doubleNums;
    private ArrayList vars;

    public Parser(string text) {
      this.lexer = new Lexer(text);
    }
    public void language() {
      this.lexer.nextWord();
      if(lexer.currentWord != "Начало") {
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
        this.zveno();
        this.oper();
      }
    }
    private void multiplicity() {

      if (this.lexer.currentWord == "Первое") {
        while (true) {
          if (this.lexer.currentWord == "Второе" || this.lexer.currentWord == "Третье" || this.lexer.currentWord == "") {
            break;
          }
          this.integerNums.Add(this.strToInt(this.lexer.currentWord));
          this.lexer.nextWord();
        }
      } else if (this.lexer.currentWord == "Второе") {
        while (true) {
          this.doubleNums.Add(this.strToDouble(this.lexer.currentWord));
          this.lexer.nextWord();
          if (this.lexer.currentWord == "Конец") {
            this.lexer.nextWord();
            if (this.lexer.currentWord != "второго") {
              this.makeException("Ожидалось \"Конец второго\"");
            } else if (this.lexer.currentWord == "второго") {
              break;
            }
          } else if (this.lexer.currentWord == ",") {
            this.lexer.nextWord();
            this.makeException("Пропущена запятаяыыы");
          } else {
            this.makeException("Пропущена запятая");
          }
        }

      } else if (this.lexer.currentWord == "Третье") {

      }

    }
    private void zveno() { }
    private void oper() { }

    private void makeException(string message) {
      throw new TException(message, this.lexer.startPos, this.lexer.endPos);
    }

    private int strToInt(string str) {      
      return Convert.ToInt32(str);
    }
    private double strToDouble(string str) {
      return Convert.ToDouble(str);
    }
  }
}
