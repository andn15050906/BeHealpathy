using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Library.MediaRequests.Dtos;

public sealed class CreateMediaResourceDto
{
    public string Description { get; set; }
    public string Artist { get; set; }

    public CreateMediaDto Media { get; set; }
}
