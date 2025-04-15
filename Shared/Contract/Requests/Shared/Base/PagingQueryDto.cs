namespace Contract.Requests.Shared.Base;

public abstract class PagingQueryDto
{
    // from 0
    public short PageIndex { get; set; }

    public byte PageSize { get; set; } = 30;
}
