using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldFrenchayInn {
    class Program {
        static void Main(string[] args) {
            TUInterface UI = new TUInterface();
            while (UI.Loop()){}
        }
    }
}
