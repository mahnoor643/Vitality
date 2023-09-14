using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Vitality.Models
{
    public partial class VitalitydbContext : DbContext
    {
        public VitalitydbContext()
        {
        }

        public VitalitydbContext(DbContextOptions<VitalitydbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<AvailedBlood> AvailedBloods { get; set; } = null!;
        public virtual DbSet<Bed> Beds { get; set; } = null!;
        public virtual DbSet<BedAllotment> BedAllotments { get; set; } = null!;
        public virtual DbSet<BloodDonor> BloodDonors { get; set; } = null!;
        public virtual DbSet<BloodGroup> BloodGroups { get; set; } = null!;
        public virtual DbSet<ContactU> ContactUs { get; set; } = null!;
        public virtual DbSet<Degree> Degrees { get; set; } = null!;
        public virtual DbSet<DischargeDetail> DischargeDetails { get; set; } = null!;
        public virtual DbSet<DoctorCategory> DoctorCategories { get; set; } = null!;
        public virtual DbSet<DoctorSlotDay> DoctorSlotDays { get; set; } = null!;
        public virtual DbSet<DoctorSlotTime> DoctorSlotTimes { get; set; } = null!;
        public virtual DbSet<DoctorsAppointment> DoctorsAppointments { get; set; } = null!;
        public virtual DbSet<DoctorsRegistration> DoctorsRegistrations { get; set; } = null!;
        public virtual DbSet<EmergencyPatient> EmergencyPatients { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<HospitalBilling> HospitalBillings { get; set; } = null!;
        public virtual DbSet<HospitalPharmacyPrescription> HospitalPharmacyPrescriptions { get; set; } = null!;
        public virtual DbSet<LabTest> LabTests { get; set; } = null!;
        public virtual DbSet<LabTestAllocation> LabTestAllocations { get; set; } = null!;
        public virtual DbSet<Medicine> Medicines { get; set; } = null!;
        public virtual DbSet<MedicineCategory> MedicineCategories { get; set; } = null!;
        public virtual DbSet<MedicineOrder> MedicineOrders { get; set; } = null!;
        public virtual DbSet<MedicineOrderDetail> MedicineOrderDetails { get; set; } = null!;
        public virtual DbSet<MedicineSupplier> MedicineSuppliers { get; set; } = null!;
        public virtual DbSet<NurseRegistration> NurseRegistrations { get; set; } = null!;
        public virtual DbSet<OtdaySlot> OtdaySlots { get; set; } = null!;
        public virtual DbSet<Otregistration> Otregistrations { get; set; } = null!;
        public virtual DbSet<OttimeSlot> OttimeSlots { get; set; } = null!;
        public virtual DbSet<PatientPayment> PatientPayments { get; set; } = null!;
        public virtual DbSet<PatientRoom> PatientRooms { get; set; } = null!;
        public virtual DbSet<PatientsAllotedRoom> PatientsAllotedRooms { get; set; } = null!;
        public virtual DbSet<PatientsIdcard> PatientsIdcards { get; set; } = null!;
        public virtual DbSet<PatientsRegistration> PatientsRegistrations { get; set; } = null!;
        public virtual DbSet<PharmacyEmployee> PharmacyEmployees { get; set; } = null!;
        public virtual DbSet<Receptionist> Receptionists { get; set; } = null!;
        public virtual DbSet<Staff> Staffs { get; set; } = null!;
        public virtual DbSet<StaffSalary> StaffSalaries { get; set; } = null!;
        public virtual DbSet<StaffSalaryStatus> StaffSalaryStatuses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
/*#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
*/                optionsBuilder.UseSqlServer("Server=Mahnoor\\SQLEXPRESS01;Database=Vitalitydb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("admins");

                entity.Property(e => e.AdminId).HasColumnName("adminID");

                entity.Property(e => e.AdminName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("adminName");

                entity.Property(e => e.AdminPwd)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("adminPwd");

                entity.Property(e => e.AdminUsername)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("adminUsername");
            });

            modelBuilder.Entity<AvailedBlood>(entity =>
            {
                entity.HasKey(e => e.BloodRecieverId)
                    .HasName("PK__availedB__7F0FA8DEBD9F56F4");

                entity.ToTable("availedBloods");

                entity.Property(e => e.BloodRecieverId).HasColumnName("bloodRecieverID");

                entity.Property(e => e.BloodDonorsId).HasColumnName("bloodDonorsID");

                entity.Property(e => e.BloodStatus).HasDefaultValueSql("((0))");

                entity.Property(e => e.PatientsCardId).HasColumnName("patientsCardID");

                entity.HasOne(d => d.BloodDonors)
                    .WithMany(p => p.AvailedBloods)
                    .HasForeignKey(d => d.BloodDonorsId)
                    .HasConstraintName("FK__availedBl__blood__3A4CA8FD");

                entity.HasOne(d => d.PatientsCard)
                    .WithMany(p => p.AvailedBloods)
                    .HasForeignKey(d => d.PatientsCardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__availedBl__patie__18EBB532");
            });

            modelBuilder.Entity<Bed>(entity =>
            {
                entity.ToTable("beds");

                entity.Property(e => e.BedId).HasColumnName("bedID");

                entity.Property(e => e.Bed1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("bed");

                entity.Property(e => e.BedAmount).HasColumnName("bedAmount");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<BedAllotment>(entity =>
            {
                entity.ToTable("bedAllotments");

                entity.Property(e => e.BedAllotmentId).HasColumnName("bedAllotmentID");

                entity.Property(e => e.AllotTill)
                    .HasColumnType("date")
                    .HasColumnName("allotTill");

                entity.Property(e => e.BedsId).HasColumnName("bedsID");

                entity.Property(e => e.CurrentDateTime)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("currentDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Days).HasColumnName("days");

                entity.Property(e => e.PatientsCardNo).HasColumnName("patientsCardNo");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Beds)
                    .WithMany(p => p.BedAllotments)
                    .HasForeignKey(d => d.BedsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__bedAllotm__bedsI__19DFD96B");

                entity.HasOne(d => d.PatientsCardNoNavigation)
                    .WithMany(p => p.BedAllotments)
                    .HasForeignKey(d => d.PatientsCardNo)
                    .HasConstraintName("FK__bedAllotm__patie__1BC821DD");
            });

            modelBuilder.Entity<BloodDonor>(entity =>
            {
                entity.HasKey(e => e.BloodDonorsId)
                    .HasName("PK__bloodDon__CB87AD18DF650602");

                entity.ToTable("bloodDonors");

                entity.Property(e => e.BloodDonorsId).HasColumnName("bloodDonorsID");

                entity.Property(e => e.BloodDonorsEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("bloodDonorsEmail");

                entity.Property(e => e.BloodDonorsName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("bloodDonorsName");

                entity.Property(e => e.BloodDonorsPhoneNo).HasColumnName("bloodDonorsPhoneNo");

                entity.Property(e => e.BloodGroup).HasColumnName("bloodGroup");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.BloodGroupNavigation)
                    .WithMany(p => p.BloodDonors)
                    .HasForeignKey(d => d.BloodGroup)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__bloodDono__blood__1CBC4616");
            });

            modelBuilder.Entity<BloodGroup>(entity =>
            {
                entity.ToTable("bloodGroups");

                entity.Property(e => e.BloodGroupId).HasColumnName("bloodGroupID");

                entity.Property(e => e.BloodGroup1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("bloodGroup");
            });

            modelBuilder.Entity<ContactU>(entity =>
            {
                entity.HasKey(e => e.ContactId);

                entity.ToTable("contactUs");

                entity.Property(e => e.ContactId).HasColumnName("contactID");

                entity.Property(e => e.ContactMsg)
                    .HasColumnType("text")
                    .HasColumnName("contactMsg");

                entity.Property(e => e.ContactSubject)
                    .HasColumnType("text")
                    .HasColumnName("contactSubject");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("userEmail");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("userName");
            });

            modelBuilder.Entity<Degree>(entity =>
            {
                entity.ToTable("degrees");

                entity.Property(e => e.DegreeId).HasColumnName("degreeID");

                entity.Property(e => e.Degree1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("degree");
            });

            modelBuilder.Entity<DischargeDetail>(entity =>
            {
                entity.HasKey(e => e.DischargeId)
                    .HasName("PK__discharg__D2946C12FE538187");

                entity.ToTable("dischargeDetails");

                entity.Property(e => e.DischargeId).HasColumnName("dischargeID");

                entity.Property(e => e.DischargeFrom)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("dischargeFrom");

                entity.Property(e => e.DischargePatientsCardId).HasColumnName("dischargePatientsCardID");

                entity.HasOne(d => d.DischargePatientsCard)
                    .WithMany(p => p.DischargeDetails)
                    .HasForeignKey(d => d.DischargePatientsCardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__discharge__disch__1DB06A4F");
            });

            modelBuilder.Entity<DoctorCategory>(entity =>
            {
                entity.ToTable("doctorCategories");

                entity.Property(e => e.DoctorCategoryId).HasColumnName("doctorCategoryID");

                entity.Property(e => e.DoctorCategory1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("doctorCategory");
            });

            modelBuilder.Entity<DoctorSlotDay>(entity =>
            {
                entity.HasKey(e => e.DoctorSlotDaysId)
                    .HasName("PK__doctorSl__20D488B2C0699914");

                entity.ToTable("doctorSlotDays");

                entity.Property(e => e.DoctorSlotDaysId).HasColumnName("doctorSlotDaysID");

                entity.Property(e => e.DoctorSlotDays)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("doctorSlotDays");

                entity.Property(e => e.DoctorsId).HasColumnName("doctorsID");

                entity.HasOne(d => d.Doctors)
                    .WithMany(p => p.DoctorSlotDays)
                    .HasForeignKey(d => d.DoctorsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__doctorSlo__docto__2180FB33");
            });

            modelBuilder.Entity<DoctorSlotTime>(entity =>
            {
                entity.ToTable("doctorSlotTimes");

                entity.Property(e => e.DoctorSlotTimeId).HasColumnName("doctorSlotTimeID");

                entity.Property(e => e.DoctorSlotTime1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("doctorSlotTime");

                entity.Property(e => e.DoctorsId).HasColumnName("doctorsID");

                entity.HasOne(d => d.Doctors)
                    .WithMany(p => p.DoctorSlotTimes)
                    .HasForeignKey(d => d.DoctorsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__doctorSlo__docto__22751F6C");
            });

            modelBuilder.Entity<DoctorsAppointment>(entity =>
            {
                entity.HasKey(e => e.DoctorAppointmentId)
                    .HasName("PK__doctorsA__A637D18DE9F28036");

                entity.ToTable("doctorsAppointments");

                entity.Property(e => e.DoctorAppointmentId).HasColumnName("doctorAppointmentID");

                entity.Property(e => e.DoctorAppointment).HasColumnName("doctorAppointment");

                entity.Property(e => e.DoctorAppointmentDate)
                    .HasColumnType("date")
                    .HasColumnName("doctorAppointmentDate");

                entity.Property(e => e.DoctorAppointmentTime).HasColumnName("doctorAppointmentTime");

                entity.Property(e => e.PatientsId).HasColumnName("patientsID");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.HasOne(d => d.DoctorAppointmentNavigation)
                    .WithMany(p => p.DoctorsAppointments)
                    .HasForeignKey(d => d.DoctorAppointment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__doctorsAp__docto__1F98B2C1");

                entity.HasOne(d => d.DoctorAppointmentTimeNavigation)
                    .WithMany(p => p.DoctorsAppointments)
                    .HasForeignKey(d => d.DoctorAppointmentTime)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__doctorsAp__docto__1EA48E88");

                entity.HasOne(d => d.Patients)
                    .WithMany(p => p.DoctorsAppointments)
                    .HasForeignKey(d => d.PatientsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__doctorsAp__patie__208CD6FA");
            });

            modelBuilder.Entity<DoctorsRegistration>(entity =>
            {
                entity.HasKey(e => e.DoctorsId)
                    .HasName("PK__doctorsR__B3FA06FE579AA833");

                entity.ToTable("doctorsRegistrations");

                entity.Property(e => e.DoctorsId).HasColumnName("doctorsID");

                entity.Property(e => e.DoctorsCategory).HasColumnName("doctorsCategory");

                entity.Property(e => e.DoctorsDegree).HasColumnName("doctorsDegree");

                entity.Property(e => e.DoctorsEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("doctorsEmail");

                entity.Property(e => e.DoctorsImg)
                    .IsUnicode(false)
                    .HasColumnName("doctorsImg");

                entity.Property(e => e.DoctorsName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("doctorsName");

                entity.Property(e => e.DoctorsPhoneNo).HasColumnName("doctorsPhoneNo");

                entity.Property(e => e.DoctorsPwd)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("doctorsPwd");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.DoctorsCategoryNavigation)
                    .WithMany(p => p.DoctorsRegistrations)
                    .HasForeignKey(d => d.DoctorsCategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__doctorsRe__docto__245D67DE");

                entity.HasOne(d => d.DoctorsDegreeNavigation)
                    .WithMany(p => p.DoctorsRegistrations)
                    .HasForeignKey(d => d.DoctorsDegree)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__doctorsRe__docto__236943A5");
            });

            modelBuilder.Entity<EmergencyPatient>(entity =>
            {
                entity.HasKey(e => e.EmergencyPatientsId)
                    .HasName("PK__emergenc__DD9F5A4CEDA9BE77");

                entity.ToTable("emergencyPatients");

                entity.Property(e => e.EmergencyPatientsId).HasColumnName("emergencyPatientsID");

                entity.Property(e => e.AmountPaid).HasColumnName("amountPaid");

                entity.Property(e => e.AmountPayable).HasColumnName("amountPayable");

                entity.Property(e => e.EmergencyPatientsName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("emergencyPatientsName");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("feedback");

                entity.Property(e => e.FeedbackId).HasColumnName("feedbackID");

                entity.Property(e => e.Feedback1)
                    .HasColumnType("text")
                    .HasColumnName("feedback");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("userEmail");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("userName");
            });

            modelBuilder.Entity<HospitalBilling>(entity =>
            {
                entity.HasKey(e => e.HospitalBillings)
                    .HasName("PK__hospital__E4147BEBD904E4B5");

                entity.ToTable("hospitalBillings");

                entity.Property(e => e.HospitalBillings)
                    .ValueGeneratedNever()
                    .HasColumnName("hospitalBillings");

                entity.Property(e => e.BillFor).HasColumnName("billFor");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("status")
                    .HasDefaultValueSql("('unpaid')");
            });

            modelBuilder.Entity<HospitalPharmacyPrescription>(entity =>
            {
                entity.HasKey(e => e.PrescriptionId)
                    .HasName("PK__hospital__7920FDC45E07D17C");

                entity.ToTable("hospitalPharmacyPrescriptions");

                entity.Property(e => e.PrescriptionId).HasColumnName("prescriptionID");

                entity.Property(e => e.Medicines)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("medicines");

                entity.Property(e => e.PatientsId).HasColumnName("patientsID");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("status")
                    .HasDefaultValueSql("('pending')");

                entity.HasOne(d => d.Patients)
                    .WithMany(p => p.HospitalPharmacyPrescriptions)
                    .HasForeignKey(d => d.PatientsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__hospitalP__patie__25518C17");
            });

            modelBuilder.Entity<LabTest>(entity =>
            {
                entity.ToTable("labTests");

                entity.Property(e => e.LabTestId).HasColumnName("labTestID");

                entity.Property(e => e.LabTest1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("labTest");

                entity.Property(e => e.LabTestAmount).HasColumnName("labTestAmount");
            });

            modelBuilder.Entity<LabTestAllocation>(entity =>
            {
                entity.ToTable("labTestAllocations");

                entity.Property(e => e.LabTestAllocationId).HasColumnName("labTestAllocationID");

                entity.Property(e => e.CurrentDateTime)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("currentDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LabTestId).HasColumnName("labTestID");

                entity.Property(e => e.PatientsCardId).HasColumnName("patientsCardID");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.LabTest)
                    .WithMany(p => p.LabTestAllocations)
                    .HasForeignKey(d => d.LabTestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__labTestAl__labTe__2645B050");

                entity.HasOne(d => d.PatientsCard)
                    .WithMany(p => p.LabTestAllocations)
                    .HasForeignKey(d => d.PatientsCardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__labTestAl__patie__2739D489");
            });

            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.ToTable("medicines");

                entity.Property(e => e.MedicineId).HasColumnName("medicineID");

                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.MedicineName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("medicineName");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.SupplierId).HasColumnName("supplierID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Medicines)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__medicines__categ__2BFE89A6");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Medicines)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__medicines__suppl__2B0A656D");
            });

            modelBuilder.Entity<MedicineCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK__medicine__23CAF1F8FDDD4263");

                entity.ToTable("medicineCategories");

                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("categoryName");
            });

            modelBuilder.Entity<MedicineOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__medicine__0809337DEBFD88EE");

                entity.ToTable("medicineOrders");

                entity.Property(e => e.OrderId).HasColumnName("orderID");

                entity.Property(e => e.EmployeeId).HasColumnName("employeeID");

                entity.Property(e => e.OrderDate)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("orderDate")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.MedicineOrders)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__medicineO__emplo__2A164134");
            });

            modelBuilder.Entity<MedicineOrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailsId)
                    .HasName("PK__medicine__5EEE52534617F8BB");

                entity.ToTable("medicineOrderDetails");

                entity.Property(e => e.OrderDetailsId).HasColumnName("orderDetailsID");

                entity.Property(e => e.MedicinesId).HasColumnName("medicinesID");

                entity.Property(e => e.OrderId).HasColumnName("orderID");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Medicines)
                    .WithMany(p => p.MedicineOrderDetails)
                    .HasForeignKey(d => d.MedicinesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__medicineO__medic__29221CFB");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.MedicineOrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__medicineO__order__282DF8C2");
            });

            modelBuilder.Entity<MedicineSupplier>(entity =>
            {
                entity.HasKey(e => e.SuppliersId)
                    .HasName("PK__medicine__E3938FDA51269C22");

                entity.ToTable("medicineSuppliers");

                entity.Property(e => e.SuppliersId).HasColumnName("suppliersID");

                entity.Property(e => e.SuppliersCity)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("suppliersCity");

                entity.Property(e => e.SuppliersName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("suppliersName");

                entity.Property(e => e.SuppliersPhoneNo).HasColumnName("suppliersPhoneNo");
            });

            modelBuilder.Entity<NurseRegistration>(entity =>
            {
                entity.HasKey(e => e.NurseRegistrationsId)
                    .HasName("PK__nurseReg__0611BB1623D6B0A1");

                entity.ToTable("nurseRegistrations");

                entity.Property(e => e.NurseRegistrationsId).HasColumnName("nurseRegistrationsID");

                entity.Property(e => e.Degree).HasColumnName("degree");

                entity.Property(e => e.NurseEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nurseEmail");

                entity.Property(e => e.NurseImg)
                    .IsUnicode(false)
                    .HasColumnName("nurseImg");

                entity.Property(e => e.NurseName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nurseName");

                entity.Property(e => e.NursePhoneNo).HasColumnName("nursePhoneNo");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.DegreeNavigation)
                    .WithMany(p => p.NurseRegistrations)
                    .HasForeignKey(d => d.Degree)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__nurseRegi__degre__2CF2ADDF");
            });

            modelBuilder.Entity<OtdaySlot>(entity =>
            {
                entity.HasKey(e => e.OtdayId)
                    .HasName("PK__OTDaySlo__794847F9D8ED03A4");

                entity.ToTable("OTDaySlots");

                entity.Property(e => e.OtdayId).HasColumnName("OTDayID");

                entity.Property(e => e.Otday)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("OTDay");
            });

            modelBuilder.Entity<Otregistration>(entity =>
            {
                entity.HasKey(e => e.PatientsOtid)
                    .HasName("PK__OTRegist__4802E8F1516AF0B4");

                entity.ToTable("OTRegistrations");

                entity.Property(e => e.PatientsOtid).HasColumnName("patientsOTID");

                entity.Property(e => e.DoctorId).HasColumnName("doctorID");

                entity.Property(e => e.Otdate)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OTDate");

                entity.Property(e => e.Ottime).HasColumnName("OTTime");

                entity.Property(e => e.PatientsCardId).HasColumnName("patientsCardID");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Otregistrations)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OTRegistr__docto__2FCF1A8A");

                entity.HasOne(d => d.OttimeNavigation)
                    .WithMany(p => p.Otregistrations)
                    .HasForeignKey(d => d.Ottime)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OTRegistr__OTTim__2EDAF651");

                entity.HasOne(d => d.PatientsCard)
                    .WithMany(p => p.Otregistrations)
                    .HasForeignKey(d => d.PatientsCardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OTRegistr__patie__2DE6D218");
            });

            modelBuilder.Entity<OttimeSlot>(entity =>
            {
                entity.HasKey(e => e.OttimeId)
                    .HasName("PK__OTTimeSl__42369CD723087CB3");

                entity.ToTable("OTTimeSlots");

                entity.Property(e => e.OttimeId).HasColumnName("OTTimeID");

                entity.Property(e => e.Ottime)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("OTTime");
            });

            modelBuilder.Entity<PatientPayment>(entity =>
            {
                entity.HasKey(e => e.PayId)
                    .HasName("PK__tmp_ms_x__082E8AE3EF90468B");

                entity.ToTable("patientPayments");

                entity.Property(e => e.PayId).HasColumnName("payID");

                entity.Property(e => e.PatientsCardId).HasColumnName("patientsCardID");

                entity.Property(e => e.Pay).HasColumnName("pay");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.PatientsCard)
                    .WithMany(p => p.PatientPayments)
                    .HasForeignKey(d => d.PatientsCardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__patientPa__patie__318258D2");
            });

            modelBuilder.Entity<PatientRoom>(entity =>
            {
                entity.HasKey(e => e.RoomId)
                    .HasName("PK__patientR__6C3BF5DE809F33CB");

                entity.ToTable("patientRooms");

                entity.Property(e => e.RoomId).HasColumnName("roomID");

                entity.Property(e => e.Room)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("room");

                entity.Property(e => e.RoomAmount).HasColumnName("roomAmount");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<PatientsAllotedRoom>(entity =>
            {
                entity.ToTable("patientsAllotedRooms");

                entity.Property(e => e.PatientsAllotedRoomId).HasColumnName("patientsAllotedRoomID");

                entity.Property(e => e.AllotTill)
                    .HasColumnType("date")
                    .HasColumnName("allotTill");

                entity.Property(e => e.CurrentDateTime)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("currentDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Days).HasColumnName("days");

                entity.Property(e => e.PatientsCardId).HasColumnName("patientsCardID");

                entity.Property(e => e.PatientsRoomId).HasColumnName("patientsRoomID");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.PatientsCard)
                    .WithMany(p => p.PatientsAllotedRooms)
                    .HasForeignKey(d => d.PatientsCardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__patientsA__patie__32AB8735");

                entity.HasOne(d => d.PatientsRoom)
                    .WithMany(p => p.PatientsAllotedRooms)
                    .HasForeignKey(d => d.PatientsRoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__patientsA__patie__30C33EC3");
            });

            modelBuilder.Entity<PatientsIdcard>(entity =>
            {
                entity.HasKey(e => e.PatientsCardId)
                    .HasName("PK__patients__D530DC68D09D60E9");

                entity.ToTable("patientsIDCards");

                entity.Property(e => e.PatientsCardId).HasColumnName("patientsCardID");

                entity.Property(e => e.AdvancePayment).HasColumnName("advancePayment");

                entity.Property(e => e.PatientsId).HasColumnName("patientsID");

                entity.Property(e => e.PayableAmount).HasColumnName("payableAmount");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.ValidDate)
                    .HasColumnType("date")
                    .HasColumnName("validDate");

                entity.HasOne(d => d.Patients)
                    .WithMany(p => p.PatientsIdcards)
                    .HasForeignKey(d => d.PatientsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__patientsI__patie__339FAB6E");
            });

            modelBuilder.Entity<PatientsRegistration>(entity =>
            {
                entity.HasKey(e => e.PatientsId)
                    .HasName("PK__patients__42574E66E3FC40F2");

                entity.ToTable("patientsRegistrations");

                entity.Property(e => e.PatientsId).HasColumnName("patientsID");

                entity.Property(e => e.PatientsEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("patientsEmail");

                entity.Property(e => e.PatientsName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("patientsName");

                entity.Property(e => e.PatientsPhoneNo).HasColumnName("patientsPhoneNo");

                entity.Property(e => e.PatientsPwd)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("patientsPwd");
            });

            modelBuilder.Entity<PharmacyEmployee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("PK__pharmacy__C134C9A14B202160");

                entity.ToTable("pharmacyEmployees");

                entity.Property(e => e.EmployeeId).HasColumnName("employeeID");

                entity.Property(e => e.EmployeeEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("employeeEmail");

                entity.Property(e => e.EmployeeImg)
                    .IsUnicode(false)
                    .HasColumnName("employeeImg");

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("employeeName");

                entity.Property(e => e.EmployeePhoneNo).HasColumnName("employeePhoneNo");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Receptionist>(entity =>
            {
                entity.ToTable("receptionists");

                entity.Property(e => e.ReceptionistId).HasColumnName("receptionistID");

                entity.Property(e => e.ReceptionistContactNo).HasColumnName("receptionistContactNo");

                entity.Property(e => e.ReceptionistEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("receptionistEmail");

                entity.Property(e => e.ReceptionistName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("receptionistName");

                entity.Property(e => e.ReceptionistPwd)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("receptionistPwd");
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.Property(e => e.StaffId).HasColumnName("staffID");

                entity.Property(e => e.StaffDesignation)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("staffDesignation");

                entity.Property(e => e.StaffName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("staffName");
            });

            modelBuilder.Entity<StaffSalary>(entity =>
            {
                entity.ToTable("staffSalaries");

                entity.Property(e => e.StaffSalaryId).HasColumnName("staffSalaryID");

                entity.Property(e => e.SalaryAmount).HasColumnName("salaryAmount");

                entity.Property(e => e.StaffId).HasColumnName("staffID");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.StaffSalaries)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__staffSala__staff__3493CFA7");
            });

            modelBuilder.Entity<StaffSalaryStatus>(entity =>
            {
                entity.HasKey(e => e.SalaryToPayId)
                    .HasName("PK__staffSal__904D4A96522D04C3");

                entity.ToTable("staffSalaryStatus");

                entity.Property(e => e.SalaryToPayId).HasColumnName("salaryToPayID");

                entity.Property(e => e.SalaryToPay).HasColumnName("salaryToPay");

                entity.Property(e => e.StaffSalariesId).HasColumnName("staffSalariesID");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.StaffSalaries)
                    .WithMany(p => p.StaffSalaryStatuses)
                    .HasForeignKey(d => d.StaffSalariesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__staffSala__staff__3587F3E0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
