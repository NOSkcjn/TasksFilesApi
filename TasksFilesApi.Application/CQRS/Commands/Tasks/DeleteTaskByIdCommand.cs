using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TasksFilesApi.Application.CQRS.Commands.Tasks
{
    public class DeleteTaskByIdCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteTaskByIdCommand(int id)
        {
            Id = id;
        }
    }

    public class DeleteTaskByIdCommandHandler : IRequestHandler<DeleteTaskByIdCommand, bool>
    {
        private readonly IMainContext _context;

        public DeleteTaskByIdCommandHandler(IMainContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteTaskByIdCommand command, CancellationToken cancellationToken)
        {
            var task = _context.Tasks.Where(x => x.Id == command.Id).FirstOrDefault();
            if (task == null) 
                return false;

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();
            return true;
        }
    }

}
