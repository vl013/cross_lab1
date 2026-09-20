# CrossApp
Наскрізний проєкт з крос-платформного програмування.
Предметна область: Бібліотека. Сутності: Book, BookCopy, Reader, Loan.
Призначення: облік видач примірників книг читачам.
Чому саме вона: Ця тема здалася мені найцікавішою для розробки

## Запуск
dotnet build

dotnet run --project src/Cli

## Публікація
 Режим                        Розмір publish     Потрібен runtime 

 win-x64 self-contained       ~76 МБ             No 
|win-x64 framework-dependent  ~198 КВ            Yes .NET 10