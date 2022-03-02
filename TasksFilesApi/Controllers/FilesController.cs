using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksFilesApi.Application.CQRS.Commands.Files;
using TasksFilesApi.Presentation.Extensions;
using MediatR;
using TasksFilesApi.Application.CQRS.Queries.Files;

namespace TasksFilesApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IMediator _mediator;

        public FilesController(ILogger<FilesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _mediator.Send(new GetAllFilesArchiveQuery());
                if (result == null)
                    return NoContent();

                return File(result.Data, result.ContentType, result.Name);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetFileByIdQuery(id));
                if (result == null)
                    return NotFound();

                return File(result.Data, result.ContentType, result.Name);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
        }

        [RequestSizeLimit(int.MaxValue)]
        [HttpPost]
        public async Task<IActionResult> Post(IList<IFormFile> files)
        {
            try
            {
                var command = new CreateFilesCommand
                {
                    Files = await Task.WhenAll(files.Select(async x =>
                    {
                        var data = x.GetBytesAsync();
                        return new FileInfo { Name = x.FileName, ContentType = x.ContentType, Data = await data };
                    }))
                };

                if (!await _mediator.Send(command))
                    return Conflict();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
        }

        [RequestSizeLimit(int.MaxValue)]
        [HttpPost("{taskId}")]
        public async Task<IActionResult> Post(int? taskId, IFormFile file)
        {
            try
            {
                if (!taskId.HasValue)
                    return BadRequest();

                var data = await file.GetBytesAsync();
                var command = new CreateTaskFileCommand { TaskId = taskId.Value, Name = file.FileName, ContentType = file.ContentType, Data = data };

                if (!await _mediator.Send(command))
                    return Conflict();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
        }

        [RequestSizeLimit(int.MaxValue)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int? id, IFormFile file)
        {
            try
            {
                if (!id.HasValue)
                    return BadRequest();

                var data = await file.GetBytesAsync();
                var command = new UpdateFileByIdCommand { Id = id.Value, Name = file.FileName, ContentType = file.ContentType, Data = data };

                if (!await _mediator.Send(command))
                    return Conflict();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteFileByIdCommand(id);
                if (!await _mediator.Send(command))
                    return NotFound();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
        }
    }
}
