using System;
using System.Collections.Generic;
using System.Linq;

namespace ticketsV3 {
    public class Ticket {
        public UInt64 ticketID { get; set; }
        public string summary {get; set;}
        public string status {get; set;}
        public string priority {get; set;}
        public string submitter {get; set;}
        public string assigned {get; set;}
        public List<string> peopleWatching {get; set;}

        public Ticket()
        {
            peopleWatching = new List<string>();
        }

        public virtual string Entry() {
            return $"{ticketID},{summary},{status},{priority},{submitter},{assigned},{string.Join("|", peopleWatching)}";
        }
    }
}