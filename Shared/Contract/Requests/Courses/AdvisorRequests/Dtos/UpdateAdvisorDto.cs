using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Courses.AdvisorRequests.Dtos;

public sealed class UpdateAdvisorDto
{
    public Guid Id { get; set; }                // AdvisorId

    public string? Intro { get; set; }
    public string? Experience { get; set; }
    public long? Balance { get; set; }

    public List<CreateMediaDto>? AddedQualifications { get; set; }
    public List<Guid>? RemovedQualifications { get; set; }
}
