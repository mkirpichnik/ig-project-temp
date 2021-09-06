using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace WeddingForward.Api.Extensions
{
    public static class AWSDatabaseExtensions
    {
        public static string GetRDSConnectionString(this IConfiguration configuration)
        {
            string dbname = "web-app";//ConfigurationManager.AppSettings["RDS_DB_NAME"];

            string username = "admin";//ConfigurationManager.AppSettings["RDS_USERNAME"];

            string password = "cYVWNkZncBu!Q5t";//;ConfigurationManager.AppSettings["RDS_PASSWORD"];

            string hostname = "aa19eugijzof4bz.cumxuir9aj74.us-east-2.rds.amazonaws.com,1433";//ConfigurationManager.AppSettings["RDS_HOSTNAME"];

            return "Data Source=" + hostname + ";Initial Catalog=" + dbname + ";User ID=" + username + ";Password=" + password + ";";
        }
    }
}
