using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class MusicDatabase
{
    private string _filePath;
    private List<MusicItem> _items;

    public MusicDatabase(string path)
    {
        _filePath = path;
        _items = new List<MusicItem>();
    }

    public void LoadFromFile()
    {
        if (File.Exists(_filePath))
        {
            try
            {
                using (FileStream fs = new FileStream(_filePath, FileMode.Open))
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    _items.Clear();
                    int count = reader.ReadInt32(); // Чтение количества записей

                    for (int i = 0; i < count; i++)
                    {
                        MusicItem item = new MusicItem();

                        item.Id = reader.ReadInt32();
                        item.Title = reader.ReadString();
                        item.Artist = reader.ReadString();
                        item.ReleaseDate = DateTime.FromBinary(reader.ReadInt64());
                        item.DurationSeconds = reader.ReadInt32();
                        item.Price = reader.ReadDecimal();
                        item.IsExplicit = reader.ReadBoolean();
                        item.Rating = reader.ReadDouble();
                        item.Genre = reader.ReadString();
                        item.ListenCount = reader.ReadInt64();

                        _items.Add(item);
                    }
                }
                Console.WriteLine($"База данных загружена из бинарного файла. Записей: {_items.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки: {ex.Message}");
                _items = new List<MusicItem>();
            }
        }
        else
        {
            Console.WriteLine("Файл базы данных не найден. Создана новая пустая база.");
            _items = new List<MusicItem>();
            SaveToFile();
        }
    }

    public void SaveToFile()
    {
        using (FileStream fs = new FileStream(_filePath, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(fs))
        {
            writer.Write(_items.Count);

            foreach (var item in _items)
            {
                writer.Write(item.Id);
                writer.Write(item.Title ?? "");
                writer.Write(item.Artist ?? "");
                writer.Write(item.ReleaseDate.ToBinary());
                writer.Write(item.DurationSeconds);
                writer.Write(item.Price);
                writer.Write(item.IsExplicit);
                writer.Write(item.Rating);
                writer.Write(item.Genre ?? "");
                writer.Write(item.ListenCount);
            }
        }
        Console.WriteLine($"Данные сохранены в бинарный файл: {_filePath}");
    }

    public void ViewAll()
    {
        if (_items.Count == 0)
        {
            Console.WriteLine("\nБаза данных пуста.");
            return;
        }

        Console.WriteLine($"\n{MusicItem.GetSeparator()}");
        Console.WriteLine(MusicItem.GetHeader());
        Console.WriteLine(MusicItem.GetSeparator());

        foreach (var item in _items)
        {
            Console.WriteLine(item.ToString());
        }

        Console.WriteLine(MusicItem.GetSeparator());
        Console.WriteLine($"Всего записей: {_items.Count}");
    }

    // Удаление записи
    public bool DeleteById(int id)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            _items.Remove(item);
            SaveToFile();
            Console.WriteLine($"Элемент с ID {id} удалён");
            return true;
        }
        Console.WriteLine($"Элемент с ID {id} не найден");
        return false;
    }

    // Добавление записи
    public void AddItem(MusicItem item)
    {
        if (item.Id == 0)
        {
            item.Id = _items.Count > 0 ? _items.Max(x => x.Id) + 1 : 1;
        }
        _items.Add(item);
        SaveToFile();
        Console.WriteLine($"Добавлен новый альбом: {item.Title}");
    }

    // Вывод альбомов дороже заданной цены
    public List<MusicItem> GetAlbumsExpensive(decimal minPrice)
    {
        return _items.Where(x => x.Price > minPrice).OrderByDescending(x => x.Price).ToList();
    }

    // Вывод альбомов с рейтингом выше заданного
    public List<MusicItem> GetAlbumsHighRaiting(double minRating)
    {
        return _items.Where(x => x.Rating > minRating).OrderByDescending(x => x.Rating).ToList();
    }

    // Общее количество прослушиваний 
    public long GetTotalListenCount()
    {
        return _items.Sum(x => x.ListenCount);
    }

    // Средняя цена альбома
    public decimal GetAveragePrice()
    {
        return _items.Count > 0 ? _items.Average(x => x.Price) : 0;
    }

    // Вспомогательные методы
    public List<MusicItem> GetAllItems()
    {
        return _items.ToList();
    }

    public bool IdExists(int id)
    {
        return _items.Any(x => x.Id == id);
    }
}
