using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldFrenchayInn {
    /// <summary>
    /// An object to hold a customer's payment details
    /// </summary>
    public class CardDetails{
        public string Number { get; internal set; }
        public string IssueDate { get; internal set; }
        public string ExpiryDate { get; internal set; }

        public CardDetails(string number, string issueDate, string expiryDate){
            Number = number;
            IssueDate = issueDate;
            ExpiryDate = expiryDate;
        }

    }
}