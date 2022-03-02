using System;
using System.Collections.Generic;
using TasksFilesApi.Application.CQRS.Queries.Files;

namespace TasksFilesApi.Application.CQRS.Queries.Tasks
{
    public class TaskResponse
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Name { get; set; }

        public int Status { get; set; }

        public IEnumerable<FileResponse> Files { get; set; }
    }
}
