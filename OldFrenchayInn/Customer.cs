using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldFrenchayInn {
    /// <summary>
    /// A class representing a distinct customer
    /// </summary>
    public class Customer {
        public string Name { get; internal set; }
        public string Email { get; internal set; }
        public CardDetails CardDetails { get; internal set; }

        public Customer(string name, string email, CardDetails cardDetails){
            Name = name;
            Email = email;
            CardDetails = cardDetails;
        }

        public override string ToString(){
            return string.Format("{0} ({1})", Name, Email);
        }
    }
}
