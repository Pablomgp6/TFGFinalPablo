using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

public class PokemonInTeam
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("moves")]
    public List<string> Moves { get; set; } = new();

    [BsonElement("ability")]
    public string Ability { get; set; }

    [BsonElement("evs")]
    public Dictionary<string, int> EVs { get; set; } = new();

    [BsonElement("ivs")]
    public Dictionary<string, int> IVs { get; set; } = new();

    [BsonElement("nature")]
    public string Nature { get; set; }

    [BsonElement("item")]
    public string Item { get; set; }
}
