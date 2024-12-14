using System;
using System.Collections.Generic;
using System.IO;

namespace EternalQuest
{
    // Base class for all goals
    public abstract class Goal
    {
        public string Name { get; private set; }
        public int Points { get; private set; }
        public bool IsCompleted { get; protected set; }

        protected Goal(string name, int points)
        {
            Name = name;
            Points = points;
            IsCompleted = false;
        }

        public abstract int RecordEvent();
        public abstract string DisplayProgress();
        public abstract string SaveData();
        public static Goal LoadData(string data);
    }

    // SimpleGoal class for one-time goals
    public class SimpleGoal : Goal
    {
        public SimpleGoal(string name, int points) : base(name, points) { }

        public override int RecordEvent()
        {
            if (!IsCompleted)
            {
                IsCompleted = true;
                return Points;
            }
            return 0;
        }

        public override string DisplayProgress()
        {
            return IsCompleted ? $"[X] {Name}" : $"[ ] {Name}";
        }

        public override string SaveData()
        {
            return $"SimpleGoal|{Name}|{Points}|{IsCompleted}";
        }
    }

    // EternalGoal class for repeatable goals
    public class EternalGoal : Goal
    {
        public EternalGoal(string name, int points) : base(name, points) { }

        public override int RecordEvent()
        {
            return Points;
        }

        public override string DisplayProgress()
        {
            return $"[∞] {Name}";
        }

        public override string SaveData()
        {
            return $"EternalGoal|{Name}|{Points}";
        }
    }

    // ChecklistGoal class for multi-step goals
    public class ChecklistGoal : Goal
    {
        public int TargetCount { get; private set; }
        public int CurrentCount { get; private set; }
        public int Bonus { get; private set; }

        public ChecklistGoal(string name, int points, int targetCount, int bonus)
            : base(name, points)
        {
            TargetCount = targetCount;
            CurrentCount = 0;
            Bonus = bonus;
        }

        public override int RecordEvent()
        {
            if (!IsCompleted)
            {
                CurrentCount++;
                if (CurrentCount >= TargetCount)
                {
                    IsCompleted = true;
                    return Points + Bonus;
                }
                return Points;
            }
            return 0;
        }

        public override string DisplayProgress()
        {
            return IsCompleted
                ? $"[X] {Name} (Completed {CurrentCount}/{TargetCount})"
                : $"[ ] {Name} (Completed {CurrentCount}/{TargetCount})";
        }

        public override string SaveData()
        {
            return $"ChecklistGoal|{Name}|{Points}|{TargetCount}|{CurrentCount}|{Bonus}|{IsCompleted}";
        }
    }

    // Program class
    public class Program
    {
        private static List<Goal> goals = new List<Goal>();
        private static int totalScore = 0;

        public static void Main(string[] args)
        {
            LoadGoals();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Eternal Quest - Goal Tracking System");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine($"Total Score: {totalScore}\n");

                Console.WriteLine("1. List Goals");
                Console.WriteLine("2. Create Goal");
                Console.WriteLine("3. Record Event");
                Console.WriteLine("4. Save Goals");
                Console.WriteLine("5. Exit");
                Console.Write("\nChoose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListGoals();
                        break;
                    case "2":
                        CreateGoal();
                        break;
                    case "3":
                        RecordEvent();
                        break;
                    case "4":
                        SaveGoals();
                        break;
                    case "5":
                        SaveGoals();
                        return;
                    default:
                        Console.WriteLine("Invalid choice! Press Enter to continue.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private static void ListGoals()
        {
            Console.Clear();
            Console.WriteLine("Your Goals:");
            if (goals.Count == 0)
            {
                Console.WriteLine("No goals yet!");
            }
            else
            {
                for (int i = 0; i < goals.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {goals[i].DisplayProgress()}");
                }
            }
            Console.WriteLine("\nPress Enter to return to the menu.");
            Console.ReadLine();
        }

        private static void CreateGoal()
        {
            Console.Clear();
            Console.WriteLine("Create a New Goal");
            Console.WriteLine("-----------------");
            Console.Write("Goal Name: ");
            string name = Console.ReadLine();

            Console.Write("Goal Points: ");
            int points = int.Parse(Console.ReadLine());

            Console.WriteLine("Choose Goal Type:");
            Console.WriteLine("1. Simple Goal");
            Console.WriteLine("2. Eternal Goal");
            Console.WriteLine("3. Checklist Goal");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    goals.Add(new SimpleGoal(name, points));
                    break;
                case "2":
                    goals.Add(new EternalGoal(name, points));
                    break;
                case "3":
                    Console.Write("Target Count: ");
                    int targetCount = int.Parse(Console.ReadLine());

                    Console.Write("Bonus Points: ");
                    int bonus = int.Parse(Console.ReadLine());

                    goals.Add(new ChecklistGoal(name, points, targetCount, bonus));
                    break;
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
            Console.WriteLine("Goal created! Press Enter to return to the menu.");
            Console.ReadLine();
        }

        private static void RecordEvent()
        {
            Console.Clear();
            Console.WriteLine("Record an Event");
            Console.WriteLine("---------------");
            for (int i = 0; i < goals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {goals[i].DisplayProgress()}");
            }
            Console.Write("\nEnter goal number to record event: ");
            int goalNumber = int.Parse(Console.ReadLine()) - 1;

            if (goalNumber >= 0 && goalNumber < goals.Count)
            {
                int pointsEarned = goals[goalNumber].RecordEvent();
                totalScore += pointsEarned;
                Console.WriteLine($"You earned {pointsEarned} points! Total Score: {totalScore}");
            }
            else
            {
                Console.WriteLine("Invalid goal number!");
            }
            Console.WriteLine("\nPress Enter to return to the menu.");
            Console.ReadLine();
        }

        private static void SaveGoals()
        {
            using (StreamWriter writer = new StreamWriter("goals.txt"))
            {
                writer.WriteLine(totalScore);
                foreach (var goal in goals)
                {
                    writer.WriteLine(goal.SaveData());
                }
            }
            Console.WriteLine("Goals saved! Press Enter to continue.");
            Console.ReadLine();
        }

        private static void LoadGoals()
        {
            if (File.Exists("goals.txt"))
            {
                string[] lines = File.ReadAllLines("goals.txt");
                totalScore = int.Parse(lines[0]);

                for (int i = 1; i < lines.Length; i++)
                {
                    goals.Add(Goal.LoadData(lines[i]));
                }
            }
        }
    }
}
