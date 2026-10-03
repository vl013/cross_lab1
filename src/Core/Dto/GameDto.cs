namespace Core.Dto;

public record GameDto(
    string Id, 
    string Genre, 
    string Title, 
    int Year, 
    string? Developer = null);