using Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Core.Database.Configurations
{
    public class StudentConfiguration : EntityTypeConfiguration<Student>
    {
        public StudentConfiguration()
        {
            ToTable("Students");

            HasKey(s => s.Id);

            Property(s => s.PersonalNumber)
                .IsRequired()
                .HasMaxLength(11)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(
                        new IndexAttribute("IX_PersonalNumberUnique")
                        {
                            IsUnique = true
                        }));

            Property(s => s.Firstname)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("nvarchar");

            Property(s => s.Lastname)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("nvarchar");

            Property(s => s.Gender)
                .IsRequired();

            Property(s => s.Birthdate)
                .IsRequired();
        }
    }
}
