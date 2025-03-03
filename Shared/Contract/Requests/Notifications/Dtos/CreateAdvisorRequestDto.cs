using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Notifications.Dtos;

public sealed class CreateAdvisorRequestDto
{
    public CreateMediaDto? CV { get; set; }
    public string? Introduction { get; set; }
    public string? Experience { get; set; }
    public List<CreateMediaDto>? Certificates { get; set; }
}