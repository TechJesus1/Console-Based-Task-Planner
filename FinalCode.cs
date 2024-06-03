using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskPlanner
{
    public class Task
    {
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public ConsoleColor ColorTag { get; set; }
        public int Priority { get; set; }

        public Task(string name, DateTime dueDate, ConsoleColor colorTag, int priority)
        {
            Name = name;
            DueDate = dueDate;
            ColorTag = colorTag;
            Priority = priority;
        }
    }

    public class Category
    {
        public string Name { get; set; }
        public List<Task> Tasks { get; set; }

        public Category(string name)
        {
            Name = name;
            Tasks = new List<Task>();
        }
    }

    class TaskPlanner
    {
        static List<Category> categories = new List<Category>();

        static void Main(string[] args)
        {
            // Initialize categories
            categories.Add(new Category("Personal"));
            categories.Add(new Category("Work"));
            categories.Add(new Category("Family"));

            while (true)
            {
                Console.Clear();
                DisplayCategories();

                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("1. Add a task");
                Console.WriteLine("2. Delete a task");
                Console.WriteLine("3. Move a task");
                Console.WriteLine("4. Add a category");
                Console.WriteLine("5. Delete a category");
                Console.WriteLine("6. Exit");
                Console.Write(">> ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddTask();
                        break;
                    case "2":
                        DeleteTask();
                        break;
                    case "3":
                        MoveTask();
                        break;
                    case "4":
                        AddCategory();
                        break;
                    case "5":
                        DeleteCategory();
                        break;
                    case "6":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }

                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void DisplayCategories()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(new string(' ', 12) + "CATEGORIES");
            Console.WriteLine(new string(' ', 10) + new string('-', 94));
            Console.WriteLine("{0,10}|{1,30}|{2,30}|{3,30}|", "item #", "Category", "Task", "Due Date");
            Console.WriteLine(new string(' ', 10) + new string('-', 94));

            int itemNumber = 0;
            foreach (Category category in categories)
            {
                foreach (Task task in category.Tasks)
                {
                    Console.Write("{0,10}|", itemNumber);
                    Console.Write("{0,30}|", category.Name);
                    Console.Write("{0,-30}|", task.Name);
                    Console.WriteLine("{0,30:yyyy-MM-dd}", task.DueDate);
                    itemNumber++;
                }
            }

            Console.ResetColor();
        }

        static void AddTask()
        {
            Console.Clear();
            DisplayCategories();

            Console.WriteLine("\nWhich category do you want to place a new task? Type 'Personal', 'Work', or 'Family'");
            Console.Write(">> ");
            string categoryName = Console.ReadLine().ToLower();

            Category category = categories.FirstOrDefault(c => c.Name.ToLower() == categoryName); if (category == null)
            {
                Console.WriteLine("Invalid category.");
                return;
            }

            Console.WriteLine("Describe your task below (max. 30 symbols).");
            Console.Write("Task name: ");
            string taskName = Console.ReadLine();

            if (taskName.Length > 30)
                taskName = taskName.Substring(0, 30);

            Console.Write("Due date (yyyy-mm-dd): ");
            string dueDateInput = Console.ReadLine();
            if (!DateTime.TryParse(dueDateInput, out DateTime dueDate))
            {
                Console.WriteLine("Invalid due date.");
                return;
            }

            Console.Write("Color tag (Red, Green, Yellow): ");
            string colorInput = Console.ReadLine().ToLower();
            ConsoleColor colorTag;
            switch (colorInput)
            {
                case "red":
                    colorTag = ConsoleColor.Red;
                    break;
                case "green":
                    colorTag = ConsoleColor.Green;
                    break;
                case "yellow":
                    colorTag = ConsoleColor.Yellow;
                    break;
                default:
                    Console.WriteLine("Invalid color tag.");
                    return;
            }

            Console.Write("Priority (1-10): ");
            string priorityInput = Console.ReadLine();
            if (!int.TryParse(priorityInput, out int priority))
            {
                Console.WriteLine("Invalid priority.");
                return;
            }

            Task task = new Task(taskName, dueDate, colorTag, priority);
            category.Tasks.Add(task);
        }

        static void DeleteTask()
        {
            Console.Clear();
            DisplayCategories();

            Console.Write("Enter the item number of the task you want to delete: ");
            string input = Console.ReadLine();
            if (!int.TryParse(input, out int itemNumber))
            {
                Console.WriteLine("Invalid item number.");
                return;
            }

            Category category = categories.FirstOrDefault(c => c.Tasks.Any(t => t.Priority == itemNumber));
            if (category == null)
            {
                Console.WriteLine("Invalid item number.");
                return;
            }

            Task taskToRemove = category.Tasks.FirstOrDefault(t => t.Priority == itemNumber);
            category.Tasks.Remove(taskToRemove);
        }

        static void MoveTask()
        {
            Console.Clear();
            DisplayCategories();

            Console.Write("Enter the item number of the task you want to move: ");
            string input = Console.ReadLine();
            if (!int.TryParse(input, out int itemNumber))
            {
                Console.WriteLine("Invalid item number.");
                return;
            }

            Category sourceCategory = categories.FirstOrDefault(c => c.Tasks.Any(t => t.Priority == itemNumber));
            if (sourceCategory == null)
            {
                Console.WriteLine("Invalid item number.");
                return;
            }

            Task taskToMove = sourceCategory.Tasks.FirstOrDefault(t => t.Priority == itemNumber);

            Console.Write("Enter the new category (Personal, Work, or Family): ");
            string newCategoryName = Console.ReadLine().ToLower();

            Category destinationCategory = categories.FirstOrDefault(c => c.Name.ToLower() == newCategoryName);
            if (destinationCategory == null)
            {
                Console.WriteLine("Invalid category.");
                return;
            }

            int newPriority = destinationCategory.Tasks.Count + 1;
            taskToMove.Priority = newPriority;
            destinationCategory.Tasks.Add(taskToMove);
            sourceCategory.Tasks.Remove(taskToMove);
        }

        static void AddCategory()
        {
            Console.Clear();
            DisplayCategories();

            Console.Write("Enter the name of the new category: ");
            string categoryName = Console.ReadLine();

            Category newCategory = new Category(categoryName);
            categories.Add(newCategory);
        }

        static void DeleteCategory()
        {
            Console.Clear();
            DisplayCategories();

            Console.Write("Enter the name of the category you want to delete: ");
            string categoryName = Console.ReadLine();

            Category categoryToRemove = categories.FirstOrDefault(c => c.Name.ToLower() == categoryName);
            if (categoryToRemove == null)
            {
                Console.WriteLine("Invalid category.");
                return;
            }

            categories.Remove(categoryToRemove);
        }
    }
}