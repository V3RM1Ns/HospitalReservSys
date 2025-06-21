namespace HospitalManagmentSystem.Models
{
    public class ReservationResult<TTime, TStatus>
    {
        public TTime ReservationDate { get; set; }
        public TStatus IsApproved { get; set; }
        public User RequestedBy { get; set; } 
    }
}