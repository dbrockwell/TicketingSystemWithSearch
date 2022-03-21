﻿using System;
using System.IO;
using NLog.Web;
using System.Linq;

namespace ticketsV3
{
    class Program
    {
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            string ticketFilePath1 = Directory.GetCurrentDirectory() + "\\tickets1.csv";
            string ticketFilePath2 = Directory.GetCurrentDirectory() + "\\tickets2.csv";
            string ticketFilePath3 = Directory.GetCurrentDirectory() + "\\tickets3.csv";
            logger.Info("Program started");
            TicketFile ticketFile = new TicketFile(ticketFilePath1, ticketFilePath2, ticketFilePath3);
            string choice;
            do {
                Console.WriteLine("1) Read ticket information");
                Console.WriteLine("2) Add ticket infomation");
                Console.WriteLine("3) Search for ticket");
                Console.WriteLine("Enter any other key to exit");
                choice = Console.ReadLine();

                if (choice == "1") {
                    Console.WriteLine("Bug Tickets");
                    foreach(Bug bug in ticketFile.Bugs)
                    {
                        Console.WriteLine(bug.Entry());
                    }
                    Console.WriteLine("Enhancement Tickets");
                    foreach(Enhancement enhancement in ticketFile.Enhancements)
                    {
                        Console.WriteLine(enhancement.Read());
                    }
                    Console.WriteLine("Task Tickets");
                    foreach(Task task in ticketFile.Tasks)
                    {
                        Console.WriteLine(task.Read());
                    }
                }
                
                if (choice == "2") {
                    Console.WriteLine("1) Enter a bug ticket");
                    Console.WriteLine("2) Enter an enhancement ticket");
                    Console.WriteLine("3) Enter a task ticket");
                    Console.WriteLine("Enter anything else to exit");
                    string ticketChoice = Console.ReadLine();
                    if (ticketChoice == "1") {
                        Bug bug = new Bug();
                        StandardInfo(bug);
                        bug.severity = NullCheck("Enter Ticket Severity", "severity");
                        ticketFile.AddBugTicket(bug);
                    }
                    else if (ticketChoice == "2") {
                        Enhancement enhancement = new Enhancement();
                        StandardInfo(enhancement);
                        enhancement.software = NullCheck("Enter Ticket Software", "software");
                        bool contineAdd = true;
                        do{
                            try {
                                enhancement.cost = Double.Parse(NullCheck("Enter Ticket Cost", "cost"));
                                contineAdd = true;
                            } catch (Exception){
                                logger.Error("Not a correct number");
                                contineAdd = false;
                            }
                        } while (contineAdd == false);
                        enhancement.reason = NullCheck("Enter Ticket Reason", "reason");
                        do{
                            try {
                                enhancement.estimate = Double.Parse(NullCheck("Enter Ticket Estimate", "estimate"));
                                contineAdd = true;
                            } catch (Exception){
                                logger.Error("Not a correct number");
                                contineAdd = false;
                            }
                        } while (contineAdd == false);
                        ticketFile.AddEnhancementTicket(enhancement);
                    }
                    else if (ticketChoice == "3") {
                        Task task = new Task();
                        StandardInfo(task);
                        task.projectName = NullCheck("Enter Ticket Project Name", "project name");
                        bool dateCheck = true;
                        do {
                            try {
                                string dateEntry = NullCheck("Enter Ticket Due Date (Month/Day/Year)", "due date");
                                string[] date = dateEntry.Split("/");
                                int month = Int32.Parse(date[0]);
                                int day = Int32.Parse(date[1]);
                                int year = Int32.Parse(date[2]);
                                task.dueDate = new DateTime (year,month,day);
                                dateCheck = true;
                            } catch (Exception) {
                                logger.Error("Incorrect date entered");
                                dateCheck = false;
                            }
                        } while(dateCheck == false);
                        ticketFile.AddTaskTicket(task);
                    }
                }

                if (choice == "3") {
                    Console.WriteLine("1) Search by status");
                    Console.WriteLine("2) Search by priority");
                    Console.WriteLine("3) Search by submitter");
                    Console.WriteLine("Enter anything else to exit");
                    string searchChoice = Console.ReadLine();
                    if (searchChoice == "1") {
                        Console.WriteLine("Enter ticket status");
                        string statusSearch = Console.ReadLine();
                        int statusNumberBug = ticketFile.Bugs.Where(b => b.status.Contains(statusSearch, StringComparison.OrdinalIgnoreCase)).Count();
                        int statusNumberEnhancements = ticketFile.Enhancements.Where(b => b.status.Contains(statusSearch, StringComparison.OrdinalIgnoreCase)).Count();
                        int statusNumberTask = ticketFile.Tasks.Where(b => b.status.Contains(statusSearch, StringComparison.OrdinalIgnoreCase)).Count();
                        int statusTotal = statusNumberBug + statusNumberEnhancements + statusNumberTask;
                        Console.WriteLine($"There are {statusTotal} tickets that have a status of {statusSearch}");
                        var statusSearchBug = ticketFile.Bugs.Where(b => b.status.Contains(statusSearch, StringComparison.OrdinalIgnoreCase));
                        Console.WriteLine($"Bug Tickets ({statusNumberBug})");
                        foreach(Bug bug in statusSearchBug)
                        {
                            Console.WriteLine(bug.Entry());
                        }
                        var statusSearchEnhancements = ticketFile.Enhancements.Where(b => b.status.Contains(statusSearch, StringComparison.OrdinalIgnoreCase));
                        Console.WriteLine($"Enhancement Tickets ({statusNumberEnhancements})");
                        foreach(Enhancement enhancement in statusSearchEnhancements)
                        {
                            Console.WriteLine(enhancement.Read());
                        }
                        var statusSearchTask = ticketFile.Tasks.Where(b => b.status.Contains(statusSearch, StringComparison.OrdinalIgnoreCase));
                        Console.WriteLine($"Task Tickets ({statusNumberTask})");
                        foreach(Task task in statusSearchTask)
                        {
                            Console.WriteLine(task.Read());
                        }
                    }
                    else if (searchChoice == "2") {
                        Console.WriteLine("Enter ticket priority");
                        string prioritySearch = Console.ReadLine();
                        int priorityNumberBug = ticketFile.Bugs.Where(b => b.priority.Contains(prioritySearch, StringComparison.OrdinalIgnoreCase)).Count();
                        int priorityNumberEnhancements = ticketFile.Enhancements.Where(b => b.priority.Contains(prioritySearch, StringComparison.OrdinalIgnoreCase)).Count();
                        int priorityNumberTask = ticketFile.Tasks.Where(b => b.priority.Contains(prioritySearch, StringComparison.OrdinalIgnoreCase)).Count();
                        int priorityTotal = priorityNumberBug + priorityNumberEnhancements + priorityNumberTask;
                        Console.WriteLine($"There are {priorityTotal} tickets that have a status of {prioritySearch}");
                        var prioritySearchBug = ticketFile.Bugs.Where(b => b.priority.Contains(prioritySearch, StringComparison.OrdinalIgnoreCase));
                        Console.WriteLine($"Bug Tickets ({priorityNumberBug})");
                        foreach(Bug bug in prioritySearchBug)
                        {
                            Console.WriteLine(bug.Entry());
                        }
                        var prioritySearchEnhancements = ticketFile.Enhancements.Where(b => b.priority.Contains(prioritySearch, StringComparison.OrdinalIgnoreCase));
                        Console.WriteLine($"Enhancement Tickets ({priorityNumberEnhancements})");
                        foreach(Enhancement enhancement in prioritySearchEnhancements)
                        {
                            Console.WriteLine(enhancement.Read());
                        }
                        var prioritySearchTask = ticketFile.Tasks.Where(b => b.priority.Contains(prioritySearch, StringComparison.OrdinalIgnoreCase));
                        Console.WriteLine($"Task Tickets ({priorityNumberTask})");
                        foreach(Task task in prioritySearchTask)
                        {
                            Console.WriteLine(task.Read());
                        }

                    }
                    else if (searchChoice == "3") {

                    }
                }
            } while (choice == "1" || choice == "2" || choice == "3");

