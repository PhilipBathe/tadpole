using System.Data;
using System.Data.SqlClient;

namespace Tadpole.IntegrationTests.Data
{
    public class RegisterTestData : IRegisterTestData
    {
        public int GetCountByEmail(string email)
        {
            int matchedEmails = 0;

            using (SqlConnection connection = new SqlConnection(Config.TestDatabaseConnectionString))
            {
                string sql = $"SELECT COUNT(1) FROM RegisteredUser WHERE Email LIKE @email";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@email", email);

                    connection.Open();
                    matchedEmails = (int)command.ExecuteScalar();
                    connection.Close();
                }
            }

            return matchedEmails;
        }

        public void WipeAll()
        {
            using (SqlConnection connection = new SqlConnection(Config.TestDatabaseConnectionString))
            {
                string sql = $"DELETE FROM RegisteredUser";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
