using System;
using System.Collections.Generic;
using System.Globalization;

namespace HospitalManagmentSystem.Models
{
    public class Doctor
    {
        public string name { get; set; }
        public int experience { get; set; }
        public List<DateTime> reservationTimes { get; set; }
        public RolePanel role { get; set; }
        public List<ReservationResult<DateTime, bool>> reservations { get; set; } = new();

        public Doctor()
        {
            role = RolePanel.Doctor;
        }

        public void ShowReservations()
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

        public void ManageWork()
        {
            while (true)
            {
                Console.Write("🕒 Enter work date and time (format: yyyy-MM-dd HH:mm) or type 'B' to go back: ");
                string input = Console.ReadLine();

                if (input.ToUpper() == "B")
                    break;

                try
                {
                    DateTime workTime = DateTime.ParseExact(input, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                    if (reservations == null)
                    {
                        reservations = new List<ReservationResult<DateTime, bool>>();
                    }

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
                if (again.ToUpper() != "Y")
                    break;
            }

            try
            {
                Console.WriteLine("💾 Work time saved.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error saving work time: {ex.Message}");
            }

            Console.WriteLine("🔙 Press any key to return...");
            Console.ReadKey();
        }
    }

    public class ReservationResult<TTime, TStatus>
    {
        public TTime ReservationDate { get; set; }
        public TStatus IsApproved { get; set; }
    }
}
