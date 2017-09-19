namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGenreToGenreTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Genres(Id, Name) values(1, 'Jazz')");
            Sql("INSERT INTO Genres(Id, Name) values(2, 'Blues')");
            Sql("INSERT INTO Genres(Id, Name) values(3, 'Rock')");
            Sql("INSERT INTO Genres(Id, Name) values(4, 'Country')");
        }

        public override void Down()
        {
            Sql("DELETE FROM Genres WHERE Id IN (1, 2, 3, 4)");
        }
    }
}
