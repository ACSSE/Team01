using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IcebreakServices
{
    public class Message
    {
        private string message_id;
        private string message;
        private int message_status;
        private string message_sender;
        private string message_receiver;
        private string message_time;

        public string Message_id
        {
            get
            {
                return message_id;
            }

            set
            {
                message_id = value;
            }
        }

        public string Msg
        {
            get
            {
                return message;
            }

            set
            {
                message = value;
            }
        }

        public int Message_status
        {
            get
            {
                return message_status;
            }

            set
            {
                message_status = value;
            }
        }

        public string Message_sender
        {
            get
            {
                return message_sender;
            }

            set
            {
                message_sender = value;
            }
        }

        public string Message_receiver
        {
            get
            {
                return message_receiver;
            }

            set
            {
                message_receiver = value;
            }
        }

        public string Message_time
        {
            get
            {
                return message_time;
            }

            set
            {
                message_time = value;
            }
        }
    }
}