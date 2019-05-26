using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ReviewApi.Models.Database
{
    public partial class ReviewsDatabaseContext : DbContext
    {
        public ReviewsDatabaseContext()
        {
        }

        public ReviewsDatabaseContext(DbContextOptions<ReviewsDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Artifact> Artifact { get; set; }
        public virtual DbSet<ArtifactDetail> ArtifactDetail { get; set; }
        public virtual DbSet<ArtifactReview> ArtifactReview { get; set; }
        public virtual DbSet<HeaderRow> HeaderRow { get; set; }
        public virtual DbSet<HeaderRowData> HeaderRowData { get; set; }
        public virtual DbSet<IbmArtifact> IbmArtifact { get; set; }
        public virtual DbSet<IbmArtifactReview> IbmArtifactReview { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectType> ProjectType { get; set; }
        public virtual DbSet<Review> Review { get; set; }
        public virtual DbSet<ReviewColumn> ReviewColumn { get; set; }
        public virtual DbSet<ReviewColumnTypeEnum> ReviewColumnTypeEnum { get; set; }
        public virtual DbSet<ReviewRole> ReviewRole { get; set; }
        public virtual DbSet<ReviewTameplate> ReviewTameplate { get; set; }
        public virtual DbSet<SystemRole> SystemRole { get; set; }
        public virtual DbSet<TaskPlan> TaskPlan { get; set; }
        public virtual DbSet<UserProject> UserProject { get; set; }
        public virtual DbSet<UserReviewRole> UserReviewRole { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Workproduct> Workproduct { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                /*
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=completeDB;Trusted_Connection=True;");*/
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Artifact>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.WorkproductId })
                    .HasName("Artifact_PK");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.WorkproductId).HasColumnName("Workproduct_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.ReviewId).HasColumnName("Review_id");

                entity.Property(e => e.WorkproductProjectId).HasColumnName("Workproduct_Project_id");

                entity.Property(e => e.WorkproductProjectId1).HasColumnName("Workproduct_Project_id1");

                entity.HasOne(d => d.Review)
                    .WithMany(p => p.Artifact)
                    .HasForeignKey(d => d.ReviewId)
                    .HasConstraintName("Artifact_Review_FK");

                entity.HasOne(d => d.Workproduct)
                    .WithMany(p => p.Artifact)
                    .HasForeignKey(d => new { d.WorkproductId, d.WorkproductProjectId1 })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Artifact_Workproduct_FK");
            });

            modelBuilder.Entity<ArtifactDetail>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ArtifactId, e.ArtifactWorkproductId })
                    .HasName("Artifact_detail_PK");

                entity.ToTable("Artifact_detail");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ArtifactId).HasColumnName("Artifact_id");

                entity.Property(e => e.ArtifactWorkproductId).HasColumnName("Artifact_Workproduct_id");

                entity.Property(e => e.ArtifactWorkproductProjectId).HasColumnName("Artifact_Workproduct_Project_id");

                entity.Property(e => e.DetailValue)
                    .HasColumnName("Detail_value")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.HasOne(d => d.Artifact)
                    .WithMany(p => p.ArtifactDetail)
                    .HasForeignKey(d => new { d.ArtifactId, d.ArtifactWorkproductId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Artifact_detail_Artifact_FK");
            });

            modelBuilder.Entity<ArtifactReview>(entity =>
            {
                entity.HasKey(e => new { e.ArtifactId, e.ArtifactWorkproductId, e.ReviewId })
                    .HasName("Relation_24_PK");

                entity.ToTable("Artifact_review");

                entity.Property(e => e.ArtifactId).HasColumnName("Artifact_id");

                entity.Property(e => e.ArtifactWorkproductId).HasColumnName("Artifact_Workproduct_id");

                entity.Property(e => e.ReviewId).HasColumnName("Review_id");

                entity.HasOne(d => d.Review)
                    .WithMany(p => p.ArtifactReview)
                    .HasForeignKey(d => d.ReviewId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASS_29");

                entity.HasOne(d => d.Artifact)
                    .WithMany(p => p.ArtifactReview)
                    .HasForeignKey(d => new { d.ArtifactId, d.ArtifactWorkproductId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASS_28");
            });

            modelBuilder.Entity<HeaderRow>(entity =>
            {
                entity.ToTable("Header_Row");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Function)
                    .HasColumnName("function")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Parameter)
                    .HasColumnName("parameter")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReviewColumnId).HasColumnName("Review_column_id");

                entity.Property(e => e.ReviewTameplateId).HasColumnName("Review_tameplate_id");

                entity.HasOne(d => d.ReviewColumn)
                    .WithMany(p => p.HeaderRow)
                    .HasForeignKey(d => d.ReviewColumnId)
                    .HasConstraintName("Header_row_review_column");

                entity.HasOne(d => d.ReviewTameplate)
                    .WithMany(p => p.HeaderRow)
                    .HasForeignKey(d => d.ReviewTameplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Header_Row_Review_tameplate_FK");
            });

            modelBuilder.Entity<HeaderRowData>(entity =>
            {
                entity.ToTable("Header_row_data");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.HeaderRowId).HasColumnName("Header_row_id");

                entity.Property(e => e.ReviewId).HasColumnName("Review_id");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.HeaderRow)
                    .WithMany(p => p.HeaderRowData)
                    .HasForeignKey(d => d.HeaderRowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Header_row_FK");

                entity.HasOne(d => d.Review)
                    .WithMany(p => p.HeaderRowData)
                    .HasForeignKey(d => d.ReviewId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Review_FK");
            });

            modelBuilder.Entity<IbmArtifact>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.IbmId })
                    .HasName("Ibm_artifact_PK");

                entity.ToTable("Ibm_artifact");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IbmId).HasColumnName("ibm_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.ReviewId).HasColumnName("Review_id");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.WorkproductId).HasColumnName("Workproduct_id");

                entity.Property(e => e.WorkproductProjectId).HasColumnName("Workproduct_Project_id");

                entity.HasOne(d => d.Review)
                    .WithMany(p => p.IbmArtifact)
                    .HasForeignKey(d => d.ReviewId)
                    .HasConstraintName("Ibm_artifact_Review_FK");

                entity.HasOne(d => d.Workproduct)
                    .WithMany(p => p.IbmArtifact)
                    .HasForeignKey(d => new { d.WorkproductId, d.WorkproductProjectId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Ibm_artifact_Workproduct_FK");
            });

            modelBuilder.Entity<IbmArtifactReview>(entity =>
            {
                entity.HasKey(e => new { e.IbmArtifactId, e.IbmArtifactIbmId, e.ReviewId })
                    .HasName("Relation_23_PK");

                entity.ToTable("Ibm_artifact_review");

                entity.Property(e => e.IbmArtifactId).HasColumnName("Ibm_artifact_id");

                entity.Property(e => e.IbmArtifactIbmId).HasColumnName("Ibm_artifact_ibm_id");

                entity.Property(e => e.ReviewId).HasColumnName("Review_id");

                entity.HasOne(d => d.Review)
                    .WithMany(p => p.IbmArtifactReview)
                    .HasForeignKey(d => d.ReviewId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASS_27");

                entity.HasOne(d => d.IbmArtifactI)
                    .WithMany(p => p.IbmArtifactReview)
                    .HasForeignKey(d => new { d.IbmArtifactId, d.IbmArtifactIbmId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASS_26");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectTypeId).HasColumnName("Project_type_id");

                entity.Property(e => e.UsersEmail)
                    .IsRequired()
                    .HasColumnName("Users_email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProjectType)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.ProjectTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Project_Project_type_FK");

                entity.HasOne(d => d.UsersEmailNavigation)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.UsersEmail)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Project_Users_FK");
            });

            modelBuilder.Entity<ProjectType>(entity =>
            {
                entity.ToTable("Project_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CloseDate)
                    .HasColumnName("Close_date")
                    .HasColumnType("date");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Html).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.ReviewTameplateId).HasColumnName("Review_tameplate_id");

                entity.Property(e => e.StartDate)
                    .HasColumnName("Start_date")
                    .HasColumnType("date");

                entity.Property(e => e.WorkproductId).HasColumnName("Workproduct_id");

                entity.Property(e => e.WorkproductProjectId).HasColumnName("Workproduct_Project_id");

                entity.HasOne(d => d.ReviewTameplate)
                    .WithMany(p => p.Review)
                    .HasForeignKey(d => d.ReviewTameplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Review_Review_tameplate_FK");

                entity.HasOne(d => d.Workproduct)
                    .WithMany(p => p.Review)
                    .HasForeignKey(d => new { d.WorkproductId, d.WorkproductProjectId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Review_Workproduct_FK");
            });

            modelBuilder.Entity<ReviewColumn>(entity =>
            {
                entity.ToTable("Review_column");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.ReviewTameplateId).HasColumnName("Review_tameplate_id");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ReviewTameplate)
                    .WithMany(p => p.ReviewColumn)
                    .HasForeignKey(d => d.ReviewTameplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Review_column_Review_tameplate_FK");
            });

            modelBuilder.Entity<ReviewColumnTypeEnum>(entity =>
            {
                entity.ToTable("Review_column_type_enum");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.ReviewColumnId).HasColumnName("Review_column_id");

                entity.HasOne(d => d.ReviewColumn)
                    .WithMany(p => p.ReviewColumnTypeEnum)
                    .HasForeignKey(d => d.ReviewColumnId)
                    .HasConstraintName("Review_column_type_enum_Review_column_FK");
            });

            modelBuilder.Entity<ReviewRole>(entity =>
            {
                entity.ToTable("Review_role");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReviewTameplateId).HasColumnName("Review_tameplate_id");

                entity.HasOne(d => d.ReviewTameplate)
                    .WithMany(p => p.ReviewRole)
                    .HasForeignKey(d => d.ReviewTameplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Review_role_Review_tameplate_FK");
            });

            modelBuilder.Entity<ReviewTameplate>(entity =>
            {
                entity.ToTable("Review_tameplate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.UsersEmail)
                    .IsRequired()
                    .HasColumnName("Users_email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.UsersEmailNavigation)
                    .WithMany(p => p.ReviewTameplate)
                    .HasForeignKey(d => d.UsersEmail)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Review_tameplate_Users_FK");
            });

            modelBuilder.Entity<SystemRole>(entity =>
            {
                entity.ToTable("System_role");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TaskPlan>(entity =>
            {
                entity.ToTable("Task_plan");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IbmId).HasColumnName("ibm_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectId).HasColumnName("Project_id");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasColumnType("text");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.TaskPlan)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Project_FK");
            });

            modelBuilder.Entity<UserProject>(entity =>
            {
                entity.HasKey(e => new { e.UsersEmail, e.ProjectId })
                    .HasName("Relation_3_PK");

                entity.ToTable("User_Project");

                entity.Property(e => e.UsersEmail)
                    .HasColumnName("Users_email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectId).HasColumnName("Project_id");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.UserProject)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASS_17");

                entity.HasOne(d => d.UsersEmailNavigation)
                    .WithMany(p => p.UserProject)
                    .HasForeignKey(d => d.UsersEmail)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASS_16");
            });

            modelBuilder.Entity<UserReviewRole>(entity =>
            {
                entity.HasKey(e => new { e.ReviewRoleId, e.UsersEmail, e.ReviewId })
                    .HasName("User_review_PKv2");

                entity.ToTable("User_review_role");

                entity.Property(e => e.ReviewRoleId).HasColumnName("Review_role_id");

                entity.Property(e => e.UsersEmail)
                    .HasColumnName("Users_email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReviewId).HasColumnName("Review_id");

                entity.HasOne(d => d.Review)
                    .WithMany(p => p.UserReviewRole)
                    .HasForeignKey(d => d.ReviewId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASS_25");

                entity.HasOne(d => d.ReviewRole)
                    .WithMany(p => p.UserReviewRole)
                    .HasForeignKey(d => d.ReviewRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("User_review_Review_role_FK");

                entity.HasOne(d => d.UsersEmailNavigation)
                    .WithMany(p => p.UserReviewRole)
                    .HasForeignKey(d => d.UsersEmail)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASS_24");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Email)
                    .HasName("Users_PK");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasColumnName("salt")
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.SystemRoleId).HasColumnName("System_role_id");

                entity.HasOne(d => d.SystemRole)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.SystemRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Users_System_role_FK");
            });

            modelBuilder.Entity<Workproduct>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ProjectId })
                    .HasName("Workproduct_PK");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ProjectId).HasColumnName("Project_id");

                entity.Property(e => e.ArtifactsUrl)
                    .HasColumnName("Artifacts_url")
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.UsersEmail)
                    .IsRequired()
                    .HasColumnName("Users_email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Workproduct)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Workproduct_Project_FK");

                entity.HasOne(d => d.UsersEmailNavigation)
                    .WithMany(p => p.Workproduct)
                    .HasForeignKey(d => d.UsersEmail)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Workproduct_Users_FK");
            });
        }
    }
}
