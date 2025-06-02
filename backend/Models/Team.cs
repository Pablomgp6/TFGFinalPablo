using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Team
{
    [BsonId] // Marca este campo como el ID del documento
    [BsonRepresentation(BsonType.ObjectId)] // Para usar string en vez de ObjectId directamente
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("pokemons")]
    public List<PokemonInTeam> Pokemons { get; set; } = new();
}
