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
                .WithColumn("Origin").AsAnsiString(3).NotNullable()
                .WithColumn("Destiny").AsAnsiString(3).NotNullable()
                .WithColumn("Value").AsDecimal(5,2).NotNullable();
        }
        public override void Down()
        {
            Delete.Table("Connections");
        }
    }
}
