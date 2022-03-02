using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TasksFilesApi.Services.Interfaces;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace TasksFilesApi.Application.CQRS.Queries.Files
{
    public class GetAllFilesArchiveQuery : IRequest<FileData>
    {
    }

    public class GetAllFilesArchiveQueryHandler : IRequestHandler<GetAllFilesArchiveQuery, FileData>
    {
        private readonly IMainContext _context;
        private readonly IStorageService _storage;

        public GetAllFilesArchiveQueryHandler(IMainContext context, IStorageService storage)
        {
            _context = context;
            _storage = storage;
        }

        public async Task<FileData> Handle(GetAllFilesArchiveQuery query, CancellationToken cancellationToken)
        {
            var filesList = await _context.Files.ToListAsync();
            if (filesList == null)
                return null;

            var filesDictionary = filesList?.AsReadOnly()?.ToDictionary(x => x.Name, y => y.ExtGuid);
            if (filesDictionary == null)
                return null;

            var result = await _storage.CreateArchiveAsync(filesDictionary);
            if (result == null)
                return null;

            return new FileData { ContentType = MediaTypeNames.Application.Zip, Data = result.Item2, Name = result.Item1 };
        }
    }
}
