using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DemoWebAPI.Library
{
    public enum Enum_LogType
    {
        None,
        Info,
        Trace,
        Debug,
        Error
    }
    public enum Enum_DumpSendType
    {
        None = 0,
        Request,
        Response
    }
    public static class LogHelper
    {
        public static ClsLogProvider LogProvider = new ClsLogProvider();
    }
    public class ClsLogProvider : IDisposable
    {
        private Logger m_Logger;

        public ClsLogProvider()
        {
            m_Logger = LogManager.GetCurrentClassLogger();
        }
        public void Info(string message, params object[] args)
        {
            WriteLog(Enum_LogType.Info, message, args);
        }
        public void WriteLog(Enum_LogType _LogType, string message, params object[] args)
        {
            switch (_LogType)
            {
                case Enum_LogType.Trace:
                    m_Logger.Trace(message, args);
                    break;
                case Enum_LogType.Error:
                    m_Logger.Error(message, args);
                    break;
                case Enum_LogType.Debug:
                    m_Logger.Error(message, args);
                    break;
                default:
                    m_Logger.Info(message, args);
                    break;
            }
        }
        public void DumpException(Enum_DumpSendType enum_DumpSendType, string sServiceName, string sFunction, string sAction, string sGUID, Exception ex)
        {
            JObject jObjectDump = new JObject();
            jObjectDump.Add("ServiceName", string.Format("{0}", sServiceName));
            jObjectDump.Add("Function", string.Format("{0}", sFunction));
            jObjectDump.Add("Guid", string.Format("{0}", sGUID));
            jObjectDump.Add("Exception", JObject.FromObject(ex));
            DumpJObject(enum_DumpSendType, sGUID, sAction, jObjectDump);
        }
        public void DumpTrace(Enum_DumpSendType enum_DumpSendType, string sServiceName, string sFunction, string sAction, string sGUID, object oContent)
        {
            DumpTrace(enum_DumpSendType, sServiceName, sFunction, sAction, sGUID, oContent == null ? "" : JsonConvert.SerializeObject(oContent));
        }
        public void DumpTrace(Enum_DumpSendType enum_DumpSendType, string sServiceName, string sFunction, string sAction, string sGUID, string sDumpContent)
        {
            JObject jObjectContent;
            bool blnRet = Common.ConvertToJObject(sDumpContent, out jObjectContent);

            JObject jObjectDump = new JObject();
            jObjectDump.Add("ServiceName", string.Format("{0}", sServiceName));
            jObjectDump.Add("Function", string.Format("{0}", sFunction));
            jObjectDump.Add("Guid", string.Format("{0}", sGUID));
            jObjectDump.Add("DumpContent", blnRet ? jObjectContent : (JToken)sDumpContent);
            DumpJObject(enum_DumpSendType, sGUID, sAction, jObjectDump);
        }
        public void DumpPostException(Enum_DumpSendType enum_DumpSendType, string sPostURL, string sServiceName, string sFunction, string sAction, string sGUID, Exception ex)
        {
            JObject jObjectDump = new JObject();
            jObjectDump.Add("ServiceName", string.Format("{0}", sServiceName));
            jObjectDump.Add("PostURL", string.Format("{0}", sPostURL));
            jObjectDump.Add("Function", string.Format("{0}", sFunction));
            jObjectDump.Add("Guid", string.Format("{0}", sGUID));
            jObjectDump.Add("Exception", string.Format("{0}", ex.Message));
            DumpJObject(enum_DumpSendType, sGUID, sAction, jObjectDump);
        }
        public void DumpPostTrace(Enum_DumpSendType enum_DumpSendType, string sPostURL, string sServiceName, string sFunction, string sAction, string sGUID, string sDumpContent)
        {
            JObject jObjectContent;
            bool blnRet = Common.ConvertToJObject(sDumpContent, out jObjectContent);

            JObject jObjectDump = new JObject();
            jObjectDump.Add("ServiceName", string.Format("{0}", sServiceName));
            jObjectDump.Add("PostURL", string.Format("{0}", sPostURL));
            jObjectDump.Add("Function", string.Format("{0}", sFunction));
            jObjectDump.Add("Guid", string.Format("{0}", sGUID));
            jObjectDump.Add("DumpContent", blnRet ? jObjectContent : (JToken)sDumpContent);
            DumpJObject(enum_DumpSendType, sGUID, sAction, jObjectDump);
        }
        public void DumpJObject(Enum_DumpSendType enum_DumpSendType, string sGUID, string sAction, JObject jObject)
        {
            m_Logger.Trace("{0} \t{1}\t{2}\t{3}", enum_DumpSendType.ToString(), sGUID, sAction, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        }
        public void Dispose()
        {
            LogManager.Flush();
            LogManager.Shutdown();
            m_Logger = null;
        }
    }
}