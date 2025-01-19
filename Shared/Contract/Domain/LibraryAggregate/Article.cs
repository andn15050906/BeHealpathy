using Contract.Domain.Shared.MultimediaBase;
using Contract.Domain.UserAggregate;

namespace Contract.Domain.LibraryAggregate;

public sealed class Article : AuditedEntity
{
    // Attributes
    public string Title { get; set; }
    public string Status { get; set; }
    public bool IsCommentDisabled { get; set; }
    public int CommentCount { get; set; }
    public int ViewCount { get; set; }

    // Navigations
    public List<ArticleSection> Sections { get; set; }
    public List<Tag> Tags { get; set; }

    public Multimedia Thumb { get; set; }
    public List<ArticleComment> Comments { get; set; }
    public List<ArticleReaction> Reactions { get; set; }
    public User Creator { get; set; }



#pragma warning disable CS8618
    public Article()
    {

    }

    public Article(
        Guid id, Guid creatorId,
        string title, string status, bool isCommentDisabled, List<ArticleSection> sections, List<Tag> tags)
    {
        Id = id;
        CreatorId = creatorId;

        Title = title;
        Status = status;
        IsCommentDisabled = isCommentDisabled;

        Sections = sections;
        Tags = tags;
    }
#pragma warning restore CS8618
}
