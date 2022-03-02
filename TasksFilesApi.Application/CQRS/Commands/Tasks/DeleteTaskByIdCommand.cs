using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TasksFilesApi.Services.Interfaces;

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
        private readonly IStorageService _storage;

        public DeleteTaskByIdCommandHandler(IMainContext context, IStorageService storage)
        {
            _context = context;
            _storage = storage;
        }

        public async Task<bool> Handle(DeleteTaskByIdCommand command, CancellationToken cancellationToken)
        {
            var task = _context.Tasks.Include(x => x.Files).Where(x => x.Id == command.Id).FirstOrDefault();
            if (task == null)
                return false;

            foreach (var guid in task.Files.Select(x => x.ExtGuid))
                _storage.Delete(guid);

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();
            return true;
        }
    }

}
