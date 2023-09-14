namespace Vitality.Models
{
    public class adminDashboard
    {
        public List<DoctorsAppointment> DoctorsAppointments { get; set; }
        public List<PatientsRegistration> PatientsRegistrations { get; set; }
        public List<DoctorsRegistration> DoctorsRegistration { get; set;}
        public List<Feedback> feedbacks { get; set; }
        public int PatientsRegistrationsCount { get; set; }
        public int DoctorsAppointmentsCount { get; set; }
        public int DoctorsRegistrationCount { get; set; }
        public int FeedbacksCount { get; set; }

    }
}
