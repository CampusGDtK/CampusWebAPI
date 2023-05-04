using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campus.Core.Domain.Entities;

public class SpecialityFaculty
{
    [Key]
    public Guid Id { get; set; }

    public Guid SpecialityId { get; set; }

    public Guid FacultyId { get; set; }

    [ForeignKey("FacultyId")]
    public Faculty? Faculty { get; set; }

    [ForeignKey("SpecialityId")]
    public Speciality? Speciality { get; set; }
}
