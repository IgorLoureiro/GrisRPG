using GrisAPI.Models;

namespace GrisAPI.DTOs;

public class CreatureDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ExtraDeckId { get; set; }
    public ExtraDeck ExtraDeck { get; set; } = new ExtraDeck();
}