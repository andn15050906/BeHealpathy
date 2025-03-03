using Contract.Requests.Courses.AdvisorRequests;
using Contract.Requests.Courses.CategoryRequests;
using Contract.Requests.Courses.EnrollmentRequests;
using Contract.Requests.Courses.LectureRequests;
using Contract.Requests.Shared.BaseRequests.Comments;
using Contract.Requests.Shared.BaseRequests.Reviews;
using Contract.Responses.Courses;
using Contract.Responses.Shared;

namespace Contract.Services.Contracts.Domain;

public interface ICourseApiService
{
    Task<Result<List<CategoryModel>>> GetCategoriesAsync(GetAllCategoriesQuery query);
    Task<Result> CreateAsync(CreateCategoryCommand command);
    Task<Result> UpdateAsync(UpdateCategoryCommand command);
    Task<Result> DeleteAsync(DeleteCategoryCommand command);



    Task<Result<PagedResult<AdvisorModel>>> GetPagedAsync(GetPagedAdvisorsQuery query);
    Task<Result> CreateAsync(CreateAdvisorCommand command);
    Task<Result> UpdateAsync(UpdateAdvisorCommand command);



    Task<Result<CourseModel>> GetByIdAsync(GetCourseByIdQuery query);
    Task<Result<PagedResult<CourseOverviewModel>>> GetPagedAsync(GetPagedCoursesQuery query);
    Task<Result<PagedResult<CourseMinModel>>> GetMinAsync(GetMinimumCoursesQuery query);
    Task<Result<List<CourseOverviewModel>>> GetMultipleAsync(GetMultipleCoursesQuery query);
    //Task<Result<List<CourseOverviewModel>>> GetSimilarAsync(Guid id);
    //thumb
    Task<Result> CreateAsync(CreateCourseCommand command);
    Task<Result> UpdateAsync(UpdateCourseCommand command);
    Task<Result> DeleteAsync(DeleteCourseCommand command);



    Task<Result<PagedResult<LectureModel>>> GetPagedAsync(GetPagedLecturesQuery query);
    Task<Result> CreateAsync(CreateLectureCommand command);
    Task<Result> UpdateAsync(UpdateLectureCommand command);
    Task<Result> DeleteAsync(DeleteLectureCommand command);



    Task<Result<PagedResult<CommentModel>>> GetPagedLectureCommentsAsync(GetPagedCommentsQuery query);
    Task<Result> CreateLectureCommentAsync(CreateCommentCommand command);
    Task<Result> UpdateLectureCommentAsync(UpdateCommentCommand command);
    Task<Result> DeleteLectureCommentAsync(DeleteCommentCommand command);



    Task<Result<PagedResult<EnrollmentModel>>> GetPagedAsync(GetPagedEnrollmentsQuery query);
    Task<Result> CreateAsync(CreateEnrollmentCommand command);
    Task<Result> DeleteAsync(DeleteEnrollmentCommand command);



    Task<Result<PagedResult<ReviewModel>>> GetPagedCourseReviewsAsync(GetPagedReviewsQuery query);
    Task<Result> CreateCourseReviewAsync(CreateReviewCommand command);
    Task<Result> UpdateCourseReviewAsync(UpdateReviewCommand command);
}