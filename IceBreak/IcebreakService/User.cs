using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace IcebreakServices
{
    [DataContract]
    public class User
    {
        private string fname;
        private string lname;
        private string username;
        private string email;
        private string password;
        private int access_level = 0;
        private long event_id = 0;
        private int age = 0;
        private string gender;
        private string occupaton;
        private string bio;
        private string catchphrase;
        private string fb_token;
        private string fb_id;
        private long _points = 0;
        private long _last_seen = 0;

        /*public User(string fname, string lname, string username, string email)
        {
            this.fname = fname;
            this.lname = lname;
            this.username = username;
            this.email = email;
        }*/
        [DataMember]
        public string Fname
        {
            get
            {
                return fname;
            }

            set
            {
                fname = value;
            }
        }

        [DataMember]
        public string Lname
        {
            get
            {
                return lname;
            }

            set
            {
                lname = value;
            }
        }

        [DataMember]
        public string Username
        {
            get
            {
                return username;
            }

            set
            {
                username = value;
            }
        }

        [DataMember]
        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
            }
        }

        [DataMember]
        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        [DataMember]
        public int Access_level
        {
            get
            {
                return access_level;
            }

            set
            {
                access_level = value;
            }
        }
        [DataMember]
        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                age = value;
            }
        }
        [DataMember]
        public string Bio
        {
            get
            {
                return bio;
            }
            set
            {
                bio = value;
            }
        }
        [DataMember]
        public string Gender
        {
            get
            {
                return gender;
            }
            set
            {
                gender = value;
            }
        }
        [DataMember]
        public string Catchphrase
        {
            get
            {
                return catchphrase;
            }
            set
            {
                catchphrase = value;
            }
        }
        [DataMember]
        public string Occupation
        {
            get
            {
                return occupaton;
            }
            set
            {
                occupaton = value;
            }
        }
        [DataMember]
        public long Event_id
        {
            get
            {
                return event_id;
            }

            set
            {
                event_id = value;
            }
        }

        public string Fb_token
        {
            get
            {
                return fb_token;
            }

            set
            {
                fb_token = value;
            }
        }

        public string Fb_id
        {
            get
            {
                return fb_id;
            }

            set
            {
                fb_id = value;
            }
        }

        [DataMember]
        public long Last_Seen
        {
            get
            {
                return _last_seen;
            }

            set
            {
                _last_seen = value;
            }
        }

        [DataMember]
        public long Points
        {
            get
            {
                return _points;
            }

            set
            {
                _points = value;
            }
        }

        public override string ToString()
        {
            return "[FN:" + this.fname + ",LN:"+ this.lname +",UN:"+ this.username +",EM:"+ this.email +",AGE:"+this.age+ ",BIO:" + this.bio + ",GDR:" + this.gender + ",CP:" + this.catchphrase + ",OCP:" + this.occupaton + ",PW:" + this.password +"]";
        }
    }
}