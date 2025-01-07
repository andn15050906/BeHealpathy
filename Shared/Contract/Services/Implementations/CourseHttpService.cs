using Contract.Helpers.Storage;
using Contract.Messaging.ApiClients.Http;
using Contract.Messaging.Models;
using Contract.Requests.Courses.CategoryRequests;
using Contract.Requests.Courses.CourseReviewRequests;
using Contract.Requests.Courses.EnrollmentRequests;
using Contract.Requests.Courses.InstructorRequests;
using Contract.Requests.Courses.LectureRequests;
using Contract.Responses.Courses.CategoryModels;
using Contract.Responses.Courses.CourseModels;
using Contract.Responses.Courses.CourseReviewModels;
using Contract.Responses.Courses.EnrollmentModels;
using Contract.Responses.Courses.InstructorModels;
using Contract.Responses.Courses.LectureCommentModels;
using Contract.Responses.Courses.LectureModels;
using Contract.Services.Contracts;
using Core.Responses;

namespace Contract.Services.Implementations;

public sealed class CourseHttpService : HttpApiClient, ICourseApiService
{
    private readonly IFileService _fileService;

    public CourseHttpService(HttpClient httpClient, IFileService fileService) : base(httpClient)
    {
        _fileService = fileService;
    }






    public async Task<Result<List<CategoryModel>>> GetCategoriesAsync(GetCategoriesQuery query)
        => await GetAsync<List<CategoryModel>>(API.Courses.CategoryBaseUri);

    public async Task<Result> CreateAsync(CreateCategoryCommand command)
        => await PostAsync(API.Courses.CategoryBaseUri, command);

    public async Task<Result> UpdateAsync(UpdateCategoryCommand command)
        => await PatchAsync(API.Courses.CategoryBaseUri, command.Rq);

    public async Task<Result> DeleteAsync(DeleteCategoryCommand command)
        => await DeleteAsync(API.Courses.DeleteCategoryByIdUri(command.Id));






    public async Task<Result<PagedResult<InstructorModel>>> GetPagedAsync(GetPagedInstructorsQuery query)
        => await GetAsync<PagedResult<InstructorModel>>(API.Courses.InstructorBaseUri);

    public async Task<Result> CreateAsync(CreateInstructorCommand command)
        => await PostAsync(API.Courses.InstructorBaseUri, command.Rq);

    public async Task<Result> UpdateAsync(UpdateInstructorCommand command)
        => await PatchAsync(API.Courses.InstructorBaseUri, command.Rq);






    public async Task<Result<PagedResult<CourseOverviewModel>>> GetPagedAsync(GetPagedCoursesQuery query)
        => await GetAsync<PagedResult<CourseOverviewModel>>(API.Courses.GetPagedCoursesUri(query));

    public async Task<Result<CourseModel>> GetByIdAsync(GetCourseByIdQuery query)
        => await GetAsync<CourseModel>(API.Courses.GetCourseByIdUri(query.Id));

    public async Task<Result<PagedResult<CourseMinModel>>> GetMinAsync(GetMinimumCoursesQuery query)
        => await GetAsync<PagedResult<CourseMinModel>>(API.Courses.GetMinCourseUri(query));

    public async Task<Result<List<CourseOverviewModel>>> GetMultipleAsync(GetMultipleCoursesQuery query)
        => await GetAsync<List<CourseOverviewModel>>(API.Courses.GetMultipleCoursesUri(query.Ids));

    //Task<Result<List<CourseOverviewModel>>> GetSimilarAsync(Guid id);

    //thumb

    public async Task<Result> CreateAsync(CreateCourseCommand command)
    {
        var thumb = await _fileService.SaveImage(
            await MediaWithStream.FromImageDto(command.Rq.Thumb, _ => _fileService.GetCourseThumbIdentifier(command.Id))
        );
        command.Rq.Thumb.ReplaceFileWithUrl(thumb);

        var result = await PostAsync(API.Courses.CourseBaseUri, command);

        if (result.IsSuccessful)
            return result;

        // Compensating transaction
        if (thumb is not null)
            await _fileService.DeleteImage(thumb.Identifier);

        return result;
    }

    public async Task<Result> UpdateAsync(UpdateCourseCommand command)
        => await PatchAsync(API.Courses.CourseBaseUri, command.Rq);

    public async Task<Result> DeleteAsync(DeleteCourseCommand command)
        => await DeleteAsync(API.Courses.DeleteCourseByIdUri(command.Id));






    public async Task<Result<PagedResult<LectureModel>>> GetPagedAsync(GetPagedLecturesQuery query)
        => await GetAsync<PagedResult<LectureModel>>(API.Courses.GetPagedLecturesUri(query));

    public async Task<Result> CreateAsync(CreateLectureCommand command)
        => await PostAsync(API.Courses.LectureBaseUri, command.Rq);

    public async Task<Result> UpdateAsync(UpdateLectureCommand command)
        => await PatchAsync(API.Courses.LectureBaseUri, command.Rq);

    public async Task<Result> DeleteAsync(DeleteLectureCommand command)
        => await DeleteAsync(API.Courses.DeleteLectureByIdUri(command.Id));






    public async Task<Result<PagedResult<LectureCommentModel>>> GetPagedAsync(GetPagedLectureCommentsQuery query)
        => await GetAsync<PagedResult<LectureCommentModel>>(API.Courses.GetPagedCourseCommentsUri(query));

    public async Task<Result> CreateAsync(CreateLectureCommentCommand command)
        => await PostAsync(API.Courses.CourseCommentBaseUri, command.Rq);

    public async Task<Result> UpdateAsync(UpdateLectureCommentCommand command)
        => await PatchAsync(API.Courses.CourseCommentBaseUri, command);

    public async Task<Result> DeleteAsync(DeleteLectureCommentCommand command)
        => await DeleteAsync(API.Courses.DeleteCourseCommentByIdUri(command.Id));






    public async Task<Result<PagedResult<CourseReviewModel>>> GetPagedAsync(GetPagedCourseReviewsQuery query)
        => await GetAsync<PagedResult<CourseReviewModel>>(API.Courses.GetPagedCourseReviewUri(query));

    public async Task<Result> CreateAsync(CreateCourseReviewCommand command)
        => await PostAsync(API.Courses.CourseReviewBaseUri, command.Rq);

    public async Task<Result> UpdateAsync(UpdateCourseReviewCommand command)
        => await PatchAsync(API.Courses.CourseReviewBaseUri, command.Rq);






    public async Task<Result<PagedResult<EnrollmentModel>>> GetPagedAsync(GetPagedEnrollmentsQuery query)
        => await GetAsync<PagedResult<EnrollmentModel>>(API.Courses.EnrollmentBaseUri);

    public async Task<Result> CreateAsync(CreateEnrollmentCommand command)
        => await PostAsync(API.Courses.EnrollmentBaseUri, command.Rq);

    public async Task<Result> DeleteAsync(DeleteEnrollmentCommand command)
        => await DeleteAsync(API.Courses.DeleteEnrollmentByIdUri(command.Id));
}
