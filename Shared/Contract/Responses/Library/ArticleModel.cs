using Contract.Domain.ToolAggregate;
using Contract.Responses.Identity;
using Contract.Responses.Shared;
using System.Linq.Expressions;

namespace Contract.Responses.Library;

public sealed class ArticleModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public string Title { get; set; }
    public string Status { get; set; }
    public bool IsCommentDisabled { get; set; }
    public int CommentCount { get; set; }
    public int ViewCount { get; set; }

    public MultimediaModel? Thumb { get; set; }
    public List<ArticleSectionModel> Sections { get; set; }
    public List<TagModel> Tags { get; set; }
    public List<CommentModel> Comments { get; set; }
    public List<ReactionModel> Reactions { get; set; }
    public UserMinModel Creator { get; set; }



    public static Expression<Func<Article, ArticleModel>> MapExpression
        = _ => new ArticleModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            LastModifierId = _.LastModifierId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,

            Title = _.Title,
            Status = _.Status,
            IsCommentDisabled = _.IsCommentDisabled,
            CommentCount = _.CommentCount,
            ViewCount = _.ViewCount,

            Tags = _.Tags.Select(tag => new TagModel
            {
                Id = tag.Id,
                Title = tag.Title
            }).ToList(),

            /*Thumb = new MultimediaModel
            {
                Id = _.Thumb.Id,
                SourceId = _.Thumb.SourceId,
                Type = _.Thumb.Type,
                Url = _.Thumb.Url,
                Title = _.Thumb.Title
            },*/

            //...
            Sections = _.Sections.Select(section => new ArticleSectionModel
            {
                Id = section.Id,
                Header = section.Header,
                Content = section.Content,
                /*Media = new MultimediaModel
                {
                    Id = section.Media.Id,
                    SourceId = section.Media.SourceId,
                    Type = section.Media.Type,
                    Url = section.Media.Url,
                    Title = section.Media.Title
                }*/
            }).ToList(),

            /*Comments = _.Comments.Select(comment => new CommentModel
            {
                Id = comment.Id,
                CreatorId = comment.CreatorId,
                LastModifierId = comment.LastModifierId,
                CreationTime = comment.CreationTime,
                LastModificationTime = comment.LastModificationTime,
                SourceId = comment.SourceId,
                Content = comment.Content,
                Status = comment.Status,
                //Medias = comment.Medias
            }).ToList(),*/

            //...
            Reactions = _.Reactions.Select(reaction => new ReactionModel
            {
                Id = reaction.Id,
                CreatorId = reaction.CreatorId,
                CreationTime = reaction.CreationTime,
                SourceId = reaction.SourceId,
                Content = reaction.Content
            }).ToList(),

            Creator = new UserMinModel
            {
                Id = _.Creator.Id,
                AvatarUrl = _.Creator.AvatarUrl,
                FullName = _.Creator.FullName
            }
        };
}