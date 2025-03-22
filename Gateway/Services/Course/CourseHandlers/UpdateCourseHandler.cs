using Contract.Domain.CourseAggregate;
using Contract.Domain.CourseAggregate.Enums;
using Contract.Helpers;
using Contract.Requests.Courses.CourseRequests;
using Contract.Requests.Courses.CourseRequests.Dtos;
using Core.Helpers;
using Infrastructure.DataAccess.SQLServer.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Courses.Services.Courses;

/// <summary>
/// Handler xử lý việc cập nhật thông tin khóa học
/// </summary>
public class UpdateCourseHandler : RequestHandler<UpdateCourseCommand, HealpathyContext>
{
    public UpdateCourseHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    /// <summary>
    /// Xử lý yêu cầu cập nhật thông tin khóa học
    /// </summary>
    public override async Task<Result> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Kiểm tra khóa học có tồn tại không
            var entity = await _context.Courses.FindExt(request.Rq.Id);
            if (entity is null)
                return NotFound(string.Empty);
            // Kiểm tra quyền cập nhật
            if (entity.CreatorId != request.UserId)
                return Unauthorized(string.Empty);

            // Áp dụng các thay đổi
            ApplyChanges(entity, request.Rq, request.UserId);
            // Xử lý thêm/xóa media
            if (request.AddedMedias is not null && request.AddedMedias.Count > 0)
                _context.Multimedia.AddRange(request.AddedMedias);
            if (request.RemovedMedias is not null && request.RemovedMedias.Count > 0)
                await _context.Multimedia.DeleteExt(request.RemovedMedias);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }

        //await _context.Replace(
        //    list[0].Id,
        //    new Course(
        //        list[0].Id, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
        //        "Web Development Bootcamp Title Updated", "ThumbUrl Updated", "Intro Updated", "Description Updated",
        //        200000, CourseLevel.Beginner, "Outcomes Updated", "Requirements Updated", new()
        //    )
        //);
    }

    /// <summary>
    /// Áp dụng các thay đổi từ DTO vào entity
    /// </summary>
    private void ApplyChanges(Course entity, UpdateCourseDto dto, Guid userId)
    {
        // Cập nhật các thông tin cơ bản
        if (dto.Intro is not null)
            entity.Intro = dto.Intro;
        if (dto.Description is not null)
            entity.Description = dto.Description;
        if (dto.Status is not null)
            entity.Status = (CourseStatus)dto.Status;
        if (dto.Price is not null)
            entity.Price = (double)dto.Price;
        if (dto.Level is not null)
            entity.Level = (CourseLevel)dto.Level;
        if (dto.Outcomes is not null)
            entity.Outcomes = dto.Outcomes;
        if (dto.Requirements is not null)
            entity.Requirements = dto.Requirements;
        if (dto.LeafCategoryId is not null)
            entity.LeafCategoryId = (Guid)dto.LeafCategoryId;

        entity.LastModificationTime = TimeHelper.Now;

        // Cập nhật các thông tin đặc biệt
        if (dto.Title is not null)
            entity.SetTitle(dto.Title);
        if (dto.Discount is not null && dto.DiscountExpiry is not null)
            entity.SetDiscount((double)dto.Discount, (DateTime)dto.DiscountExpiry!);
        entity.LastModifierId = userId;

        entity.ThumbUrl = dto.Thumb?.Url ?? string.Empty;

        // Sections
        //List<Section> removedSections = new();
        //List<Section> addedSections = new();

        //if (dto.RemovedSections != null)
        //{
        //    removedSections = _context.Sections.Where(_ =>
        //        dto.RemovedSections.Contains(_.Id) &&
        //        _.CourseId == dto.Id &&
        //        _.LectureCount == 0
        //    ).ToList();
        //}
        //if (dto.AddedSections != null)
        //{
        //    _context.Entry(entity).Collection(p => p.Sections).Load();
        //    byte currentIndex = entity.Sections.Count > 0
        //        ? entity.Sections.Max(_ => _.Index)
        //        : (byte)0;
        //    addedSections = dto.AddedSections.Select((_, index) => new Section(Guid.NewGuid(), (byte)(currentIndex + index + 1), _)).ToList();
        //}

        //foreach (var section in removedSections)
        //    entity.Sections.Remove(section);
        //entity.Sections.AddRange(addedSections);
    }
}