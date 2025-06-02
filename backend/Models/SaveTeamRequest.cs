namespace backend.Models
{
    public class SaveTeamRequest
    {
        public string Email { get; set; }
        public string TeamName { get; set; }
        public List<PokemonInTeam> Pokemons { get; set; }
    }
}
