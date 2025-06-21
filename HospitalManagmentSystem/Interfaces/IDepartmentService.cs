namespace HospitalManagmentSystem.Interfaces;

public interface IDepartmentService
{
    void AddDepartment(string name);
    void ListDepartments();
    Department GetDepartmentByName(string name);
}
