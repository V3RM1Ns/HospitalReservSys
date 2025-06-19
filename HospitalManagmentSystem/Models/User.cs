namespace HospitalManagmentSystem.Models;

public class User
{
    public string name { get; set; }
    public string surname { get; set; }
    public string email { get; set; }
    public string phoneNumber { get; set; }
    public RolePanel role { get; set; }
}