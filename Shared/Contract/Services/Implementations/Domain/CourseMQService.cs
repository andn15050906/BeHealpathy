using MassTransit;
using Contract.Services.Contracts;
using Contract.Messaging.ApiClients.MQ;
using Contract.Requests.Courses.CategoryRequests;
using Contract.Requests.Courses.LectureRequests;
using Contract.Requests.Courses.EnrollmentRequests;
using Contract.Helpers.Storage;
using Contract.Responses.Shared;
using Contract.Requests.Courses.AdvisorRequests;
using Contract.Responses.Courses;
using Contract.Requests.Shared.BaseRequests.Comments;
using Contract.Requests.Shared.BaseRequests.Reviews;
using Contract.Services.Contracts.Domain;

namespace Contract.Services.Implementations;

public sealed class CourseMQService : MQApiClient, ICourseApiService
{
    private readonly IFileService _fileService;

    public CourseMQService(IScopedClientFactory factory, IFileService fileService) : base(factory)
    {
        _fileService = fileService;
    }






    public async Task<Result<List<CategoryModel>>> GetCategoriesAsync(GetAllCategoriesQuery query)
        => await Send<GetAllCategoriesQuery, List<CategoryModel>>(query);

    public async Task<Result> CreateAsync(CreateCategoryCommand command)
        => await Send(command);

    public async Task<Result> UpdateAsync(UpdateCategoryCommand command)
        => await Send(command);

    public async Task<Result> DeleteAsync(DeleteCategoryCommand command)
        => await Send(command);






    public async Task<Result<PagedResult<AdvisorModel>>> GetPagedAsync(GetPagedAdvisorsQuery query)
        => await Send<GetPagedAdvisorsQuery, PagedResult<AdvisorModel>>(query);

    public async Task<Result> CreateAsync(CreateAdvisorCommand command)
        => await Send(command);

    public async Task<Result> UpdateAsync(UpdateAdvisorCommand command)
        => await Send(command);






    public async Task<Result<PagedResult<CourseModel>>> GetPagedAsync(GetPagedCoursesQuery query)
        => await Send<GetPagedCoursesQuery, PagedResult<CourseModel>>(query);

    public async Task<Result<CourseModel>> GetByIdAsync(GetCourseByIdQuery query)
        => await Send<GetCourseByIdQuery, CourseModel>(query);

    public async Task<Result<PagedResult<CourseMinModel>>> GetMinAsync(GetMinimumCoursesQuery query)
        => await Send<GetMinimumCoursesQuery, PagedResult<CourseMinModel>>(query);

    public async Task<Result<List<CourseModel>>> GetMultipleAsync(GetMultipleCoursesQuery query)
        => await Send<GetMultipleCoursesQuery, List<CourseModel>>(query);

    public async Task<Result> CreateAsync(CreateCourseCommand command)
    {
        var thumb = await _fileService.SaveImage(
            await MediaWithStream.FromImageDto(command.Rq.Thumb, command.Id, _ => _fileService.GetCourseThumbIdentifier(command.Id))
        );

        command.Rq.Thumb.UpdateAfterSave(thumb);
        return await Send(command);
    }

    public async Task<Result> UpdateAsync(UpdateCourseCommand command)
    {
        if (command.Rq.Thumb is not null)
        {
            var thumb = await _fileService.SaveImage(
                await MediaWithStream.FromImageDto(command.Rq.Thumb, command.Rq.Id, _ => _fileService.GetCourseThumbIdentifier(command.Rq.Id))
            );

            command.Rq.Thumb.UpdateAfterSave(thumb);
        }

        return await Send(command);
    }

    public async Task<Result> DeleteAsync(DeleteCourseCommand command)
        => await Send(command);






    public async Task<Result<PagedResult<LectureModel>>> GetPagedAsync(GetPagedLecturesQuery query)
        => await Send<GetPagedLecturesQuery, PagedResult<LectureModel>>(query);

    public async Task<Result> CreateAsync(CreateLectureCommand command)
        => await Send(command);

    public async Task<Result> UpdateAsync(UpdateLectureCommand command)
        => await Send(command);

    public async Task<Result> DeleteAsync(DeleteLectureCommand command)
        => await Send(command);






    public async Task<Result<PagedResult<CommentModel>>> GetPagedLectureCommentsAsync(GetPagedCommentsQuery query)
        => await Send<GetPagedCommentsQuery, PagedResult<CommentModel>>(query);

    public async Task<Result> CreateLectureCommentAsync(CreateCommentCommand command)
        => await Send(command);

    public async Task<Result> UpdateLectureCommentAsync(UpdateCommentCommand command)
        => await Send(command);

    public async Task<Result> DeleteLectureCommentAsync(DeleteCommentCommand command)
        => await Send(command);






    public async Task<Result<PagedResult<ReviewModel>>> GetPagedCourseReviewsAsync(GetPagedReviewsQuery query)
        => await Send<GetPagedReviewsQuery, PagedResult<ReviewModel>>(query);

    public async Task<Result> CreateCourseReviewAsync(CreateReviewCommand command)
        => await Send(command);

    public async Task<Result> UpdateCourseReviewAsync(UpdateReviewCommand command)
        => await Send(command);






    public async Task<Result<PagedResult<CourseProgressModel>>> GetPagedAsync(GetPagedEnrollmentsQuery query)
        => await Send<GetPagedEnrollmentsQuery, PagedResult<CourseProgressModel>>(query);

    public async Task<Result> CreateAsync(CreateEnrollmentCommand command)
        => await Send(command);

    public async Task<Result> DeleteAsync(DeleteEnrollmentCommand command)
        => await Send(command);
}