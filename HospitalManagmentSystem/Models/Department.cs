namespace HospitalManagmentSystem.Models;

public class Department
{
    public string Name { get; set; }
    public List<Doctor> Doctors { get; set; }
}