namespace HospitalManagmentSystem.Models;

public class Doctor
{
    public Department department { get; set; }
    public string name { get; set; }
    public int experience { get; set; }
    public Dictionary<DateTime,List<TimeSlot>> appointmentSlots { get; set; }
    
}