using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace IcebreakServices
{
    public class Hash
    {
        //private int saltByteSize = 20;

        public static string ComputeHash(string input, HashAlgorithm algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            string hashedpass = "";
            for (int i = 0; i < hashedBytes.Length; i++)
                hashedpass += hashedBytes[i].ToString("x2");
            return hashedpass;//BitConverter.ToString(hashedBytes);
        }

        public static string HashString(string pass)
        {
            SHA1 HashAlgorithm = SHA1.Create(); //Hash algorithm declaration
            Byte[] c; //Byte array to store the returned hashed data

            //Convert the input string to a byte array and compute the hash.
            c = HashAlgorithm.ComputeHash(Encoding.Default.GetBytes(pass));
            //String variable that will store the returned hashed string
            string hashedpass = "";

            //Loop through each byte of the hashed data and format each one as a hexadecimal string. 
            for (int i = 0; i < c.Length; i++)
                hashedpass += c[i].ToString("x2");
            //Return the hexadecimal string. 
            return hashedpass;
        }

        /*public static string HashString(string input)
        {
            /*var deriveBytes = new Rfc2898DeriveBytes(input, saltByteSize);
            byte[] salt = deriveBytes.Salt;
            byte[] key = deriveBytes.GetBytes(saltByteSize);  //derive a {saltByteSize}byte key
            //Loop through each byte of the hashed data and format each one as a hexadecimal string. 
            for (int i = 0; i < (key.Length - 1); i++)
                hashedpass += key[i].ToString("x2");
            *
            //SHA1 hashing
            string hashedpass = ComputeHash(input, new SHA1CryptoServiceProvider());
            //Return the hexadecimal string.
            return HashPassword(input);//hashedpass;
        }*/
    }
}