﻿using System;
using System.Collections;
using System.Collections.Generic;
using AssocArray = System.Collections.Generic.Dictionary<string, object>;
using AssocElem = System.Collections.Generic.KeyValuePair<string, object>;
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
          this.doubleNums.Add(this.strToDouble(this.lexer.currentWord));
          this.lexer.nextWord();
          if (this.lexer.currentWord == "Конец") {
            this.lexer.nextWord();
            if (this.lexer.currentWord != "второго") {
              this.makeException("Ожидалось \"Конец второго\"");
            } else if (this.lexer.currentWord == "второго") {
              this.lexer.nextWord();
              break;
            }
          } else if (this.lexer.currentWord == ",") {
            this.lexer.nextWord();
          } else {
            this.makeException("Пропущена запятая");
          }
        }
      } else if (this.lexer.currentWord == "Третье") {
        this.lexer.nextWord();
        while (true) {
          if (this.isVar(this.lexer.currentWord)) {
            this.varsArray[this.lexer.currentWord] = null;
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
      while (true) {
        if (this.isInt(this.lexer.currentWord)) {
          this.zvenoNums.Add(this.strToInt(this.lexer.currentWord));
          this.lexer.nextWord();
          if (this.isLabel(this.lexer.currentWord) || this.isVar(this.lexer.currentWord)) {
            break;
          }
        } else {
          this.makeException("Ожидалось целое число");
        }
      }

    }
    private void oper() { }

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
    private bool isVar(string str) {
      Regex regex = new Regex("^[a-zA-Zа-яА-Я]{1,}([0-9]*|[a-zA-Zа-яА-Я]*)*$");
      return regex.IsMatch(str);
    }
    private bool isLabel(string str) {
      Regex regex = new Regex("^[0-9]*\\:$");
      return regex.IsMatch(str);
    }
    private bool isInt(string str) {
      if(str != "") {
        Regex regex = new Regex("^[0-9]*$");
        return regex.IsMatch(str);
      } else {
        return false;
      }
      
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
        Console.WriteLine(elem.Key.ToString() + " " + elem.Value);
      }
      
    }
  }
}
