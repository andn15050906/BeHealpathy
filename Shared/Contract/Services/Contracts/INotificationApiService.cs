using Contract.Messaging.Models;
using Contract.Requests.Notifications.Dtos;
using Contract.Responses.Notifications;
using Core.Responses;

namespace Contract.Services.Contracts;

public interface INotificationApiService
{
    Task<Result<PagedResult<NotificationModel>>> GetPagedAsync(QueryNotificationDto dto);

    Task<Result<Guid>> CreateInstructorNotificationAsync(CreateAdvisorRequestDto dto);
    Task<Result<Guid>> CreateWithdrawalNotificationAsync(CreateWithdrawalRequestDto dto);
}