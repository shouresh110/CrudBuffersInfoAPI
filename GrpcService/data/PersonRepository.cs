using System.Data;
using System.Data.SqlClient;

namespace GrpcService.data
{
    public class PersonRepository
    {
        private string connectionString = "Server=PRODUCTION\\SQL2019;Initial Catalog=Employee;Integrated Security=True";

        public List<Person> GetPerson(int Id)
        {
            List<Person> persons = new List<Person>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Sp_Read_Person", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", Id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Person person = new Person
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                NationalCode = reader["NationalCode"].ToString(),
                                BirthDate = reader["BirthDate"].ToString(),
                            };

                            persons.Add(person);
                        }
                    }
                }
            }

            return persons;
        }

        private readonly List<Person> _persons = new();

        public IEnumerable<Person> GetAll() => _persons;

        public Person? GetById(int id) => _persons.FirstOrDefault(p => p.Id == id);

        public void Create(Person person) => _persons.Add(person);

        public bool Update(Person updatedPerson)
        {
            var person = GetById(updatedPerson.Id);
            if (person == null) return false;

            person.FirstName = updatedPerson.FirstName;
            person.LastName = updatedPerson.LastName;
            person.NationalCode = updatedPerson.NationalCode;
            person.BirthDate = updatedPerson.BirthDate;

            return true;
        }

        public bool Delete(int id)
        {
            var person = GetById(id);
            return person != null && _persons.Remove(person);
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string BirthDate { get; set; }
    }
}
