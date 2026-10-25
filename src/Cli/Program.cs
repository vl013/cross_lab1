using Core.Domain;
using Core.Dto;
using Core.Import;

Console.WriteLine("----- Сценарій 1: Успіх -----");
Game game = Game.Create("G-100", "RPG", "The Witcher 3", 2015, "CD Projekt Red");
Console.WriteLine(game);

game.AddReview(10);
game.AddReview(8);
Console.WriteLine(game);

game.Archive();
Console.WriteLine(game);

Console.WriteLine("\n----- Сценарій 2: Порушення інваріантів -----");
TryDo("Створення з порожньою назвою", () => Game.Create("G-101", "Action", "", 2020, "Dev"));
TryDo("Оцінка поза межами (11)", () => game.AddReview(11));
TryDo("Відгук для архівної гри", () => game.AddReview(9));
TryDo("Повторна архівація", () => game.Archive());

Console.WriteLine("\n----- Додаткове завдання 1: Парсинг + Доменні правила -----");
// Імітуємо результати 3-ї лаби (файловий парсинг)
var dtoList = new List<GameDto> 
{ 
    new GameDto("G-001", "Sandbox", "Minecraft", 2009, "Mojang"),
    new GameDto("G-002", "Shooter", "Doom", 1993, "id Software"),
    new GameDto("G-ERR", "", "Bad Game", 2025, null) // Порожній жанр
};
var importResult = new ImportResult<GameDto>(dtoList, new List<string> { "рядок 5: якась помилка парсингу файлу" });

var (validGames, domainErrors) = GameDomainImporter.MapToDomain(importResult);
Console.WriteLine($"Успішно створено сутностей: {validGames.Count}");
foreach (var e in domainErrors)
    Console.WriteLine($" ! {e}");

// Метод-помічник для відловлювання винятків
static void TryDo(string title, Action action)
{
    try
    {
        action();
        Console.WriteLine($" {title}: виняток НЕ спрацював - інваріант відсутній!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($" {title}: {ex.GetType().Name} - {ex.Message}");
    }
}