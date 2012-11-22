using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldFrenchayInn {
    class IsNumericValidator {
        static public bool IsNumeric(string item){
            int num;
            return int.TryParse(item, out num);
        }
    }
}
