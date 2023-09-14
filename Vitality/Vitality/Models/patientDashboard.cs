namespace Vitality.Models
{
    public class patientDashboard
    {
        public List<Feedback> Feedback { get; set; }
        public List<PatientsIdcard> PatientsCard { get; set; }
        public List<PatientsRegistration> PatientsRegistration { get; set;}
        public List<DoctorsAppointment> DoctorsAppointment { get; set;}
    }
}
