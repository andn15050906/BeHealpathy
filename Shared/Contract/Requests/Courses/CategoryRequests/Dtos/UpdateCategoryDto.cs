﻿namespace Contract.Requests.Courses.CategoryRequests.Dtos;

public sealed class UpdateCategoryDto
{
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool? IsLeaf { get; set; }
}
