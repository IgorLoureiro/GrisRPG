namespace GrisAPI.DTOs;

public sealed class CreatureFilterRequest
{
    public int CurrentPage { get; set; } = 0;
    public int Quantity { get; set; } = 50;
    public string Name { get; set; } = string.Empty;
}