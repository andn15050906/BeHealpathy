using Contract.Domain.ProgressAggregate;
using Contract.Helpers;
using Contract.Requests.Progress.RoadmapRequests;
using Contract.Requests.Progress.RoadmapRequests.Dtos;
using Infrastructure.DataAccess.SQLServer.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Progress.RoadmapHandlers;

public sealed class CURoadmapHandler(HealpathyContext context, IAppLogger logger)
    : RequestHandler<CURoadmapCommand, HealpathyContext>(context, logger)
{
    public override async Task<Result> Handle(CURoadmapCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (command.Rq.Id is null)
            {
                Roadmap entity = AddRoadmap(command);
                await _context.Roadmaps.InsertExt(entity);
            }
            else
            {
                Roadmap? entity = await _context.Roadmaps
                    .Include(_ => _.Phases).ThenInclude(_ => _.Milestones)
                    .Include(_ => _.Phases).ThenInclude(_ => _.Recommendations)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(_ => _.Id == command.Rq.Id);

                if (entity is null)
                    return NotFound(string.Empty);
                UpdateRoadmap(entity, command.Rq);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return command.Rq.Id is null ? Created() : Ok();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private static Roadmap AddRoadmap(CURoadmapCommand command)
    {
        var roadmap = new Roadmap
        {
            Title = command.Rq.Title,
            IntroText = command.Rq.IntroText,
            Phases = [],
            CreatorId = command.UserId
        };
        
        //...
        for (var index = 0; index < command.Rq.Phases.Count; index++)
            AddPhase(roadmap, command.Rq.Phases[index], index);
        return roadmap;
    }

    private static Roadmap AddPhase(Roadmap roadmap, CURoadmapPhaseDto dto, int index)
    {
        var phase = new RoadmapPhase
        {
            Index = index,
            Title = dto.Title,
            Description = dto.Description,
            TimeSpan = dto.TimeSpan,
            Milestones = []
        };

        foreach (var milestoneDto in dto.Milestones)
            ;// AddMilestone(phase, milestoneDto);
        roadmap.Phases.Add(phase);
        return roadmap;
    }

    /*
    private static RoadmapPhase AddMilestone(RoadmapPhase phase, CURoadmapMilestoneDto dto)
    {
        var milestone = new RoadmapMilestone
        {
            Title = dto.Title,
            EventName = dto.EventName,
            RepeatTimesRequired = dto.RepeatTimesRequired,
            Index = dto.Index,
            IsRequired = dto.IsRequired,
            Recommendations = []
        };

        foreach (var recommendationDto in dto.Recommendations)
            AddRecommendation(milestone, recommendationDto);
        phase.Milestones.Add(milestone);
        return phase;
    }

    private static RoadmapMilestone AddRecommendation(RoadmapMilestone milestone, CURoadmapRecommendationDto dto)
    {
        var recommendation = new RoadmapRecommendation
        {
            TargetEntityId = dto.TargetEntityId,
            EntityType = dto.EntityType,
            Milestone = milestone,
            Trait = dto.Trait,
            TraitDescription = dto.TraitDescription
        };
        milestone.Recommendations.Add(recommendation);
        return milestone;
    }
    */

    private static void UpdateRoadmap(Roadmap entity, CURoadmapDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Title) && dto.Title != entity.Title)
            entity.Title = dto.Title;
        if (!string.IsNullOrEmpty(dto.IntroText) && dto.IntroText != entity.IntroText)
            entity.IntroText = dto.IntroText;

        entity.Phases.RemoveAll(phase => !dto.Phases.Any(_ => _.Id == phase.Id));
        foreach (var child in dto.Phases)
        {
            var existingChild = child.Id is not null ? entity.Phases.FirstOrDefault(_ => _.Id == child.Id) : null;
            if (existingChild is null)
                AddPhase(entity, child, child.Index);
            else if (existingChild is not null)
                UpdatePhase(existingChild, child);
        }
    }

    private static void UpdatePhase(RoadmapPhase entity, CURoadmapPhaseDto dto)
    {
        entity.Index = dto.Index;
        if (!string.IsNullOrEmpty(dto.Title) && dto.Title != entity.Title)
            entity.Title = dto.Title;
        if (!string.IsNullOrEmpty(dto.Description) && dto.Description != entity.Description)
            entity.Description = dto.Description;
        entity.TimeSpan = dto.TimeSpan;

        entity.Milestones.RemoveAll(milestone => !dto.Milestones.Any(_ => _.Id == milestone.Id));
        foreach (var child in dto.Milestones)
        {
            var existingChild = child.Id is not null ? entity.Milestones.FirstOrDefault(_ => _.Id == child.Id) : null;
            /*
            if (existingChild is null)
                AddMilestone(entity, child);
            else if (existingChild is not null)
                UpdateMilestone(existingChild, child);
            */
        }
    }

    /*
    private static void UpdateMilestone(RoadmapMilestone entity, CURoadmapMilestoneDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Title) && dto.Title != entity.Title)
            entity.Title = dto.Title;
        if (!string.IsNullOrEmpty(dto.EventName) && dto.EventName != entity.EventName)
            entity.EventName = dto.EventName;
        entity.RepeatTimesRequired = dto.RepeatTimesRequired;
        entity.Recommendations.RemoveAll(recommendation => !dto.Recommendations.Any(_ => _.Id == recommendation.Id));

        foreach (var child in dto.Recommendations)
        {
            var existingChild = child.Id is not null ? entity.Recommendations.FirstOrDefault(_ => _.Id == child.Id) : null;
            if (existingChild is null)
                AddRecommendation(entity, child);
            else if (existingChild is not null)
                UpdateRecommendation(existingChild, child, entity);
        }
    }

    private static void UpdateRecommendation(RoadmapRecommendation entity, CURoadmapRecommendationDto dto, RoadmapMilestone parent)
    {
        entity.TargetEntityId = dto.TargetEntityId;
        if (!string.IsNullOrEmpty(dto.EntityType) && dto.EntityType != entity.EntityType)
            entity.EntityType = dto.EntityType;
        entity.Milestone = parent;
        entity.Trait = dto.Trait;
        entity.TraitDescription = dto.TraitDescription;
    }
    */
}