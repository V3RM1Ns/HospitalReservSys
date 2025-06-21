namespace HospitalManagmentSystem.Interfaces;

public interface IDoctorService
{
    void ShowCurrentReservations();
    void ManageWorkTime();
    void AcceptReservation(List<User> users, List<Department> departments);
    void ShowPatientDetails(User user);
}
