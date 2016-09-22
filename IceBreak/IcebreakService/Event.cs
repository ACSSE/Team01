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
        private int access_code;
        private string time;
        private string end_time;
        private string date;
        private string meeting_places;

        public bool isEqualTo(Event e)
        {
            bool equal = true;
            if (!title.Equals(e.Title))
                equal = false;
            if (!description.Equals(e.Description))
                equal = false;
            if (!address.Equals(e.Address))
                equal = false;
            if (!radius.Equals(e.Radius))
                equal = false;
            if (!gps_location.Equals(e.Gps_location))
                equal = false;
            if (!access_code.Equals(e.AccessCode))
                equal = false;
            if (!time.Equals(e.Time))
                equal = false;
            if (!end_time.Equals(e.EndTime))
                equal = false;
            if (!date.Equals(e.Date))
                equal = false;
            if (!meeting_places.Equals(e.Meeting_Places))
                equal = false;

            return equal;
        }

        public string Meeting_Places
        {
            get
            {
                return meeting_places;
            }
            set
            {
                meeting_places = value;
            }
        }


        public int AccessCode
        {
            get
            {
                return access_code;
            }

            set
            {
                access_code = value;
            }
        }
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
        public string Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
            }
        }
        public string EndTime
        {
            get
            {
                return end_time;
            }
            set
            {
                end_time = value;
            }
        }
        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
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