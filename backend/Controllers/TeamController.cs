using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using backend.Models;
using MongoDB.Bson; // importa tus modelos reales

namespace backend.Controllers
{
    [ApiController]
    [Route("api/teams")]
    public class TeamController : ControllerBase
    {
        private readonly IMongoCollection<User> _users;

        public TeamController(IMongoClient mongoClient, IConfiguration config)
        {
            var db = mongoClient.GetDatabase(config["MongoDB:DatabaseName"]);
            _users = db.GetCollection<User>("Users");
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveTeam([FromBody] SaveTeamRequest req)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, req.Email);
            var update = Builders<User>.Update.Push("teams", new Team
            {
                Name = req.TeamName,
                Pokemons = req.Pokemons
            });

            var result = await _users.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0 ? Ok("Equipo guardado.") : BadRequest("No se encontró el usuario.");
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetTeamsByUser([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("El parámetro 'email' es obligatorio.");
            }

            var user = await _users.Find(u => u.Email == email).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            return Ok(user.Teams ?? new List<Team>());
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteTeam([FromQuery] string id)
        {
            var filter = Builders<User>.Filter.ElemMatch(u => u.Teams, t => t.Id == id);
            var update = Builders<User>.Update.PullFilter("teams", Builders<Team>.Filter.Eq("_id", ObjectId.Parse(id)));

            var result = await _users.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0 ? Ok("Equipo eliminado.") : NotFound("Equipo no encontrado.");
        }

        public class RemovePokemonByIndexRequest
        {
            public string TeamId { get; set; }
            public int PokemonIndex { get; set; }
        }


        [HttpPut("remove-pokemon")]
        public async Task<IActionResult> RemovePokemonFromTeam([FromBody] RemovePokemonByIndexRequest request)
        {
            // Filtra al usuario que tenga ese equipo
            var filter = Builders<User>.Filter.ElemMatch(u => u.Teams, t => t.Id == request.TeamId);

            // UNSET al Pokémon en la posición X dentro del array pokemons del equipo seleccionado
            var unset = Builders<User>.Update.Unset($"teams.$.pokemons.{request.PokemonIndex}");

            var resultUnset = await _users.UpdateOneAsync(filter, unset);
            if (resultUnset.ModifiedCount == 0)
                return NotFound("No se pudo borrar el Pokémon (unset)");

            // PULL para limpiar los elementos null
            var pull = Builders<User>.Update.Pull("teams.$.pokemons", BsonNull.Value);
            await _users.UpdateOneAsync(filter, pull);

            return Ok("Pokémon eliminado correctamente");
        }

        [HttpPut("add-pokemon")]
        public async Task<IActionResult> AddPokemonToTeam([FromBody] AddPokemonRequest req)
        {
            var filter = Builders<User>.Filter.ElemMatch(u => u.Teams, t => t.Id == req.TeamId);
            var update = Builders<User>.Update.Push("teams.$.pokemons", req.Pokemon);

            var result = await _users.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0 ? Ok() : NotFound();
        }

        public class AddPokemonRequest
        {
            public string TeamId { get; set; }
            public PokemonInTeam Pokemon { get; set; }
        }

        [HttpPut("update-pokemon")]
        public async Task<IActionResult> UpdatePokemonInTeam([FromBody] UpdatePokemonRequest request)
        {
            var filter = Builders<User>.Filter.ElemMatch(u => u.Teams, t => t.Id == request.TeamId);

            var update = Builders<User>.Update
                .Set($"teams.$.pokemons.{request.PokemonIndex}", request.UpdatedPokemon);

            var result = await _users.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0 ? Ok("Pokémon actualizado") : NotFound("Equipo o Pokémon no encontrado");
        }

        public class UpdatePokemonRequest
        {
            public string TeamId { get; set; }
            public int PokemonIndex { get; set; }
            public PokemonInTeam UpdatedPokemon { get; set; }
        }







    }
}
