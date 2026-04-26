using System;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "Music Catalog Database";

        string dbPath = "music_catalog.bin";
        MusicDatabase db = new MusicDatabase(dbPath);

        db.LoadFromFile();

        if (db.GetAllItems().Count == 0)
        {
            Console.WriteLine("\nБаза данных пуста. Заполняем тестовыми данными...");
            SeedData.FillDatabase(db);
        }

        bool exit = false;
        Console.ForegroundColor = ConsoleColor.Cyan;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine(@"
            ___  ___          _                  _        _             
            |  \/  |         (_)                | |      | |            
            | .  . |_   _ ___ _  ___    ___ __ _| |_ __ _| | ___   __ _ 
            | |\/| | | | / __| |/ __|  / __/ _` | __/ _` | |/ _ \ / _` |
            | |  | | |_| \__ \ | (__  | (_| (_| | || (_| | | (_) | (_| |
            \_|  |_/\__,_|___/_|\___|  \___\__,_|\__\__,_|_|\___/ \__, |
                                                                   __/ |
                                                                  |___/ ");
            Console.WriteLine("\n1. Просмотр всей базы данных");
            Console.WriteLine("2. Добавить новый альбом");
            Console.WriteLine("3. Удалить альбом по ID");
            Console.WriteLine("\nЗАПРОСЫ");
            Console.WriteLine("4. Показать альбомы дороже заданной цены (перечень)");
            Console.WriteLine("5. Показать альбомы с рейтингом выше заданного (перечень)");
            Console.WriteLine("6. Показать общее количество прослушиваний (одно значение)");
            Console.WriteLine("7. Показать среднюю цену альбома (одно значение)");
            Console.WriteLine("8. Выход");

            Console.Write("\nВыберите пункт: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    db.ViewAll();
                    break;

                case "2":
                    AddNewItem(db);
                    break;

                case "3":
                    DeleteItem(db);
                    break;

                case "4":
                    QueryAlbumsByPrice(db);
                    break;

                case "5":
                    QueryAlbumsByRating(db);
                    break;

                case "6":
                    Console.WriteLine($"\nОбщее количество прослушиваний: {db.GetTotalListenCount():N0}");
                    break;

                case "7":
                    Console.WriteLine($"\nСредняя цена альбома: {db.GetAveragePrice():C}");
                    break;

                case "8":
                    exit = true;
                    continue;

                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    static void AddNewItem(MusicDatabase db)
    {
        Console.Clear();
        Console.WriteLine("ДОБАВЛЕНИЕ НОВОГО АЛЬБОМА\n");

        Console.Write("Название: ");
        string title = Console.ReadLine();

        Console.Write("Исполнитель: ");
        string artist = Console.ReadLine();

        Console.Write("Дата релиза (ГГГГ-ММ-ДД): ");
        DateTime releaseDate;
        while (!DateTime.TryParse(Console.ReadLine(), out releaseDate))
        {
            Console.Write("Неверный формат. Повторите (ГГГГ-ММ-ДД): ");
        }

        Console.Write("Длительность (секунд): ");
        int duration = int.Parse(Console.ReadLine());

        Console.Write("Цена (руб, например 599,99): ");
        decimal price = decimal.Parse(Console.ReadLine());

        Console.Write("Содержит ненормативную лексику? (да/нет): ");
        bool isExplicit = Console.ReadLine().ToLower() == "да";

        Console.Write("Рейтинг (0-5): ");
        double rating = double.Parse(Console.ReadLine());

        Console.Write("Жанр: ");
        string genre = Console.ReadLine();

        Console.Write("Количество прослушиваний: ");
        long listenCount = long.Parse(Console.ReadLine());

        var newItem = new MusicItem(0, title, artist, releaseDate, duration, price, isExplicit, rating, genre, listenCount);
        db.AddItem(newItem);

        Console.WriteLine($"\n✓ Альбом \"{title}\" успешно добавлен с ID {newItem.Id}");
    }

    static void DeleteItem(MusicDatabase db)
    {
        Console.WriteLine("УДАЛЕНИЕ АЛЬБОМА\n");
        db.ViewAll();

        Console.Write("\nВведите ID альбома для удаления: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var item = db.GetAllItems().FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                Console.Write($"Удалить \"{item.Title}\" - {item.Artist}? (да/нет): ");
                if (Console.ReadLine().ToLower() == "да")
                {
                    if (db.DeleteById(id))
                    {
                        Console.WriteLine($"✓ Альбом с ID {id} удалён");
                    }
                }
                else
                {
                    Console.WriteLine("Удаление отменено");
                }
            }
            else
            {
                Console.WriteLine($"Альбом с ID {id} не найден");
            }
        }
        else
        {
            Console.WriteLine("Неверный ID");
        }
    }

    static void QueryAlbumsByPrice(MusicDatabase db)
    {
        Console.Clear();
        Console.Write("Введите минимальную цену (руб): ");
        if (decimal.TryParse(Console.ReadLine(), out decimal minPrice))
        {
            var results = db.GetAlbumsExpensive(minPrice);
            if (results.Count == 0)
            {
                Console.WriteLine($"\nНет альбомов дороже {minPrice:C}");
            }
            else
            {
                Console.WriteLine($"\nАльбомы дороже {minPrice:C}:\n");
                Console.WriteLine(MusicItem.GetHeader());
                Console.WriteLine(MusicItem.GetSeparator());
                foreach (var item in results)
                {
                    Console.WriteLine(item.ToString());
                }
                Console.WriteLine($"\nНайдено: {results.Count}");
            }
        }
    }

    static void QueryAlbumsByRating(MusicDatabase db)
    {
        Console.Clear();
        Console.Write("Введите минимальный рейтинг (0-5): ");
        if (double.TryParse(Console.ReadLine(), out double minRating))
        {
            var results = db.GetAlbumsHighRaiting(minRating);
            if (results.Count == 0)
            {
                Console.WriteLine($"\nНет альбомов с рейтингом выше {minRating:F1}");
            }
            else
            {
                Console.WriteLine($"\nАльбомы с рейтингом выше {minRating:F1}:\n");
                Console.WriteLine(MusicItem.GetHeader());
                Console.WriteLine(MusicItem.GetSeparator());
                foreach (var item in results)
                {
                    Console.WriteLine(item.ToString());
                }
                Console.WriteLine($"\nНайдено: {results.Count}");
            }
        }
    }
}