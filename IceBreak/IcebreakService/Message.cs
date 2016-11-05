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

        public static Message operator +(Message a, Message b)
        {
            Message temp = new Message();

            temp.Message_id = a.Message_id;
            temp.Msg = a.Msg;
            temp.Message_status = a.Message_status;
            temp.Message_sender = a.Message_sender;
            temp.Message_receiver = a.Message_receiver;
            temp.Message_time = a.Message_time;
            temp.Event_id = a.Event_id;

            a.Message_id = b.Message_id;
            a.Msg = b.Msg;
            a.Message_status = b.Message_status;
            a.Message_sender = b.Message_sender;
            a.Message_receiver = b.Message_receiver;
            a.Message_time = b.Message_time;
            a.Event_id = b.Event_id;

            b.Message_id = temp.Message_id;
            b.Msg = temp.Msg;
            b.Message_status = temp.Message_status;
            b.Message_sender = temp.Message_sender;
            b.Message_receiver = temp.Message_receiver;
            b.Message_time = temp.Message_time;
            a.Event_id = temp.Event_id;

            temp = null;
            return b;
        }

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