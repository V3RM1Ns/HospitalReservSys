using System;
using System.Collections.Generic;
using System.Globalization;
using HospitalManagmentSystem.Enums;
using HospitalManagmentSystem.Interfaces;
namespace HospitalManagmentSystem.Models;

    public class Doctor : User, IDoctorService
    {
        public int experience { get; set; }
        public List<ReservationResult<DateTime, bool>> reservations { get; set; } = new();

        public Doctor()
        {
            role = RolePanel.Doctor;
        }

        public void ShowPatientDetails(User user)
        {
            user.ShowProfile();
        }

        public void ShowCurrentReservations()
        {
            if (reservations == null || reservations.Count == 0)
            {
                Console.WriteLine("⛔ No reservations yet.");
                return;
            }

            int i = 1;
            foreach (var reservation in reservations)
            {
                string status = reservation.IsApproved ? "✅ Accepted" : "⏳ Pending";
                Console.WriteLine($"{i++}. Date: {reservation.ReservationDate:dd.MM.yyyy HH:mm} - Status: {status}");
            }
        }

        public void ManageWorkTime()
        {
            while (true)
            {
                Console.Write("🕒 Enter work date and time (format: yyyy-MM-dd HH:mm) or type 'B' to go back: ");
                string input = Console.ReadLine();

                if (input.Trim().ToUpper() == "B")
                    break;

                try
                {
                    DateTime workTime = DateTime.ParseExact(input, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                    reservations.Add(new ReservationResult<DateTime, bool>
                    {
                        ReservationDate = workTime,
                        IsApproved = false
                    });

                    Console.WriteLine("✅ Work time added successfully.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("❌ Invalid format. Please use exactly: yyyy-MM-dd HH:mm");
                }

                Console.Write("➕ Add another time? (Y/N): ");
                string again = Console.ReadLine();
                if (again.Trim().ToUpper() != "Y")
                    break;
            }
        }

        public void AcceptReservation(List<User> users, List<Department> departments)
        {
            var accepted = reservations.FirstOrDefault(r => !r.IsApproved && r.RequestedBy != null);
            if (accepted != null)
            {
                accepted.IsApproved = true;
                Console.WriteLine("✅ Reservation approved.");
                DataStorage.SaveData(users, departments);
            }
            else
            {
                Console.WriteLine("⛔ No valid pending requests to accept.");
            }
        }

        public void RespondToRequests(List<User> users, List<Department> departments)
        {
            var pendingRequests = reservations
                .Where(r => !r.IsApproved && r.RequestedBy != null)
                .ToList();

            if (pendingRequests.Count == 0)
            {
                Console.WriteLine("⛔ No pending reservation requests.");
                return;
            }

            Console.WriteLine("📥 Pending reservation requests:");
            for (int i = 0; i < pendingRequests.Count; i++)
            {
                var r = pendingRequests[i];
                string patient = r.RequestedBy?.name ?? "Unknown";
                Console.WriteLine($"{i + 1}. Time: {r.ReservationDate:dd.MM.yyyy HH:mm} | Patient: {patient}");
            }

            Console.Write("Enter reservation number to accept or 0 to cancel: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= pendingRequests.Count)
            {
                pendingRequests[choice - 1].IsApproved = true;
                Console.WriteLine("✅ Reservation approved.");
                DataStorage.SaveData(users, departments);
            }
            else if (choice != 0)
            {
                Console.WriteLine("❌ Invalid selection.");
            }
        }
    }

