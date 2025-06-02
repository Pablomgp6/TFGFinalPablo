using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

public class Stats
{
    [BsonElement("hp")]
    [JsonPropertyName("hp")]
    public int HP { get; set; }

    [BsonElement("atk")]
    [JsonPropertyName("atk")]
    public int Attack { get; set; }

    [BsonElement("def")]
    [JsonPropertyName("def")]
    public int Defense { get; set; }

    [BsonElement("spAtk")]
    [JsonPropertyName("spAtk")]
    public int SpecialAttack { get; set; }

    [BsonElement("spDef")]
    [JsonPropertyName("spDef")]
    public int SpecialDefense { get; set; }

    [BsonElement("speed")]
    [JsonPropertyName("speed")]
    public int Speed { get; set; }
}
