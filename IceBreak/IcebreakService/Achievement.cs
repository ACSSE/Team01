using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IcebreakServices
{
    public class Achievement
    {
        private long _id;
        private string _name;
        private string _description;
        private int _value;
        private int _target;
        private long _date_achieved;
        private string _method;

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

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
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

        public int Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        public int Target
        {
            get
            {
                return _target;
            }

            set
            {
                _target = value;
            }
        }

        public long DateAchieved
        {
            get
            {
                return _date_achieved;
            }

            set
            {
                _date_achieved = value;
            }
        }

        public string Method
        {
            get
            {
                return _method;
            }

            set
            {
                _method = value;
            }
        }
    }
}