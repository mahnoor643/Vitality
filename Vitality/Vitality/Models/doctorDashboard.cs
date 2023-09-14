namespace Vitality.Models
{
    public class doctorDashboard
    {
        public List<DoctorsAppointment> DoctorsAppointments  { get; set; }
        public int PatientsRegistrationsCount { get; set; }
        public int DoctorsAppointmentsCount { get; set; }
        public int TodaysAppointment { get;  set; }
    }
}
