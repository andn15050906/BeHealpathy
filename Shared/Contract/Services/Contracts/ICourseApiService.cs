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
using Core.Responses;

namespace Contract.Services.Contracts;

public interface ICourseApiService
{
    Task<Result<List<CategoryModel>>> GetCategoriesAsync(GetCategoriesQuery query);
    Task<Result> CreateAsync(CreateCategoryCommand command);
    Task<Result> UpdateAsync(UpdateCategoryCommand command);
    Task<Result> DeleteAsync(DeleteCategoryCommand command);



    Task<Result<PagedResult<InstructorModel>>> GetPagedAsync(GetPagedInstructorsQuery query);
    Task<Result> CreateAsync(CreateInstructorCommand command);
    Task<Result> UpdateAsync(UpdateInstructorCommand command);



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



    Task<Result<PagedResult<LectureCommentModel>>> GetPagedAsync(GetPagedLectureCommentsQuery query);
    Task<Result> CreateAsync(CreateLectureCommentCommand command);
    Task<Result> UpdateAsync(UpdateLectureCommentCommand command);
    Task<Result> DeleteAsync(DeleteLectureCommentCommand command);



    Task<Result<PagedResult<EnrollmentModel>>> GetPagedAsync(GetPagedEnrollmentsQuery query);
    Task<Result> CreateAsync(CreateEnrollmentCommand command);
    Task<Result> DeleteAsync(DeleteEnrollmentCommand command);



    Task<Result<PagedResult<CourseReviewModel>>> GetPagedAsync(GetPagedCourseReviewsQuery query);
    Task<Result> CreateAsync(CreateCourseReviewCommand command);
    Task<Result> UpdateAsync(UpdateCourseReviewCommand command);
}