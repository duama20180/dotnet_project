using Sereda.TaskPlanner.Domain.Logic;
using Sereda.TaskPlanner.Domain.Models.Enums;
using System.Diagnostics.Metrics;
using Sereda.TaskPlanner.DataAccess;

internal static class Program
{
    public static void Main(string[] args)
    {

        var repository = new FileWorkItemsRepository();
        bool isRunning = true;

        while (isRunning)
        {
            Console.WriteLine("Task Planner:");
            Console.WriteLine("[A]dd work item");
            Console.WriteLine("[B]uild a plan");
            Console.WriteLine("[M]ark work item as completed");
            Console.WriteLine("[R]emove a work item");
            Console.WriteLine("[Q]uit the app");
            Console.Write("Choose an option: ");
            string option = Console.ReadLine().ToUpper();


            switch (option)
            {
                case "A":
                    AddWorkItem(repository);
                    break;
                case "B":
                    BuildPlan(repository);
                    break;
                case "M":
                    MarkWorkItemCompleted(repository);
                    break;
                case "R":
                    RemoveWorkItem(repository);
                    break;
                case "Q":
                    isRunning = false;
                    repository.SaveChanges();
                    Console.WriteLine("Exiting the application...");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }

            Console.WriteLine();
        }

        }

    static void AddWorkItem(FileWorkItemsRepository repository)
    {
        Console.Write("Enter title: ");
        string title = Console.ReadLine();

        Console.Write("Enter description: ");
        string description = Console.ReadLine();

        Console.Write("Enter due date (dd.MM.yyyy): ");
        DateTime dueDate = DateTime.Parse(Console.ReadLine());

        Console.Write("Enter priority (None, Low, Medium, High, Urgent): ");
        Priority priority = Enum.Parse<Priority>(Console.ReadLine(), true);

        Console.Write("Enter complexity ( None, Minutes, Hours, Days, Weeks): ");
        Complexity complexity = Enum.Parse<Complexity>(Console.ReadLine(), true);

        var workItem = new WorkItem
        {
            Title = title,
            Description = description,
            DueDate = dueDate,
            Priority = priority,
            Complexity = complexity,
            CreationDate = DateTime.Now,
            IsCompleted = false
        };

        repository.Add(workItem);
        repository.SaveChanges();
        Console.WriteLine();
        Console.WriteLine("Work item added successfully.");
    }

    static void BuildPlan(FileWorkItemsRepository repository)
    {
        var workItems = repository.GetAll();
        if (workItems.Length == 0)
        {
            Console.WriteLine();
            Console.WriteLine("No work items available.");
        }
        else
        {
            Console.WriteLine("Current work items:");
            foreach (var item in workItems)
            {
                Console.WriteLine();
                Console.WriteLine(item);
            }
        }
    }

    static void MarkWorkItemCompleted(FileWorkItemsRepository repository)
    {
        Console.Write("Enter the ID of the work item to mark as completed: ");
        if (Guid.TryParse(Console.ReadLine(), out Guid id))
        {
            var workItem = repository.Get(id);
            if (workItem != null)
            {
                workItem.IsCompleted = true;
                repository.Update(workItem);
                repository.SaveChanges();
                Console.WriteLine();
                Console.WriteLine("Work item marked as completed.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Work item not found.");
            }
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Invalid ID format.");
        }
    }

    static void RemoveWorkItem(FileWorkItemsRepository repository)
    {
        Console.Write("Enter the ID of the work item to remove: ");
        if (Guid.TryParse(Console.ReadLine(), out Guid id))
        {
            if (repository.Remove(id))
            {
                repository.SaveChanges();
                Console.WriteLine();
                Console.WriteLine("Work item removed.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Work item not found.");
            }
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Invalid ID format.");
        }
    }
}

//previous version of the console program

    //var workItems = new List<WorkItem>();

    //int taskCount;
    //while (true)
    //{
    //    Console.Write("Enter the number of tasks you want to add: ");
    //    if (int.TryParse(Console.ReadLine(), out taskCount) && taskCount > 0)
    //    {
    //        break;
    //    }
    //    Console.WriteLine("Invalid number. Try again.");
    //}

    //for (int i = 0; i < taskCount; i++)
    //{
    //    Console.WriteLine($"\nEntering name for task  #{i + 1}");

    //    Console.Write("Entering name for task: ");
    //    string title = Console.ReadLine()?.Trim();

    //    Console.Write($"\nEnter the description of the task:  ");
    //    string description = Console.ReadLine()?.Trim();

    //    Console.Write($"\nEnter the date of creation(dd.MM.YYYY): ");
    //    if (!DateTime.TryParse(Console.ReadLine(), out DateTime creationDate))
    //    {
    //        Console.WriteLine("Invalid date format. Try again.");
    //        i--;
    //        continue;
    //    }

    //    Console.Write($"\nEnter the end date (dd.mm.yyyy): ");
    //    if (!DateTime.TryParse(Console.ReadLine(), out DateTime dueDate))
    //    {
    //        Console.WriteLine("Invalid date format. Try again.");
    //        i--;
    //        continue;
    //    }

    //    Console.Write($"\nEnter the priority (None, Low, Medium, High, Urgent): ");
    //    if (!Enum.TryParse(Console.ReadLine(), true, out Priority priority))
    //    {
    //        Console.WriteLine("Invalid priority. Try again.");
    //        i--;
    //        continue;
    //    }

    //    Console.Write($"\nEnter the complexity (None, Minutes, Hours, Days, Weeks): ");
    //    if (!Enum.TryParse(Console.ReadLine(), true, out Complexity complexity))
    //    {
    //        Console.WriteLine("Invalid complexity. Try again.");
    //        i--;
    //        continue;
    //    }

    //    workItems.Add(new WorkItem
    //    {
    //        Title = title,
    //        Description = description,
    //        CreationDate = creationDate,
    //        DueDate = dueDate,
    //        Priority = priority,
    //        Complexity = complexity,
    //        IsCompleted = false
    //    });

    //    Console.WriteLine("Task added!");
    //}

    //var planner = new SimpleTaskPlanner();
    //var sortedWorkItems = planner.CreatePlan(workItems.ToArray());

    //Console.WriteLine("\nSorted tasks:");
    //foreach (var item in sortedWorkItems)
    //{
    //    Console.WriteLine(item.ToString());
    //}