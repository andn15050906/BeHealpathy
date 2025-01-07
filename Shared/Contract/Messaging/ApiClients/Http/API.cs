using Contract.Requests.Courses.CourseReviewRequests;
using Contract.Requests.Courses.InstructorRequests;
using Contract.Requests.Courses.LectureRequests;
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

        public static string GetPagedInstructorUri(GetPagedInstructorsQuery query)
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

        public static string GetPagedCourseReviewUri(GetPagedCourseReviewsQuery query)
            => $"{CourseReviewBaseUri}?{QueryBuilder.BuildQuery(query.Rq)}";






        public const string CourseCommentBaseUri = "api/coursecomments";

        public static string GetPagedCourseCommentsUri(GetPagedLectureCommentsQuery query)
            => $"{CourseCommentBaseUri}?{QueryBuilder.BuildQuery(query.Rq)}";

        public static string DeleteCourseCommentByIdUri(Guid id)
            => $"{CourseCommentBaseUri}/{id}";






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
