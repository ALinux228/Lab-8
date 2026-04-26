using System;

public class SeedData
{
    public static void FillDatabase(MusicDatabase db)
    {
        db.AddItem(new MusicItem(1, "Dark Side of the Moon", "Pink Floyd",
            new DateTime(1973, 3, 1), 2580, 499.99m, false, 4.9, "Rock", 12500000));

        db.AddItem(new MusicItem(2, "Lover", "Taylor Swift",
            new DateTime(2019, 8, 23), 3661, 649.00m, false, 4.7, "Pop", 8900000));

        db.AddItem(new MusicItem(3, "DAMN.", "Kendrick Lamar",
            new DateTime(2017, 4, 14), 3299, 549.50m, true, 4.8, "Hip Hop", 15300000));

        db.AddItem(new MusicItem(4, "Random Access Memories", "Daft Punk",
            new DateTime(2013, 5, 17), 4452, 799.00m, false, 4.9, "Electronic", 6700000));

        db.AddItem(new MusicItem(5, "Русский альбом", "Кино",
            new DateTime(1990, 1, 20), 2845, 399.90m, false, 4.9, "Rock", 3200000));

        db.AddItem(new MusicItem(6, "Whole Lotta Red", "Playboi Carti",
            new DateTime(2020, 12, 25), 3624, 599.00m, true, 4.2, "Hip Hop", 21000000));

        db.AddItem(new MusicItem(7, "Blonde", "Frank Ocean",
            new DateTime(2016, 8, 20), 3660, 699.00m, true, 4.8, "R&B", 9800000));

        Console.WriteLine("База данных успешно заполнена 7 записями");
    }
}