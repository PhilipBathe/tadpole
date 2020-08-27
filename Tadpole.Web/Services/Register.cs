using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Tadpole.Web.Services
{
    public class Register : IRegister
    {
        private readonly string _connectionString;

        public Register(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TadpoleConnection");
        }

        public bool IsEmailAlreadyRegistered(string email)
        {
            int matchedEmails = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
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

            return matchedEmails > 0;
        }

        public void Save(RegistrationUser user)
        {
            //specifically asked to use plain ADO.Net - I'd rather have used Dapper :-)

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = $"INSERT INTO RegisteredUser (Email, PasswordHash, PasswordSalt) VALUES (@email, @passwordHash, @passwordSalt)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@email", user.Email);
                    command.Parameters.AddWithValue("@passwordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@passwordSalt", user.PasswordSalt);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
