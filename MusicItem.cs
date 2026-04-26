using System;

[Serializable]
public class MusicItem
{
    private int _id;
    private string _title;
    private string _artist;
    private DateTime _releaseDate;
    private int _durationSeconds;
    private decimal _price;
    private bool _isExplicit;
    private double _rating;
    private string _genre;
    private long _listenCount;

    public MusicItem()
    {
        _id = 0;
        _title = string.Empty;
        _artist = string.Empty;
        _releaseDate = DateTime.Now;
        _durationSeconds = 0;
        _price = 0;
        _isExplicit = false;
        _rating = 0;
        _genre = string.Empty;
        _listenCount = 0;
    }

    public MusicItem(int id, string title, string artist, DateTime releaseDate,
                     int durationSeconds, decimal price, bool isExplicit,
                     double rating, string genre, long listenCount)
    {
        _id = id;
        _title = title;
        _artist = artist;
        _releaseDate = releaseDate;
        _durationSeconds = durationSeconds;
        _price = price;
        _isExplicit = isExplicit;
        _rating = rating;
        _genre = genre;
        _listenCount = listenCount;
    }

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    public string Artist
    {
        get { return _artist; }
        set { _artist = value; }
    }

    public DateTime ReleaseDate
    {
        get { return _releaseDate; }
        set { _releaseDate = value; }
    }

    public int DurationSeconds
    {
        get { return _durationSeconds; }
        set { _durationSeconds = value; }
    }

    public decimal Price
    {
        get { return _price; }
        set { _price = value; }
    }

    public bool IsExplicit
    {
        get { return _isExplicit; }
        set { _isExplicit = value; }
    }

    public double Rating
    {
        get { return _rating; }
        set { _rating = value; }
    }

    public string Genre
    {
        get { return _genre; }
        set { _genre = value; }
    }

    public long ListenCount
    {
        get { return _listenCount; }
        set { _listenCount = value; }
    }


    public override string ToString()
    {
        int minutes = _durationSeconds / 60;
        int seconds = _durationSeconds % 60;
        string duration = $"{minutes:D2}:{seconds:D2}";
        string price = $"{_price:F0}";
        string explicitFlag = _isExplicit ? "18+" : "нет";

        return $"{_id,-4} | {_artist,-20} | {_title,-30} | {_releaseDate:yyyy-MM-dd} | " +
               $"{duration,-7} | {price,8} ₽ | {explicitFlag,-3} | " +
               $"{_rating,4:F1} | {_genre,-15} | {_listenCount,12:N0}  |";
    }

    public static string GetHeader()
    {
        return $"{"ID",-4} | {"Исполнитель",-20} | {"Название",-30} | {"Дата",-10} | " +
               $"{"Длит.",-7} | {"Цена",10} | {"18+",-3} | {"Рейт",4} | {"Жанр",-15} | {"Прослушиваний",12} |";
    }

    public static string GetSeparator()
    {
        return new string('-', 145);
    }
}