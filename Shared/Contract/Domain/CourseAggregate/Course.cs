﻿using Contract.Domain.CourseAggregate.Enums;
using Contract.Domain.UserAggregate;
using Core.Helpers;

namespace Contract.Domain.CourseAggregate;

// Aggregate Root
public sealed class Course : AuditedEntity
{
    // Attributes
    public string Title { get; private set; }
    public string MetaTitle { get; private set; }
    public string ThumbUrl { get; set; }
    public string Intro { get; set; }
    public string Description { get; set; }
    public CourseStatus Status { get; set; }
    public double Price { get; set; }
    public double Discount { get; private set; }
    public DateTime DiscountExpiry { get; private set; }
    public CourseLevel Level { get; set; }
    public string Outcomes { get; set; }
    public string Requirements { get; set; }
    public byte LectureCount { get; private set; }
    public int LearnerCount { get; set; }
    public int RatingCount { get; private set; }
    public long TotalRating { get; private set; }

    // FKs
    public Guid LeafCategoryId { get; set; }
    public Guid InstructorId { get; set; }

    // Navigations
    //public List<Section> Sections { get; set; }
    public Category LeafCategory { get; set; }
    public List<Lecture> Lectures { get; set; }
    public List<CourseReview> Reviews { get; set; }
    public List<Enrollment> Enrollments { get; set; }
    public User Creator { get; set; }






#pragma warning disable CS8618
    public Course()
    {

    }

    public Course(Guid id, Guid creatorId, Guid instructorId, Guid leafCategoryId,
        string title, string thumbUrl, string intro, string description, double price,
        CourseLevel level, string outcomes, string requirements, List<Lecture> lectures)
    {
        Id = id;
        CreatorId = creatorId;

        InstructorId = instructorId;
        LeafCategoryId = leafCategoryId;
        SetTitle(title);
        ThumbUrl = thumbUrl;
        Intro = intro;
        Description = description;
        Status = CourseStatus.Ongoing;
        Price = price;
        Level = level;
        Outcomes = outcomes;
        Requirements = requirements;
        Lectures = lectures;
    }
#pragma warning restore CS8618






    public void SetTitle(string title)
    {
        Title = title;
        MetaTitle = TextHelper.Normalize(title);
    }

    public void SetDiscount(double discount, DateTime expiry)
    {
        if (discount < 0 || discount > 1)
            throw new Exception(BusinessMessages.Course.INVALID_DISCOUNT);
        if (expiry < DateTime.UtcNow)
            throw new Exception(BusinessMessages.Course.INVALID_DISCOUNT_EXPIRY);
        Discount = discount;
        DiscountExpiry = expiry;
    }
}
