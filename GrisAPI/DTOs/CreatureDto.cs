using GrisAPI.Models;

namespace GrisAPI.DTOs;

public class CreatureDto
{
    public CreatureDto(){}
    
    public CreatureDto(Creature creature)
    {
        Id = creature.Id;
        Name = creature.Name;
        ExtraDeck = creature.ExtraDeck;
        ExtraDeckId = creature.ExtraDeckId;
    }
    
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public int ExtraDeckId { get; set; }
    public ExtraDeck ExtraDeck { get; set; } = new ExtraDeck();
}