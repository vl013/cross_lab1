using Core.Dto;

namespace Core.Domain;

public sealed class Game
{
    // Приватні поля стану
    private int _reviewsCount;
    private double _totalScore;

    // Властивості лише для читання (інкапсуляція)
    public string Id { get; }
    public string Genre { get; }
    public string Title { get; }
    public int Year { get; }
    public string? Developer { get; }
    
    // Властивості стану
    public double AverageRating => _reviewsCount == 0 ? 0 : Math.Round(_totalScore / _reviewsCount, 1);
    public GameStatus Status { get; private set; } // Змінюється лише всередині класу

    // Приватний конструктор (створити об'єкт можна ТІЛЬКИ через Create)
    private Game(string id, string genre, string title, int year, string? developer, GameStatus status = GameStatus.Active, int reviewsCount = 0, double totalScore = 0)
    {
        Id = id;
        Genre = genre;
        Title = title;
        Year = year;
        Developer = developer;
        Status = status;
        _reviewsCount = reviewsCount;
        _totalScore = totalScore;
    }

    // Фабричний метод з перевіркою інваріантів
    public static Game Create(string id, string genre, string title, int year, string? developer)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Ідентифікатор обов'язковий", nameof(id));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Назва гри не може бути порожньою", nameof(title));
        if (string.IsNullOrWhiteSpace(genre))
            throw new ArgumentException("Жанр не може бути порожнім", nameof(genre));
        if (year < 1950 || year > DateTime.Now.Year + 5)
            throw new ArgumentOutOfRangeException(nameof(year), year, "Рік випуску поза допустимими межами");

        return new Game(id.Trim(), genre.Trim(), title.Trim(), year, developer?.Trim());
    }

    // Метод зміни стану: додавання відгуку
    public void AddReview(int score)
    {
        if (Status == GameStatus.Archived)
            throw new InvalidOperationException($"Не можна додати відгук: гру {Id} ({Title}) перенесено в архів.");
        
        if (score < 1 || score > 10)
            throw new ArgumentOutOfRangeException(nameof(score), score, "Оцінка має бути від 1 до 10");

        _reviewsCount++;
        _totalScore += score;
    }

    // Метод зміни стану з перевіркою переходів (Додаткове завдання 3)
    public void Archive()
    {
        if (Status == GameStatus.Archived)
            throw new InvalidOperationException($"Гра {Id} вже знаходиться в архіві.");
        
        Status = Status switch
        {
            GameStatus.Active => GameStatus.Archived,
            _ => throw new InvalidOperationException("Недопустимий перехід стану")
        };
    }

    // Мапінг DTO для зв'язку з 3-м тижнем
    public GameDto ToDto() => new(Id, Genre, Title, Year, Developer);
    
    public static Game FromDto(GameDto dto) => 
        Create(dto.Id, dto.Genre, dto.Title, dto.Year, dto.Developer);

    public override string ToString() => 
        $"{Id} | {Title} ({Year}) | Рейтинг: {AverageRating}/10 | Статус: {Status}";
}