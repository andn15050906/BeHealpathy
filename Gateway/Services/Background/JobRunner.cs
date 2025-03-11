using Contract.Domain.ProgressAggregates;
using Contract.Helpers;
using Contract.Responses.Progress;
using Core.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Gateway.Services.Background;

public sealed class JobRunner
{
    public class CalculateRoadmapProgress
    {
        private readonly HealpathyContext _context;
        private readonly IAppLogger _appLogger;

        public CalculateRoadmapProgress(HealpathyContext context, IAppLogger logger)
        {
            _context = context;
            _appLogger = logger;
        }

        public async Task Execute()
        {
            try
            {
                var allUserIds = await _context.Users.Where(_ => !_.IsDeleted).Select(_ => _.Id).ToListAsync();

                foreach (var userId in allUserIds)
                {
                    var currentRoadmapId = await _context.Users.Where(_ => _.Id == userId).Select(_ => _.RoadmapId).FirstOrDefaultAsync();
                    if (currentRoadmapId == null)
                        continue;
                    var currentRoadmap = await _context.Roadmaps.Include(_ => _.Phases).ThenInclude(_ => _.Milestones).Where(_ => _.Id == currentRoadmapId).Select(RoadmapModel.MapExpression).FirstOrDefaultAsync();
                    if (currentRoadmap == null)
                        continue;

                    foreach (var phase in currentRoadmap.Phases.OrderBy(_ => _.Index))
                    {
                        var currentProgress = await _context.RoadmapProgress.Where(_ => _.CreatorId == userId && _.RoadmapPhaseId == phase.Id).OrderByDescending(_ => _.CreationTime).FirstOrDefaultAsync();

                        var currentMilestoneIdList = JsonSerializer.Deserialize<List<Guid>>(currentProgress?.MilestonesCompleted ?? "[]") ?? [];
                        var pendingMilestoneIdList = phase.Milestones.Select(_ => _.Id).Except(currentMilestoneIdList);
                        if (pendingMilestoneIdList.Any())
                        {
                            var addedMilestones = new List<Guid>();
                            var logs = await _context.ActivityLogs.Where(_ => _.CreatorId == userId && _.CreationTime.AddDays(phase.TimeSpan) > TimeHelper.Now).ToListAsync();
                            foreach (var pendingMilestoneId in pendingMilestoneIdList)
                            {
                                var pendingMilestone = phase.Milestones.First(_ => _.Id == pendingMilestoneId);
                                var eventCount = logs.Count(_ => _.Content.Contains(pendingMilestone.EventName));
                                if (eventCount >= pendingMilestone.RepeatTimesRequired)
                                    addedMilestones.Add(pendingMilestoneId);
                            }
                            if (addedMilestones.Count > 0)
                            {
                                currentMilestoneIdList.AddRange(addedMilestones);
                                var completedMilestones = JsonSerializer.Serialize(currentMilestoneIdList);

                                if (currentProgress is null)
                                {
                                    _context.RoadmapProgress.Add(new RoadmapProgress(userId, phase.Id, completedMilestones));
                                }
                                else
                                {
                                    currentProgress.MilestonesCompleted = completedMilestones;
                                }
                            }
                            break;
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _appLogger.Warn(ex.Message);
            }
        }
    }
}