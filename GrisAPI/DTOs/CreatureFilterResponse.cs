namespace GrisAPI.DTOs;

public sealed class CreatureFilterResponse
{
    public int MaxNumberOfPages { get; set; } = 0;
    public List<CreatureDto> Creatures { get; set; } = [];
}