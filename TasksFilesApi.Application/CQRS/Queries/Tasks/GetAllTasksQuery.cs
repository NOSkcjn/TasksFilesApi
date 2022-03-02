using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TasksFilesApi.Application.CQRS.Queries.Files;

namespace TasksFilesApi.Application.CQRS.Queries.Tasks
{
    public class GetAllTasksQuery : IRequest<IEnumerable<TaskResponse>>
    {
    }

    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskResponse>>
    {
        private readonly IMainContext _context;

        public GetAllTasksQueryHandler(IMainContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskResponse>> Handle(GetAllTasksQuery query, CancellationToken cancellationToken)
        {
            var tasksList = await _context.Tasks.Include(x => x.Files).ToListAsync();

            if (tasksList == null)
                return null;

            return tasksList.AsReadOnly().Select(x =>
                new TaskResponse
                {
                    Id = x.Id,
                    Date = x.Date,
                    Name = x.Name,
                    Status = (int)x.Status,
                    Files = x.Files.Select(x => new FileResponse { Id = x.Id, Name = x.Name, ContentType = x.ContentType })
                });
        }
    }
}
