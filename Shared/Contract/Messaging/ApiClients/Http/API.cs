using Contract.Requests.Courses.AdvisorRequests;
using Contract.Requests.Courses.LectureRequests;
using Contract.Requests.Shared.BaseRequests.Comments;
using Contract.Requests.Shared.BaseRequests.Reviews;
using Core.Helpers;

namespace Contract.Messaging.ApiClients.Http;

public static class API
{
    public static class Courses
    {
        public const string CategoryBaseUri = "api/categories";

        public static string DeleteCategoryByIdUri(Guid id)
            => $"{CategoryBaseUri}/{id}";






        public const string InstructorBaseUri = "api/instructors";

        public static string GetPagedInstructorUri(GetPagedAdvisorsQuery query)
            => $"{InstructorBaseUri}?{QueryBuilder.BuildQuery(query.Rq)}";






        public const string CourseBaseUri = "api/courses";

        public static string GetCourseByIdUri(Guid id)
            => $"{CourseBaseUri}/{id}";

        public static string GetPagedCoursesUri(GetPagedCoursesQuery query)
            => $"{CourseBaseUri}?{QueryBuilder.BuildQuery(query.Rq)}";

        public static string GetMinCourseUri(GetMinimumCoursesQuery query)
            => $"{CourseBaseUri}?{QueryBuilder.BuildQuery(query.Rq)}";

        public static string GetMultipleCoursesUri(IEnumerable<Guid> ids)
            => QueryBuilder.BuildWithArray($"{CourseBaseUri}/multiple?", "ids={0}&", ids.Select(_ => _.ToString()));

        //public static string GetSingleCourseUri(GetSingleCourseQuery query)
        //    => $"{CourseBaseUri}/single?{QueryBuilder.BuildQuery(query.Rq)}";

        public static string DeleteCourseByIdUri(Guid id)
            => $"{CourseBaseUri}/{id}";






        public const string LectureBaseUri = "api/lectures";

        public static string GetPagedLecturesUri(GetPagedLecturesQuery query)
            => $"{LectureBaseUri}?{QueryBuilder.BuildQuery(query.Rq)}";

        public static string DeleteLectureByIdUri(Guid id)
            => $"{LectureBaseUri}/{id}";






        public const string CourseReviewBaseUri = "api/coursereviews";

        public static string GetPagedCourseReviewUri(GetPagedReviewsQuery query)
            => $"{CourseReviewBaseUri}?{QueryBuilder.BuildQuery(query.Rq)}";






        public const string LectureCommentBaseUri = "api/LectureComments";

        public static string GetPagedLectureCommentsUri(GetPagedCommentsQuery query)
            => $"{LectureCommentBaseUri}?{QueryBuilder.BuildQuery(query.Rq)}";

        public static string DeleteLectureCommentByIdUri(Guid id)
            => $"{LectureCommentBaseUri}/{id}";






        public const string EnrollmentBaseUri = "api/enrollments";

        public static string DeleteEnrollmentByIdUri(Guid id)
            => $"{EnrollmentBaseUri}/{id}";
    }






    public static class Assessment
    {
        public const string AssignmentBaseUri = "api/assignments";

        public static string GetAssignmentByIdUri(Guid id)
            => $"{AssignmentBaseUri}/{id}";

        public static string GetMinAssignmentByIdUri(Guid id)
            => $"{AssignmentBaseUri}/{id}/min";

        /*public static string GetPagedAssignmentsUri(GetPagedAssignmentsQuery query)
            => $"{AssignmentBaseUri}?{QueryBuilder.BuildQuery(query.Rq)}";*/

        public static string DeleteAssignmentByIdUri(Guid id)
            => $"{AssignmentBaseUri}/{id}";






        public const string SubmissionBaseUri = "api/submissions";

        public static string GetSubmissionByIdUri(Guid id)
            => $"{SubmissionBaseUri}/{id}";

        /*public static string GetPagedSubmissionsUri(GetPagedSubmissionsQuery query)
            => $"{SubmissionBaseUri}/{QueryBuilder.BuildQuery(query.Rq)}";*/
    }
}
