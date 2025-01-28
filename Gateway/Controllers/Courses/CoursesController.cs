using Contract.Domain.Shared.MultimediaBase;
using Contract.Domain.UserAggregate.Constants;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Courses.CourseRequests;
using Contract.Requests.Courses.CourseRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Courses;

public sealed class CoursesController : ContractController
{
    public CoursesController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    //[ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetPaged([FromQuery] QueryCourseDto dto)
    {
        GetPagedCoursesQuery query = new(dto);
        return await Send(query);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        GetCourseByIdQuery query = new(id);
        return await Send(query);
    }

    [HttpGet("min")]
    //[ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetMin([FromQuery] QueryCourseDto dto)
    {
        GetMinimumCoursesQuery query = new(dto);
        return await Send(query);
    }

    [HttpGet("multiple")]
    //[ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetMultiple([FromQuery] Guid[] ids)
    {
        GetMultipleCoursesQuery query = new(ids);
        return await Send(query);
    }

    //[HttpGet("BySection")]
    //[ResponseCache(Duration = 60)]
    //public async Task<IActionResult> GetBySection([FromQuery] Guid sectionId)
    //{
    //    var query = new GetSingleCourseQuery(new QuerySingleCourseDto() { SectionId = sectionId });
    //    var result = await _coursesService.GetSingleAsync(query);
    //    return result.AsResponse();
    //}

    //[HttpGet("{id}/similar")]
    //[ResponseCache(Duration = 60)]
    //public async Task<IActionResult> GetSimilar(Guid id)
    //{
    //    var result = await _coursesService.GetAsync();
    //    return result.AsResponse();
    //}







    [HttpPost]
    [Authorize(Roles = RoleConstants.ADVISOR)]
    public async Task<IActionResult> Create([FromForm] CreateCourseDto dto, [FromServices] IFileService fileService)
    {
        if (InstructorId is null)
            return Forbid();

        var id = Guid.NewGuid();
        List<Multimedia> medias = [];
        if (dto.Thumb is not null)
        {
            var image = await fileService.SaveImageAndUpdateDto(dto.Thumb, id);
            if (image is not null)
                medias.Add(image);
        }

        var command = new CreateCourseCommand(id, dto, ClientId, (Guid)InstructorId, medias);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize(Roles = RoleConstants.ADVISOR)]
    public async Task<IActionResult> Update([FromForm] UpdateCourseDto dto, [FromServices] IFileService fileService)
    {
        if (InstructorId is null)
            return Forbid();

        //List<Multimedia> addedMedias = [];
        //var mediaDtos = dto.AddedMedias is null
        //    ? []
        //    : dto.AddedMedias.Select(_ => (_, dto.Id)).ToList();
        //if (mediaDtos is not null && mediaDtos.Count > 0)
        //{
        //    var medias = await fileService.SaveMediasAndUpdateDtos(mediaDtos!);
        //    if (medias is not null)
        //        addedMedias.AddRange(medias!);
        //}
        //List<Guid> removedMedias = dto.RemovedMedias?.ToList() ?? [];

        var command = new UpdateCourseCommand(dto, ClientId, (Guid)InstructorId, null, null);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteCourseCommand(id, ClientId);
        return await Send(command);
    }
}