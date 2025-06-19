namespace HospitalManagmentSystem.Models;

public class TimeSlot
{
    public DateTime startTime { get; set; }
    public DateTime endTime { get; set; }
    public bool isReserved = false;
    
    public TimeSlot(DateTime start, DateTime end)
    {
        startTime = start;
        endTime = end;
        isReserved = false;
    }
}