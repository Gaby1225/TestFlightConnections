using FluentMigrator;

namespace FlightConnections.DbMigrations.Versions
{
    [Migration(202301151406)]
    public class CreateConnectionsTable : Migration
    {
        public override void Up()
        {
            Create.Table("Connections")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Origin").AsAnsiString(50).NotNullable()
                .WithColumn("Destiny").AsAnsiString(50).NotNullable()
                .WithColumn("Value").AsAnsiString(50).NotNullable();
        }
        public override void Down()
        {
            Delete.Table("Connections");
        }
    }
}
