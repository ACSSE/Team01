using System;

namespace IcebreakServices
{
    public class Point
    {
        private double _lat = 0.0;
        private double _lng = 0.0;

        public Point(double latitude, double longitude)
        {
            Lat = latitude;
            Lng = longitude;
        }

        public bool isZero()
        {
            return (_lat == 0.0 && _lng == 0.0);
        }

        public double Lat
        {
            get
            {
                return _lat;
            }

            set
            {
                _lat = value;
            }
        }

        public double Lng
        {
            get
            {
                return _lng;
            }

            set
            {
                _lng = value;
            }
        }
    }

    public class Event
    {
        private long _id;
        private string _title;
        private string _description;
        private string _address;
        //private int _radius;
        private string _gps_location;
        private int _access_code;
        private long _end_date;
        private long _date;
        private string _meeting_places;
        private string _manager;

        public Point getOrigin()
        {
            double maxLat = 0.0;
            double maxLng = 0.0;
            double minLat = 0.0;
            double minLng = 0.0;

            string[] boundary = _gps_location.Split(';');

            if (boundary != null)
            {
                if (boundary.Length>0)
                {

                    if (boundary[0].Contains(","))
                    {
                        double lat = 0.0, lng = 0.0;
                        double.TryParse(boundary[0].Split(',')[0], out lat);
                        double.TryParse(boundary[0].Split(',')[1], out lng);

                        maxLat = lat;
                        maxLng = lng;
                        minLat = lat;
                        minLng = lng;
                    }

                    foreach (string loc in boundary)
                    {
                        if (loc.Contains(","))
                        {
                            double lat = 0.0, lng = 0.0;
                            double.TryParse(loc.Split(',')[0], out lat);
                            double.TryParse(loc.Split(',')[1], out lng);
                            //return new Point(, boundary[0].Split(',')[1]);
                            if (lat < maxLat)
                                maxLat = lat;
                            else if (lat > minLat)
                                minLat = lat;

                            if (lng > maxLng)
                                maxLng = lng;
                            else if (lng < minLng)
                                minLng = lng;
                        }
                    }
                }
            }
            return new Point(minLat + ((maxLat - minLat) / 2), minLng + ((maxLng - minLng) / 2));
        }

        public bool isValidForAdding()
        {
            bool valid = true;
            if (string.IsNullOrEmpty(Title))
                valid=false;
            if (string.IsNullOrEmpty(Description))
                valid = false;
            if (string.IsNullOrEmpty(Address))
                valid = false;
            //if (Radius<=0)
            //    valid = false;
            if (string.IsNullOrEmpty(Gps_location))
                valid = false;
            if (AccessCode<=0 || AccessCode.ToString().Length<4)
                valid = false;
            if (End_Date<=0)
                valid = false;
            if (Date<=0)
                valid = false;
            if (string.IsNullOrEmpty(Meeting_Places))
                valid = false;
            if (string.IsNullOrEmpty(Manager))
                valid = false;
            return valid;
        }

        public bool isEqualTo(Event e)
        {
            bool equal = true;
            if (!_title.Equals(e.Title))
                equal = false;
            if (!_description.Equals(e.Description))
                equal = false;
            if (!_address.Equals(e.Address))
                equal = false;
            //if (!_radius.Equals(e.Radius))
            //    equal = false;
            if (!_gps_location.Equals(e.Gps_location))
                equal = false;
            if (!_access_code.Equals(e.AccessCode))
                equal = false;
            if (_end_date!=e.End_Date)
                equal = false;
            if (_date!=e.Date)
                equal = false;
            if (!_meeting_places.Equals(e.Meeting_Places))
                equal = false;

            return equal;
        }

        public string Meeting_Places
        {
            get
            {
                return _meeting_places;
            }
            set
            {
                _meeting_places = value;
            }
        }


        public int AccessCode
        {
            get
            {
                return _access_code;
            }

            set
            {
                _access_code = value;
            }
        }
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        public string Address
        {
            get
            {
                return _address;
            }

            set
            {
                _address = value;
            }
        }

        /*public int Radius
        {
            get
            {
                return _radius;
            }

            set
            {
                _radius = value;
            }
        }*/

        public string Gps_location
        {
            get
            {
                return _gps_location;
            }

            set
            {
                _gps_location = value;
            }
        }
        
        public long End_Date
        {
            get
            {
                return _end_date;
            }
            set
            {
                _end_date = value;
            }
        }
        public long Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }
        public long Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public string Manager
        {
            get
            {
                return _manager;
            }

            set
            {
                _manager = value;
            }
        }
    }
}