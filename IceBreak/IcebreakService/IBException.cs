using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IcebreakServices
{
    public class IBException
    {
        private int _err_code;
        private string _err_msg;
        private string _err_method;
        private long _date;

        public IBException(int _err_code, string _err_msg, string _err_method, long _date)
        {
            this._err_code = _err_code;
            this._err_msg = _err_msg;
            this._err_method = _err_method;
            this._date = _date;
        }

        public int Err_code
        {
            get
            {
                return _err_code;
            }

            set
            {
                _err_code = value;
            }
        }

        public string Err_msg
        {
            get
            {
                return _err_msg;
            }

            set
            {
                _err_msg = value;
            }
        }

        public string Err_method
        {
            get
            {
                return _err_method;
            }

            set
            {
                _err_method = value;
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
    }
}