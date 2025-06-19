namespace HospitalManagmentSystem.Models;

public class Reservation
{
    public Guid id { get; set; }
    public User patient{get;set;}
    public Doctor doctor{get;set;}
    public DateTime date{get;set;}
    public TimeSlot timeSlot{get;set;}
}