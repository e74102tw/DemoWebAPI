using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http.ModelBinding;
using System.Xml;
using System.Xml.Serialization;
using DemoWebAPI.Enums;
using DemoWebAPI.Models;

namespace DemoWebAPI
{
    public static class Methods
    {
        #region Object
        public static void InitializePropertyDefaultValues(this object obj)
        {
            PropertyInfo[] props = obj.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                var oldValue = prop.GetValue(obj);
                if (oldValue == null)//取不到值才塞預設值
                {
                    var d = prop.GetCustomAttribute<DefaultValueAttribute>();
                    if (d != null)
                        prop.SetValue(obj, d.Value);
                }
            }
        }
        public static string GetEnumDescription<T>(this T enumerationValue)
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                return "";
            }
            try
            {
                //Tries to find a DescriptionAttribute for a potential friendly name
                //for the enum
                MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
                if (memberInfo != null && memberInfo.Length > 0)
                {
                    object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                    if (attrs != null && attrs.Length > 0)
                    {
                        //Pull out the description value
                        return ((DescriptionAttribute)attrs[0]).Description;
                    }
                }
                //If we have no description attribute, just return the ToString of the enum
                return enumerationValue.ToString();
            }
            catch
            {
                return "";
            }
        }
        public static T? GetEnumWithValue<T>(int value) where T : struct, Enum
        {
            if (Enum.IsDefined(typeof(T), value))
                return (T)Enum.Parse(typeof(T), value.ToString());
            return null;
        }
        #endregion

        #region For string
        public static string ParmString(this string Str, string DefaultStr = "")
        {
            string strRet = DefaultStr;
            try
            {
                strRet = string.IsNullOrEmpty(Str) ? DefaultStr : Str;
            }
            catch (Exception ex)
            {
                strRet = DefaultStr;
            }
            return strRet;
        }
        #endregion

        #region For DateTime
        public static string ParmDateTimeToString(this DateTime DT, string DateTimeFormate, string DefaultStr = "")
        {
            string strRet = DefaultStr;
            DateTime DTDefault = default(DateTime);
            try
            {
                strRet = (DT == null || DT == DTDefault) ? DefaultStr : DT.ToString(DateTimeFormate);
            }
            catch (Exception ex)
            {
                strRet = DefaultStr;
            }
            return strRet;
        }
        #endregion

        #region For double
        public static int ParmDoubleToInt(this double DBT, string DoubleFormate = "", int DefaultInt = 0)
        {
            int intRet = DefaultInt;
            try
            {
                intRet = DBT == null ? DefaultInt : DoubleFormate.Trim() == "" ? int.Parse(DBT.ToString()) : int.Parse(DBT.ToString(DoubleFormate.Trim()));
            }
            catch (Exception ex)
            {
                intRet = DefaultInt;
            }
            return intRet;
        }
        public static string ParmDoubleToString(this double DBT, string DoubleFormate = "", string DefaultStr = "")
        {
            string strRet = DefaultStr;
            try
            {
                strRet = (DBT == null) ? DefaultStr : DoubleFormate.Trim() == "" ? DBT.ToString() : DBT.ToString(DoubleFormate.Trim());
            }
            catch (Exception ex)
            {
                strRet = DefaultStr;
            }
            return strRet;
        }
        #endregion

        #region Model驗證
        /// <summary>
        /// 驗證基本必須傳入參數
        /// </summary>
        /// <param name="ModelState"></param>
        /// <param name="Model"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        public static bool BaseValidation(ModelStateDictionary ModelState, object DynamicModel, ResponseModel Model)
        {
            if (!CheckParam(ModelState, DynamicModel, Model))
            {
                Model.ResultCode = (int)ResultCode.WrongArgument;
                return false;
            }
            Model.ResultCode = (int)ResultCode.Success;
            return true;
        }
        /// <summary>
        /// 驗證傳入參數
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ModelState"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public static bool CheckParam<T>(ModelStateDictionary ModelState, object model, T response) where T : ResponseModel
        {
            bool Result = false;
            if (model == null)
            {
                response.ErrorMessage = "未傳入必要參數";
            }
            else
            {
                if (ModelState.IsValid == false)
                {
                    string sErrMSG = "";
                    foreach (var item in ModelState)
                    {
                        if (!ModelState.IsValidField(item.Key))
                        {
                            foreach (ModelError modelErr in item.Value.Errors)
                            {
                                sErrMSG = string.Format("{0}{1}", sErrMSG.Trim(), modelErr.ErrorMessage);
                            }
                        }
                    }
                    response.ErrorMessage = "參數不正確:" + sErrMSG;
                }
                else
                {
                    Result = true;
                }
            }
            return Result;
        }
        #endregion
        public static string EncryptStringToBase64_AES(string plainText, string key, string iv)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            byte[] encrypted;
            using (AesManaged aes = new AesManaged())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }
    }
}
