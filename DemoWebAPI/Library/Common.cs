using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;
using Newtonsoft.Json.Linq;
namespace DemoWebAPI.Library
{
    internal static class Common
    {
        public static string App_Data;
        public static string AssemblyVersion { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }
        public static void Initialize(bool console = false)
        {
            App_Data = console ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data") : HttpContext.Current.Server.MapPath("~/App_Data");
            //設定SecurityProtocol
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            LogHelper.LogProvider.WriteLog(Enum_LogType.Trace, "開始");
        }
        public static void Uninitialize()
        {
            LogHelper.LogProvider.WriteLog(Enum_LogType.Trace, "結束");
            LogHelper.LogProvider.Dispose();
            App_Data = null;
        }
        public static string GetClientIP(this HttpRequestMessage sender)
        {
            object l_Value;
            if(sender == null)
                return null;
            if (sender.Properties.TryGetValue("MS_HttpContext", out l_Value))
                return ((HttpContextWrapper)l_Value).Request.UserHostAddress;
            else if (sender.Properties.TryGetValue(RemoteEndpointMessageProperty.Name, out l_Value))
                return ((RemoteEndpointMessageProperty)l_Value).Address;
            else if (HttpContext.Current != null)
                return HttpContext.Current.Request.UserHostAddress;
            return null;
        }

        private static T GetSetting<T>()
        {
            string sSectionName = typeof(T).Name;
            ConfigurationManager.RefreshSection(sSectionName);
            return (T)ConfigurationManager.GetSection(sSectionName);
        }

        public static Dictionary<string, object> GetPropertyDic(this PropertyInformationCollection propertys)
        {
            if (propertys == null)
                return null;
            Dictionary<string, object> dicProperty = new Dictionary<string, object>();
            foreach (PropertyInformation property in propertys)
            {
                dicProperty.Add(property.Name, property.Value);
            }
            return dicProperty;
        }
        public static bool ConvertToJObject(string json, out JObject jRet)
        {
            bool bRet = false;
            try
            {
                jRet = JObject.Parse(json);
                bRet = true;
            }
            catch
            {
                bRet = false;
                jRet = new JObject();
            }
            return bRet;
        }
    }
}
