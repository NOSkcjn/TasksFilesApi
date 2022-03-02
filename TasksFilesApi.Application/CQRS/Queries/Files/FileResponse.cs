using System.Text.Json.Serialization;

namespace TasksFilesApi.Application.CQRS.Queries.Files
{
    public class FileResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }
    }
}