            logger.Info("Program Ended");
        }

        public static void StandardInfo(Ticket ticket) {
            ticket.summary = NullCheck("Enter Ticket Summary", "summary");
            ticket.status = NullCheck("Enter Ticket Status", "status");
            ticket.priority = NullCheck("Enter Ticket Priority", "priority");
            ticket.submitter = NullCheck("Enter the Ticket Submitter", "submitter");
            ticket.assigned = NullCheck("Enter Person Assigned", "assigned");
            ticket.peopleWatching.Add(NullCheck("Enter Person Watching", "person watching"));
            string anotherWatcher;
            do {
                Console.WriteLine("Enter Another Person Watching Or Just Press 'ENTER' To Continue");
                anotherWatcher = Console.ReadLine();
                if (anotherWatcher != "") {
                    ticket.peopleWatching.Add(anotherWatcher);
                }
            } while (anotherWatcher != "");
        }

        public static string NullCheck(string question, string errorName) {
            bool continueLoop = true;
            string entry;
            do {
                Console.WriteLine(question);
                entry = Console.ReadLine();
                if (entry == "") {
                    logger.Error("No input for {0} was entered", errorName);
                    continueLoop = true;
                }
                else {
                    continueLoop = false;
                }
            } while (continueLoop == true);

            return entry;
        }
    }
}
