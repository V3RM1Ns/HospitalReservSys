namespace HospitalManagmentSystem.Models;

public class Department
{
    public string name { get; set; }
    public List<Doctor> doctors { get; set; } = new List<Doctor>();
}