using GrisAPI.Models.Enums;

namespace GrisAPI.DTOs;

public class CardFilterRequest
{
    public int CurrentPage { get; set; } = 0;
    public int Quantity { get; set; } = 50;
    public Symbol? Symbol { get; set; } = null!;
    public Manifestation? Manifestation { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
}