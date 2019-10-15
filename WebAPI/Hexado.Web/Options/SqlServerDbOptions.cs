namespace Hexado.Web.Options
{
    #nullable disable
    public class SqlServerDbOptions
    {
        public const string SectionName = "SqlServerDb";

        public string ConnectionString { get; set; }
    }
    #nullable restore
}