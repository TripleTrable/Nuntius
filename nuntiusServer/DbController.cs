
namespace NuntiusServer
{
    /// <summary>
    /// Factory for the DbController
    /// </summary>
    public static class DbController
    {
        /// <summary>
        /// True = psql ; false = mysql
        /// </summary>
       static bool dbms = true; 

        public static IDbController Instance
        {
            get 
            {
                if(dbms)
                    return PsqlController.Instance;
                else
                    return MysqlController.Instance;
            }
        }
    }
}