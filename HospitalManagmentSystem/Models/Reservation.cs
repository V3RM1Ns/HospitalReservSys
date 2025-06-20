namespace HospitalManagmentSystem.Models;

public class Reservation
{
    public Guid id { get; set; }
    public User patient{get;set;}
    public Doctor doctor{get;set;}
    public DateTime date{get;set;}
    public TimeSlot timeSlot{get;set;}

    public Reservation()
    {
        id=Guid.NewGuid();
        patient=new User();
        doctor=new Doctor();
        date=DateTime.Now;
        
    }
}