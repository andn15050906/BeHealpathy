﻿namespace Contract.Requests.Progress.McqRequests.Dtos;

public sealed class CreateMcqAnswerDto
{
    public string Content { get; set; }
    public int? Score { get; set; }
}