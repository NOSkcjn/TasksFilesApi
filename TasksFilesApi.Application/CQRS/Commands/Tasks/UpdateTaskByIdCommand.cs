using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TasksFilesApi.Domain.Entities;

namespace TasksFilesApi.Application.CQRS.Commands.Tasks
{
    public class UpdateTaskByIdCommand : IRequest<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public Status Status { get; set; }
    }

    public class UpdateTaskByIdCommandHandler : IRequestHandler<UpdateTaskByIdCommand, int>
    {
        private readonly IMainContext _context;

        public UpdateTaskByIdCommandHandler(IMainContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateTaskByIdCommand command, CancellationToken cancellationToken)
        {
            var task = _context.Tasks.Where(x => x.Id == command.Id).FirstOrDefault();
            task.Name = command.Name;
            task.Date = command.Date;
            task.Status = command.Status;

            await _context.SaveChangesAsync();
            return task.Id;
        }
    }
}
