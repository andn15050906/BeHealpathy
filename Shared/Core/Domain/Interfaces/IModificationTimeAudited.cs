using Core.Helpers;

namespace Core.Domain.Interfaces;

public interface IModificationTimeAudited
{
    DateTime LastModificationTime { get; set; }

    void SetModificationTime()
    {
        LastModificationTime = TimeHelper.Now;
    }
}