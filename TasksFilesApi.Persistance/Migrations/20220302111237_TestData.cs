using Microsoft.EntityFrameworkCore.Migrations;

namespace TasksFilesApi.Persistance.Migrations
{
    public partial class TestData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO Tasks (Date, Name, Status) VALUES (GETDATE(), 'Task1', 0);
                INSERT INTO Tasks (Date, Name, Status) VALUES (GETDATE(), 'Task2', 1);
                INSERT INTO Tasks (Date, Name, Status) VALUES (GETDATE(), 'Task3', 2);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
