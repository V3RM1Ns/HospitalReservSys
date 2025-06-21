using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using HospitalManagmentSystem.Enums;
using HospitalManagmentSystem.Models;
using HospitalManagmentSystem.Interfaces;

namespace HospitalManagmentSystem
{
    
    class Program
    {
        static List<User> users;
        static List<Department> departments;
        static User _currentUserService = null;
        

        static void Main(string[] args)
        {
            try
            {
                (users, departments) = DataStorage.LoadData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading data: {ex.Message}");
                users = new List<User>();
                departments = new List<Department>();
            }

            while (true)
            {
                while (_currentUserService == null)
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
                        case "1": Login(); break;
                        case "2": Register(); break;
                        case "3": return;
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
                    Console.WriteLine($"👤 Logged in : {_currentUserService.name} as 🧩 ({_currentUserService.role})");
                    Console.WriteLine("=== Hospital Reservation System ===");
                    Console.WriteLine("1. Make a new reservation");
                    Console.WriteLine("2. My existing reservations");
                    Console.WriteLine("3. Cancel or modify a reservation");
                    Console.WriteLine("4. Admin Panel");
                    Console.WriteLine("5. Doctor Panel");
                    Console.WriteLine("6. Logout");
                    Console.Write("Please enter your choice (1-6): ");

                    switch (Console.ReadLine())
                    {
                        case "1": MakeNewReservation(); break;
                        case "2": ((IReservationService)_currentUserService).ListReservationsForPatient(_currentUserService); Console.ReadKey(); break;
                        case "3": CancelOrModifyReservation(); break;
                        case "4": AdminPanel(); break;
                        case "5": DoctorPanel(); break;
                        case "6": _currentUserService = null; exit = true; break;
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

            _currentUserService = users.FirstOrDefault(x => x.email == email && x.password == password);
            Console.WriteLine(_currentUserService != null ? "✅ Login successful." : "❌ Invalid credentials.");
            Console.ReadKey();
        }

        static void Register()
        {
            Console.Clear();
            Console.WriteLine("📝 === Register ===");
            Console.Write("Enter Your Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Phone: ");
            string phone = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            if (users.Any(u => u.email == email))
            {
                Console.WriteLine("⚠️ Email already exists.");
                Console.ReadKey(); return;
            }

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            users.Add(new User { name = name, phoneNumber = phone, email = email, password = password, role = RolePanel.Patient });
            DataStorage.SaveData(users, departments);
            Console.WriteLine("✅ Registration complete.");
            Console.ReadKey();
        }

        static void MakeNewReservation()
        {
            Console.Clear();
            Console.WriteLine("📅 === New Reservation ===");
            if (departments.Count == 0) { Console.WriteLine("⚠️ No departments available."); return; }

            for (int i = 0; i < departments.Count; i++)
                Console.WriteLine($"{i + 1}. {departments[i].name}");

            Console.Write("Select Department: ");
            int deptIndex = int.Parse(Console.ReadLine()) - 1;
            Department dept = departments[deptIndex];

            for (int i = 0; i < dept.doctors.Count; i++)
                Console.WriteLine($"{i + 1}. {dept.doctors[i].name} - {dept.doctors[i].experience} years exp");

            Console.Write("Select Doctor: ");
            int docIndex = int.Parse(Console.ReadLine()) - 1;
            Doctor doctor = dept.doctors[docIndex];

            doctor.ShowCurrentReservations();
            Console.Write("Enter index of desired time: ");
            int timeIndex = int.Parse(Console.ReadLine()) - 1;

            ((IReservationService)_currentUserService).MakeReservation(_currentUserService, doctor, doctor.reservations[timeIndex].ReservationDate);
            DataStorage.SaveData(users, departments);
        }

        static void CancelOrModifyReservation()
        {
            Console.Write("Enter reservation time to cancel (yyyy-MM-dd HH:mm): ");
            if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime time))
            {
                ((IReservationService)_currentUserService).CancelReservation(_currentUserService, time);
                DataStorage.SaveData(users, departments);
            }
            else
            {
                Console.WriteLine("❌ Invalid date format.");
            }
        }

        static void AdminPanel()
        {
            try
            {
                Console.Clear();
                if (_currentUserService.role != RolePanel.Admin)
                {
                    Console.WriteLine("Only Admin can access this.");
                    Console.ReadKey();
                    return;
                }

                bool exit = false;
                while (!exit)
                {
                    Console.Clear();
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in Admin Panel: {ex.Message}");
                Console.ReadKey();
            }
        }

        static void DoctorPanel()
        {
            if (_currentUserService is Doctor doc)
            {
                bool exit = false;
                while (!exit)
                {
                    Console.Clear();
                    Console.WriteLine("=== Doctor Panel ===");
                    Console.WriteLine("1. Show Reservations");
                    Console.WriteLine("2. Accept Reservation");
                    Console.WriteLine("3. Manage Work Time");
                    Console.WriteLine("4. Back");

                    switch (Console.ReadLine())
                    {
                        case "1": doc.ShowCurrentReservations(); Console.ReadKey(); break;
                        case "2": doc.AcceptReservation(users, departments); Console.ReadKey(); break;
                        case "3": doc.ManageWorkTime(); DataStorage.SaveData(users, departments); break;
                        case "4": exit = true; break;
                        default:
                            Console.WriteLine("❌ Invalid choice.");
                            Console.ReadKey();
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Only doctors can access the department.");
                Thread.Sleep(2500);
            }
        }
        public static List<User> GetUsers()
        {
            return users;
        }

        public static List<Department> GetDepartments()
        {
            return departments;
        }

    }
}