﻿using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Progress.DiaryNoteRequests.Dtos;
using Contract.Responses.Progress;

namespace Contract.Requests.Progress.DiaryNoteRequests;

public sealed class CreateDiaryNoteCommand : CreateCommand<DiaryNoteModel>
{
    public CreateDiaryNoteDto Rq { get; init; }
    public Guid UserId { get; init; }
    public List<Multimedia> Medias { get; init; }



    public CreateDiaryNoteCommand(Guid id, CreateDiaryNoteDto rq, Guid userId, List<Multimedia> medias, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
        Medias = medias;
    }
}
