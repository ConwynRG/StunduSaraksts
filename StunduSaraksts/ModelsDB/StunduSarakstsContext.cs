using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class StunduSarakstsContext : DbContext
    {
        public StunduSarakstsContext()
        {
        }

        public StunduSarakstsContext(DbContextOptions<StunduSarakstsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Change> Changes { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Consultation> Consultations { get; set; }
        public virtual DbSet<ConsultationAttendance> ConsultationAttendances { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<LessonTime> LessonTimes { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationContent> NotificationContents { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomSubjectSpecialization> RoomSubjectSpecializations { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudyPlan> StudyPlans { get; set; }
        public virtual DbSet<StudySemester> StudySemesters { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<TeacherLessonTime> TeacherLessonTimes { get; set; }
        public virtual DbSet<TeacherSubject> TeacherSubjects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
 //               optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=StunduSaraksts;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Name)
                    .HasMaxLength(30);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.PersonalCode)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SecondName).HasMaxLength(30);

                entity.Property(e => e.Surname)
                    .HasMaxLength(30);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Change>(entity =>
            {
                entity.ToTable("Change");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CommentForInvolved).HasMaxLength(600);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.ReplyComment).HasMaxLength(600);

                entity.Property(e => e.ReplyDate).HasColumnType("datetime");

                entity.Property(e => e.RequestComment)
                    .IsRequired()
                    .HasMaxLength(600);

                entity.Property(e => e.RequestDate).HasColumnType("datetime");

                entity.HasOne(d => d.AcceptorNavigation)
                    .WithMany(p => p.ChangeAcceptorNavigations)
                    .HasForeignKey(d => d.Acceptor)
                    .HasConstraintName("FK__Change__Acceptor__46B27FE2");

                entity.HasOne(d => d.ClassNavigation)
                    .WithMany(p => p.Changes)
                    .HasForeignKey(d => d.Class)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Change__Class__47A6A41B");

                entity.HasOne(d => d.CreatorNavigation)
                    .WithMany(p => p.ChangeCreatorNavigations)
                    .HasForeignKey(d => d.Creator)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Change__Creator__489AC854");

                entity.HasOne(d => d.LessonNavigation)
                    .WithMany(p => p.Changes)
                    .HasForeignKey(d => d.Lesson)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Change__Lesson__498EEC8D");

                entity.HasOne(d => d.LessonTimeNavigation)
                    .WithMany(p => p.Changes)
                    .HasForeignKey(d => d.LessonTime)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Change__LessonTi__4A8310C6");

                entity.HasOne(d => d.RoomNavigation)
                    .WithMany(p => p.Changes)
                    .HasForeignKey(d => d.Room)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Change__Room__4B7734FF");

                entity.HasOne(d => d.SubjectNavigation)
                    .WithMany(p => p.Changes)
                    .HasForeignKey(d => d.Subject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Change__Subject__4C6B5938");

                entity.HasOne(d => d.TeacherNavigation)
                    .WithMany(p => p.ChangeTeacherNavigations)
                    .HasForeignKey(d => d.Teacher)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Change__Teacher__4D5F7D71");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("Class");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.HasOne(d => d.CuratorNavigation)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.Curator)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Class__Curator__4E53A1AA");

                entity.HasOne(d => d.StudySemesterNavigation)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.StudySemester)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Class__StudySeme__4F47C5E3");
            });

            modelBuilder.Entity<Consultation>(entity =>
            {
                entity.ToTable("Consultation");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasMaxLength(600);

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.RegisterDate).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.RoomReservationNavigation)
                    .WithMany(p => p.Consultations)
                    .HasForeignKey(d => d.RoomReservation)
                    .HasConstraintName("FK__Consultat__RoomR__503BEA1C");

                entity.HasOne(d => d.TeacherNavigation)
                    .WithMany(p => p.Consultations)
                    .HasForeignKey(d => d.Teacher)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Consultat__Teach__51300E55");
            });

            modelBuilder.Entity<ConsultationAttendance>(entity =>
            {
                
                entity.ToTable("ConsultationAttendance");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasMaxLength(600);

                entity.Property(e => e.RegisterDate).HasColumnType("datetime");

                entity.HasOne(d => d.ConsultationNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Consultation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Consultat__Consu__5224328E");

                entity.HasOne(d => d.StudentNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Student)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Consultat__Stude__531856C7");
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.ToTable("Lesson");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.ClassNavigation)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.Class)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lesson__Class__540C7B00");

                entity.HasOne(d => d.LessonTimeNavigation)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.LessonTime)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lesson__LessonTi__55009F39");

                entity.HasOne(d => d.RoomNavigation)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.Room)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lesson__Room__55F4C372");

                entity.HasOne(d => d.StudySemesterNavigation)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.StudySemester)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lesson__StudySem__56E8E7AB");

                entity.HasOne(d => d.SubjectNavigation)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.Subject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lesson__Subject__57DD0BE4");

                entity.HasOne(d => d.TeacherNavigation)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.Teacher)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lesson__Teacher__58D1301D");
            });

            modelBuilder.Entity<LessonTime>(entity =>
            {
                entity.ToTable("LessonTime");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.StudySemesterNavigation)
                    .WithMany(p => p.LessonTimes)
                    .HasForeignKey(d => d.StudySemester)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LessonTim__Study__59C55456");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ReceivedTime).HasColumnType("datetime");

                entity.Property(e => e.Recipient)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Sender)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.SentTime).HasColumnType("datetime");

                entity.HasOne(d => d.ContentNavigation)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.Content)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notificat__Conte__5AB9788F");

                entity.HasOne(d => d.RecipientNavigation)
                    .WithMany(p => p.NotificationRecipientNavigations)
                    .HasForeignKey(d => d.Recipient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notificat__Recip__5BAD9CC8");

                entity.HasOne(d => d.SenderNavigation)
                    .WithMany(p => p.NotificationSenderNavigations)
                    .HasForeignKey(d => d.Sender)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notificat__Sende__5CA1C101");
            });

            modelBuilder.Entity<NotificationContent>(entity =>
            {
                entity.ToTable("NotificationContent");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(600);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("Reservation");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.ReplyComment).HasMaxLength(600);

                entity.Property(e => e.ReplyDate).HasColumnType("datetime");

                entity.Property(e => e.RequestComment)
                    .IsRequired()
                    .HasMaxLength(600);

                entity.Property(e => e.RequestDate).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.Owner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reservati__Owner__5D95E53A");

                entity.HasOne(d => d.RoomNavigation)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.Room)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reservatio__Room__5E8A0973");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasOne(d => d.StudySemesterNavigation)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.StudySemester)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Room__StudySemes__5F7E2DAC");
            });

            modelBuilder.Entity<RoomSubjectSpecialization>(entity =>
            {
                entity.ToTable("RoomSubjectSpecialization");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.RoomNavigation)
                    .WithMany(p => p.RoomSubjectSpecializations)
                    .HasForeignKey(d => d.Room)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RoomSubjec__Room__634EBE90");

                entity.HasOne(d => d.SpecializationNavigation)
                    .WithMany(p => p.RoomSubjectSpecializations)
                    .HasForeignKey(d => d.Specialization)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RoomSubje__Speci__607251E5");

                entity.HasOne(d => d.StudySemesterNavigation)
                    .WithMany(p => p.RoomSubjectSpecializations)
                    .HasForeignKey(d => d.StudySemester)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RoomSubje__Study__6166761E");

                entity.HasOne(d => d.SubjectNavigation)
                    .WithMany(p => p.RoomSubjectSpecializations)
                    .HasForeignKey(d => d.Subject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RoomSubje__Subje__625A9A57");
            });

            modelBuilder.Entity<Specialization>(entity =>
            {
                entity.ToTable("Specialization");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.AccountNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.Account)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student__Account__6442E2C9");

                entity.HasOne(d => d.ClassNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.Class)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student__Class__65370702");
            });

            modelBuilder.Entity<StudyPlan>(entity =>
            {
                entity.ToTable("StudyPlan");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.ClassNavigation)
                    .WithMany(p => p.StudyPlans)
                    .HasForeignKey(d => d.Class)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StudyPlan__Class__662B2B3B");

                entity.HasOne(d => d.StudySemesterNavigation)
                    .WithMany(p => p.StudyPlans)
                    .HasForeignKey(d => d.StudySemester)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StudyPlan__Study__671F4F74");

                entity.HasOne(d => d.SubjectNavigation)
                    .WithMany(p => p.StudyPlans)
                    .HasForeignKey(d => d.Subject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StudyPlan__Subje__681373AD");
            });

            modelBuilder.Entity<StudySemester>(entity =>
            {
                entity.ToTable("StudySemester");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subject");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.StudySemesterNavigation)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.StudySemester)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Subject__StudySe__690797E6");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("Teacher");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.IsAdmin).HasDefaultValueSql("((0))");

                entity.Property(e => e.WorkEnd).HasColumnType("datetime");

                entity.Property(e => e.WorkStart).HasColumnType("datetime");

                entity.HasOne(d => d.AccountNavigation)
                    .WithMany(p => p.Teachers)
                    .HasForeignKey(d => d.Account)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Teacher__Account__69FBBC1F");
            });

            modelBuilder.Entity<TeacherLessonTime>(entity =>
            {
                
                entity.ToTable("TeacherLessonTime");

                entity.Property(d => d.Id).HasColumnName("ID");

                entity.HasOne(d => d.LessonTimeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.LessonTime)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TeacherLe__Lesso__6AEFE058");

                entity.HasOne(d => d.TeacherNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Teacher)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TeacherLe__Teach__6BE40491");
            });

            modelBuilder.Entity<TeacherSubject>(entity =>
            {


                entity.ToTable("TeacherSubject");

                entity.Property(d => d.Id).HasColumnName("ID");

                entity.HasOne(d => d.StudySemesterNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.StudySemester)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TeacherSu__Study__6CD828CA");

                entity.HasOne(d => d.SubjectNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Subject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TeacherSu__Subje__6DCC4D03");

                entity.HasOne(d => d.TeacherNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Teacher)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TeacherSu__Teach__6EC0713C");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
