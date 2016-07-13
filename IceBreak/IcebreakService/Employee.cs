using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IcebreakServices
{
    public class Employee
    {
        private int id;
        private string fname;
        private string lname;
        private int age;

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