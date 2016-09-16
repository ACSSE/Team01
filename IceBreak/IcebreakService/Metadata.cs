using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IcebreakServices
{
    public class Metadata
    {
        private string _entry;
        private string _meta;

        public string Entry
        {
            get
            {
                return _entry;
            }

            set
            {
                _entry = value;
            }
        }

        public string Meta
        {
            get
            {
                return _meta;
            }

            set
            {
                _meta = value;
            }
        }
    }
}