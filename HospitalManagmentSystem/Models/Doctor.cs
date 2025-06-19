namespace HospitalManagmentSystem.Models;

public class Doctor
{
    public string name { get; set; }
    public string surname { get; set; }
    public int experience { get; set; }
    public Dictionary<DateTime,List<TimeSlot>> appointmentSlots { get; set; }
    
}