using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Library.MediaRequests.Dtos;

public sealed class UpdateMediaResourceDto
{
    public Guid Id { get; set; }

    public string Description { get; set; }
    public string Artist { get; set; }

    public CreateMediaDto ReplacedMedia { get; set; }
}
