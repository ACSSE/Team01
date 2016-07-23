namespace IcebreakServices
{
    public class Event
    {
        private int id;
        private string title;
        private string description;
        private string address;
        private int radius;
        private string gps_location;

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        public string Address
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
            }
        }

        public int Radius
        {
            get
            {
                return radius;
            }

            set
            {
                radius = value;
            }
        }

        public string Gps_location
        {
            get
            {
                return gps_location;
            }

            set
            {
                gps_location = value;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }
    }
}