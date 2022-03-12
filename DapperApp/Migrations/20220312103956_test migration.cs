using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DapperApp.Migrations
{
    public partial class testmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROC usp_GetEmployee
                    @Id int
                AS 
                BEGIN 
                    SELECT *
                    FROM Employees
                    WHERE Id = @Id
                END
                GO
            ");

            migrationBuilder.Sql(@"
                CREATE PROC usp_GetALLEmployee
                AS 
                BEGIN 
                    SELECT *
                    FROM Employees
                END
                GO
            ");

            migrationBuilder.Sql(@"
                CREATE PROC usp_AddEmployee
                    @Id int OUTPUT,
                    @Name varchar(MAX),
	                @Age  varchar(MAX),
	                @Position varchar(MAX),
	                @CompanyId int
                AS
                BEGIN 
                    INSERT INTO Employees (Name,Age,Position,CompanyId)
                                VALUES(@Name,@Age,@Position,@CompanyId);
	                SELECT @Id = SCOPE_IDENTITY();
                END
                GO
            ");

            migrationBuilder.Sql(@"
                CREATE PROC usp_UpdateCompany
	                @Id int,
                    @Name varchar(MAX),
	                @Age  varchar(MAX),
	                @Position varchar(MAX),
	                @CompanyId int
                AS
                BEGIN 
                    UPDATE Employees  
	                SET 
		                Name = @Name, 
		                Age = @Age,
		                Position = @Position, 
		                CompanyId = @CompanyId
	                WHERE Id = @Id;
	                SELECT @Id = SCOPE_IDENTITY();
                END
                GO
            ");

            migrationBuilder.Sql(@"
                CREATE PROC usp_RemoveEmployees
                    @Id int
                AS 
                BEGIN 
                    DELETE
                    FROM Employees
                    WHERE Id  = @Id
                END
                GO	
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
