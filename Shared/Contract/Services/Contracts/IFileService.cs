using Contract.Domain.Shared.MultimediaBase;
using Contract.Helpers.Storage;
using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Services.Contracts;

public interface IFileService
{
    Task<List<Multimedia>> SaveMediasAndUpdateDtos(List<(CreateMediaDto, Guid)> dto_sourceIds);
    Task<Multimedia> SaveImageAndUpdateDto(CreateMediaDto dto, Guid sourceId);

    Task<Multimedia?> SaveImage(MediaWithStream? media);
    Task<Multimedia?> ReplaceImage(MediaWithStream? media, string identifier);
    Task<bool> DeleteImage(string identifier);

    Task<List<Multimedia?>> SaveMedias(List<MediaWithStream?> medias);
    Task<List<Multimedia>> SaveNotNullMedias(List<MediaWithStream?> medias);
    Task<List<Multimedia?>> UpdateMedias(List<MediaWithStream?> medias, List<string>? removedIdentifiers);
    Task<List<Multimedia>> UpdateNotNullMedias(List<MediaWithStream?> medias, List<string>? removedIdentifiers);
    Task<bool[]> DeleteMedias(List<string>? identifier);



    string GetUserAvatarIdentifier(Guid userId)
        => $"{userId}";

    string GetCourseThumbIdentifier(Guid courseId)
        => $"{courseId}";

    string GetInstructorQualificationIdentifier(Guid instructorId, int index)
        => $"{instructorId}_{index}";

    string GetLectureCommentMediaIdentifier(Guid commentId, int index)
        => $"{commentId}_{index}";

    string GetArticleCommentMediaIdentifier(Guid commentId, int index)
        => $"{commentId}_{index}";

    string GetCourseReviewMediaIdentifier(Guid reviewId, int index)
        => $"{reviewId}_{index}";

    string GetLectureMaterialIdentifier(Guid lectureId, int index)
        => $"{lectureId}_{index}";
}