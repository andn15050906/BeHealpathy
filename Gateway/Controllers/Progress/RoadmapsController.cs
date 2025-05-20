using Contract.Domain.ProgressAggregate;
using Contract.Domain.Shared.MultimediaBase;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Progress.RoadmapRequests;
using Contract.Requests.Progress.RoadmapRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Gateway.Controllers.Progress;

public sealed class RoadmapsController : ContractController
{
    public RoadmapsController(IMediator mediator) : base(mediator)
    {
    }

    public static MentalHealthDataService DataService = new MentalHealthDataService();

    [HttpGet]
    public async Task<IActionResult> GetAllRoadmaps([FromServices] HealpathyContext context)
    {
        var roadmaps = await context.Roadmaps
            .Include(_ => _.Phases).ThenInclude(_ => _.Recommendations)
            .Select(Contract.Responses.Progress.RoadmapModel.MapExpression)
            .ToListAsync();

        return Ok(roadmaps);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CURoadmapDto dto, [FromServices] IFileService fileService)
    {
        var roadmapId = Guid.NewGuid();
        List<Multimedia> medias = [];

        if (dto.Thumb is not null)
        {
            var image = await fileService.SaveImageAndUpdateDto(dto.Thumb, roadmapId);
            if (image is not null)
                medias.Add(image);
        }

        CURoadmapCommand command = new(roadmapId, dto, ClientId, medias);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update(CURoadmapDto dto, [FromServices] IFileService fileService)
    {
        if (dto.Id is null)
            return BadRequest();

        var roadmapId = (Guid)dto.Id;
        List<Multimedia> medias = [];

        if (dto.Thumb is not null)
        {
            var image = await fileService.SaveImageAndUpdateDto(dto.Thumb, roadmapId);
            if (image is not null)
                medias.Add(image);
        }

        CURoadmapCommand command = new(roadmapId, dto, ClientId, medias);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteRoadmapCommand command = new(id, ClientId);
        return await Send(command);
    }






    public class CompletionData
    {
        public Statistics Statistics { get; set; }
        public string CouponCode { get; set; }
        public string CouponDescription { get; set; }
        public List<AdvancedRoadmap> AdvancedRoadmaps { get; set; }
    }

    public class Statistics
    {
        public int DaysActive { get; set; }
        public int ActionsCompleted { get; set; }
        public int EmotionalImprovement { get; set; }
    }

    public class AdvancedRoadmap
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class CompletionViewData
    {
        public List<RoadmapStep> RoadmapSteps { get; set; }
        public List<ImprovementOption> ImprovementOptions { get; set; }
        public Statistics Statistics { get; set; }
        public Coupon Coupon { get; set; }
        public List<AdvancedRoadmap> AdvancedRoadmaps { get; set; }
    }

    public class RoadmapStep
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
        public string Icon { get; set; }
    }

