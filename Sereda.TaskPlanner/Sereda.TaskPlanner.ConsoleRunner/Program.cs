using Sereda.TaskPlanner.Domain.Logic;
using Sereda.TaskPlanner.Domain.Models.Enums;
using System.Diagnostics.Metrics;

internal static class Program
{
    public static void Main(string[] args)
    {
        var workItems = new List<WorkItem>();

        int taskCount;
        while (true)
        {
            Console.Write("Enter the number of tasks you want to add: ");
            if (int.TryParse(Console.ReadLine(), out taskCount) && taskCount > 0)
            {
                break;
            }
            Console.WriteLine("Invalid number. Try again.");
        }

        for (int i = 0; i < taskCount; i++)
        {
            Console.WriteLine($"\nEntering name for task  #{i + 1}");

            Console.Write("Entering name for task: ");
            string title = Console.ReadLine()?.Trim();

            Console.Write($"\nEnter the description of the task:  ");
            string description = Console.ReadLine()?.Trim();

            Console.Write($"\nEnter the date of creation(dd.MM.YYYY): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime creationDate))
            {
                Console.WriteLine("Invalid date format. Try again.");
                i--;
                continue;
            }

            Console.Write($"\nEnter the end date (dd.mm.yyyy): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dueDate))
            {
                Console.WriteLine("Invalid date format. Try again.");
                i--;
                continue;
            }

            Console.Write($"\nEnter the priority (None, Low, Medium, High, Urgent): ");
            if (!Enum.TryParse(Console.ReadLine(), true, out Priority priority))
            {
                Console.WriteLine("Invalid priority. Try again.");
                i--;
                continue;
            }

            Console.Write($"\nEnter the complexity (None, Minutes, Hours, Days, Weeks): ");
            if (!Enum.TryParse(Console.ReadLine(), true, out Complexity complexity))
            {
                Console.WriteLine("Invalid complexity. Try again.");
                i--;
                continue;
            }

            // Додаємо новий елемент у список
            workItems.Add(new WorkItem
            {
                Title = title,
                Description = description,
                CreationDate = creationDate,
                DueDate = dueDate,
                Priority = priority,
                Complexity = complexity,
                IsCompleted = false
            });

            Console.WriteLine("Task added!");
        }

        var planner = new SimpleTaskPlanner();
        var sortedWorkItems = planner.CreatePlan(workItems.ToArray());

        Console.WriteLine("\nSorted tasks:");
        foreach (var item in sortedWorkItems)
        {
            Console.WriteLine(item.ToString());
        }
    }
}