using Contract.Domain.UserAggregate.Enums;
using Core.Helpers;

namespace Contract.Domain.UserAggregate;

// Aggregate Root
public sealed class User : TimeAuditedEntity
{
    // Attributes
    public string UserName { get; set; }
    public string Password { get; private set; }
    public string Email { get; set; }
    public string FullName { get; private set; }
    public string MetaFullName { get; private set; }
    public string AvatarUrl { get; set; }
    public Role Role { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public bool IsVerified { get; set; }
    public bool IsApproved { get; private set; }
    public bool IsBanned { get; private set; }
    public byte AccessFailedCount { get; private set; }
    public DateTime? UnbanDate { get; set; }
    public string Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Phone { get; set; }

    public int EnrollmentCount { get; set; }
    public long Balance { get; set; }

    // FKs
    public Guid? AdvisorId { get; set; }

    // Navigations
    public List<UserLogin> UserLogins { get; set; }
    public List<Preference> Preferences { get; set; }
    public List<Setting> Settings { get; set; }






#pragma warning disable CS8618
    public User()
    {
        UserName = string.Empty;
        Password = string.Empty;
        Email = string.Empty;
        FullName = string.Empty;
        MetaFullName = string.Empty;
        AvatarUrl = string.Empty;
        Token = string.Empty;
        RefreshToken = string.Empty;
        Bio = string.Empty;
    }

    /// <summary>
    /// Used for registration
    /// </summary>
    public User(Guid id, string userName, string inputPassword, string email, Role role)
    {
        Id = id;
        UserName = userName;
        SetPassword(inputPassword);
        Email = email;
        SetFullName(userName);                  // default Full Name is UserName
        Role = role;
        GenerateToken();
        AvatarUrl = string.Empty;
        Bio = string.Empty;

        if (Role == Role.Admin)
            IsApproved = true;
        DateOfBirth = TimeHelper.DefaultDateOfBirth;
    }

    public User(Guid id, string loginProvider, string providerKey, string? email, string userName, Role role)
    {
        Id = id;
        UserName = userName;
        Password = string.Empty;
        Email = email is not null ? email : string.Empty;
        SetFullName(userName);                  // default Full Name is UserName
        Role = role;
        Token = string.Empty;
        RefreshToken = string.Empty;
        AvatarUrl = string.Empty;
        Bio = string.Empty;

        IsVerified = true;
        // IsApproved
        UserLogins =
        [
            new UserLogin(loginProvider, providerKey)
        ];
        DateOfBirth = TimeHelper.DefaultDateOfBirth;
    }
#pragma warning restore CS8618






    public void Verify()
    {
        IsVerified = true;
    }

    public void SetFullName(string fullName)
    {
        FullName = fullName;
        MetaFullName = TextHelper.Normalize(fullName);
    }

    public void SetPassword(string plainTextPassword)
    {
        Password = TextHelper.Hash(plainTextPassword);
    }

    public void GenerateToken()
    {
        Token = Guid.NewGuid().ToString();
        RefreshToken = string.Empty;
    }

    public void SetRefreshToken(string refreshToken)
    {
        RefreshToken = refreshToken;
        LastModificationTime = TimeHelper.Now;
    }

    public void IncreaseAccessFailedCount()
    {
        AccessFailedCount++;
    }

    public void ResetAccessFailedCount()
    {
        AccessFailedCount = 0;
    }

    public void Deposit(long amount)
    {
        if (amount < 0)
            throw new Exception(BusinessMessages.User.INVALID_AMOUNT);

        Balance += amount;
    }

    public void Withdraw(long amount)
    {
        Balance -= amount;

        if (amount < 0 || Balance < 0)
            throw new Exception(BusinessMessages.User.INVALID_AMOUNT);
    }






    public const int BLOCKED_ACCESS_FAILED_COUNT = 100;

    public bool IsNotApproved()
    {
        return Role == Role.Admin ? !IsApproved : !IsVerified;
    }

    public bool IsBlocked()
    {
        return AccessFailedCount == BLOCKED_ACCESS_FAILED_COUNT;
    }

    public static bool IsMatchPasswords(string plainTextPassword, string hashedPassword)
    {
        if (string.IsNullOrEmpty(plainTextPassword))
            return false;
        return hashedPassword == TextHelper.Hash(plainTextPassword);
    }
}