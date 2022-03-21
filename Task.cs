using System;

namespace ticketsV3 {
    public class Task : Ticket {
        public string projectName { get; set; }
        public DateTime dueDate { get; set; }

        public override string Entry(){
            return $"{ticketID},{summary},{status},{priority},{submitter},{assigned},{string.Join("|", peopleWatching)},{projectName},{dueDate}";
        }

        public string Read(){
            return $"{ticketID},{summary},{status},{priority},{submitter},{assigned},{string.Join("|", peopleWatching)},{projectName},{dueDate:MM}/{dueDate:dd}/{dueDate:yyyy}";
        }
    }
}