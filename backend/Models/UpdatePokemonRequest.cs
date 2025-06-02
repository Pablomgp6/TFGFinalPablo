namespace backend.Models
{
    public class UpdatePokemonRequest
    {
        public string TeamId { get; set; }
        public int PokemonIndex { get; set; }
        public PokemonInTeam UpdatedPokemon { get; set; }
    }
}
