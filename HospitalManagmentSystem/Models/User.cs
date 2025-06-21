using HospitalManagmentSystem.Interfaces;

namespace HospitalManagmentSystem.Models;

public class User : IUserService,IReservationService
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
        
    }

    public void EditReservation(User patient, DateTime date)
    {
        
    }

    public void ListReservationsForDoctor(Doctor doctor)
    {
        throw new NotImplementedException();
    }

    public void ListReservationsForPatient(User patient)
    {
        throw new NotImplementedException();
    }

    public void MakeReservation(User patient, Doctor doctor, DateTime date)
    {
        throw new NotImplementedException();
    }
}