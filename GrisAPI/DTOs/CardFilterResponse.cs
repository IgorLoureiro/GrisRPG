namespace GrisAPI.DTOs;

public class CardFilterResponse
{
    public int MaxNumberOfPages { get; set; } = 0;
    public List<CardDto> Cards { get; set; } = [];
}