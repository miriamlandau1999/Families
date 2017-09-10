using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication1.data
{
    public class FamiliesDb
    {
        private string _connectionString;
        public FamiliesDb(string connectionString)
        {
            _connectionString = connectionString;
        }
        public int AddFamily(Family family)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Families(LastName, IsChasidush, Type) VALUES(@name, 0, 0) SELECT @@IDENTITY";
            command.Parameters.AddWithValue("@name", family.LastName);
            connection.Open();
            return int.Parse(command.ExecuteScalar().ToString());
        }
        public IEnumerable<FamilyWithNumberOfKids> GetFamiliesWithNumberOfKids()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "FamiliesWithNubmerOfKids";
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            List<FamilyWithNumberOfKids> families = new List<FamilyWithNumberOfKids>();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                FamilyWithNumberOfKids family = new FamilyWithNumberOfKids
                {
                    Id = (int)reader["Id"],
                    LastName = (string)reader["LastName"],
                    NumberOfKids = (int)reader["NumberOfKids"]
                };
                families.Add(family);
            }
            return families;
        }
        public void DeleteFamily(Family family)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Kids WHERE FamilyId = @id DELETE FROM Families WHERE ID = @id";
            command.Parameters.AddWithValue("@id", family.Id);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public Activity AddedDeleted(string table, Family family)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {table} WHERE Id = @id";
            command.Parameters.AddWithValue("@id",family.Id);
            connection.Open();
            if(command.ExecuteScalar()  != null)
            {
                return Activity.Added;
            }
            return Activity.Deleted;
        }
        public void AddKid(Kid kid)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Kids VALUES(@firstName, @age, @familyId)";
            command.Parameters.AddWithValue("@firstName", kid.FirstName);
            command.Parameters.AddWithValue("@age", kid.Age);
            command.Parameters.AddWithValue("@familyId", kid.FamilyId);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public IEnumerable<Kid> GetKids(int FamilyId)
        {

            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Kids WHERE FamilyId = @id ORDER BY Age";
            command.Parameters.AddWithValue("@id", FamilyId);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<Kid> kids = new List<Kid>();
            while (reader.Read())
            {
                Kid kid = new Kid
                {
                    FirstName = (string)reader["FirstName"],
                    Age = (int)reader["Age"]
                };
                kids.Add(kid);
            }
            return kids;
        }
    }
}
