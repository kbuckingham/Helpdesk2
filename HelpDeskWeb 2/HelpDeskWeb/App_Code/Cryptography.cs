﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using System.Xml;
using System.Text; 

/// <summary>
/// Summary description for Cryptography
/// </summary>
public static class Cryptography
{
    static Cryptography()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
		///    Decrypts  a particular string with a specific Key
		/// </summary>
		public static string Decrypt( string stringToDecrypt, string sEncryptionKey) 
		{
			byte[] key = {}; 
			byte[] IV = {10, 20, 30, 40, 50, 60, 70, 80}; 
			byte[] inputByteArray = new byte[stringToDecrypt.Length]; 
			try 
			{ 
				key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0,8)); 
				DESCryptoServiceProvider des = new DESCryptoServiceProvider(); 
				inputByteArray = Convert.FromBase64String(stringToDecrypt); 
				MemoryStream ms = new MemoryStream(); 
				CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write); 
				cs.Write(inputByteArray, 0, inputByteArray.Length); 
				cs.FlushFinalBlock(); 
				Encoding encoding = Encoding.UTF8 ; 
				return encoding.GetString(ms.ToArray()); 
			} 
			catch (System.Exception ex) 
			{ 
			   return (string.Empty);
			} 
		} 

		/// <summary>
		///   Encrypts  a particular string with a specific Key
		/// </summary>
		/// <param name="stringToEncrypt"></param>
		/// <param name="sEncryptionKey"></param>
		/// <returns></returns>
		public static string Encrypt( string stringToEncrypt, string sEncryptionKey) 
		{
			byte[] key = {}; 
			byte[] IV = {10, 20, 30, 40, 50, 60, 70, 80}; 
			byte[] inputByteArray; //Convert.ToByte(stringToEncrypt.Length) 

			try 
			{ 
				key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0,8)); 
				DESCryptoServiceProvider des = new DESCryptoServiceProvider(); 
				inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt); 
				MemoryStream ms = new MemoryStream(); 
				CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write); 
				cs.Write(inputByteArray, 0, inputByteArray.Length); 
				cs.FlushFinalBlock(); 
				return Convert.ToBase64String(ms.ToArray()); 
			} 
			catch (System.Exception ex) 
			{ 
				return (string.Empty);
			} 
		}


        public static string NumberEncrypt(string number)
        {
            try
            {
                int change = 0;
                if (int.TryParse(number, out change))
                {
                    change = change * 29;
                    change += 32;
                }
                return "" + change;
            }
            catch (Exception ex)
            {
                return "Error";
            }

        }


        public static string NumberDecrypt(string number)
        {
            try
            {
                int change = 0;
                if (int.TryParse(number, out change))
                {
                    change -= 32;
                    change = change / 29;
                }
                return "" + change;
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }
        static public string EncodeTo64(string toEncode)
        {

            byte[] toEncodeAsBytes

                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

            string returnValue

                  = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;

        }

        static public string DecodeFrom64(string encodedData)
        {

            byte[] encodedDataAsBytes

                = System.Convert.FromBase64String(encodedData);

            string returnValue =

               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

            return returnValue;

        }
}

