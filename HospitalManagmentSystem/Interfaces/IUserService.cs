namespace HospitalManagmentSystem.Interfaces;

public interface IUserService
{
    string name { get; set; }
    string email { get; set; }
    string phoneNumber { get; set; }
    string password { get; set; }
    RolePanel role { get; set; }
    void ShowProfile();
}