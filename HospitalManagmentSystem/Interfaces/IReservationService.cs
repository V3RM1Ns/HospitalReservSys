namespace HospitalManagmentSystem.Interfaces;

public interface IReservationService
{
    void MakeReservation(User patient, Doctor doctor, DateTime date);
    void CancelReservation(User patient, DateTime date);
    void ListReservationsForDoctor(Doctor doctor);
    void ListReservationsForPatient(User patient);
}