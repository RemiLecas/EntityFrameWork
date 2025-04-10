using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<SessionSpeaker> SessionSpeakers { get; set; }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Room> Rooms { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Event
            modelBuilder.Entity<Event>(entity =>
            {
                  entity.HasKey(e => e.Id);
                  entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                  entity.Property(e => e.Description).HasMaxLength(1000);
                  entity.Property(e => e.StartDate).IsRequired();
                  entity.Property(e => e.EndDate).IsRequired();

                  entity.Property(e => e.Status)
                      .HasConversion<string>()
                      .HasDefaultValue(EventStatus.Planned);

                  entity.HasOne(e => e.Category)
                        .WithMany(c => c.Events)
                        .HasForeignKey(e => e.CategoryId)
                        .OnDelete(DeleteBehavior.Cascade);

                  entity.HasOne(e => e.Location)
                        .WithMany(l => l.Events)
                        .HasForeignKey(e => e.LocationId)
                        .OnDelete(DeleteBehavior.Cascade);
            });

            // Participant
            modelBuilder.Entity<Participant>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.LastName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Email).IsRequired();
                entity.Property(p => p.Company).HasMaxLength(100);
                entity.Property(p => p.JobTitle).HasMaxLength(100);
            });

            // EventParticipant
            modelBuilder.Entity<EventParticipant>(entity =>
            {
                entity.HasKey(ep => new { ep.EventId, ep.ParticipantId });

                entity.Property(ep => ep.RegistrationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(ep => ep.AttendanceStatus)
                      .HasConversion<string>()
                      .HasDefaultValue(AttendanceStatus.Pending);

                entity.HasOne(ep => ep.Event)
                      .WithMany(e => e.EventParticipants)
                      .HasForeignKey(ep => ep.EventId);

                entity.HasOne(ep => ep.Participant)
                      .WithMany(p => p.EventParticipants)
                      .HasForeignKey(ep => ep.ParticipantId);
            });

            // Session
            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Title).IsRequired().HasMaxLength(200);
                entity.Property(s => s.Description).HasMaxLength(1000);
                entity.Property(s => s.StartTime).IsRequired();
                entity.Property(s => s.EndTime).IsRequired();

                entity.HasOne(s => s.Event)
                      .WithMany(e => e.Sessions)
                      .HasForeignKey(s => s.EventId);

                entity.HasOne(s => s.Room)
                      .WithMany(r => r.Sessions)
                      .HasForeignKey(s => s.RoomId);
            });

            // Speaker
            modelBuilder.Entity<Speaker>(entity =>
            {
                entity.HasKey(sp => sp.Id);
                entity.Property(sp => sp.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(sp => sp.LastName).IsRequired().HasMaxLength(100);
                entity.Property(sp => sp.Email).IsRequired();
                entity.Property(sp => sp.Company).HasMaxLength(100);
                entity.Property(sp => sp.Bio).HasMaxLength(1000);
            });

            // SessionSpeaker
            modelBuilder.Entity<SessionSpeaker>(entity =>
            {
                entity.HasKey(ss => new { ss.SessionId, ss.SpeakerId });

                entity.Property(ss => ss.Role)
                      .HasConversion<string>()
                      .HasDefaultValue(SpeakerRole.Speaker);

                entity.HasOne(ss => ss.Session)
                      .WithMany(s => s.SessionSpeakers)
                      .HasForeignKey(ss => ss.SessionId);

                entity.HasOne(ss => ss.Speaker)
                      .WithMany(sp => sp.SessionSpeakers)
                      .HasForeignKey(ss => ss.SpeakerId);
            });

            // Location
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Name).IsRequired().HasMaxLength(200);
                entity.Property(l => l.Address).HasMaxLength(300);
                entity.Property(l => l.City).HasMaxLength(100);
                entity.Property(l => l.Country).HasMaxLength(100);
                entity.Property(l => l.Capacity).IsRequired();
            });

            // Room
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
                entity.Property(r => r.Capacity).IsRequired();

                entity.HasOne(r => r.Location)
                      .WithMany(l => l.Rooms)
                      .HasForeignKey(r => r.LocationId);
            });

            // Rating
            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Score).IsRequired();
                entity.Property(r => r.Comment).HasMaxLength(1000);

                entity.HasOne(r => r.Session)
                      .WithMany(s => s.Ratings)
                      .HasForeignKey(r => r.SessionId);

                entity.HasOne(r => r.Participant)
                      .WithMany(p => p.Ratings)
                      .HasForeignKey(r => r.ParticipantId);
            });
        }
    }
}
