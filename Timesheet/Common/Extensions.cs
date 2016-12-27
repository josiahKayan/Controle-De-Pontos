using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Apassos.Common.Extensions
{
    public static class Extensions
    {

        static byte[] bytVector = {
		        240,
		        3,
		        45,
		        29,
		        0,
		        76,
		        173,
		        59
	        };

        static string lscryptoKey = "P4rt3R";

        public static bool In<T>(this T item, params T[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            return items.Contains(item);
        }



        public static string sysPassEncrypt(this string strPass)
        {
            string strReturn = string.Empty;

            if (strPass != null)
            {

                TripleDESCryptoServiceProvider loCryptoClass = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider loCryptoProvider = new MD5CryptoServiceProvider();
                byte[] bytBuffer = null;

                try
                {
                    bytBuffer = System.Text.Encoding.ASCII.GetBytes(strPass);
                    loCryptoClass.Key = loCryptoProvider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lscryptoKey));
                    loCryptoClass.IV = bytVector;
                    strReturn = Convert.ToBase64String(loCryptoClass.CreateEncryptor().TransformFinalBlock(bytBuffer, 0, bytBuffer.Length));
                }
                catch (AcessosException ex)
                {
                    throw ex;
                }
                finally
                {
                    loCryptoClass.Clear();
                    loCryptoProvider.Clear();
                    loCryptoClass = null;
                    loCryptoProvider = null;
                }
            }

            return strReturn;
        }

        public static string sysPassDecrypt(this string strPass)
        {
            string strReturn = string.Empty;

            if (strPass != null)
            {
                byte[] bytBuffer = null;
                TripleDESCryptoServiceProvider loCryptoClass = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider loCryptoProvider = new MD5CryptoServiceProvider();
                //
                try
                {
                    bytBuffer = Convert.FromBase64String(strPass);
                    loCryptoClass.Key = loCryptoProvider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lscryptoKey));
                    loCryptoClass.IV = bytVector;
                    strReturn = Encoding.ASCII.GetString(loCryptoClass.CreateDecryptor().TransformFinalBlock(bytBuffer, 0, bytBuffer.Length));
                }
                catch (AcessosException ex)
                {
                    throw ex;
                }
                finally
                {
                    loCryptoClass.Clear();
                    loCryptoProvider.Clear();
                    loCryptoClass = null;
                    loCryptoProvider = null;
                }
            }

            return strReturn;
        }

        public static string sysPassEncryptXML(this string strPass)
        {
            string functionReturnValue = null;
            TripleDESCryptoServiceProvider loCryptoClass = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider loCryptoProvider = new MD5CryptoServiceProvider();
            byte[] bytBuffer = null;

            try
            {
                bytBuffer = System.Text.Encoding.Unicode.GetBytes(strPass);
                loCryptoClass.Key = loCryptoProvider.ComputeHash(ASCIIEncoding.Unicode.GetBytes(lscryptoKey));
                loCryptoClass.IV = bytVector;
                strPass = Convert.ToBase64String(loCryptoClass.CreateEncryptor().TransformFinalBlock(bytBuffer, 0, bytBuffer.Length));
                functionReturnValue = strPass;
            }
            catch (CryptographicException ex)
            {
                throw ex;
            }
            catch (FormatException ex)
            {
                throw ex;
            }
            catch (AcessosException ex)
            {
                throw ex;
            }
            finally
            {
                loCryptoClass.Clear();
                loCryptoProvider.Clear();
                loCryptoClass = null;
                loCryptoProvider = null;
            }
            return functionReturnValue;
        }

        public static string sysPassDecryptXML(this string strPass)
        {
            string functionReturnValue = null;
            byte[] bytBuffer = null;
            TripleDESCryptoServiceProvider loCryptoClass = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider loCryptoProvider = new MD5CryptoServiceProvider();
            //
            try
            {
                bytBuffer = Convert.FromBase64String(strPass);
                loCryptoClass.Key = loCryptoProvider.ComputeHash(ASCIIEncoding.Unicode.GetBytes(lscryptoKey));
                loCryptoClass.IV = bytVector;
                functionReturnValue = Encoding.Unicode.GetString(loCryptoClass.CreateDecryptor().TransformFinalBlock(bytBuffer, 0, bytBuffer.Length));
            }
            catch (AcessosException ex)
            {
                throw ex;
            }
            finally
            {
                loCryptoClass.Clear();
                loCryptoProvider.Clear();
                loCryptoClass = null;
                loCryptoProvider = null;
            }
            return functionReturnValue;
        }

        public static bool IsBetween<T>(this T item, T start, T end)
        {
            if (item == null)
            {
                return false;
            }
            return Comparer<T>.Default.Compare(item, start) >= 0
                && Comparer<T>.Default.Compare(item, end) <= 0;
        }

    }
}