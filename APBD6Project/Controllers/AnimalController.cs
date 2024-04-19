using System.Data.SqlClient;
using APBD6.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBD6.Controllers;

[Route("api/animals")]
[ApiController]
public class AnimalController : ControllerBase
{
    private readonly string _connectionString = @"Data Source=DAWID\SQLEXPRESS;Initial Catalog=master;Integrated Security=True";
    
    [HttpGet]
    public IActionResult GetAnimalsList([FromQuery] string orderBy = "name")
    {
        List<Animal> animals = new List<Animal>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = $"SELECT * FROM Animal ORDER BY {orderBy} ASC";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Animal animal = new Animal
                    {
                        idAnimal = Convert.ToInt32(reader["IdAnimal"]),
                        name = reader["Name"].ToString(),
                        description = reader["Description"].ToString(),
                        category = reader["Category"].ToString(),
                        area = reader["Area"].ToString()
                    };
                    animals.Add(animal);
                }
                
                reader.Close();
            }
        }

        return Ok(animals);
    }

    [HttpPost]
    public IActionResult AddAnimal([FromBody] Animal animal)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Animal (IdAnimal ,Name, Description, Category, Area) " +
                               "VALUES (@IdAnimal, @Name, @Description, @Category, @Area)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdAnimal", animal.idAnimal);
                    command.Parameters.AddWithValue("@Name", animal.name);
                    command.Parameters.AddWithValue("@Description", animal.description);
                    command.Parameters.AddWithValue("@Category", animal.category);
                    command.Parameters.AddWithValue("@Area", animal.area);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok("Animal added successfully.");
                    }
                    else
                    {
                        return BadRequest("Failed to add animal.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{idAnimal}")]
    public IActionResult UpdateAnimal(int idAnimal, [FromBody] Animal animal)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query =
                    "UPDATE Animal SET Name = @Name, Description = @Description, Category = @Category, Area = @Area " +
                        "WHERE IdAnimal = @IdAnimal";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdAnimal", idAnimal);
                    command.Parameters.AddWithValue("@Name", animal.name);
                    command.Parameters.AddWithValue("@Description", animal.description);
                    command.Parameters.AddWithValue("@Category", animal.category);
                    command.Parameters.AddWithValue("@Area", animal.area);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok("Animal changed successfully.");
                    }
                    else
                    {
                        return BadRequest("Failed to change animal.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{idAnimal}")]
    public IActionResult DeleteAnimal(int idAnimal, [FromBody] Animal animal)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Animal WHERE IdAnimal = @IdAnimal";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdAnimal", idAnimal);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok("Animal deleted successfully.");
                    }
                    else
                    {
                        return BadRequest("Failed to delete animal.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}