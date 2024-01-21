namespace HotelBookingSystem.Domain;

public class Constants
{
    public class Common
    {
        public const int MinNameLength = 2;
        public const int MaxNameLength = 50;
        public const int Zero = 0;
    }

    public class Room
    {
        public const int MinRoomNumber = 1;
        public const int MaxRoomNumber = 10_000;
        public const int MaxRoomCapacity = 20;
        public const int MinRoomCapacity = 0;
        public const decimal MinRoomPrice = 1;
        public const decimal MaxRoomPrice = 100_000;
    }

    public class Hotel
    {
        public const short MaxHotelStars = 5;
        public const short MinHotelStars = 1;
    }

    public class City
    {
        // 5-digits ZIP code
        public const short PostOfficeLength = 5;
    }

    public class Image
    {
        public const int MaxImageUrlLength = 2048;
        public const int MaxAlternativeTextLength = 100;
    }

    public class Review
    {
        public const int MinRating = 1;
        public const int MaxRating = 5;
        public const int MinReviewTitleLength = 2;
        public const int MaxReviewTitleLength = 50;
        public const int MinReviewDescriptionLength = 2;
        public const int MaxReviewDescriptionLength = 1000;
    }

    public class Location
    {
        public const double MinLatitude = -90;
        public const double MaxLatitude = 90;
        public const double MinLongitude = -180;
        public const double MaxLongitude = 180;
    }

}
