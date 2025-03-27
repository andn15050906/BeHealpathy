using Contract.Domain.CommunityAggregate;
using Contract.Domain.Shared;
using Contract.Domain.Shared.NotificationAggregate;
using Infrastructure.DataAccess.SQLServer;
using Infrastructure.DataAccess.SQLServer.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace Gateway.Infrastructure;

/*public sealed class GatewayContext : BaseContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    //public DbSet<ConversationMember> ConversationMembers { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    //public DbSet<MessageReaction> MessageReactions { get; set; }



    public GatewayContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .ApplyConfiguration(new UserConfig())

            .ApplyConfiguration(new NotificationConfig())

            .ApplyConfiguration(new BillConfig())

            .ApplyConfiguration(new ConversationConfig())
            .ApplyConfiguration(new ConversationMemberConfig())
            .ApplyConfiguration(new ChatMessageConfig())
            .ApplyConfiguration(new MessageReactionConfig());
    }






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
            // IsPremium
            // PremiumExpiry
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
        }
    }

    class NotificationConfig : EntityConfiguration<Notification>
    {
        protected override Dictionary<Expression<Func<Notification, object?>>, string> Columns => new()
        {
            { _ => _.Message, NVARCHAR255 },
            // Type below
            // Status below
            // ReceiverId
        };

        public override void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder
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
                .SetColumnsTypes(Columns)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);

            builder.HasOne<User>().WithMany().HasForeignKey(_ => _.CreatorId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(_ => _.Members).WithOne().OnDelete(DeleteBehavior.NoAction);
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
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .HasKey(_ => new { _.CreatorId, _.ConversationId });

            builder.HasOne<User>().WithMany().HasForeignKey(_ => _.CreatorId).OnDelete(DeleteBehavior.NoAction);
        }
    }

    class ChatMessageConfig : EntityConfiguration<ChatMessage>
    {
        protected override Dictionary<Expression<Func<ChatMessage, object?>>, string> Columns => new()
        {
            { _ => _.Content, NVARCHAR255 },
            // Status below
        };

        public override void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder
                .SetColumnsTypes(Columns)
                .SetEnumParsing(_ => _.Status)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
                .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);

            builder.HasOne<User>().WithMany().HasForeignKey(_ => _.CreatorId).OnDelete(DeleteBehavior.NoAction);
        }
    }

    class MessageReactionConfig : EntityConfiguration<MessageReaction>
    {
        protected override Dictionary<Expression<Func<MessageReaction, object?>>, string> Columns => new()
        {
            // SourceId
            { _ => _.Content, VARCHAR45 }
        };

        public override void Configure(EntityTypeBuilder<MessageReaction> builder)
        {
            builder
                .SetColumnsTypes(Columns)
                .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);

            builder.HasOne<User>().WithMany().HasForeignKey(_ => _.CreatorId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
*/