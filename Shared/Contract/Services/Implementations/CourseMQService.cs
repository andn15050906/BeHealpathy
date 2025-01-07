using MassTransit;
using Contract.Services.Contracts;
using Contract.Messaging.ApiClients.MQ;
using Contract.Messaging.Models;
using Contract.Responses.Courses.CategoryModels;
using Contract.Requests.Courses.CategoryRequests;
using Core.Responses;
using Contract.Responses.Courses.InstructorModels;
using Contract.Requests.Courses.InstructorRequests;
using Contract.Responses.Courses.CourseModels;
using Contract.Responses.Courses.LectureModels;
using Contract.Requests.Courses.LectureRequests;
using Contract.Responses.Courses.LectureCommentModels;
using Contract.Requests.Courses.CourseReviewRequests;
using Contract.Responses.Courses.CourseReviewModels;
using Contract.Requests.Courses.EnrollmentRequests;
using Contract.Responses.Courses.EnrollmentModels;
using Contract.Helpers.Storage;

namespace Contract.Services.Implementations;

public sealed class CourseMQService : MQApiClient, ICourseApiService
{
    private readonly IFileService _fileService;

    public CourseMQService(IScopedClientFactory factory, IFileService fileService) : base(factory)
    {
        _fileService = fileService;
    }






    public async Task<Result<List<CategoryModel>>> GetCategoriesAsync(GetCategoriesQuery query)
        => await Send<GetCategoriesQuery, List<CategoryModel>>(query);

    public async Task<Result> CreateAsync(CreateCategoryCommand command)
        => await Send(command);

    public async Task<Result> UpdateAsync(UpdateCategoryCommand command)
        => await Send(command);

    public async Task<Result> DeleteAsync(DeleteCategoryCommand command)
        => await Send(command);






    public async Task<Result<PagedResult<InstructorModel>>> GetPagedAsync(GetPagedInstructorsQuery query)
        => await Send<GetPagedInstructorsQuery, PagedResult<InstructorModel>>(query);

    public async Task<Result> CreateAsync(CreateInstructorCommand command)
        => await Send(command);

    public async Task<Result> UpdateAsync(UpdateInstructorCommand command)
        => await Send(command);






    public async Task<Result<PagedResult<CourseOverviewModel>>> GetPagedAsync(GetPagedCoursesQuery query)
        => await Send<GetPagedCoursesQuery, PagedResult<CourseOverviewModel>>(query);

    public async Task<Result<CourseModel>> GetByIdAsync(GetCourseByIdQuery query)
        => await Send<GetCourseByIdQuery, CourseModel>(query);

    public async Task<Result<PagedResult<CourseMinModel>>> GetMinAsync(GetMinimumCoursesQuery query)
        => await Send<GetMinimumCoursesQuery, PagedResult<CourseMinModel>>(query);

    public async Task<Result<List<CourseOverviewModel>>> GetMultipleAsync(GetMultipleCoursesQuery query)
        => await Send<GetMultipleCoursesQuery, List<CourseOverviewModel>>(query);

    //Task<Result<List<CourseOverviewModel>>> GetSimilarAsync(Guid id);

    //thumb

    public async Task<Result> CreateAsync(CreateCourseCommand command)
    {
        var thumb = await _fileService.SaveImage(
            await MediaWithStream.FromImageDto(command.Rq.Thumb, _ => _fileService.GetCourseThumbIdentifier(command.Id))
        );

        command.Rq.Thumb.ReplaceFileWithUrl(thumb);
        return await Send(command);
    }

    public async Task<Result> UpdateAsync(UpdateCourseCommand command)
    {
        if (command.Rq.Thumb is not null)
        {
            var thumb = await _fileService.SaveImage(
                await MediaWithStream.FromImageDto(command.Rq.Thumb, _ => _fileService.GetCourseThumbIdentifier(command.Rq.Id))
            );

            command.Rq.Thumb.ReplaceFileWithUrl(thumb);
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






    public async Task<Result<PagedResult<LectureCommentModel>>> GetPagedAsync(GetPagedLectureCommentsQuery query)
        => await Send<GetPagedLectureCommentsQuery, PagedResult<LectureCommentModel>>(query);

    public async Task<Result> CreateAsync(CreateLectureCommentCommand command)
        => await Send(command);

    public async Task<Result> UpdateAsync(UpdateLectureCommentCommand command)
        => await Send(command);

    public async Task<Result> DeleteAsync(DeleteLectureCommentCommand command)
        => await Send(command);






    public async Task<Result<PagedResult<CourseReviewModel>>> GetPagedAsync(GetPagedCourseReviewsQuery query)
        => await Send<GetPagedCourseReviewsQuery, PagedResult<CourseReviewModel>>(query);

    public async Task<Result> CreateAsync(CreateCourseReviewCommand command)
        => await Send(command);

    public async Task<Result> UpdateAsync(UpdateCourseReviewCommand command)
        => await Send(command);






    public async Task<Result<PagedResult<EnrollmentModel>>> GetPagedAsync(GetPagedEnrollmentsQuery query)
        => await Send<GetPagedEnrollmentsQuery, PagedResult<EnrollmentModel>>(query);

    public async Task<Result> CreateAsync(CreateEnrollmentCommand command)
        => await Send(command);

    public async Task<Result> DeleteAsync(DeleteEnrollmentCommand command)
        => await Send(command);
}