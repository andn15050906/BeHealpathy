using Contract.Helpers.Storage;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Courses.AdvisorRequests;
using Contract.Requests.Courses.CategoryRequests;
using Contract.Requests.Courses.EnrollmentRequests;
using Contract.Requests.Courses.LectureRequests;
using Contract.Requests.Shared.BaseRequests.Comments;
using Contract.Requests.Shared.BaseRequests.Reviews;
using Contract.Responses.Courses;
using Contract.Responses.Shared;
using Contract.Services.Contracts;

namespace Contract.Services.Implementations;

public sealed class CourseHttpService : HttpApiClient, ICourseApiService
{
    private readonly IFileService _fileService;

    public CourseHttpService(HttpClient httpClient, IFileService fileService) : base(httpClient)
    {
        _fileService = fileService;
    }






    public async Task<Result<List<CategoryModel>>> GetCategoriesAsync(GetAllCategoriesQuery query)
        => await GetAsync<List<CategoryModel>>(API.Courses.CategoryBaseUri);

    public async Task<Result> CreateAsync(CreateCategoryCommand command)
        => await PostAsync(API.Courses.CategoryBaseUri, command);

    public async Task<Result> UpdateAsync(UpdateCategoryCommand command)
        => await PatchAsync(API.Courses.CategoryBaseUri, command.Rq);

    public async Task<Result> DeleteAsync(DeleteCategoryCommand command)
        => await DeleteAsync(API.Courses.DeleteCategoryByIdUri(command.Id));






    public async Task<Result<PagedResult<AdvisorModel>>> GetPagedAsync(GetPagedAdvisorsQuery query)
        => await GetAsync<PagedResult<AdvisorModel>>(API.Courses.InstructorBaseUri);

    public async Task<Result> CreateAsync(CreateAdvisorCommand command)
        => await PostAsync(API.Courses.InstructorBaseUri, command.Rq);

    public async Task<Result> UpdateAsync(UpdateAdvisorCommand command)
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
            await MediaWithStream.FromImageDto(command.Rq.Thumb, command.Id, _ => _fileService.GetCourseThumbIdentifier(command.Id))
        );
        command.Rq.Thumb.UpdateAfterSave(thumb);

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






    public async Task<Result<PagedResult<CommentModel>>> GetPagedLectureCommentsAsync(GetPagedCommentsQuery query)
        => await GetAsync<PagedResult<CommentModel>>(API.Courses.GetPagedLectureCommentsUri(query));

    public async Task<Result> CreateLectureCommentAsync(CreateCommentCommand command)
        => await PostAsync(API.Courses.LectureCommentBaseUri, command.Rq);

    public async Task<Result> UpdateLectureCommentAsync(UpdateCommentCommand command)
        => await PatchAsync(API.Courses.LectureCommentBaseUri, command);

    public async Task<Result> DeleteLectureCommentAsync(DeleteCommentCommand command)
        => await DeleteAsync(API.Courses.DeleteLectureCommentByIdUri(command.Id));






    public async Task<Result<PagedResult<ReviewModel>>> GetPagedCourseReviewsAsync(GetPagedReviewsQuery query)
        => await GetAsync<PagedResult<ReviewModel>>(API.Courses.GetPagedCourseReviewUri(query));

    public async Task<Result> CreateCourseReviewAsync(CreateReviewCommand command)
        => await PostAsync(API.Courses.CourseReviewBaseUri, command.Rq);

    public async Task<Result> UpdateCourseReviewAsync(UpdateReviewCommand command)
        => await PatchAsync(API.Courses.CourseReviewBaseUri, command.Rq);






    public async Task<Result<PagedResult<EnrollmentModel>>> GetPagedAsync(GetPagedEnrollmentsQuery query)
        => await GetAsync<PagedResult<EnrollmentModel>>(API.Courses.EnrollmentBaseUri);

    public async Task<Result> CreateAsync(CreateEnrollmentCommand command)
        => await PostAsync(API.Courses.EnrollmentBaseUri, command.Rq);

    public async Task<Result> DeleteAsync(DeleteEnrollmentCommand command)
        => await DeleteAsync(API.Courses.DeleteEnrollmentByIdUri(command.Id));
}
