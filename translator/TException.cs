using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace translator {
  class TException : Exception {
    public int startPositon;
    public int endPosition;
    public TException(string message, int startPos, int endPos) : base(message){
      this.startPositon = startPos;
      this.endPosition = endPos;
    }
  }
}
