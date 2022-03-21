using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog.Web;

namespace ticketsV3 {
    public class TicketFile {
        public string filePath1 {get; set;}
        public string filePath2 {get; set;}
        public string filePath3 {get; set;}
        public List<Bug> Bugs {get; set;}
        public List<Enhancement> Enhancements {get; set;}
        public List<Task> Tasks {get; set;}
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();

        public TicketFile(string ticketFilePath1, string ticketFilePath2, string ticketFilePath3) {
            filePath1 = ticketFilePath1;
            filePath2 = ticketFilePath2;
            filePath3 = ticketFilePath3;
            Bugs = new List<Bug>();
            Enhancements = new List<Enhancement>();
            Tasks = new List<Task>();
            if(File.Exists(filePath1))
            {
                StreamReader ticketRead = new StreamReader(filePath1);
                while (!ticketRead.EndOfStream)
                {
                    Bug ticket = new Bug();
                    string line = ticketRead.ReadLine();
                    string[] ticketSplit = line.Split(',');
                    ticket.ticketID = UInt64.Parse(ticketSplit[0]);
                    ticket.summary = ticketSplit[1];
                    ticket.status = ticketSplit[2];
                    ticket.priority = ticketSplit[3];
                    ticket.submitter = ticketSplit[4];
                    ticket.assigned = ticketSplit[5];
                    ticket.peopleWatching = ticketSplit[6].Split('|').ToList();
                    ticket.severity = ticketSplit[7];
                    Bugs.Add(ticket);
                }
                ticketRead.Close();
            }
            if(File.Exists(filePath2))
            {
                StreamReader ticketRead = new StreamReader(filePath2);
                while (!ticketRead.EndOfStream)
                {
                    Enhancement ticket = new Enhancement();
                    string line = ticketRead.ReadLine();
                    string[] ticketSplit = line.Split(',');
                    ticket.ticketID = UInt64.Parse(ticketSplit[0]);
                    ticket.summary = ticketSplit[1];
                    ticket.status = ticketSplit[2];
                    ticket.priority = ticketSplit[3];
                    ticket.submitter = ticketSplit[4];
                    ticket.assigned = ticketSplit[5];
                    ticket.peopleWatching = ticketSplit[6].Split('|').ToList();
                    ticket.software = ticketSplit[7];
                    ticket.cost = double.Parse(ticketSplit[8]);
                    ticket.reason = ticketSplit[9];
                    ticket.estimate = double.Parse(ticketSplit[10]);
                    Enhancements.Add(ticket);
                }
                ticketRead.Close();
            }
            if(File.Exists(filePath3))
            {
                StreamReader ticketRead = new StreamReader(filePath3);
                while (!ticketRead.EndOfStream)
                {
                    Task ticket = new Task();
                    string line = ticketRead.ReadLine();
                    string[] ticketSplit = line.Split(',');
                    ticket.ticketID = UInt64.Parse(ticketSplit[0]);
                    ticket.summary = ticketSplit[1];
                    ticket.status = ticketSplit[2];
                    ticket.priority = ticketSplit[3];
                    ticket.submitter = ticketSplit[4];
                    ticket.assigned = ticketSplit[5];
                    ticket.peopleWatching = ticketSplit[6].Split('|').ToList();
                    ticket.projectName = ticketSplit[7];
                    ticket.dueDate = DateTime.Parse(ticketSplit[8]);
                    Tasks.Add(ticket);
                }
                ticketRead.Close();
            }
            if (!File.Exists(filePath1) || !File.Exists(filePath2) || !File.Exists(filePath3))
            {
                logger.Error("No files exist");
            }
        }

        public void AddBugTicket(Bug bug) {
            if(File.Exists(filePath1)) {
                bug.ticketID = Bugs.Max(t => t.ticketID) + 1;
                File.AppendAllText(filePath1, bug.Entry() + "\n");
                logger.Info("Bug Ticket ID {0} added", bug.ticketID);
            }
            else {
                StreamWriter ticketWrite = new StreamWriter(filePath1);
                bug.ticketID = 1;
                ticketWrite.Write(bug.Entry() + "\n");
                ticketWrite.Close();
                logger.Info("File {0} added and Bug Ticket ID {1} added", filePath1, bug.ticketID);
            }
        }

        public void AddEnhancementTicket(Enhancement enhancement) {
            if(File.Exists(filePath2)) {
                enhancement.ticketID = Enhancements.Max(t => t.ticketID) + 1;
                File.AppendAllText(filePath2, enhancement.Entry() + "\n");
                logger.Info("Enhancement Ticket ID {0} added", enhancement.ticketID);
            }
            else {
                StreamWriter ticketWrite = new StreamWriter(filePath2);
                enhancement.ticketID = 1;
                ticketWrite.Write(enhancement.Entry() + "\n");
                ticketWrite.Close();
                logger.Info("File {0} added and Enhancement Ticket ID {1} added", filePath2, enhancement.ticketID);
            }
        }

        public void AddTaskTicket(Task task) {
            if(File.Exists(filePath3)) {
                task.ticketID = Tasks.Max(t => t.ticketID) + 1;
                File.AppendAllText(filePath3, task.Entry() + "\n");
                logger.Info("Task Ticket ID {0} added", task.ticketID);
            }
            else {
                StreamWriter ticketWrite = new StreamWriter(filePath3);
                task.ticketID = 1;
                ticketWrite.Write(task.Entry() + "\n");
                ticketWrite.Close();
                logger.Info("File {0} added and Task Ticket ID {1} added", filePath3, task.ticketID);
            }
        }
    }
}