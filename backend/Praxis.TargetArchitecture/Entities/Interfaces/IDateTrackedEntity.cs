namespace Praxis.TargetArchitecture.Entities.Interfaces;

public interface IDateTrackedEntity
{
    DateTime CreatedUtc { get; set; }
    string CreatedBy { get; set; }
    DateTime UpdatedUtc { get; set; }
    string UpdatedBy { get; set; }
}
