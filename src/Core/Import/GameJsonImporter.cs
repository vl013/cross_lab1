using System.Text.Json;
using Core.Dto;

namespace Core.Import;

public static class GameJsonImporter
{
    public static ImportResult<GameDto> Load(string path)
    {
        try
        {
            string json = File.ReadAllText(path);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            // Якщо Deserialize поверне null, беремо порожній список [][cite: 12, 15]
            var items = JsonSerializer.Deserialize<List<GameDto>>(json, options) ?? [];
            
            return new ImportResult<GameDto>(items, []); // JSON зазвичай валідується цілком
        }
        catch (Exception ex)
        {
            // Якщо файл зламаний, повертаємо помилку[cite: 15]
            return new ImportResult<GameDto>([], [$"Помилка розбору JSON: {ex.Message}"]);
        }
    }
}