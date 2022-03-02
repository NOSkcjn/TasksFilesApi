using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TasksFilesApi.Application.CQRS.Queries.Files;

namespace TasksFilesApi.Application.CQRS.Queries.Tasks
{
    public class GetTaskByIdQuery : IRequest<TaskResponse>
    {
        public int Id { get; set; }

        public GetTaskByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskResponse>
    {
        private readonly IMainContext _context;

        public GetTaskByIdQueryHandler(IMainContext context)
        {
            _context = context;
        }

        public async Task<TaskResponse> Handle(GetTaskByIdQuery query, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.Include(x => x.Files).FirstOrDefaultAsync(x => x.Id == query.Id);
            if (task == null)
                return null;

            return new TaskResponse
            {
                Id = task.Id,
                Date = task.Date,
                Name = task.Name,
                Status = (int)task.Status,
                Files = task.Files.Select(x => new FileResponse { Id = x.Id, Name = x.Name, ContentType = x.ContentType })
            };
        }
    }
}
