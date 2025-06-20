using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace HospitalManagmentSystem
{
    class Program
    {
        static List<User> users;
        static List<Department> departments;
        static User currentUser = null;

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
                    Console.WriteLine("5. Doctor Panel");
                    Console.WriteLine("6. Logout");
                    Console.Write("Please enter your choice (1-6): ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            MakeNewReservation();
                            break;
                        case "2":

                            break;
                        case "3":

                            break;
                        case "4":
                            AdminPanel();
                            break;
                        case "5":
                            DoctorPanel();
                            break;
                        case "6":
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
            try
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error during login: {ex.Message}");
            }

            Console.ReadKey();
        }

        static void Register()
        {
            try
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

                try
                {
                    DataStorage.SaveData(users, departments);
                    Console.WriteLine("✅ Registration successful! You can now login. Press any key...");
                }
                catch (Exception saveEx)
                {
                    Console.WriteLine($"❌ Error saving data: {saveEx.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error during registration: {ex.Message}");
            }

            Console.ReadKey();
        }

        static void MakeNewReservation()
        {
            try
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
                Console.Write("Please select a Doctor.");
                int z = 1;
                foreach (var Doctor in selectedDept.doctors)
                {
                    Console.WriteLine($"\n{z}. Name:{Doctor.name}, Experience:{Doctor.experience} years.");
                }

                Doctor selectedDoctor;
                int k = 0;
                while (true)
                {
                    Console.Write("Enter choice: ");
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out k) && k >= 1 && k <= selectedDept.doctors.Count)
                    {
                        selectedDoctor = selectedDept.doctors[k - 1];
                        break;
                    }


                    Console.WriteLine("❌ Invalid input. Please enter a number from the list.");
                }

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"Doctor {selectedDoctor.name}'s Work times are:");

                    if (selectedDoctor.reservations == null || selectedDoctor.reservations.Count == 0)
                    {
                        Console.WriteLine("⛔ No reservations yet.");
                    }
                    else
                    {
                        int i = 1;
                        foreach (var res in selectedDoctor.reservations)
                        {
                            string status = res.IsApproved ? "✅ Approved" : "⏳ Pending";
                            Console.WriteLine($"{i++}. {res.ReservationDate:dd.MM.yyyy HH:mm} — {status}");
                        }
                    }

                    Console.WriteLine("\nPress any key to continue...");
                    string input = Console.ReadLine();
                    break;
                }


                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error making reservation: {ex.Message}");
                Console.ReadKey();
            }
        }

        static void AdminPanel()
        {
            try
            {
                Console.Clear();
                if (currentUser.role != RolePanel.Admin)
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

        static void AddDepartment()
        {
            try
            {
                Console.Clear();
                Console.Write("Enter the department name: ");
                string departmentName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(departmentName))
                {
                    Console.WriteLine("⚠️ Department name cannot be empty. Press any key...");
                    Console.ReadKey();
                    return;
                }

                if (departments.Exists(d => d.name.Equals(departmentName, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("⚠️ Department already exists. Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                departments.Add(new Department { name = departmentName });
                DataStorage.SaveData(users, departments);
                Console.WriteLine("✅ Department added successfully. Press any key...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error adding department: {ex.Message}");
                Console.ReadKey();
            }
        }

        static void AddDoctor()
        {
            try
            {
                if (departments == null || departments.Count == 0)
                {
                    Console.WriteLine("No departments yet. Please be patient.");
                    Thread.Sleep(3000);
                    return;
                }

                Console.Clear();
                Console.WriteLine("🏥 Select doctor's department:");
                for (int i = 0; i < departments.Count; i++)
                    Console.WriteLine($"{i + 1}. {departments[i].name}");

                int deptChoice;
                while (true)
                {
                    Console.Write("Enter choice: ");
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out deptChoice) && deptChoice >= 1 && deptChoice <= departments.Count)
                        break;

                    Console.WriteLine("❌ Invalid input. Please enter a number from the list.");
                }

                Department selectedDept = departments[deptChoice - 1];
                Console.WriteLine($"\n✅ Department selected: {selectedDept.name}");

                Console.Write("Enter the doctor name: ");
                string doctorName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(doctorName))
                {
                    Console.WriteLine("⚠️ Doctor name cannot be empty. Press any key...");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Enter doctor's email: ");
                string email = Console.ReadLine();
                if (users.Any(u => u.email == email))
                {
                    Console.WriteLine("⚠️ Email already exists. Press any key...");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Enter password for doctor: ");
                string password = Console.ReadLine();

                Console.Write("Enter doctor's phone number: ");
                string phone = Console.ReadLine();

                Console.Write("Enter doctor's experience (in years): ");
                if (!int.TryParse(Console.ReadLine(), out int doctorExperience))
                {
                    Console.WriteLine("❌ Invalid experience input. Must be a number. Press any key...");
                    Console.ReadKey();
                    return;
                }

                Doctor doctor = new Doctor
                {
                    name = doctorName,
                    email = email,
                    password = password,
                    phoneNumber = phone,
                    experience = doctorExperience
                };

                if (selectedDept.doctors == null)
                    selectedDept.doctors = new List<Doctor>();

                selectedDept.doctors.Add(doctor);

                users.Add(doctor);

                DataStorage.SaveData(users, departments);

                Console.WriteLine("✅ Doctor added successfully. Press any key...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error adding doctor: {ex.Message}");
                Console.ReadKey();
            }
        }














        static void DoctorPanel()
        {
            if (currentUser.role == RolePanel.Patient)
            {
                Console.WriteLine("You can not access this panel.");
                Console.ReadKey();
                return;
            }

            if (currentUser.role == RolePanel.Doctor)
            {
                bool exit = false;
                while (!exit)
                {
                    Console.Clear();
                    Console.WriteLine("=== Hospital Doctor System ===");
                    Console.WriteLine("1. Show current Reservations");
                    Console.WriteLine("2. Show patient details");
                    Console.WriteLine("3. Reservation Acceptance");
                    Console.WriteLine("4. Manage work time");
                    Console.WriteLine("5. Logout");
                    Console.Write("Please enter your choice (1-5): ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            ShowReservs();
                            break;
                        case "2":

                            break;
                        case "3":
                            Accept();
                            break;

                        case "4":
                            Managework();
                            break;
                        case "5":
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
        }

        public static void Accept()
        {
            Doctor currentDoctor = null;

            if (currentUser is Doctor doc)
            {
                currentDoctor = doc;
            }
            else
            {
                currentDoctor = departments
                    .SelectMany(d => d.doctors)
                    .FirstOrDefault(d => d.name.Equals(currentUser.name, StringComparison.OrdinalIgnoreCase));
            }

            if (currentDoctor == null)
            {
                Console.WriteLine("⛔ Doctor not found.");
                Console.ReadKey();
                return;
            }

            currentDoctor.AcceptReservation(users, departments);
            Console.WriteLine("✅ Reservation updated successfully.");
            Console.ReadKey();
        }



        public static void Managework()
        {
            Doctor currentDoctor =
                departments.SelectMany(d => d.doctors).FirstOrDefault(d => d.name == currentUser.name);
            currentDoctor.ManageWork();
            DataStorage.SaveData(users, departments);
        }

        public static void ShowReservs()
        {

            if (currentUser is Doctor doctor)
            {
                doctor.ShowReservations();
            }
            else
            {
                Doctor currentDoctor = departments.SelectMany(d => d.doctors)
                    .FirstOrDefault(d => d.name.ToLower() == currentUser.name.ToLower());

                if (currentDoctor == null)
                {
                    Console.WriteLine("⛔ Doctor not found.");
                }
                else
                {
                    currentDoctor.ShowReservations();
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

    }
}
