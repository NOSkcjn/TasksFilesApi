using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TasksFilesApi.Domain.Entities;

namespace TasksFilesApi.Application.CQRS.Commands.Tasks
{
    public class CreateTaskCommand : IRequest<int>
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public int Status { get; set; }
    }

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, int>
    {
        private readonly IMainContext _context;

        public CreateTaskCommandHandler(IMainContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
        {
            var task = new ServiceTask();
            task.Name = command.Name;
            task.Date = command.Date;
            task.Status = (Status)command.Status;

            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();
            return task.Id;
        }
    }
}
