namespace HospitalManagmentSystem;

class Program
{
    static void Main(string[] args)
    {
        #region Hekim Yarat
        List<Department> departments = new List<Department>();
        var pediatriya = new Department { Name = "Pediatriya" };
        for (int i = 1; i <= 3; i++)
        {
            var doctor = new Doctor
            {
                name = $"Pediatr{i}",
                surname = $"Həkim{i}",
                experience = 5 + i
            };
            AddTimeSlotsToDoctor(doctor);
            pediatriya.Doctors.Add(doctor);
        }
        var travmatologiya = new Department { Name = "Travmatologiya" };
        for (int i = 1; i <= 2; i++)
        {
            var doctor = new Doctor
            {
                name = $"Travma{i}",
                surname = $"Həkim{i}",
                experience = 7 + i
            };
            AddTimeSlotsToDoctor(doctor);
            travmatologiya.Doctors.Add(doctor);
        }
        
        var stomatologiya = new Department { Name = "Stomatologiya" };
        for (int i = 1; i <= 4; i++)
        {
            var doctor = new Doctor
            {
                name = $"Stomatolog{i}",
                surname = $"Həkim{i}",
                experience = 3 + i
            };
            AddTimeSlotsToDoctor(doctor);
            stomatologiya.Doctors.Add(doctor);
        }

        departments.Add(pediatriya);
        departments.Add(travmatologiya);
        departments.Add(stomatologiya);
        #endregion
    }
    
   static  void AddTimeSlotsToDoctor(Doctor doctor)
    {
        for (int day = 0; day < 7; day++)
        {
            var date = DateTime.Today.AddDays(day);
            var slots = new List<TimeSlot>
            {
                new TimeSlot(date.AddHours(9), date.AddHours(11)),
                new TimeSlot(date.AddHours(12), date.AddHours(14)),
                new TimeSlot(date.AddHours(15), date.AddHours(17))
            };
            doctor.appointmentSlots[date] = slots;
        }
    }

}