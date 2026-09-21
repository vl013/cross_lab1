using Core;

Console.OutputEncoding = System.Text.Encoding.UTF8;
EnvironmentReport report = EnvironmentInfo.Collect();

Console.WriteLine("CrossApp - інформація про середовище");
Console.WriteLine("Студент: Федащук Владислав, група ФЕІ-35");
Console.WriteLine(new string('-', 52));
Console.WriteLine($"OC : {report.OsDescription}");
Console.WriteLine($"Runtime : {report.FrameworkDescription}");
Console.WriteLine($"Архітектура: {report.ProcessArchitecture}");
Console.WriteLine($"RID (визначено): {report.DetectedRid}");
Console.WriteLine($"RID (від .NET): {report.ReportedRid}");
Console.WriteLine($"Каталог : {report.BaseDirectory}");
Console.WriteLine(new string('-', 52));
Console.WriteLine("Предметна область: Бібліотека (видання, примірник, читач, видача)");
Console.WriteLine($"Інфо збірки: {report.BuildNote}");