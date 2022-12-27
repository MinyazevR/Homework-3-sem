namespace MyNUnitWeb;

using Microsoft.EntityFrameworkCore;

public class DataBaseContext: DbContext
{
    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }
}
