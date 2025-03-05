using Contract.Domain.CommunityAggregate;
using Contract.Domain.Shared.NotificationAggregate;
using Contract.Domain.Shared;
using Infrastructure.DataAccess.SQLServer;
using Microsoft.EntityFrameworkCore;
using Contract.Domain.CourseAggregate;
using Contract.Domain.LibraryAggregate;
using Contract.Domain.ProgressAggregates;
using Infrastructure.DataAccess.SQLServer.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;
using Contract.Domain.Shared.MultimediaBase.Enums;
using Contract.Domain.Shared.MultimediaBase;

namespace Gateway.Infrastructure;

public sealed class HealpathyContext : BaseContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<Preference> Preferences { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }

    public DbSet<Survey> Surveys { get; set; }
    public DbSet<SurveyScoreBand> SurveyScoreBands { get; set; }
    public DbSet<McqQuestion> McqQuestions { get; set; }
    public DbSet<McqAnswer> McqAnswers { get; set; }
    public DbSet<McqChoice> McqChoices { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<Routine> Routines { get; set; }
    public DbSet<RoutineLog> RoutineLogs { get; set; }
    public DbSet<DiaryNote> DiaryNotes { get; set; }

    public DbSet<Article> Articles { get; set; }
    public DbSet<ArticleSection> ArticleSections { get; set; }
    public DbSet<ArticleComment> ArticleComments { get; set; }
    public DbSet<ArticleReaction> ArticleReactions { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ArticleTag> ArticleTags { get; set; }
    public DbSet<MediaResource> MediaResources { get; set; }

    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<ConversationMember> ConversationMembers { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<MessageReaction> MessageReactions { get; set; }
    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<MeetingParticipant> MeetingParticipants { get; set; }

    public DbSet<Advisor> Advisors { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<CourseReview> CourseReviews { get; set; }
    public DbSet<Lecture> Lectures { get; set; }
    public DbSet<LectureComment> LectureComments { get; set; }
    public DbSet<LectureReaction> LectureReactions { get; set; }

    // DbSet instead of Shared Type Entity
    public DbSet<Multimedia> Multimedia { get; set; }



    struct RelationsConfig
    {
        internal const string USER = "Users";
        internal const string SETTING = "Settings";
        internal const string PREFERENCE = "Preferences";
        internal const string NOTIFICATION = "Notifications";
        internal const string BILL = "Bills";
        internal const string ACTIVITY_LOG = "ActivityLogs";
        internal const string CONVERSATION = "Conversations";
        internal const string CONVERSATION_MEMBER = "ConversationMembers";
        internal const string CHAT_MESSAGE = "ChatMessages";
        internal const string MESSAGE_REACTION = "MessageReactions";
        internal const string MEETING = "Meetings";
        internal const string MEETING_PARTICIPANT = "MeetingParticipants";

        internal const string SURVEY = "Surveys";
        internal const string SURVEY_SCORE_BAND = "SurveyScoreBands";
        internal const string MCQ_QUESTION = "McqQuestions";
        internal const string MCQ_ANSWER = "McqAnswers";
        internal const string MCQ_CHOICE = "McqChoices";
        internal const string SUBMISSION = "Submissions";
        internal const string ROUTINE = "Routines";
        internal const string ROUTINE_LOG = "RoutineLogs";
        internal const string DIARY_NOTE = "DiaryNotes";

        internal const string ARTICLE = "Articles";
        internal const string ARTICLE_SECTION = "ArticleSections";
        internal const string ARTICLE_COMMENT = "ArticleComments";
        internal const string ARTICLE_REACTION = "ArticleReactions";
        internal const string TAG = "Tags";
        internal const string ARTICLE_TAG = "ArticleTags";
        internal const string MEDIA_RESOURCE = "MediaResources";
        internal const string ADVISOR = "Advisors";
        internal const string CATEGORY = "Categories";
        internal const string COURSE = "Courses";
        internal const string ENROLLMENT = "Enrollments";
        internal const string COURSE_REVIEW = "CourseReviews";
        internal const string LECTURE = "Lectures";
        internal const string LECTURE_COMMENT = "LectureComments";
        internal const string LECTURE_REACTION = "LectureReactions";

        internal const string MULTIMEDIA = "Multimedias";
    }

    private struct Triggers
    {
        internal const string onCourseInsertDelete = "onCourseInsertDelete";
        internal const string onLectureInsertDelete = "onLectureInsertDelete";
        internal const string onEnrollmentInsertDelete = "onEnrollmentInsertDelete";
        internal const string onCourseReviewInsertDelete = "onCourseReviewInsertDelete";
    }



    public HealpathyContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Multimedia>(media =>
        {
            media.Property(_ => _.Type).HasConversion(_ => _.ToString(), _ => (MediaType)Enum.Parse(typeof(MediaType), _));
            media.Property(_ => _.Url).HasColumnType("VARCHAR(255)");
            media.Property(_ => _.Title).HasColumnType("NVARCHAR(255)");
        });

        modelBuilder
            .ApplyConfiguration(new UserConfig())
            .ApplyConfiguration(new SettingConfig())
            .ApplyConfiguration(new PreferenceConfig())
            .ApplyConfiguration(new NotificationConfig())
            .ApplyConfiguration(new BillConfig())
            .ApplyConfiguration(new ActivityLogConfig())
            .ApplyConfiguration(new ConversationConfig())
            .ApplyConfiguration(new ConversationMemberConfig())
            .ApplyConfiguration(new ChatMessageConfig())
            .ApplyConfiguration(new MessageReactionConfig())
            .ApplyConfiguration(new MeetingConfig())
            .ApplyConfiguration(new MeetingParticipantConfig())

            .ApplyConfiguration(new SurveyConfig())
            .ApplyConfiguration(new SurveyScoreBandConfig())
            .ApplyConfiguration(new McqQuestionConfig())
            .ApplyConfiguration(new McqAnswerConfig())
            .ApplyConfiguration(new McqChoiceConfig())
            .ApplyConfiguration(new SubmissionConfig())
            .ApplyConfiguration(new RoutineConfig())
            .ApplyConfiguration(new RoutineLogConfig())
            .ApplyConfiguration(new DiaryNoteConfig())

            .ApplyConfiguration(new ArticleConfig())
            .ApplyConfiguration(new ArticleSectionConfig())
            .ApplyConfiguration(new ArticleCommentConfig())
            .ApplyConfiguration(new ArticleReactionConfig())
            .ApplyConfiguration(new TagConfig())
            .ApplyConfiguration(new ArticleTagConfig())
            .ApplyConfiguration(new MediaResourceConfig())
            .ApplyConfiguration(new AdvisorConfig())
            .ApplyConfiguration(new CategoryConfig())
            .ApplyConfiguration(new CourseConfig())
            .ApplyConfiguration(new EnrollmentConfig())
            .ApplyConfiguration(new CourseReviewConfig())
            .ApplyConfiguration(new LectureConfig())
            .ApplyConfiguration(new LectureCommentConfig())
            .ApplyConfiguration(new LectureReactionConfig());
    }



    #region Gateway
    class UserConfig : EntityConfiguration<User>
    {
        protected override Dictionary<Expression<Func<User, object?>>, string> Columns => new()
        {
            { _ => _.UserName, VARCHAR45 },
            { _ => _.Password, VARCHAR100 },
            { _ => _.Email, VARCHAR45 },
            { _ => _.FullName, NVARCHAR45 },
            { _ => _.MetaFullName, VARCHAR45 },
            { _ => _.AvatarUrl, VARCHAR100 },
            // Role below
            { _ => _.Token, VARCHAR100 },
            { _ => _.RefreshToken, VARCHAR100 },
            // IsVerified
            // IsApproved
            // IsBanned
            // AccessFailedCount
            // UnbanDate
            { _ => _.Bio, NVARCHAR1000 },
            // DateOfBirth
            { _ => _.Phone, VARCHAR45 },

            // EnrollmentCount
        };

        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .ToTable(RelationsConfig.USER)
                .SetColumnsTypes(Columns)
                .SetEnumParsing(_ => _.Role)
                .SetUnique(_ => _.UserName, _ => _.Email, _ => _.Phone)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);

            builder.OwnsMany(_ => _.UserLogins, login =>
            {
                login.Property(_ => _.LoginProvider).HasColumnType(VARCHAR100);
                login.Property(_ => _.ProviderKey).HasColumnType(VARCHAR100);
            });

            // System User
            builder.HasData(new User { Id = Guid.Parse("00000000-0000-0000-0000-000000000001") });
        }
    }

    class SettingConfig : EntityConfiguration<Setting>
    {
        protected override Dictionary<Expression<Func<Setting, object?>>, string> Columns => new()
        {
            { _ => _.Title, VARCHAR100 },
            { _ => _.Choice, VARCHAR100 }
        };

        public override void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder
                .ToTable(RelationsConfig.SETTING)
                .SetColumnsTypes(Columns)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);
        }
    }

    class PreferenceConfig : EntityConfiguration<Preference>
    {
        protected override Dictionary<Expression<Func<Preference, object?>>, string> Columns => new()
        {
            { _ => _.Value, NVARCHAR100 }
        };

        public override void Configure(EntityTypeBuilder<Preference> builder)
        {
            builder
                .ToTable(RelationsConfig.PREFERENCE)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);
        }
    }

    class NotificationConfig : EntityConfiguration<Notification>
    {
        protected override Dictionary<Expression<Func<Notification, object?>>, string> Columns => new()
        {
            { _ => _.Message, NVARCHAR1000 },
            // Type below
            // Status below
            // ReceiverId
        };

        public override void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder
                .ToTable(RelationsConfig.NOTIFICATION)
                .SetColumnsTypes(Columns)
                .SetEnumParsing(_ => _.Type).SetEnumParsing(_ => _.Status)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);

            builder.HasOne<User>().WithMany().HasForeignKey(_ => _.CreatorId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne<User>().WithMany().HasForeignKey(_ => _.ReceiverId).OnDelete(DeleteBehavior.NoAction);
        }
    }

    class BillConfig : EntityConfiguration<Bill>
    {
        protected override Dictionary<Expression<Func<Bill, object?>>, string> Columns => new()
        {
            { _ => _.Action, VARCHAR100 },
            { _ => _.Note, NVARCHAR255 },
            // Amount
            { _ => _.Gateway, VARCHAR20 },
            { _ => _.TransactionId, VARCHAR100 },
            { _ => _.ClientTransactionId, VARCHAR100 },
            { _ => _.Token, VARCHAR100 }
            // IsSuccessful
        };

        public override void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder
                .ToTable(RelationsConfig.BILL)
                .SetColumnsTypes(Columns)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);

            builder.HasOne<User>().WithMany().HasForeignKey(_ => _.CreatorId).OnDelete(DeleteBehavior.NoAction);
        }
    }

    class ActivityLogConfig : EntityConfiguration<ActivityLog>
    {
        protected override Dictionary<Expression<Func<ActivityLog, object?>>, string> Columns => new()
        {
            { _ => _.Content, NVARCHARMAX }
        };

        public override void Configure(EntityTypeBuilder<ActivityLog> builder)
        {
            builder
                .ToTable(RelationsConfig.ACTIVITY_LOG)
                .SetColumnsTypes(Columns)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);

            builder.HasOne<User>().WithMany().HasForeignKey(_ => _.CreatorId).OnDelete(DeleteBehavior.NoAction);
        }
    }

    class ConversationConfig : EntityConfiguration<Conversation>
    {
        protected override Dictionary<Expression<Func<Conversation, object?>>, string> Columns => new()
        {
            { _ => _.Title, NVARCHAR45 },
            // IsPrivate
            { _ => _.AvatarUrl, NVARCHAR255 }
        };

        public override void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder
                .ToTable(RelationsConfig.CONVERSATION)
                .SetColumnsTypes(Columns)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);

            builder.HasOne<User>().WithMany().HasForeignKey(_ => _.CreatorId).OnDelete(DeleteBehavior.NoAction);
        }
    }

    class ConversationMemberConfig : EntityConfiguration<ConversationMember>
    {
        protected override Dictionary<Expression<Func<ConversationMember, object?>>, string> Columns => new()
        {
            // IsAdmin
            // LastVisit
        };

        public override void Configure(EntityTypeBuilder<ConversationMember> builder)
        {
            builder
                .ToTable(RelationsConfig.CONVERSATION_MEMBER)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .HasKey(_ => new { _.CreatorId, _.ConversationId });
        }
    }

    class ChatMessageConfig : EntityConfiguration<ChatMessage>
    {
        protected override Dictionary<Expression<Func<ChatMessage, object?>>, string> Columns => new()
        {
            { _ => _.Content, NVARCHARMAX },
            // Status below
        };

        public override void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder
                .ToTable(RelationsConfig.CHAT_MESSAGE)
                .SetColumnsTypes(Columns)
                .SetEnumParsing(_ => _.Status)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);

            builder.HasOne<User>().WithMany().HasForeignKey(_ => _.CreatorId).OnDelete(DeleteBehavior.NoAction);
            //...
            builder.Ignore(_ => _.Attachments);
        }
    }

    class MessageReactionConfig : EntityConfiguration<MessageReaction>
    {
        protected override Dictionary<Expression<Func<MessageReaction, object?>>, string> Columns => new()
        {
            // SourceId
            { _ => _.Content, NVARCHAR45 }
        };

        public override void Configure(EntityTypeBuilder<MessageReaction> builder)
        {
            builder
                .ToTable(RelationsConfig.MESSAGE_REACTION)
                .SetColumnsTypes(Columns)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);

            builder.HasOne<User>().WithMany().OnDelete(DeleteBehavior.NoAction);
        }
    }

    class MeetingConfig : EntityConfiguration<Meeting>
    {
        protected override Dictionary<Expression<Func<Meeting, object?>>, string> Columns => new()
        {
            { _ => _.Title, NVARCHAR100 },
            { _ => _.Description, NVARCHAR1000 },
            // StartAt
            // EndAt
            // MaxParticipants
        };

        public override void Configure(EntityTypeBuilder<Meeting> builder)
        {
            builder
                .ToTable(RelationsConfig.MEETING)
                .SetColumnsTypes(Columns)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);

            builder.HasMany(_ => _.Participants).WithOne();
            builder.HasOne(_ => _.Creator).WithMany().OnDelete(DeleteBehavior.NoAction);
        }
    }

    class MeetingParticipantConfig : EntityConfiguration<MeetingParticipant>
    {
        protected override Dictionary<Expression<Func<MeetingParticipant, object?>>, string> Columns => new()
        {
            // MeetingId
            // IsHost
        };

        public override void Configure(EntityTypeBuilder<MeetingParticipant> builder)
        {
            builder
                .ToTable(RelationsConfig.MEETING_PARTICIPANT)
                .SetColumnsTypes(Columns)
                .SetEnumParsing(_ => _.Status)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .HasKey(_ => new { _.MeetingId, _.UserId });
        }
    }
    #endregion



    #region Progress
    class SurveyConfig : EntityConfiguration<Survey>
    {
        protected override Dictionary<Expression<Func<Survey, object?>>, string> Columns => new()
        {
            { _ => _.Name, NVARCHAR255 },
            { _ => _.Description, NVARCHAR1000 }
        };

        public override void Configure(EntityTypeBuilder<Survey> builder)
        {
            builder
                .ToTable(RelationsConfig.SURVEY)
                .SetColumnsTypes(Columns);

            builder.HasMany(_ => _.Questions).WithOne();
            builder.HasMany(_ => _.Bands).WithOne();
        }
    }

    class SurveyScoreBandConfig : EntityConfiguration<SurveyScoreBand>
    {
        protected override Dictionary<Expression<Func<SurveyScoreBand, object?>>, string> Columns => new()
        {
            // MinScore
            // MaxScore
            { _ => _.BandName, NVARCHAR45 },
            { _ => _.BandRating, NVARCHAR45 }
        };
        
        public override void Configure(EntityTypeBuilder<SurveyScoreBand> builder)
        {
            builder
                .ToTable(RelationsConfig.SURVEY_SCORE_BAND)
                .SetColumnsTypes(Columns);
        }
    }

    class McqQuestionConfig : EntityConfiguration<McqQuestion>
    {
        protected override Dictionary<Expression<Func<McqQuestion, object?>>, string> Columns => new()
        {
            { _ => _.Content, NVARCHAR255 },
            { _ => _.Explanation, NVARCHAR255 }
            // SurveyId
        };

        public override void Configure(EntityTypeBuilder<McqQuestion> builder)
        {
            builder
                .ToTable(RelationsConfig.MCQ_QUESTION)
                .SetColumnsTypes(Columns);

            builder.HasMany(_ => _.Answers).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }

    class McqAnswerConfig : EntityConfiguration<McqAnswer>
    {
        protected override Dictionary<Expression<Func<McqAnswer, object?>>, string> Columns => new()
        {
            { _ => _.Content, NVARCHAR255 }
            // Score
        };

        public override void Configure(EntityTypeBuilder<McqAnswer> builder)
        {
            builder
                .ToTable(RelationsConfig.MCQ_ANSWER)
                .SetColumnsTypes(Columns);
        }
    }

    class McqChoiceConfig : EntityConfiguration<McqChoice>
    {
        protected override Dictionary<Expression<Func<McqChoice, object?>>, string> Columns => new()
        {
            // SubmissionId
            // McqQuestionId
            // McqAnswerId
        };

        public override void Configure(EntityTypeBuilder<McqChoice> builder)
        {
            builder
                .ToTable(RelationsConfig.MCQ_CHOICE)
                .SetColumnsTypes(Columns)
                .HasKey(_ => new { _.SubmissionId, _.McqQuestionId });
        }
    }

    class SubmissionConfig : EntityConfiguration<Submission>
    {
        protected override Dictionary<Expression<Func<Submission, object?>>, string> Columns => new()
        {
        };

        public override void Configure(EntityTypeBuilder<Submission> builder)
        {
            builder
                .ToTable(RelationsConfig.SUBMISSION)
                .SetColumnsTypes(Columns)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);

            //builder.HasMany(_ => _.Choices).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(_ => _.Survey).WithMany().OnDelete(DeleteBehavior.NoAction);
        }
    }

    class RoutineConfig : EntityConfiguration<Routine>
    {
        protected override Dictionary<Expression<Func<Routine, object?>>, string> Columns => new()
        {
            { _ => _.Title, NVARCHAR100 },
            { _ => _.Description, NVARCHAR255 },
            { _ => _.Objective, NVARCHAR255 }
            // Frequency below
        };

        public override void Configure(EntityTypeBuilder<Routine> builder)
        {
            builder
                .ToTable(RelationsConfig.ROUTINE)
                .SetColumnsTypes(Columns)
                .SetEnumParsing(_ => _.Frequency)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);

            builder.HasMany(_ => _.Logs).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }

    class RoutineLogConfig : EntityConfiguration<RoutineLog>
    {
        protected override Dictionary<Expression<Func<RoutineLog, object?>>, string> Columns => new()
        {
            // RoutineId
            { _ => _.Content, VARCHAR100 }
        };

        public override void Configure(EntityTypeBuilder<RoutineLog> builder)
        {
            builder
                .ToTable(RelationsConfig.ROUTINE_LOG)
                .SetColumnsTypes(Columns)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);
        }
    }

    class DiaryNoteConfig : EntityConfiguration<DiaryNote>
    {
        protected override Dictionary<Expression<Func<DiaryNote, object?>>, string> Columns => new()
        {
            { _ => _.Title, NVARCHAR255 },
            { _ => _.Content, NVARCHAR255 },
            { _ => _.Mood, NVARCHAR255 },
            { _ => _.Theme, NVARCHAR255 }
        };

        public override void Configure(EntityTypeBuilder<DiaryNote> builder)
        {
            builder
                .ToTable(RelationsConfig.DIARY_NOTE)
                .SetColumnsTypes(Columns)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);

            //...
            builder.Ignore(_ => _.Attachments);
        }
    }
    #endregion



    #region Resource
    class ArticleConfig : EntityConfiguration<Article>
    {
        protected override Dictionary<Expression<Func<Article, object?>>, string> Columns => new()
        {
            { _ => _.Title, NVARCHAR255 },
            { _ => _.Status, VARCHAR100 },
            // IsCommentDisabled
            // CommentCount
            // ViewCount
        };

        public override void Configure(EntityTypeBuilder<Article> builder)
        {
            builder
                .ToTable(RelationsConfig.ARTICLE)
                .SetColumnsTypes(Columns)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);

            builder.Ignore(_ => _.Thumb);
            builder.HasMany(_ => _.Tags).WithMany().UsingEntity<ArticleTag>();
            builder.HasMany(_ => _.Comments).WithOne().OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(_ => _.Reactions).WithOne().OnDelete(DeleteBehavior.NoAction);
        }
    }

    class ArticleSectionConfig : EntityConfiguration<ArticleSection>
    {
        protected override Dictionary<Expression<Func<ArticleSection, object?>>, string> Columns => new()
        {
            { _ => _.Header, NVARCHAR255 },
            { _ => _.Content, NVARCHAR3000 }
        };

        public override void Configure(EntityTypeBuilder<ArticleSection> builder)
        {
            builder
                .ToTable(RelationsConfig.ARTICLE_SECTION)
                .SetColumnsTypes(Columns);

            builder.Ignore(_ => _.Media);
        }
    }

    class ArticleCommentConfig : EntityConfiguration<ArticleComment>
    {
        protected override Dictionary<Expression<Func<ArticleComment, object?>>, string> Columns => new()
        {
            { _ => _.Content, NVARCHAR500 },
            // Status below
        };

        public override void Configure(EntityTypeBuilder<ArticleComment> builder)
        {
            builder
                .ToTable(RelationsConfig.ARTICLE_COMMENT)
                .SetColumnsTypes(Columns)
                .SetEnumParsing(_ => _.Status)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);

            //builder.OwnsMany(_ => _.Medias);
            builder.Ignore(_ => _.Medias);
            builder.HasMany(_ => _.Replies).WithOne().OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(_ => _.Reactions).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }

    class ArticleReactionConfig : EntityConfiguration<ArticleReaction>
    {
        protected override Dictionary<Expression<Func<ArticleReaction, object?>>, string> Columns => new()
        {
            // SourceId
            { _ => _.Content, NVARCHAR45 }
        };

        public override void Configure(EntityTypeBuilder<ArticleReaction> builder)
        {
            builder
                .ToTable(RelationsConfig.ARTICLE_REACTION)
                .SetColumnsTypes(Columns)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);

            builder.HasOne(_ => _.Creator).WithMany().OnDelete(DeleteBehavior.NoAction);
        }
    }

    class TagConfig : EntityConfiguration<Tag>
    {
        protected override Dictionary<Expression<Func<Tag, object?>>, string> Columns => new()
        {
            { _ => _.Title, VARCHAR100 }
        };

        public override void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder
                .ToTable(RelationsConfig.TAG)
                .SetColumnsTypes(Columns);
        }
    }

    class ArticleTagConfig : EntityConfiguration<ArticleTag>
    {
        protected override Dictionary<Expression<Func<ArticleTag, object?>>, string> Columns => new()
        {
            // ArticleId
            // TagId
        };

        public override void Configure(EntityTypeBuilder<ArticleTag> builder)
        {
            builder
                .ToTable(RelationsConfig.ARTICLE_TAG)
                .SetColumnsTypes(Columns)
                .HasKey(_ => new { _.ArticleId, _.TagId });
        }
    }

    class MediaResourceConfig : EntityConfiguration<MediaResource>
    {
        protected override Dictionary<Expression<Func<MediaResource, object?>>, string> Columns => new()
        {
            { _ => _.Description, NVARCHAR500 },
            { _ => _.Title, NVARCHAR255 },
            { _ => _.Artist, NVARCHAR100 }
            // Type below
        };

        public override void Configure(EntityTypeBuilder<MediaResource> builder)
        {
            builder
                .ToTable(RelationsConfig.MEDIA_RESOURCE)
                .SetColumnsTypes(Columns)
                .SetEnumParsing(_ => _.Type)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);

            builder.Ignore(_ => _.Media);
        }
    }

    class AdvisorConfig : EntityConfiguration<Advisor>
    {
        protected override Dictionary<Expression<Func<Advisor, object?>>, string> Columns => new()
        {
            { _ => _.Intro, NVARCHAR500 },
            { _ => _.Experience, NVARCHAR1000 }
            // Balance
            // CourseCount
        };

        public override void Configure(EntityTypeBuilder<Advisor> builder)
        {
            builder
                .ToTable(RelationsConfig.ADVISOR)
                .SetColumnsTypes(Columns)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);

            //builder.OwnsMany(_ => _.Qualifications);
            builder.Ignore(_ => _.Medias);
        }
    }

    class CategoryConfig : EntityConfiguration<Category>
    {
        protected override Dictionary<Expression<Func<Category, object?>>, string> Columns => new()
        {
            { _ => _.Path, VARCHAR255 },
            { _ => _.Title, VARCHAR100 },
            { _ => _.Description, NVARCHAR1000 },
            // IsLeaf
            // CourseCount
        };

        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                .ToTable(RelationsConfig.CATEGORY)
                .SetColumnsTypes(Columns)
                .SetUnique(_ => _.Path, _ => _.Title, _ => _.Description);
        }
    }

    class CourseConfig : EntityConfiguration<Course>
    {
        protected override Dictionary<Expression<Func<Course, object?>>, string> Columns => new()
        {
            { _ => _.Title, NVARCHAR255 },
            { _ => _.MetaTitle, VARCHAR255 },
            { _ => _.ThumbUrl, NVARCHAR255 },
            { _ => _.Intro, NVARCHAR500 },
            { _ => _.Description, NVARCHAR1000 },
            // Status below
            // Price
            // Discount
            // DiscountExpiry
            // Level below
            { _ => _.Outcomes, NVARCHAR500 },
            { _ => _.Requirements, NVARCHAR500 },
            // LectureCount
            // LearnerCount
            // RatingCount
            // TotalRating
        };

        public override void Configure(EntityTypeBuilder<Course> builder)
        {
            builder
                .ToTable(RelationsConfig.COURSE, _ => _.HasTrigger(Triggers.onCourseInsertDelete))
                .SetColumnsTypes(Columns)
                .SetEnumParsing(_ => _.Status).SetEnumParsing(_ => _.Level)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);
        }
    }

    class EnrollmentConfig : EntityConfiguration<Enrollment>
    {
        protected override Dictionary<Expression<Func<Enrollment, object?>>, string> Columns => new() { };

        public override void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder
                .ToTable(RelationsConfig.ENROLLMENT, _ => _.HasTrigger(Triggers.onEnrollmentInsertDelete))
                .SetEnumParsing(_ => _.Status)
                .SetUnique(_ => _.BillId)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .HasKey(_ => new { _.CreatorId, _.CourseId });

            builder.HasOne(_ => _.Creator).WithMany().OnDelete(DeleteBehavior.NoAction);
        }
    }

    class CourseReviewConfig : EntityConfiguration<CourseReview>
    {
        protected override Dictionary<Expression<Func<CourseReview, object?>>, string> Columns => new()
        {
            { _ => _.Content, NVARCHAR500 },
            // Rating
        };

        public override void Configure(EntityTypeBuilder<CourseReview> builder)
        {
            builder
                .ToTable(RelationsConfig.COURSE_REVIEW, _ => _.HasTrigger(Triggers.onCourseReviewInsertDelete))
                .SetColumnsTypes(Columns)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);

            //builder.OwnsMany(_ => _.Medias);
            builder.Ignore(_ => _.Medias);
        }
    }

    class LectureConfig : EntityConfiguration<Lecture>
    {
        protected override Dictionary<Expression<Func<Lecture, object?>>, string> Columns => new()
        {
            { _ => _.Title, NVARCHAR255 },
            { _ => _.Content, NVARCHAR3000 }
            // IsPreviewable
        };

        public override void Configure(EntityTypeBuilder<Lecture> builder)
        {
            builder
                .ToTable(RelationsConfig.LECTURE, _ => _.HasTrigger(Triggers.onLectureInsertDelete))
                .SetColumnsTypes(Columns)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);

            //builder.OwnsMany(_ => _.Materials);
            builder.Ignore(_ => _.Materials);
        }
    }

    class LectureCommentConfig : EntityConfiguration<LectureComment>
    {
        protected override Dictionary<Expression<Func<LectureComment, object?>>, string> Columns => new()
        {
            { _ => _.Content, NVARCHAR500 },
            // Status below
        };

        public override void Configure(EntityTypeBuilder<LectureComment> builder)
        {
            builder
                .ToTable(RelationsConfig.LECTURE_COMMENT)
                .SetColumnsTypes(Columns)
                .SetEnumParsing(_ => _.Status)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);

            //builder.OwnsMany(_ => _.Medias);
            builder.Ignore(_ => _.Medias);
        }
    }

    class LectureReactionConfig : EntityConfiguration<LectureReaction>
    {
        protected override Dictionary<Expression<Func<LectureReaction, object?>>, string> Columns => new()
        {
            // SourceId
            { _ => _.Content, NVARCHAR45 }
        };

        public override void Configure(EntityTypeBuilder<LectureReaction> builder)
        {
            builder
                .ToTable(RelationsConfig.LECTURE_REACTION)
                .SetColumnsTypes(Columns)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);
            //.HasKey(_ => new { _.CreatorId, _.SourceId });

            builder.HasOne(_ => _.Creator).WithMany().OnDelete(DeleteBehavior.NoAction);
        }
    }
    #endregion
}
