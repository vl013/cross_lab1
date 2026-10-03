using Core.Dto;
using Core.Import;

Console.WriteLine("CrossApp - Імпорт Ігор\n");

string path = args.Length > 0 ? args[0] : Path.Combine("data", "sample.csv");

if (!File.Exists(path))
{
    Console.WriteLine($"Файл не знайдено: {Path.GetFullPath(path)}");
    return 1;
}

// ДОДАТКОВЕ 1: Вибір імпортера на основі розширення файлу через switch[cite: 15]
string extension = Path.GetExtension(path).ToLowerInvariant();
ImportResult<GameDto> result = extension switch
{
    ".csv" => GameCsvImporter.Load(path),
    ".json" => GameJsonImporter.Load(path),
    _ => throw new NotSupportedException($"Формат {extension} не підтримується")
};

Console.WriteLine($"Завантажено записів: {result.Items.Count}");
Console.WriteLine(new string('-', 90));
Console.WriteLine($" {"ID",-6} | {"Жанр",-15} | {"Назва",-35} | {"Рік",-5} | Розробник");
Console.WriteLine(new string('-', 90));

foreach (GameDto b in result.Items.Take(5))
{
    Console.WriteLine($" {b.Id,-6} | {b.Genre,-15} | {b.Title,-35} | {b.Year,-5} | {b.Developer ?? "-"}");
}

Console.WriteLine();

if (result.Errors.Count > 0)
{
    Console.WriteLine($"Пропущено рядків: {result.Errors.Count}");
    foreach (string e in result.Errors)
    {
        Console.WriteLine($"  ! {e}");
    }
}

// ДОДАТКОВЕ 2: Статистика одним рядком[cite: 16]
int total = result.Items.Count + result.Errors.Count;
double errorRate = total > 0 ? (double)result.Errors.Count / total * 100 : 0;

Console.WriteLine(new string('=', 90));
Console.WriteLine($"СТАТИСТИКА: Усього: {total} | Прийнято: {result.Items.Count} | Пропущено: {result.Errors.Count} | Відсоток помилок: {errorRate:F1}%");

return 0;