using HospitalManagmentSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagmentSystem.Models
{
    public class User : IUserService, IReservationService
    {
        public string email { get; set; }
        public string password { get; set; }
        public RolePanel role { get; set; }
        public string name { get; set; }
        public string phoneNumber { get; set; }

        public void ShowProfile()
        {
            Console.WriteLine($"Name: {name}, Phone: {phoneNumber}, Email: {email}");
        }

        public void CancelReservation(User patient, DateTime date)
        {
            Doctor doctor = Program.GetDepartments()
                .SelectMany(d => d.doctors)
                .FirstOrDefault(doc => doc.reservations.Any(r => r.ReservationDate == date && r.IsApproved));

            if (doctor != null)
            {
                var reservation = doctor.reservations.First(r => r.ReservationDate == date);
                reservation.IsApproved = false;
                reservation.RequestedBy = null;

                Console.WriteLine($"❗ Reservation for {date:dd.MM.yyyy HH:mm} has been canceled.");
                Console.WriteLine($"📢 Doctor {doctor.name} notified: Slot reverted to ⏳ Pending");
                DataStorage.SaveData(Program.GetUsers(), Program.GetDepartments());
            }
            else
            {
                Console.WriteLine("❌ Reservation not found.");
            }
        }

        public void EditReservation(User patient, DateTime date)
        {
            Console.WriteLine("🛠️ EditReservation not implemented.");
        }

        public void ListReservationsForDoctor(Doctor doctor)
        {
            Console.WriteLine($"\n📋 Reservations with Doctor {doctor.name}:");
            foreach (var r in doctor.reservations)
            {
                if (r.IsApproved)
                {
                    Console.WriteLine($"✅ {r.ReservationDate:dd.MM.yyyy HH:mm}");
                }
            }
        }

        public void ListReservationsForPatient(User patient)
        {
            Console.WriteLine("🛠️ ListReservationsForPatient not implemented.");
        }

        public void MakeReservation(User patient, Doctor doctor, DateTime date)
        {
            var targetReservation = doctor.reservations
                .FirstOrDefault(r => r.ReservationDate == date && !r.IsApproved);

            if (targetReservation == null)
            {
                Console.WriteLine("❌ Selected time is either invalid or already reserved.");
                Thread.Sleep(2000);
                return;
            }

            targetReservation.RequestedBy = patient;
            Console.WriteLine($"📩 Reservation request sent for {date:dd.MM.yyyy HH:mm} to Dr. {doctor.name}.");
            DataStorage.SaveData(Program.GetUsers(), Program.GetDepartments());
        }
    }
}
