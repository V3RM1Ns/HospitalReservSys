namespace HospitalManagmentSystem.Interfaces
{
    public interface IDoctorService : IUserService
    {
        void ShowCurrentReservations();
        void ManageWorkTime();
        void AcceptReservation(List<User> users, List<Department> departments);
        void RespondToRequests(List<User> users, List<Department> departments);
        void ShowPatientDetails(User user);
    }
}