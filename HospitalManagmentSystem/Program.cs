using System;
using System.Collections.Generic;
using System.Threading;

namespace HospitalManagmentSystem
{
    class Program
    {
        static List<User> users = new();
        static User currentUser = null;
        static List<Department> departments = new();

        static void Main(string[] args)
        {
            while (true)
            {
                while (currentUser == null)
                {
                    Console.Clear();
                    Console.WriteLine("=== Welcome to Hospital Reservation System ===");
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Register");
                    Console.WriteLine("3. Exit");
                    Console.Write("Please enter your choice (1-3): ");

                    string authChoice = Console.ReadLine();
                    switch (authChoice)
                    {
                        case "1":
                            Login();
                            break;
                        case "2":
                            Register();
                            break;
                        case "3":
                            Console.WriteLine("Exiting...");
                            return;
                        default:
                            Console.WriteLine("❌ Invalid choice. Press any key to try again.");
                            Console.ReadKey();
                            break;
                    }
                }

                bool exit = false;
                while (!exit)
                {
                    Console.Clear();
                    Console.WriteLine($"👤 Logged in : {currentUser.name} as 🧩 ({currentUser.role})");
                    Console.WriteLine("=== Hospital Reservation System ===");
                    Console.WriteLine("1. Make a new reservation");
                    Console.WriteLine("2. My existing reservations");
                    Console.WriteLine("3. Cancel or modify a reservation");
                    Console.WriteLine("4. Admin Panel");
                    Console.WriteLine("5. Logout");
                    Console.Write("Please enter your choice (1-5): ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            MakeNewReservation();
                            break;
                        case "2":
                            MyExistingReservations();
                            break;
                        case "3":
                            CancelOrModifyReservation();
                            break;
                        case "4":
                            AdminPanel();
                            break;
                        case "5":
                            Console.WriteLine("🔓 Logging out... Press any key to continue.");
                            Console.ReadKey();
                            currentUser = null;
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("❌ Invalid choice. Press any key to try again.");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }

        static void Login()
        {
            Console.Clear();
            Console.WriteLine("🔐 === Login ===");
            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            var foundUser = users.Find(x => x.email == email && x.password == password);
            if (foundUser != null)
            {
                currentUser = foundUser;
                Console.WriteLine("✅ Successfully logged in. Press any key to continue...");
            }
            else
            {
                Console.WriteLine("❌ Invalid credentials. Press any key to try again...");
            }

            Console.ReadKey();
        }

        static void Register()
        {
            Console.Clear();
            Console.WriteLine("📝 === Register ===");

            Console.Write("Enter Your Name: ");
            string username = Console.ReadLine();

            Console.Write("Enter your Phone: ");
            string phone = Console.ReadLine();

            Console.Write("Enter your Email: ");
            string email = Console.ReadLine();

            if (users.Exists(x => x.email == email))
            {
                Console.WriteLine("⚠️ Email already exists. Press any key...");
                Console.ReadKey();
                return;
            }

            Console.Write("Choose a password: ");
            string password = Console.ReadLine();

            User newUser = new User
            {
                name = username,
                email = email,
                phoneNumber = phone,
                password = password,
                role = RolePanel.Patient
            };

            users.Add(newUser);
            Console.WriteLine("✅ Registration successful! You can now login. Press any key...");
            Console.ReadKey();
        }

        static void MakeNewReservation()
        {
            Console.Clear();
            Console.WriteLine("📅 === New Reservation ===");

            if (departments.Count == 0)
            {
                Console.WriteLine("⚠️ No departments yet. Please be patient.");
                Thread.Sleep(3000);
                return;
            }

            Console.WriteLine("🏥 Select a department:");
            for (int i = 0; i < departments.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {departments[i].name}");
            }

            int deptChoice = 0;
            while (true)
            {
                Console.Write("Enter choice: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out deptChoice) && deptChoice >= 1 && deptChoice <= departments.Count)
                {
                    break;
                }
                Console.WriteLine("❌ Invalid input. Please enter a number from the list.");
            }

            Department selectedDept = departments[deptChoice - 1];
            Console.WriteLine($"\n✅ Department selected: {selectedDept.name}");
            Console.WriteLine("🔄 Press any key to continue...");
            Console.ReadKey();
        }

        static void MyExistingReservations()
        {
            Console.Clear();
            Console.WriteLine("📋 === Your Reservations ===");
            Console.ReadKey();
        }

        static void CancelOrModifyReservation()
        {
            Console.Clear();
            Console.WriteLine("🗑️ === Cancel or Modify Reservation ===");
            Console.WriteLine("🔧 Feature under development...");
            Console.ReadKey();
        }

        static void AdminPanel()
        {
            Console.Clear();
            if (currentUser.role != RolePanel.Patient)
            {
                Console.WriteLine("Only Admin can access this.");
                return;
            }

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine($"👤 Logged in : {currentUser.name}  as 🧩 ({currentUser.role})");
                Console.WriteLine("🛠️ === Admin Panel ===");
                Console.WriteLine("1. Make a new Department");
                Console.WriteLine("2. Add Doctor");
                Console.WriteLine("3. Back");
                Console.Write("Please enter your choice (1-3): ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddDepartment();
                        break;
                    case "2":
                        AddDoctor();
                        break;
                    case "3":
                        Console.WriteLine("🔙 Returning... Press any key to continue.");
                        Console.ReadKey();
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("❌ Invalid choice. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }

                Console.ReadKey();
            }

            static void AddDepartment()
            {
                Console.Clear();
                Console.Write("Enter the department name: ");
                string departmentName = Console.ReadLine();

                if (departments.Exists(d => d.name.Equals(departmentName, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("⚠️ Department already exists. Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                departments.Add(new Department { name = departmentName });
                Console.WriteLine("✅ Department added successfully. Press any key...");
                Console.ReadKey();
            }

            static void AddDoctor()
            {
                if (departments.Count == 0)
                {
                    Console.WriteLine("No departments yet. Please be patient.");
                    Thread.Sleep(3000);
                    return;
                }
                Console.Clear();
                Console.WriteLine("🏥 Select doctor's department:");
                for (int i = 0; i < departments.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {departments[i].name}");
                }

                int deptChoice = 0;
                while (true)
                {
                    Console.Write("Enter choice: ");
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out deptChoice) && deptChoice >= 1 && deptChoice <= departments.Count)
                    {
                        break;
                    }
                    Console.WriteLine("❌ Invalid input. Please enter a number from the list.");
                }

                Department selectedDept = departments[deptChoice - 1];
                Console.WriteLine($"\n✅ Department selected: {selectedDept.name}");
                
                
                Console.WriteLine("Enter the doctor name: ");
                string doctorName = Console.ReadLine();
                Console.WriteLine("Enter doctor's experience: ");
                int doctorExperience = int.Parse(Console.ReadLine());
                
                Doctor doctor = new Doctor { name = doctorName, experience = doctorExperience };
                
            }
        }
    }
    
}