    public class ImprovementOption
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }

    public class Coupon
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class DetailedStep
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> FocusAreas { get; set; }
        public List<Activity> Activities { get; set; }
    }

    public class Activity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Recommended { get; set; }
        public string Icon { get; set; }
    }

    public class MentalProfile
    {
        public string UserType { get; set; }
        public string StressSource { get; set; }
        public string ImprovementGoal { get; set; }
        public int StressLevel { get; set; }
        public int DepressionRisk { get; set; }
        public List<EmotionalIndexItem> EmotionalIndex { get; set; }
    }

    public class EmotionalIndexItem
    {
        public DateTime Date { get; set; }
        public int Value { get; set; }
    }

    public class PhaseDetail
    {
        public string Id { get; set; }
        public string RoadmapId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThemeColor { get; set; }
        public string Introduction { get; set; }
        public string VideoUrl { get; set; }
        public List<Tip> Tips { get; set; }
        public List<Action> Actions { get; set; }
        public bool CanSkip { get; set; }
        public bool RequireConfirmation { get; set; }
        public string NextPhaseId { get; set; }
        public List<CompletionCriterion> CompletionCriteria { get; set; }
        public List<Resource> Resources { get; set; }
    }

    public class Tip
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Icon { get; set; }
    }

    public class Action
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public bool Completed { get; set; }
        public bool Required { get; set; }
        public List<string> MoodTags { get; set; }
    }

    public class CompletionCriterion
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class Resource
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }

    public class Recommendation
    {
        public string Content { get; set; }
        public bool IsMotivation { get; set; }
        public string Source { get; set; }
    }

    public class RoadmapModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> IntroText { get; set; }
        public int Steps { get; set; }
        public string Category { get; set; }
        public bool Featured { get; set; }
        public int CompletionRate { get; set; }
        public string Image { get; set; }
        public List<PhaseModel> Phases { get; set; }
    }

    public class PhaseModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string AudioUrl { get; set; }
        public int ExerciseCount { get; set; }
        public bool Completed { get; set; }
        public bool Current { get; set; }
        public string ThemeColor { get; set; }
        public string Icon { get; set; }
    }

    public class SuggestionData
    {
        public List<Option> UserTypeOptions { get; set; }
        public Dictionary<string, List<Option>> IssueOptions { get; set; }
        public List<Option> WhereOptions { get; set; }
        public List<Option> WhenOptions { get; set; }
        public List<Option> RelatedOptions { get; set; }
        public Dictionary<string, Dictionary<string, List<SuggestedRoadmap>>> SuggestedRoadmaps { get; set; }
        public Dictionary<string, string> RoadmapIcons { get; set; }
        public List<string> AudioTracks { get; set; }
    }

    public class Option
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public class SuggestedRoadmap
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Match { get; set; }
        public int Steps { get; set; }
        public bool IsPaid { get; set; }
        public decimal Price { get; set; }
        public List<string> Features { get; set; }
    }

    public class MentalHealthDataService
    {
        private readonly List<RoadmapModel> _defaultRoadmaps;
        private readonly Dictionary<string, Dictionary<string, PhaseDetail>> _allPhases;
        private readonly MentalProfile _mentalProfileData;
        private readonly List<Recommendation> _recommendationData;
        private readonly SuggestionData _suggestionData;
        private readonly CompletionData _completionData;
        private readonly Dictionary<int, DetailedStep> _detailedSteps;
        private readonly CompletionViewData _completionViewData;

        public MentalHealthDataService()
        {
            // Initialize default roadmaps with complete data
            _defaultRoadmaps = new List<RoadmapModel>
            {
                new RoadmapModel
                {
                    Id = "1",
                    Title = "Vượt qua lo âu",
                    Description = "Học cách nhận biết và vượt qua các triệu chứng lo âu phổ biến",
                    IntroText = new List<string>
                    {
                        "Lộ trình này được thiết kế dựa trên các phương pháp đã được chứng minh hiệu quả trong việc hỗ trợ sức khỏe tinh thần.",
                        "Trong quá trình này, bạn sẽ học cách nhận diện những suy nghĩ tiêu cực, thách thức chúng và thay thế bằng những suy nghĩ tích cực hơn.",
                        "Mỗi bước trong lộ trình sẽ cung cấp cho bạn các công cụ và kỹ thuật thực tế để cải thiện sức khỏe tinh thần."
                    },
                    Steps = 5,
                    Category = "anxiety",
                    Featured = true,
                    CompletionRate = 78,
                    Image = "/images/anxiety.jpg",
                    Phases = new List<PhaseModel>
                    {
                        new PhaseModel
                        {
                            Id = "1",
                            Title = "Nhận diện vấn đề",
                            Description = "Nhận biết các triệu chứng lo âu và hiểu nguồn gốc của chúng",
                            VideoUrl = "/videos/phase1.mp4",
                            ExerciseCount = 2,
                            Completed = false,
                            Current = true,
                            ThemeColor = "indigo",
                            Icon = "mdi-eye-outline"
                        },
                        new PhaseModel
                        {
                            Id = "2",
                            Title = "Giảm nhẹ tức thì",
                            Description = "Học các kỹ thuật thư giãn nhanh để giảm lo âu trong tình huống khẩn cấp",
                            VideoUrl = "/videos/phase2.mp4",
                            AudioUrl = "/audio/relaxation.mp3",
                            ExerciseCount = 3,
                            Completed = false,
                            Current = false,
                            ThemeColor = "teal",
                            Icon = "mdi-hand-heart-outline"
                        },
                        new PhaseModel
                        {
                            Id = "3",
                            Title = "Ổn định tâm trí",
                            Description = "Thực hành chánh niệm và các bài tập thiền để ổn định tâm trí",
                            VideoUrl = "/videos/phase3.mp4",
                            AudioUrl = "/audio/meditation.mp3",
                            ExerciseCount = 4,
                            Completed = false,
                            Current = false,
                            ThemeColor = "purple",
                            Icon = "mdi-sprout-outline"
                        },
                        new PhaseModel
                        {
                            Id = "4",
                            Title = "Đối mặt với vấn đề",
                            Description = "Phát triển chiến lược để đối mặt với các tình huống gây lo âu",
                            VideoUrl = "/videos/phase4.mp4",
                            ExerciseCount = 5,
                            Completed = false,
                            Current = false,
                            ThemeColor = "blue",
                            Icon = "mdi-brain"
                        },
                        new PhaseModel
                        {
                            Id = "5",
                            Title = "Duy trì và phát triển",
                            Description = "Xây dựng kế hoạch duy trì lâu dài và tiếp tục phát triển",
                            VideoUrl = "/videos/phase5.mp4",
                            ExerciseCount = 3,
                            Completed = false,
                            Current = false,
                            ThemeColor = "green",
                            Icon = "mdi-heart-pulse"
                        }
                    }
                },
                new RoadmapModel
                {
                    Id = "2",
                    Title = "Xây dựng sự tự tin",
                    Description = "Phát triển sự tự tin và khả năng đối mặt với thử thách mới",
                    IntroText = new List<string>
                    {
                        "Lộ trình này được thiết kế dựa trên các phương pháp đã được chứng minh hiệu quả trong việc hỗ trợ sức khỏe tinh thần.",
                        "Trong quá trình này, bạn sẽ học cách nhận diện những suy nghĩ tiêu cực, thách thức chúng và thay thế bằng những suy nghĩ tích cực hơn.",
                        "Mỗi bước trong lộ trình sẽ cung cấp cho bạn các công cụ và kỹ thuật thực tế để cải thiện sức khỏe tinh thần."
                    },
                    Steps = 4,
                    Category = "confidence",
                    Featured = true,
                    CompletionRate = 85,
                    Image = "/images/confidence.jpg",
                    Phases = new List<PhaseModel>
                    {
                        new PhaseModel
                        {
                            Id = "1",
                            Title = "Nhận diện vấn đề",
                            Description = "Nhận biết các triệu chứng lo âu và hiểu nguồn gốc của chúng",
                            VideoUrl = "/videos/phase1.mp4",
                            ExerciseCount = 2,
                            Completed = false,
                            Current = true,
                            ThemeColor = "indigo",
                            Icon = "mdi-eye-outline"
                        },
                        new PhaseModel
                        {
                            Id = "2",
                            Title = "Giảm nhẹ tức thì",
                            Description = "Học các kỹ thuật thư giãn nhanh để giảm lo âu trong tình huống khẩn cấp",
                            VideoUrl = "/videos/phase2.mp4",
                            AudioUrl = "/audio/relaxation.mp3",
                            ExerciseCount = 3,
                            Completed = false,
                            Current = false,
                            ThemeColor = "teal",
                            Icon = "mdi-hand-heart-outline"
                        },
                        new PhaseModel
                        {
                            Id = "3",
                            Title = "Ổn định tâm trí",
                            Description = "Thực hành chánh niệm và các bài tập thiền để ổn định tâm trí",
                            VideoUrl = "/videos/phase3.mp4",
                            AudioUrl = "/audio/meditation.mp3",
                            ExerciseCount = 4,
                            Completed = false,
                            Current = false,
                            ThemeColor = "purple",
                            Icon = "mdi-sprout-outline"
                        },
                        new PhaseModel
                        {
                            Id = "4",
                            Title = "Đối mặt với vấn đề",
                            Description = "Phát triển chiến lược để đối mặt với các tình huống gây lo âu",
                            VideoUrl = "/videos/phase4.mp4",
                            ExerciseCount = 5,
                            Completed = false,
                            Current = false,
                            ThemeColor = "blue",
                            Icon = "mdi-brain"
                        },
                        new PhaseModel
                        {
                            Id = "5",
                            Title = "Duy trì và phát triển",
                            Description = "Xây dựng kế hoạch duy trì lâu dài và tiếp tục phát triển",
                            VideoUrl = "/videos/phase5.mp4",
                            ExerciseCount = 3,
                            Completed = false,
                            Current = false,
                            ThemeColor = "green",
                            Icon = "mdi-heart-pulse"
                        }
                    }
                },
                new RoadmapModel
                {
                    Id = "3",
                    Title = "Kiểm soát cảm xúc",
                    Description = "Học cách nhận biết và điều chỉnh cảm xúc tiêu cực",
                    IntroText = new List<string>
                    {
                        "Lộ trình này được thiết kế dựa trên các phương pháp đã được chứng minh hiệu quả trong việc hỗ trợ sức khỏe tinh thần.",
                        "Trong quá trình này, bạn sẽ học cách nhận diện những suy nghĩ tiêu cực, thách thức chúng và thay thế bằng những suy nghĩ tích cực hơn.",
                        "Mỗi bước trong lộ trình sẽ cung cấp cho bạn các công cụ và kỹ thuật thực tế để cải thiện sức khỏe tinh thần."
                    },
                    Steps = 3,
                    Category = "emotions",
                    Featured = false,
                    CompletionRate = 62,
                    Image = "/images/emotions.jpg",
                    Phases = new List<PhaseModel>()
                },
                new RoadmapModel
                {
                    Id = "4",
                    Title = "Cân bằng công việc - cuộc sống",
                    Description = "Tạo sự cân bằng giữa công việc và cuộc sống cá nhân",
                    IntroText = new List<string>
                    {
                        "Lộ trình này được thiết kế dựa trên các phương pháp đã được chứng minh hiệu quả trong việc hỗ trợ sức khỏe tinh thần.",
                        "Trong quá trình này, bạn sẽ học cách nhận diện những suy ngh�� tiêu cực, thách thức chúng và thay thế bằng những suy nghĩ tích cực hơn.",
                        "Mỗi bước trong lộ trình sẽ cung cấp cho bạn các công cụ và kỹ thuật thực tế để cải thiện sức khỏe tinh thần."
                    },
                    Steps = 4,
                    Category = "work-life",
                    Featured = false,
                    CompletionRate = 70,
                    Image = "/images/balance.jpg",
                    Phases = new List<PhaseModel>()
                },
                new RoadmapModel
                {
                    Id = "5",
                    Title = "Giấc ngủ chất lượng",
                    Description = "Phương pháp giúp ngủ ngon và sâu giấc",
                    IntroText = new List<string>
                    {
                        "Lộ trình này được thiết kế dựa trên các phương pháp đã được chứng minh hiệu quả trong việc hỗ trợ sức khỏe tinh thần.",
                        "Trong quá trình này, bạn sẽ học cách nhận diện những suy nghĩ tiêu cực, thách thức chúng và thay thế bằng những suy nghĩ tích cực hơn.",
                        "Mỗi bước trong lộ trình sẽ cung cấp cho bạn các công cụ và kỹ thuật thực tế để cải thiện sức khỏe tinh thần."
                    },
                    Steps = 3,
                    Category = "sleep",
                    Featured = false,
                    CompletionRate = 90,
                    Image = "/images/sleep.jpg",
                    Phases = new List<PhaseModel>()
                }
            };

            // Initialize all phases with complete data
            _allPhases = new Dictionary<string, Dictionary<string, PhaseDetail>>
            {
                ["1"] = new Dictionary<string, PhaseDetail>
                {
                    ["1"] = new PhaseDetail
                    {
                        Id = "1",
                        RoadmapId = "1",
                        Title = "Nhận diện triệu chứng lo âu",
                        Description = "Học cách nhận biết các dấu hiệu và triệu chứng của lo âu",
                        ThemeColor = "indigo",
                        Introduction = "Bước đầu tiên để vượt qua lo âu là nhận diện và thừa nhận vấn đề. Trong phase này, bạn sẽ học cách nhận biết các triệu chứng lo âu, hiểu nguồn gốc của chúng và tác động của chúng đến cuộc sống hàng ngày của bạn.",
                        VideoUrl = "/videos/phase1.mp4",
                        Tips = new List<Tip>
                        {
                            new Tip
                            {
                                Title = "Lắng nghe cơ thể",
                                Content = "Chú ý đến các phản ứng thể chất như tim đập nhanh, khó thở, hoặc căng cơ",
                                Icon = "mdi-heart-pulse"
                            },
                            new Tip
                            {
                                Title = "Ghi chú hàng ngày",
                                Content = "Ghi lại các triệu chứng và tình huống gây lo âu để nhận diện mẫu hình",
                                Icon = "mdi-notebook"
                            }
                        },
                        Actions = new List<Action>
                        {
                            new Action
                            {
                                Id = "1-1",
                                Title = "Nhật ký triệu chứng",
                                Description = "Ghi lại các triệu chứng lo âu bạn gặp phải trong ngày",
                                Duration = "10 phút",
                                Completed = false,
                                Required = true,
                                MoodTags = new List<string> { "Tự nhận thức", "Theo dõi triệu chứng" }
                            },
                            new Action
                            {
                                Id = "1-2",
                                Title = "Đánh giá mức độ lo âu",
                                Description = "Đánh giá mức độ lo âu của bạn trên thang điểm từ 1-10",
                                Duration = "5 phút",
                                Completed = false,
                                Required = true,
                                MoodTags = new List<string> { "Đo lường", "Tự đánh giá" }
                            },
                            new Action
                            {
                                Id = "1-3",
                                Title = "Xác định yếu tố kích hoạt",
                                Description = "Nhận diện các tình huống hoặc suy nghĩ gây ra lo âu",
                                Duration = "15 phút",
                                Completed = false,
                                Required = false,
                                MoodTags = new List<string> { "Phân tích", "Nhận thức" }
                            }
                        },
                        CanSkip = false,
                        RequireConfirmation = false,
                        NextPhaseId = "2",
                        CompletionCriteria = new List<CompletionCriterion>
                        {
                            new CompletionCriterion
                            {
                                Title = "Nhận diện triệu chứng",
                                Description = "Bạn có thể nhận diện được các triệu chứng lo âu của mình"
                            },
                            new CompletionCriterion
                            {
                                Title = "Hiểu nguồn gốc",
                                Description = "Bạn hiểu được nguồn gốc của lo âu và các yếu tố kích hoạt"
                            },
                            new CompletionCriterion
                            {
                                Title = "Áp dụng kỹ thuật",
                                Description = "Bạn đã thử và áp dụng được ít nhất một kỹ thuật giảm lo âu"
                            }
                        },
                        Resources = new List<Resource>
                        {
                            new Resource
                            {
                                Title = "Hiểu về lo âu và các triệu chứng",
                                Url = "/docs/anxiety-symptoms"
                            },
                            new Resource
                            {
                                Title = "Kỹ thuật thở để giảm lo âu",
                                Url = "/docs/breathing-techniques"
                            },
                            new Resource
                            {
                                Title = "Nhận diện và thách thức suy nghĩ tiêu cực",
                                Url = "/docs/negative-thoughts"
                            }
                        }
                    },
                    ["2"] = new PhaseDetail
                    {
                        Id = "2",
                        RoadmapId = "1",
                        Title = "Giảm nhẹ tức thì",
                        Description = "Học các kỹ thuật thư giãn nhanh để giảm lo âu trong tình huống khẩn cấp",
                        ThemeColor = "teal",
                        Introduction = "Sau khi đã nhận diện vấn đề, phase tiếp theo là học các kỹ thuật giảm nhẹ tức thì. Những kỹ thuật này sẽ giúp bạn đối phó với các tình huống gây lo âu và giảm triệu chứng ngay lập tức khi chúng xuất hiện.",
                        VideoUrl = "/videos/phase2.mp4",
                        Tips = new List<Tip>
                        {
                            new Tip
                            {
                                Title = "Thực hành thường xuyên",
                                Content = "Các kỹ thuật thở và thư giãn hiệu quả hơn khi được thực hành thường xuyên",
                                Icon = "mdi-repeat"
                            },
                            new Tip
                            {
                                Title = "Tìm không gian yên tĩnh",
                                Content = "Nếu có thể, hãy tìm một nơi yên tĩnh để thực hành các kỹ thuật này",
                                Icon = "mdi-meditation"
                            }
                        },
                        Actions = new List<Action>
                        {
                            new Action
                            {
                                Id = "2-1",
                                Title = "Kỹ thuật thở 4-7-8",
                                Description = "Hít vào trong 4 giây, giữ 7 giây, và thở ra trong 8 giây",
                                Duration = "5 phút",
                                Completed = false,
                                Required = true,
                                MoodTags = new List<string> { "Giảm nhẹ nhanh", "Kiểm soát hơi thở" }
                            },
                            new Action
                            {
                                Id = "2-2",
                                Title = "Thư giãn cơ bắp tiến triển",
                                Description = "Căng và thả lỏng từng nhóm cơ để giảm căng thẳng",
                                Duration = "10 phút",
                                Completed = false,
                                Required = false,
                                MoodTags = new List<string> { "Thư giãn cơ thể", "Giảm căng thẳng" }
                            },
                            new Action
                            {
                                Id = "2-3",
                                Title = "Kỹ thuật neo đậu 5-4-3-2-1",
                                Description = "Sử dụng 5 giác quan để kéo bạn trở lại hiện tại",
                                Duration = "5 phút",
                                Completed = false,
                                Required = true,
                                MoodTags = new List<string> { "Chánh niệm", "Tập trung" }
                            }
                        },
                        CanSkip = true,
                        RequireConfirmation = false,
                        NextPhaseId = "3",
                        CompletionCriteria = new List<CompletionCriterion>
                        {
                            new CompletionCriterion
                            {
                                Title = "Thực hành thiền định",
                                Description = "Bạn đã thực hành thiền định ít nhất 3 lần một tuần"
                            },
                            new CompletionCriterion
                            {
                                Title = "Xây dựng thói quen",
                                Description = "Bạn đã bắt đầu xây dựng ít nhất một thói quen tích cực mới"
                            },
                            new CompletionCriterion
                            {
                                Title = "Áp dụng kỹ thuật",
                                Description = "Bạn đã áp dụng các kỹ thuật đối phó khi gặp tình huống lo âu"
                            }
                        },
                        Resources = new List<Resource>
                        {
                            new Resource
                            {
                                Title = "Hướng dẫn thiền định cho người mới bắt đầu",
                                Url = "/docs/meditation-guide"
                            },
                            new Resource
                            {
                                Title = "Xây dựng thói quen tích cực",
                                Url = "/docs/positive-habits"
                            },
                            new Resource
                            {
                                Title = "Kỹ thuật đối phó với lo âu",
                                Url = "/docs/coping-techniques"
                            }
                        }
                    },
                    ["3"] = new PhaseDetail
                    {
                        Id = "3",
                        RoadmapId = "1",
                        Title = "Ổn định tâm trí",
                        Description = "Thực hành chánh niệm và các bài tập thiền để ổn định tâm trí",
                        ThemeColor = "purple",
                        Introduction = "Suy nghĩ tiêu cực thường là nguyên nhân chính gây ra lo âu. Trong phase này, bạn sẽ học cách nhận diện, thách thức và thay đổi các mẫu suy nghĩ tiêu cực để giảm lo âu và cải thiện sức khỏe tinh thần.",
                        VideoUrl = "/videos/phase3.mp4",
                        Tips = new List<Tip>
                        {
                            new Tip
                            {
                                Title = "Tìm bằng chứng",
                                Content = "Khi có suy nghĩ tiêu cực, hãy tìm bằng chứng ủng hộ và phản bác nó",
                                Icon = "mdi-scale-balance"
                            },
                            new Tip
                            {
                                Title = "Đặt câu hỏi",
                                Content = "Hỏi bản thân: 'Điều tồi tệ nhất có thể xảy ra là gì? Khả năng xảy ra là bao nhiêu?'",
                                Icon = "mdi-help-circle"
                            },
                            new Tip
                            {
                                Title = "Thay thế suy nghĩ",
                                Content = "Thực hành thay thế suy nghĩ tiêu cực bằng suy nghĩ cân bằng hơn",
                                Icon = "mdi-swap-horizontal"
                            }
                        },
                        Actions = new List<Action>
                        {
                            new Action
                            {
                                Id = "3-1",
                                Title = "Nhật ký suy nghĩ",
                                Description = "Ghi lại các suy nghĩ tiêu cực và tác động của chúng",
                                Duration = "15 phút",
                                Completed = false,
                                Required = true,
                                MoodTags = new List<string> { "Nhận thức", "Phân tích" }
                            },
                            new Action
                            {
                                Id = "3-2",
                                Title = "Nhận diện lỗi suy nghĩ",
                                Description = "Học cách nhận diện các lỗi suy nghĩ phổ biến như suy nghĩ nhị nguyên, đọc suy nghĩ",
                                Duration = "10 phút",
                                Completed = false,
                                Required = true,
                                MoodTags = new List<string> { "Nhận thức", "Tư duy phản biện" }
                            },
                            new Action
                            {
                                Id = "3-3",
                                Title = "Thực hành tái cấu trúc nhận thức",
                                Description = "Thách thức và thay đổi suy nghĩ tiêu cực thành cân bằng hơn",
                                Duration = "20 phút",
                                Completed = false,
                                Required = false,
                                MoodTags = new List<string> { "Thay đổi suy nghĩ", "Tích cực hóa" }
                            }
                        },
                        CanSkip = false,
                        RequireConfirmation = false,
                        NextPhaseId = "4",
                        CompletionCriteria = new List<CompletionCriterion>
                        {
                            new CompletionCriterion
                            {
                                Title = "Thực hành chánh niệm",
                                Description = "Bạn đã thực hành chánh niệm ít nhất 5 phút mỗi ngày"
                            },
                            new CompletionCriterion
                            {
                                Title = "Nhận biết suy nghĩ",
                                Description = "Bạn có thể nhận biết suy nghĩ tiêu cực khi chúng xuất hiện"
                            },
                            new CompletionCriterion
                            {
                                Title = "Ổn định tâm trí",
                                Description = "Bạn có thể ổn định tâm trí khi cảm thấy lo âu"
                            }
                        },
                        Resources = new List<Resource>
                        {
                            new Resource
                            {
                                Title = "Hướng dẫn thực hành chánh niệm",
                                Url = "/docs/mindfulness-guide"
                            },
                            new Resource
                            {
                                Title = "Kỹ thuật thiền cho người mới bắt đầu",
                                Url = "/docs/meditation-beginners"
                            },
                            new Resource
                            {
                                Title = "Ổn định tâm trí trong cuộc sống hàng ngày",
                                Url = "/docs/daily-mindfulness"
                            }
                        }
                    },
                    ["4"] = new PhaseDetail
                    {
                        Id = "4",
                        RoadmapId = "1",
                        Title = "Đối mặt với vấn đề",
                        Description = "Phát triển chiến lược để đối mặt với các tình huống gây lo âu",
                        ThemeColor = "blue",
                        Introduction = "Đối mặt với vấn đề là một bước quan trọng trong việc vượt qua lo âu. Trong phase này, bạn sẽ học cách phát triển các chiến lược để đối mặt với các tình huống gây lo âu và xây dựng sự tự tin.",
                        VideoUrl = "/videos/phase4.mp4",
                        Tips = new List<Tip>
                        {
                            new Tip
                            {
                                Title = "Tiếp cận từng bước",
                                Content = "Chia nhỏ các tình huống khó khăn thành các bước nhỏ hơn, dễ quản lý hơn",
                                Icon = "mdi-stairs"
                            },
                            new Tip
                            {
                                Title = "Thực hành thường xuyên",
                                Content = "Đối mặt thường xuyên với các tình huống gây lo âu sẽ giúp giảm phản ứng lo âu theo thời gian",
                                Icon = "mdi-repeat"
                            }
                        },
                        Actions = new List<Action>
                        {
                            new Action
                            {
                                Id = "4-1",
                                Title = "Xây dựng thang đo lo âu",
                                Description = "Tạo danh sách các tình huống gây lo âu từ nhẹ đến nặng",
                                Duration = "15 phút",
                                Completed = false,
                                Required = true,
                                MoodTags = new List<string> { "Lập kế hoạch", "Tự nhận thức" }
                            },
                            new Action
                            {
                                Id = "4-2",
                                Title = "Thực hành đối mặt",
                                Description = "Đối mặt với một tình huống gây lo âu nhẹ từ thang đo của bạn",
                                Duration = "30 phút",
                                Completed = false,
                                Required = true,
                                MoodTags = new List<string> { "Đối mặt", "Xây dựng sự tự tin" }
                            },
                            new Action
                            {
                                Id = "4-3",
                                Title = "Phản ánh và điều chỉnh",
                                Description = "Đánh giá trải nghiệm đối mặt và điều chỉnh chiến lược si cần",
                                Duration = "10 phút",
                                Completed = false,
                                Required = false,
                                MoodTags = new List<string> { "Phản ánh", "Học hỏi" }
                            }
                        },
                        CanSkip = false,
                        RequireConfirmation = false,
                        NextPhaseId = "5",
                        CompletionCriteria = new List<CompletionCriterion>
                        {
                            new CompletionCriterion
                            {
                                Title = "Xây dựng thang đo",
                                Description = "Bạn đã xây dựng thang đo các tình huống gây lo âu"
                            },
                            new CompletionCriterion
                            {
                                Title = "Đối mặt với tình huống",
                                Description = "Bạn đã đối mặt với ít nhất một tình huống gây lo âu"
                            },
                            new CompletionCriterion
                            {
                                Title = "Áp dụng kỹ thuật",
                                Description = "Bạn đã áp dụng các kỹ thuật đã học để đối phó với tình huống"
                            }
                        },
                        Resources = new List<Resource>
                        {
                            new Resource
                            {
                                Title = "Kỹ thuật đối mặt với lo âu",
                                Url = "/docs/facing-anxiety"
                            },
                            new Resource
                            {
                                Title = "Xây dựng sự tự tin",
                                Url = "/docs/building-confidence"
                            },
                            new Resource
                            {
                                Title = "Chiến lược đối phó với tình huống khó khăn",
                                Url = "/docs/coping-strategies"
                            }
                        }
                    },
                    ["5"] = new PhaseDetail
                    {
                        Id = "5",
                        RoadmapId = "1",
                        Title = "Đánh giá & Duy trì",
                        Description = "Đánh giá tiến độ và xây dựng kế hoạch duy trì lâu dài",
                        ThemeColor = "green",
                        Introduction = "Phase cuối cùng tập trung vào việc đánh giá tiến độ của bạn và xây dựng kế hoạch duy trì lâu dài. Bạn sẽ xem xét những gì đã học được, đánh giá sự tiến bộ và phát triển chiến lược để duy trì những kỹ năng mới.",
                        VideoUrl = "/videos/phase5.mp4",
                        Tips = new List<Tip>
                        {
                            new Tip
                            {
                                Title = "Ghi nhận tiến bộ",
                                Content = "Dành thời gian để ghi nhận và ăn mừng những tiến bộ bạn đã đạt được",
                                Icon = "mdi-trophy"
                            },
                            new Tip
                            {
                                Title = "Xây dựng thói quen",
                                Content = "Tích hợp các kỹ thuật bạn đã học vào thói quen hàng ngày",
                                Icon = "mdi-calendar-check"
                            }
                        },
                        Actions = new List<Action>
                        {
                            new Action
                            {
                                Id = "5-1",
                                Title = "Đánh giá tiến độ",
                                Description = "Xem lại nhật ký và đánh giá sự tiến bộ của bạn",
                                Duration = "20 phút",
                                Completed = false,
                                Required = true,
                                MoodTags = new List<string> { "Đánh giá", "Phản ánh" }
                            },
                            new Action
                            {
                                Id = "5-2",
                                Title = "Xây dựng kế hoạch duy trì",
                                Description = "Phát triển kế hoạch để duy trì và tiếp tục cải thiện",
                                Duration = "30 phút",
                                Completed = false,
                                Required = true,
                                MoodTags = new List<string> { "Lập kế hoạch", "Duy trì" }
                            },
                            new Action
                            {
                                Id = "5-3",
                                Title = "Xác định nguồn hỗ trợ",
                                Description = "Xác định các nguồn hỗ trợ để giúp bạn duy trì tiến độ",
                                Duration = "15 phút",
                                Completed = false,
                                Required = false,
                                MoodTags = new List<string> { "Hỗ trợ", "Kết nối" }
                            }
                        },
                        CanSkip = false,
                        RequireConfirmation = false,
                        NextPhaseId = null,
                        CompletionCriteria = new List<CompletionCriterion>
                        {
                            new CompletionCriterion
                            {
                                Title = "Đánh giá tiến độ",
                                Description = "Bạn đã đánh giá tiến độ của mình trong toàn bộ lộ trình"
                            },
                            new CompletionCriterion
                            {
                                Title = "Xây dựng kế hoạch",
                                Description = "Bạn đã xây dựng kế hoạch duy trì lâu dài"
                            },
                            new CompletionCriterion
                            {
                                Title = "Xác định nguồn hỗ trợ",
                                Description = "Bạn đã xác định các nguồn hỗ trợ để giúp duy trì tiến độ"
                            }
                        },
                        Resources = new List<Resource>
                        {
                            new Resource
                            {
                                Title = "Duy trì tiến bộ dài hạn",
                                Url = "/docs/maintaining-progress"
                            },
                            new Resource
                            {
                                Title = "Phòng ngừa tái phát",
                                Url = "/docs/relapse-prevention"
                            },
                            new Resource
                            {
                                Title = "Xây dựng lối sống cân bằng",
                                Url = "/docs/balanced-lifestyle"
                            }
                        }
                    }
                }
            };

            // Initialize mental profile data
            _mentalProfileData = new MentalProfile
            {
                UserType = "Người đi làm",
                StressSource = "Công việc",
                ImprovementGoal = "Cân bằng công việc - cuộc sống",
                StressLevel = 65,
                DepressionRisk = 40,
                EmotionalIndex = new List<EmotionalIndexItem>
                {
                    new EmotionalIndexItem { Date = DateTime.Parse("2023-05-05"), Value = 65 },
                    new EmotionalIndexItem { Date = DateTime.Parse("2023-05-06"), Value = 58 },
                    new EmotionalIndexItem { Date = DateTime.Parse("2023-05-07"), Value = 72 },
                    new EmotionalIndexItem { Date = DateTime.Parse("2023-05-08"), Value = 68 },
                    new EmotionalIndexItem { Date = DateTime.Parse("2023-05-09"), Value = 75 }
                }
            };

            // Initialize recommendation data
            _recommendationData = new List<Recommendation>
            {
                new Recommendation
                {
                    Content = "Hãy dành 5 phút mỗi ngày để thực hành hít thở sâu. Điều này sẽ giúp giảm căng thẳng và cải thiện tập trung.",
                    IsMotivation = false,
                    Source = "TS. Nguyễn An Tâm, Chuyên gia tâm lý"
                },
                new Recommendation
                {
                    Content = "Mỗi ngày hãy ghi lại 3 điều bạn biết ơn. Thói quen này sẽ giúp bạn tập trung vào những điều tích cực trong cuộc sống.",
                    IsMotivation = false,
                    Source = "Nghiên cứu về Tâm lý học Tích cực"
                },
                new Recommendation
                {
                    Content = "Hãy nhớ rằng, mỗi bước nhỏ đều quan trọng. Bạn không cần phải hoàn hảo, chỉ cần tiến bộ mỗi ngày.",
                    IsMotivation = true,
                    Source = ""
                },
                new Recommendation
                {
                    Content = "Thử thách không phải để đánh bại bạn, mà để giúp bạn khám phá sức mạnh bên trong mình.",
                    IsMotivation = true,
                    Source = ""
                }
            };

            // Initialize suggestion data
            _suggestionData = new SuggestionData
            {
                UserTypeOptions = new List<Option>
                {
                    new Option { Text = "Học sinh", Value = "student" },
                    new Option { Text = "Sinh viên", Value = "university" },
                    new Option { Text = "Người đi làm", Value = "worker" },
                    new Option { Text = "Người cao tuổi", Value = "elderly" },
                    //new Option { Text = "Khác", Value = "other" }
                },
                IssueOptions = new Dictionary<string, List<Option>>
                {
                    ["student"] = new List<Option>
                    {
                        new Option { Text = "Áp lực học tập, thi cử", Value = "study_pressure" },
                        new Option { Text = "Bị bắt nạt hoặc cô lập ở trường", Value = "bullying" },
                        new Option { Text = "Không có bạn thân", Value = "no_close_friend" },
                        new Option { Text = "Mâu thuẫn với cha mẹ", Value = "parent_conflict" },
                        new Option { Text = "Mất động lực", Value = "no_motivation" }
                    },
                    ["university"] = new List<Option>
                    {
                        new Option { Text = "Lo lắng về tương lai", Value = "future_worry" },
                        new Option { Text = "Mất định hướng nghề nghiệp", Value = "career_confusion" },
                        new Option { Text = "Cô đơn", Value = "loneliness" },
                        new Option { Text = "Chán học", Value = "boredom" },
                        new Option { Text = "Stress vì thực tập/thi cử", Value = "intern_stress" }
                    },
                    ["worker"] = new List<Option>
                    {
                        new Option { Text = "Căng thẳng công việc", Value = "work_stress" },
                        new Option { Text = "Mâu thuẫn đồng nghiệp", Value = "colleague_conflict" },
                        new Option { Text = "Cảm giác không được công nhận", Value = "not_recognized" },
                        new Option { Text = "Không còn đam mê", Value = "no_passion" },
                        new Option { Text = "Mất cân bằng cuộc sống - công việc", Value = "work_life_balance" }
                    }
                },
                WhereOptions = new List<Option>
                {
                    new Option { Text = "Ở nhà", Value = "home" },
                    new Option { Text = "Ở trường/lớp học", Value = "school" },
                    new Option { Text = "Nơi làm việc", Value = "work" },
                    new Option { Text = "Trên mạng xã hội", Value = "social" },
                    //new Option { Text = "Khác", Value = "other" }
                },
                WhenOptions = new List<Option>
                {
                    new Option { Text = "Buổi sáng", Value = "morning" },
                    new Option { Text = "Trước khi ngủ", Value = "before_sleep" },
                    new Option { Text = "Buổi tối", Value = "evening" },
                    new Option { Text = "Khi đi học/làm", Value = "at_work" },
                    new Option { Text = "Luôn luôn", Value = "always" },
                    //new Option { Text = "Khác", Value = "other" }
                },
                RelatedOptions = new List<Option>
                {
                    new Option { Text = "Bố mẹ", Value = "parent" },
                    new Option { Text = "Giáo viên / giảng viên", Value = "teacher" },
                    new Option { Text = "Bạn bè / người yêu", Value = "friend" },
                    new Option { Text = "Sếp / đồng nghiệp", Value = "boss" },
                    new Option { Text = "Chính bản thân mình", Value = "myself" },
                    new Option { Text = "Không rõ", Value = "unknown" },
                    //new Option { Text = "Khác", Value = "other" }
                },
                SuggestedRoadmaps = new Dictionary<string, Dictionary<string, List<SuggestedRoadmap>>>
                {
                    ["student"] = new Dictionary<string, List<SuggestedRoadmap>>
                    {
                        ["study_pressure"] = new List<SuggestedRoadmap>
                        {
                            new SuggestedRoadmap
                            {
                                Id = "1",
                                Title = "Vượt qua áp lực học tập",
                                Description = "Phương pháp giảm căng thẳng và cải thiện hiệu suất học tập",
                                Match = 95,
                                Steps = 5,
                                IsPaid = true,
                                Price = 500000,
                                Features = new List<string>
                                {
                                    "5 bài tập thư giãn chuyên sâu",
                                    "Hướng dẫn quản lý thời gian hiệu quả",
                                    "Kỹ thuật học tập tối ưu",
                                    "Tư vấn 1-1 với chuyên gia",
                                    "Theo dõi tiến độ cá nhân"
                                }
                            }
                        },
                        ["future_worry"] = new List<SuggestedRoadmap>
                        {
                            new SuggestedRoadmap
                            {
                                Id = "1",
                                Title = "Vượt qua áp lực học tập",
                                Description = "Phương pháp giảm căng thẳng và cải thiện hiệu suất học tập",
                                Match = 95,
                                Steps = 5,
                                IsPaid = true,
                                Price = 500000,
                                Features = new List<string>
                                {
                                    "5 bài tập thư giãn chuyên sâu",
                                    "Hướng dẫn quản lý thời gian hiệu quả",
                                    "Kỹ thuật học tập tối ưu",
                                    "Tư vấn 1-1 với chuyên gia",
                                    "Theo dõi tiến độ cá nhân"
                                }
                            }
                        },
                        ["bullying"] = new List<SuggestedRoadmap>
                        {
                            new SuggestedRoadmap
                            {
                                Id = "2",
                                Title = "Xây dựng kỹ năng xã hội",
                                Description = "Phát triển sự tự tin và kỹ năng giao tiếp hiệu quả",
                                Match = 90,
                                Steps = 5,
                                IsPaid = true,
                                Price = 450000,
                                Features = new List<string>
                                {
                                    "Bài tập rèn luyện sự tự tin",
                                    "Kỹ thuật giao tiếp hiệu quả",
                                    "Xử lý tình huống khó khăn",
                                    "Hỗ trợ từ cộng đồng",
                                    "Tài liệu chuyên sâu về kỹ năng xã hội"
                                }
                            }
                        },
                        ["no_close_friend"] = new List<SuggestedRoadmap>
                        {
                            new SuggestedRoadmap
                            {
                                Id = "2",
                                Title = "Xây dựng kỹ năng xã hội",
                                Description = "Phát triển sự tự tin và kỹ năng giao tiếp hiệu quả",
                                Match = 90,
                                Steps = 5,
                                IsPaid = true,
                                Price = 450000,
                                Features = new List<string>
                                {
                                    "Bài tập rèn luyện sự tự tin",
                                    "Kỹ thuật giao tiếp hiệu quả",
                                    "Xử lý tình huống khó khăn",
                                    "Hỗ trợ từ cộng đồng",
                                    "Tài liệu chuyên sâu về kỹ năng xã hội"
                                }
                            }
                        }
                    },
                    ["university"] = new Dictionary<string, List<SuggestedRoadmap>>
                    {
                        ["career_confusion"] = new List<SuggestedRoadmap>
                        {
                            new SuggestedRoadmap
                            {
                                Id = "3",
                                Title = "Định hướng nghề nghiệp",
                                Description = "Khám phá đam mê và xây dựng lộ trình sự nghiệp rõ ràng",
                                Match = 93,
                                Steps = 5,
                                IsPaid = true,
                                Price = 550000,
                                Features = new List<string>
                                {
                                    "Bài kiểm tra định hướng nghề nghiệp",
                                    "Tư vấn 1-1 với chuyên gia hướng nghiệp",
                                    "Kế hoạch phát triển cá nhân",
                                    "Kỹ năng phỏng vấn và tìm việc",
                                    "Mạng lưới kết nối chuyên nghiệp"
                                }
                            }
                        },
                        ["future_worry"] = new List<SuggestedRoadmap>
                        {
                            new SuggestedRoadmap
                            {
                                Id = "3",
                                Title = "Định hướng nghề nghiệp",
                                Description = "Khám phá đam mê và xây dựng lộ trình sự nghiệp rõ ràng",
                                Match = 93,
                                Steps = 5,
                                IsPaid = true,
                                Price = 550000,
                                Features = new List<string>
                                {
                                    "Bài kiểm tra định hướng nghề nghiệp",
                                    "Tư vấn 1-1 với chuyên gia hướng nghiệp",
                                    "Kế hoạch phát triển cá nhân",
                                    "Kỹ năng phỏng vấn và tìm việc",
                                    "Mạng lưới kết nối chuyên nghiệp"
                                }
                            }
                        },
                        ["loneliness"] = new List<SuggestedRoadmap>
                        {
                            new SuggestedRoadmap
                            {
                                Id = "4",
                                Title = "Sống trọn vẹn đời sinh viên",
                                Description = "Tận hưởng và phát triển bản thân trong thời gian đại học",
                                Match = 88,
                                Steps = 5,
                                IsPaid = false,
                                Price = 0,
                                Features = new List<string>()
                            }
                        },
                        ["boredom"] = new List<SuggestedRoadmap>
                        {
                            new SuggestedRoadmap
                            {
                                Id = "4",
                                Title = "Sống trọn vẹn đời sinh viên",
                                Description = "Tận hưởng và phát triển bản thân trong thời gian đại học",
                                Match = 88,
                                Steps = 5,
                                IsPaid = false,
                                Price = 0,
                                Features = new List<string>()
                            }
                        }
                    },
                    ["worker"] = new Dictionary<string, List<SuggestedRoadmap>>
                    {
                        ["work_stress"] = new List<SuggestedRoadmap>
                        {
                            new SuggestedRoadmap
                            {
                                Id = "5",
                                Title = "Cân bằng công việc - cuộc sống",
                                Description = "Phương pháp quản lý stress và tạo sự cân bằng",
                                Match = 96,
                                Steps = 5,
                                IsPaid = true,
                                Price = 600000,
                                Features = new List<string>
                                {
                                    "5 kỹ thuật quản lý thời gian",
                                    "Phương pháp thiền mindfulness",
                                    "Kỹ năng đặt ranh giới lành mạnh",
                                    "Tư vấn 1-1 với chuyên gia",
                                    "Theo dõi mức độ stress hàng ngày"
                                }
                            }
                        },
                        ["work_life_balance"] = new List<SuggestedRoadmap>
                        {
                            new SuggestedRoadmap
                            {
                                Id = "5",
                                Title = "Cân bằng công việc - cuộc sống",
                                Description = "Phương pháp quản lý stress và tạo sự cân bằng",
                                Match = 96,
                                Steps = 5,
                                IsPaid = true,
                                Price = 600000,
                                Features = new List<string>
                                {
                                    "5 kỹ thuật quản lý thời gian",
                                    "Phương pháp thiền mindfulness",
                                    "Kỹ năng đặt ranh giới lành mạnh",
                                    "Tư vấn 1-1 với chuyên gia",
                                    "Theo dõi mức độ stress hàng ngày"
                                }
                            }
                        },
                        ["no_passion"] = new List<SuggestedRoadmap>
                        {
                            new SuggestedRoadmap
                            {
                                Id = "6",
                                Title = "Tìm lại đam mê trong công việc",
                                Description = "Khám phá lại ý nghĩa và niềm vui trong sự nghiệp",
                                Match = 92,
                                Steps = 5,
                                IsPaid = true,
                                Price = 500000,
                                Features = new List<string>
                                {
                                    "Bài tập khám phá giá trị cốt lõi",
                                    "Kỹ thuật đặt mục tiêu SMART",
                                    "Phương pháp tạo động lực nội tại",
                                    "Tư vấn phát triển sự nghiệp",
                                    "Công cụ đánh giá sự hài lòng"
                                }
                            }
                        },
                        ["not_recognized"] = new List<SuggestedRoadmap>
                        {
                            new SuggestedRoadmap
                            {
                                Id = "6",
                                Title = "Tìm lại đam mê trong công việc",
                                Description = "Khám phá lại ý nghĩa và niềm vui trong sự nghiệp",
                                Match = 92,
                                Steps = 5,
                                IsPaid = true,
                                Price = 500000,
                                Features = new List<string>
                                {
                                    "Bài tập khám phá giá trị cốt lõi",
                                    "Kỹ thuật đặt mục tiêu SMART",
                                    "Phương pháp tạo động lực nội tại",
                                    "Tư vấn phát triển sự nghiệp",
                                    "Công cụ đánh giá sự hài lòng"
                                }
                            }
                        }
                    },
                    ["default"] = new Dictionary<string, List<SuggestedRoadmap>>
                    {
                        ["default"] = new List<SuggestedRoadmap>
                        {
                            new SuggestedRoadmap
                            {
                                Id = "7",
                                Title = "Thư giãn với âm nhạc",
                                Description = "Bộ sưu tập nhạc thư giãn và thiền định giúp giảm căng thẳng",
                                Match = 85,
                                Steps = 5,
                                IsPaid = false,
                                Price = 0,
                                Features = new List<string>()
                            },
                            new SuggestedRoadmap
                            {
                                Id = "8",
                                Title = "Yoga cơ bản",
                                Description = "Các bài tập yoga đơn giản giúp thư giãn cơ thể và tâm trí",
                                Match = 80,
                                Steps = 5,
                                IsPaid = false,
                                Price = 0,
                                Features = new List<string>()
                            }
                        }
                    },
                    ["normal"] = new Dictionary<string, List<SuggestedRoadmap>>
                    {
                        ["normal"] = new List<SuggestedRoadmap>
                        {
                            new SuggestedRoadmap
                            {
                                Id = "9",
                                Title = "Duy trì sức khỏe tinh thần",
                                Description = "Các hoạt động và thói quen giúp duy trì trạng thái tâm lý tích cực",
                                Match = 98,
                                Steps = 5,
                                IsPaid = false,
                                Price = 0,
                                Features = new List<string>()
                            }
                        }
                    }
                },
                RoadmapIcons = new Dictionary<string, string>
                {
                    ["1"] = "mdi-school",
                    ["2"] = "mdi-account-group",
                    ["3"] = "mdi-briefcase",
                    ["4"] = "mdi-school",
                    ["5"] = "mdi-briefcase-check",
                    ["6"] = "mdi-heart-pulse",
                    ["7"] = "mdi-music",
                    ["8"] = "mdi-yoga",
                    ["9"] = "mdi-meditation",
                    ["default-paid"] = "mdi-star-circle"
                },
                AudioTracks = new List<string>
                {
                    "Relaxing Nature Sounds",
                    "Meditation Music",
                    "Deep Sleep Music",
                    "Stress Relief Melody"
                }
            };

            _completionData = new CompletionData
            {
                Statistics = new Statistics
                {
                    DaysActive = 14,
                    ActionsCompleted = 23,
                    EmotionalImprovement = 68
                },
                CouponCode = "MENTAL25",
                CouponDescription = "Sử dụng mã này để được giảm 25% cho khóa học \"Tâm lý học ứng dụng\" của chúng tôi",
                AdvancedRoadmaps = new List<AdvancedRoadmap>
                {
                    new AdvancedRoadmap
                    {
                        Title = "Lộ trình nâng cao: Vượt qua lo âu",
                        Description = "Các kỹ thuật nâng cao để đối phó với lo âu mãn tính"
                    },
                    new AdvancedRoadmap
                    {
                        Title = "Thiền định hàng ngày",
                        Description = "Xây dựng thói quen thiền định để cải thiện sức khỏe tinh thần"
                    }
                }
            };

            _detailedSteps = new Dictionary<int, DetailedStep>
            {
                [3] = new DetailedStep
                {
                    Id = 3,
                    Title = "Xây dựng thói quen lành mạnh",
                    Description = "Phát triển thói quen lành mạnh nhất quán tạo nền tảng cho sức khỏe tinh thần tốt. Những thay đổi nhỏ, bền vững trong thói quen hàng ngày của bạn có thể tạo ra tác động tích cực đáng kể theo thời gian.",
                    FocusAreas = new List<string>
                    {
                        "Thiết lập mô hình giấc ngủ đều đặn",
                        "Đưa hoạt động thể chất vào thói quen của bạn",
                        "Phát triển thói quen ăn uống lành mạnh",
                        "Tạo ranh giới giữa thời gian làm việc và cá nhân"
                    },
                    Activities = new List<Activity>
                    {
                        new Activity
                        {
                            Id = 1,
                            Title = "Danh sách kiểm tra vệ sinh giấc ngủ",
                            Description = "Tạo và thực hiện thói quen trước khi đi ngủ để cải thiện chất lượng giấc ngủ.",
                            Recommended = true,
                            Icon = "mdi-sleep"
                        },
                        new Activity
                        {
                            Id = 2,
                            Title = "Lịch vận động",
                            Description = "Lên lịch hoạt động thể chất thường xuyên mà bạn thích, ngay cả khi đó chỉ là đi bộ ngắn hàng ngày.",
                            Recommended = true,
                            Icon = "mdi-run"
                        },
                        new Activity
                        {
                            Id = 3,
                            Title = "Lập kế hoạch bữa ăn",
                            Description = "Lên kế hoạch các bữa ăn cân bằng bao gồm thực phẩm hỗ trợ tâm trạng như omega-3 và carbohydrate phức hợp.",
                            Recommended = false,
                            Icon = "mdi-food-apple-outline"
                        },
                        new Activity
                        {
                            Id = 4,
                            Title = "Ranh giới kỹ thuật số",
                            Description = "Đặt thời gian cụ thể để ngắt kết nối khỏi email và thông báo công việc.",
                            Recommended = false,
                            Icon = "mdi-cellphone-off"
                        }
                    }
                }
            };

            _completionViewData = new CompletionViewData
            {
                RoadmapSteps = new List<RoadmapStep>
                {
                    new RoadmapStep
                    {
                        Id = 1,
                        Title = "Self-Awareness",
                        Completed = true,
                        Icon = "mdi-eye-outline"
                    },
                    new RoadmapStep
                    {
                        Id = 2,
                        Title = "Stress Reduction",
                        Completed = true,
                        Icon = "mdi-hand-heart-outline"
                    },
                    new RoadmapStep
                    {
                        Id = 3,
                        Title = "Mindfulness",
                        Completed = true,
                        Icon = "mdi-sprout-outline"
                    },
                    new RoadmapStep
                    {
                        Id = 4,
                        Title = "Problem Solving",
                        Completed = false,
                        Icon = "mdi-brain"
                    },
                    new RoadmapStep
                    {
                        Id = 5,
                        Title = "Maintenance",
                        Completed = false,
                        Icon = "mdi-heart-pulse"
                    }
                },
                ImprovementOptions = new List<ImprovementOption>
                {
                    new ImprovementOption { Label = "Tôi đã cải thiện một chút", Value = "little" },
                    new ImprovementOption { Label = "Tôi đã cải thiện kha khá", Value = "moderate" },
                    new ImprovementOption { Label = "Tôi đã cải thiện rất nhiều", Value = "significant" }
                },
                Statistics = new Statistics
                {
                    DaysActive = 14,
                    ActionsCompleted = 23,
                    EmotionalImprovement = 68
                },
                Coupon = new Coupon
                {
                    Code = "MENTAL25",
                    Description = "Sử dụng mã này để được giảm 25% cho khóa học \"Tâm lý học ứng dụng\" của chúng tôi"
                },
                AdvancedRoadmaps = new List<AdvancedRoadmap>
                {
                    new AdvancedRoadmap
                    {
                        Title = "Lộ trình nâng cao: Vượt qua lo âu",
                        Description = "Các kỹ thuật nâng cao để đối phó với lo âu mãn tính"
                    },
                    new AdvancedRoadmap
                    {
                        Title = "Thiền định hàng ngày",
                        Description = "Xây dựng thói quen thiền định để cải thiện sức khỏe tinh thần"
                    }
                }
            };
        }

        public void MigrateToDb(HealpathyContext context)
        {
            var advisorId_deepSea1 = Guid.Parse("BB093A37-6450-48EF-8C2A-6605EB620444");



            var roadmaps = _defaultRoadmaps.Select(_ => {
                var id = Guid.NewGuid();
                var currentPhaseIndex = 0;
                return new Roadmap
                {
                    Id = id,
                    AdvisorId = advisorId_deepSea1,

                    Title = _.Title,
                    IntroText = JsonSerializer.Serialize(_.IntroText),
                    Description = _.Description,
                    Category = _.Category,
                    ThumbUrl = _.Image,

                    Price = null,
                    Discount = null,
                    DiscountExpiry = null,
                    Coupons = string.Empty,

                    Phases = _allPhases.Values.SelectMany(phase => phase.Values.Select(phase =>
                    {
                        currentPhaseIndex++;
                        var phaseId = Guid.NewGuid();

                        var nonActions = phase.Tips.Select(tip => new RoadmapRecommendation
                        {
                            RoadmapPhaseId = phaseId,

                            Title = tip.Title,
                            Content = tip.Content,
                            Description = null,
                            IsAction = false,

                            Duration = null,
                            MoodTags = null,
                            IsGeneralTip = true,
                            Source = null,

                            TargetEntityId = null,
                            EntityType = null
                        });

                        var actions = phase.Actions.Select(action => new RoadmapRecommendation
                        {
                            RoadmapPhaseId = phaseId,

                            Title = action.Title,
                            Content = null,
                            Description = action.Description,
                            IsAction = true,

                            Duration = null,
                            MoodTags = JsonSerializer.Serialize(action.MoodTags),
                            IsGeneralTip = false,
                            Source = null,

                            TargetEntityId = null,
                            EntityType = null
                        });

                        return new RoadmapPhase
                        {
                            Id = phaseId,
                            RoadmapId = id,

                            Title = phase.Title,
                            Description = JsonSerializer.Serialize(phase.CompletionCriteria),
                            Introduction = phase.Description ?? phase.Introduction,
                            Index = currentPhaseIndex,
                            TimeSpan = 3,
                            IsRequiredToAdvance = false,
                            QuestionsToAdvance = null,
                            VideoUrl = null,

                            // Milestones
                            Recommendations = nonActions.Union(actions).ToList()
                        };
                    }).ToList()).ToList()
                };
            });
            context.Roadmaps.AddRange(roadmaps);



            // Mental profile



            var surveyId = Guid.NewGuid();

            var answerId_student = Guid.NewGuid();
            var answerId_university = Guid.NewGuid();
            var answerId_worker = Guid.NewGuid();
            var answerId_elder = Guid.NewGuid();

            var answer_student = _suggestionData.UserTypeOptions.Where(_ => _.Value == "student").FirstOrDefault();
            var answer_university = _suggestionData.UserTypeOptions.Where(_ => _.Value == "university").FirstOrDefault();
            var answer_worker = _suggestionData.UserTypeOptions.Where(_ => _.Value == "worker").FirstOrDefault();
            var answer_elder = _suggestionData.UserTypeOptions.Where(_ => _.Value == "elderly").FirstOrDefault();

            var question1 = new McqQuestion
            {
                SurveyId = surveyId,
                Content = "Công việc hiện tại của bạn là gì?",
                Precondition = null,
                Index = 0,
                Answers = [
                    new McqAnswer {
                        Id = answerId_student,
                        Content = answer_student?.Text ?? string.Empty,
                        OptionValue = answer_student?.Value ?? string.Empty,
                        Score = 0,
                        Index = 0
                    },
                    new McqAnswer {
                        Id = answerId_university,
                        Content = answer_university?.Text ?? string.Empty,
                        OptionValue = answer_university?.Value ?? string.Empty,
                        Score = 0,
                        Index = 1
                    },
                    new McqAnswer {
                        Id = answerId_worker,
                        Content = answer_worker?.Text ?? string.Empty,
                        OptionValue = answer_worker?.Value ?? string.Empty,
                        Score = 0,
                        Index = 2
                    },
                    new McqAnswer {
                        Id = answerId_elder,
                        Content = answer_elder?.Text ?? string.Empty,
                        OptionValue = answer_elder?.Value ?? string.Empty,
                        Score = 0,
                        Index = 3
                    }
                ]
            };
            var question2_student = new McqQuestion
            {
                SurveyId = surveyId,
                Content = "Vấn đề hiện tại của bạn là gì?",
                Precondition = answerId_student.ToString(),
                Index = 1,
                Answers = _suggestionData.IssueOptions["student"]
                    .Select(_ => new McqAnswer { Content = _.Text, OptionValue = _.Value, Score = 0, Index = 1 }).ToList()
            };
            var question2_university = new McqQuestion
            {
                SurveyId = surveyId,
                Content = "Vấn đề hiện tại của bạn là gì?",
                Precondition = answerId_university.ToString(),
                Index = 1,
                Answers = _suggestionData.IssueOptions["university"]
                    .Select(_ => new McqAnswer { Content = _.Text, OptionValue = _.Value, Score = 0, Index = 1 }).ToList()
            };
            var question2_worker = new McqQuestion
            {
                SurveyId = surveyId,
                Content = "Vấn đề hiện tại của bạn là gì?",
                Precondition = answerId_worker.ToString(),
                Index = 1,
                Answers = _suggestionData.IssueOptions["worker"]
                    .Select(_ => new McqAnswer { Content = _.Text, OptionValue = _.Value, Score = 0, Index = 1 }).ToList()
            };
            //var question2_elder = new McqQuestion
            //{
            //    SurveyId = surveyId,
            //    Content = "Vấn đề hiện tại của bạn là gì?",
            //    Precondition = answerId_elder.ToString(),
            //    Index = 1,
            //    Answers = _suggestionData.IssueOptions["elderly"]
            //        .Select(_ => new McqAnswer { Content = _.Text, OptionValue = _.Value, Score = 0, Index = 1 }).ToList()
            //};
            var question3 = new McqQuestion
            {
                SurveyId = surveyId,
                Content = "Vấn đề đó thường xảy ra ở đâu?",
                Precondition = null,
                Index = 2,
                Answers = _suggestionData.WhereOptions
                    .Select(_ => new McqAnswer { Content = _.Text, OptionValue = _.Value, Score = 0, Index = 2 }).ToList()
            };
            var question4 = new McqQuestion
            {
                SurveyId = surveyId,
                Content = "Vấn đề này thường xảy ra vào thời gian nào?",
                Precondition = null,
                Index = 3,
                Answers = _suggestionData.WhenOptions
                    .Select(_ => new McqAnswer { Content = _.Text, OptionValue = _.Value, Score = 0, Index = 3 }).ToList()
            };
            var question5 = new McqQuestion
            {
                SurveyId = surveyId,
                Content = "Những ai là người liên quan đến vấn đề này?",
                Precondition = null,
                Index = 4,
                Answers = _suggestionData.RelatedOptions
                    .Select(_ => new McqAnswer { Content = _.Text, OptionValue = _.Value, Score = 0, Index = 4 }).ToList()
            };

            var survey = new Survey
            {
                Id = surveyId,
                Name = "First_Evaluation",
                Description = "First_Evaluation",
                IsScientific = false,
                Questions = [
                    question1,
                    question2_student,
                    question2_university,
                    question2_worker,
                    //question2_elder,
                    question3,
                    question4,
                    question5
                ]
            };
            context.Surveys.Add(survey);

            context.SaveChanges();
        }

        public List<PhaseModel> GetRoadmapSteps()
        {
            return _defaultRoadmaps[0].Phases;
        }

        public List<RoadmapModel> GetMentalHealthRoadmaps()
        {
            return _defaultRoadmaps;
        }

        public RoadmapModel? GetRoadmapDetails(string id)
        {
            if (string.IsNullOrEmpty(id) || !_defaultRoadmaps.Any(r => r.Id == id))
                return _defaultRoadmaps[0];

            return _defaultRoadmaps.FirstOrDefault(r => r.Id == id);
        }

        public PhaseDetail? GetPhaseDetails(string roadmapId, string phaseId)
        {
            if (!string.IsNullOrEmpty(roadmapId) && !string.IsNullOrEmpty(phaseId) &&
                _allPhases.ContainsKey(roadmapId) && _allPhases[roadmapId].ContainsKey(phaseId))
            {
                return _allPhases[roadmapId][phaseId];
            }

            return null;
        }

        public MentalProfile GetMentalProfileData()
        {
            return _mentalProfileData;
        }

        public List<Recommendation> GetRecommendationData()
        {
            return _recommendationData;
        }

        public SuggestionData GetSuggestionData()
        {
            return _suggestionData;
        }

        public CompletionData GetCompletionData()
        {
            return _completionData;
        }

        public DetailedStep? GetDetailedStep(int? stepId)
        {
            if (stepId.HasValue && _detailedSteps.ContainsKey(stepId.Value))
            {
                return _detailedSteps[stepId.Value];
            }

            return null;
        }

        public CompletionViewData GetCompletionViewData()
        {
            return _completionViewData;
        }
    }
}