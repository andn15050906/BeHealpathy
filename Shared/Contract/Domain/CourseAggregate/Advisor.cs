using Contract.Domain.Shared.MultimediaBase;

namespace Contract.Domain.CourseAggregate;

public sealed class Advisor : AuditedEntity
{
    // Attributes
    public string Intro { get; set; }
    public string Experience { get; set; }
    public long Balance { get; set; }
    public byte CourseCount { get; set; }

    // Navigations
    public List<Multimedia> Medias { get; set; }
    public List<Course> Courses { get; set; }






#pragma warning disable CS8618
    public Advisor()
    {

    }

    public Advisor(Guid id, Guid creatorId, string intro, string experience, List<Multimedia> qualifications)
    {
        Id = id;
        CreatorId = creatorId;

        Intro = intro;
        Experience = experience;
        CourseCount = 0;
        Balance = 0;
        Medias = qualifications;
    }
#pragma warning restore CS8618



    public void Withdraw(long amount)
    {
        Balance -= amount;
        if (Balance < 0)
            throw new Exception(BusinessMessages.Instructor.INVALID_BALANCE);
    }
}