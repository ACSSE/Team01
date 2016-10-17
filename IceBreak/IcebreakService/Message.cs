using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IcebreakServices
{
    public class Message
    {
        private string _message_id;
        private string _message;
        private int _message_status;
        private string _message_sender;
        private string _message_receiver;
        private long _message_time;
        private long _event_id;

        public string Message_id
        {
            get
            {
                return _message_id;
            }

            set
            {
                _message_id = value;
            }
        }

        public string Msg
        {
            get
            {
                return _message;
            }

            set
            {
                _message = value;
            }
        }

        public int Message_status
        {
            get
            {
                return _message_status;
            }

            set
            {
                _message_status = value;
            }
        }

        public string Message_sender
        {
            get
            {
                return _message_sender;
            }

            set
            {
                _message_sender = value;
            }
        }

        public string Message_receiver
        {
            get
            {
                return _message_receiver;
            }

            set
            {
                _message_receiver = value;
            }
        }

        public long Message_time
        {
            get
            {
                return _message_time;
            }

            set
            {
                _message_time = value;
            }
        }

        public long Event_id
        {
            get
            {
                return _event_id;
            }

            set
            {
                _event_id = value;
            }
        }
    }
}