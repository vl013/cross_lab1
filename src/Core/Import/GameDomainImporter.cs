using Core.Dto;
using Core.Domain;

namespace Core.Import;

public static class GameDomainImporter
{
    // Перетворює ImportResult<GameDto> (дані) на сутності Game з перевіркою бізнес-правил
    public static (IReadOnlyList<Game> Games, IReadOnlyList<string> Errors) MapToDomain(ImportResult<GameDto> importResult)
    {
        var games = new List<Game>();
        var errors = new List<string>(importResult.Errors); // Копіюємо помилки парсингу

        foreach (var dto in importResult.Items)
        {
            try
            {
                var game = Game.FromDto(dto); // Тут спрацьовують інваріанти домену
                games.Add(game);
            }
            catch (Exception ex)
            {
                errors.Add($"Помилка домену для гри {dto.Id}: {ex.Message}");
            }
        }

        return (games.AsReadOnly(), errors.AsReadOnly());
    }
}