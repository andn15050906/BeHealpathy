using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Progress.SurveyRequests.Dtos;
using Contract.Requests.Progress.SurveyRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Contract.Helpers.Storage;
using Contract.Helpers;

namespace Gateway.Controllers.Progress;

public sealed class SurveysController : ContractController
{
    private readonly IAppLogger _logger;

    public SurveysController(IMediator mediator, IAppLogger logger) : base(mediator)
    {
        _logger = logger;
    }



    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] QuerySurveyDto dto)
    {
        GetPagedSurveysQuery query = new(dto);
        return await Send(query);
    }

    /*
    [HttpGet("all-questions")]
    public IActionResult GetQuestions()
    {
        var id_pupil = Guid.NewGuid();
        var id_student = Guid.NewGuid();
        var id_laborer = Guid.NewGuid();

        var questions = new List<Question>()
        {
            new("Công việc hiện tại của bạn là gì?", [
                new Answer(id_pupil, "Học sinh"),
                new Answer(id_student, "Sinh viên"),
                new Answer(id_laborer, "Người đi làm")
            ]),
            new("Vấn đề hiện tại của bạn là gì?", [
                new Answer("Áp lực học tập, thi cử", id_pupil),
                new Answer("Bị bắt nạt hoặc cô lập ở trường", id_pupil),
                new Answer("Không có bạn thân", id_pupil),
                new Answer("Mâu thuẫn với cha mẹ", id_pupil),
                new Answer("Mất động lực", id_pupil),
                new Answer("Khác", id_pupil),

                new Answer("Lo lắng về tương lai", id_student),
                new Answer("Mất định hướng nghề nghiệp", id_student),
                new Answer("Cô đơn", id_student),
                new Answer("Chán học", id_student),
                new Answer("Stress vì học tập / thi cử", id_student),
                new Answer("Khác", id_student),

                new Answer("Căng thẳng công việc", id_laborer),
                new Answer("Mâu thuẫn đồng nghiệp", id_laborer),
                new Answer("Không được công nhận", id_laborer),
                new Answer("Không còn đam mê", id_laborer),
                new Answer("Mất cân bằng cuộc sống", id_laborer),
                new Answer("Khác", id_laborer)
            ]),
            new("Vấn đề này xảy ra ở những đâu?", [
                new Answer("Ở nhà"),
                new Answer("Ở trường/lớp học"),
                new Answer("Ở nơi làm việc"),
                new Answer("Trên mạng xã hội"),
                new Answer("Không rõ"),
                new Answer("Khác")
            ]),
            new("Vấn đề thường xảy ra khi nào?", [
                new Answer("Buổi sáng"),
                new Answer("Buổi chiều"),
                new Answer("Buổi tối"),
                new Answer("Trước khi ngủ"),
                new Answer("Khi đang học"),
                new Answer("Khi đang làm việc"),
                new Answer("Khi ở một mình"),
                new Answer("Trên mạng xã hội"),
                new Answer("Luôn luôn diễn ra"),
                new Answer("Không rõ"),
                new Answer("Khác"),
            ]),
            new("Theo bạn, ai là người đã gây ra vấn đề này?", [
                new Answer("Cha mẹ"),
                new Answer("Giáo viên"),
                new Answer("Bạn bè"),
                new Answer("Người yêu"),
                new Answer("Sếp"),
                new Answer("Đồng nghiệp"),
                new Answer("Bản thân"),
                new Answer("Không rõ"),
                new Answer("Khác"),
            ]),
        };

        return Ok(questions);
    }
    */

    public class Answer(string content, Guid? treePath = null)
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; } = content;
        public Guid? PreCondition { get; set; } = treePath;

        public Answer(Guid id, string content, Guid? treePath = null) : this(content, treePath)
        {
            Id = id;
        }
    }

    public class Question(string content, List<Answer> answers)
    {
        public string Content { get; set; } = content;
        public List<Answer> Answers { get; set; } = answers;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateSurveyDto dto)
    {
        CreateSurveyCommand command = new(Guid.NewGuid(), dto, ClientId);
        return await Send(command);
    }

    [HttpPost("import")]
    [Authorize]
    public async Task<IActionResult> Create(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(BusinessMessages.Survey.INVALID_FILE_FORMAT);

        try
        {
            var dto = FileConverter.ProcessSurveyFromExcelFile(file);
            CreateSurveyCommand command = new(Guid.NewGuid(), dto, ClientId);
            return await Send(command);
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return BadRequest(BusinessMessages.Survey.INVALID_FILE_FORMAT);
        }
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update(UpdateSurveyDto dto)
    {
        UpdateSurveyCommand command = new(dto, ClientId);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteSurveyCommand command = new(id, ClientId);
        return await Send(command);
    }
}