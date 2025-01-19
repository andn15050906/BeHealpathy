using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Courses.AdvisorRequests.Dtos;

public sealed class CreateAdvisorDto
{
    public string Intro { get; set; }
    public string Experience { get; set; }
    public List<CreateMediaDto>? Qualifications { get; set; }
}
