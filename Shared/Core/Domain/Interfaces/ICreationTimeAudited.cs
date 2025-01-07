using Core.Helpers;

namespace Core.Domain.Interfaces;

public interface ICreationTimeAudited
{
    DateTime CreationTime { get; set; }

    void SetCreationTime()
    {
        CreationTime = TimeHelper.Now;
    }
}