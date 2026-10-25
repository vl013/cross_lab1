using System.Globalization;
using Core.Dto;

namespace Core.Import;

public static class GameCsvImporter
{
    private const char Separator = ';'; // Роздільник[cite: 6]

    public static ImportResult<GameDto> Load(string path)
    {
        var items = new List<GameDto>();
        var errors = new List<string>();
        string[] lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length; i++)
        {
            int number = i + 1;
            string line = lines[i];

            // Пропускаємо порожні рядки або коментарі[cite: 6]
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
                continue;

            // Пропускаємо заголовок таблиці[cite: 6]
            if (number == 1 && line.StartsWith("id", StringComparison.OrdinalIgnoreCase))
                continue;

            switch (ParseLine(line))
            {
                case ParseOk ok:
                    items.Add(ok.Value);
                    break;
                case ParseFailed failed:
                    errors.Add($"рядок {number}: {failed.Reason}"); // Фіксуємо номер рядка[cite: 7]
                    break;
            }
        }
        return new ImportResult<GameDto>(items, errors);
    }

    private static ParseOutcome ParseLine(string line)
    {
        string[] parts = line.Split(Separator, StringSplitOptions.TrimEntries);

        return parts switch
        {
            // Патерн властивостей: перевірка кількості колонок. Відкидає рядки де бракує колонок
            { Length: < 4 } => new ParseFailed($"очікую мінімум 4 колонки, отримав {parts.Length}"),
            
            // Патерни списків + константні патерни: перевірка на порожні поля. Перевіряє щоб жанр або назва не були порожніми
            [_, "", _, ..] or [_, _, "", ..] => new ParseFailed("Жанр або назва порожні"),
            
            // Охоронна умова when: перевірка року (має бути числом і в межах епохи відеоігор). Перевіряє щоб рік був саме числом і не менше ніж 1950
            [_, _, _, var year, ..] when !int.TryParse(year, NumberStyles.Integer, CultureInfo.InvariantCulture, out int y) || y < 1950 || y > DateTime.Now.Year 
                => new ParseFailed($"рік '{year}' поза допустимими межами або не є числом"),
            
            // Якщо все ідеально (без розробника)
            [var id, var genre, var title, var year] 
                => new ParseOk(new GameDto(id, genre, title, int.Parse(year))),
                
            // Якщо все ідеально (з розробником)
            [var id, var genre, var title, var year, var developer] 
                => new ParseOk(new GameDto(id, genre, title, int.Parse(year), developer)),
                
            // Гілка за замовчуванням (забагато колонок)
            _ => new ParseFailed($"занадто багато колонок: {parts.Length}")
        };
    }

    // Внутрішня ієрархія результатів розбору
    private abstract record ParseOutcome;
    private sealed record ParseOk(GameDto Value) : ParseOutcome;
    private sealed record ParseFailed(string Reason) : ParseOutcome;
}