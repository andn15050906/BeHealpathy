using Contract.Helpers;
using Contract.Requests.Library.ArticleRequests;
using Contract.Responses.Library;
using Contract.Responses.Shared;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Library.ArticleHandlers;

public class GetArticleByIdHandler : RequestHandler<GetArticleByIdQuery, ArticleModel, HealpathyContext>
{
    public GetArticleByIdHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<ArticleModel>> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var article = await _context.Articles
            .Where(_ => _.Id == request.Id)
            .Select(ArticleModel.MapExpression)
            .FirstOrDefaultAsync(cancellationToken);

            if (article == null)
                return NotFound("Article not found");

            var thumbSourceIds = new List<Guid> { article.Id };
            var sectionSourceIds = article.Sections?.Select(s => s.Id).ToList() ?? new();
            thumbSourceIds.AddRange(sectionSourceIds);
            var sourceIds = thumbSourceIds;

            var medias = await _context.Multimedia
                .Where(_ => sourceIds.Contains(_.SourceId) && !_.IsDeleted)
                .Select(MultimediaModel.MapExpression)
                .ToListAsync(cancellationToken);

            var reactions = await _context.ArticleReactions
                .Where(_ => sourceIds.Contains(_.SourceId) && !_.IsDeleted)
                .Select(ReactionModel.MapExpression)
                .ToListAsync(cancellationToken);

            article.Thumb = medias.FirstOrDefault(_ => _.SourceId == article.Id);
            foreach (var section in article.Sections)
                section.Media = medias.FirstOrDefault(_ => _.SourceId == section.Id);
            article.Reactions = reactions.Where(_ => _.SourceId == article.Id).ToList() ?? [];

            return ToQueryResult(article);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}