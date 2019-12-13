namespace BookOrganizer.Data.SqlServer
{
    public class ConnectionString
    {
        public string Identifier { get; set; }
        public string Server { get; set; } //DataSource
        public string Database { get; set; } //InitialCatalog
        public bool Trusted_Connection { get; set; } //IntegratedSecurity
        public bool Default { get; set; }
    }
}
