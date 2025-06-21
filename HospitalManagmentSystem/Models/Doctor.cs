using System;
using System.Collections.Generic;
using System.Globalization;
using HospitalManagmentSystem.Enums;
using HospitalManagmentSystem.Interfaces;

public class Doctor : User,IDoctorService
{
    public int experience { get; set; }
    public List<ReservationResult<DateTime, bool>> reservations { get; set; } = new();
    
    
    public void ShowProfile()
    {
        Console.WriteLine($"👨‍⚕️ Name: {name}\n📧 Email: {email}\n📞 Phone: {phoneNumber}\n💼 Experience: {experience} years");
    }

    public void ShowPatientDetails(User user)
    {
        user.ShowProfile();
    }

    public void AcceptReservation(List<User> users, List<Department> departments)
    {
        if (reservations.Count == 0)
        {
            Console.WriteLine("⚠️ No reservations to accept.");
            return;
        }

        Console.WriteLine("🏥 Select the reservation to accept:");
        for (int i = 0; i < reservations.Count; i++)
        {
            var r = reservations[i];
            string status = r.IsApproved ? "✅ Accepted" : "⏳ Pending";
            Console.WriteLine($"{i + 1}. Time: {r.ReservationDate:dd.MM.yyyy HH:mm} - Status: {status}");
        }

        int choice;
        while (true)
        {
            Console.Write("Enter choice: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out choice) && choice >= 1 && choice <= reservations.Count)
                break;

            Console.WriteLine("❌ Invalid input. Please enter a valid reservation number.");
        }

        reservations[choice - 1].IsApproved = true;

        DataStorage.SaveData(users, departments);
        Console.WriteLine("✅ Reservation accepted and saved to database.");
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


    public class ReservationResult<TTime, TStatus>
    {
        public TTime ReservationDate { get; set; }
        public TStatus IsApproved { get; set; }
    }
    public Doctor()
    {
        role = RolePanel.Doctor;
    }

    
}

