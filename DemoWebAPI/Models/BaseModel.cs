using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DemoWebAPI.Enums;

namespace DemoWebAPI.Models
{
    public class ResponseModel
    {
        /// <summary>
        /// 回傳電腦名稱
        /// </summary>
        public string MachineName { get { return Environment.MachineName; } }
        public int ResultCode { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
        public ResponseModel()
        {
            ResultCode = (int)DemoWebAPI.Enums.ResultCode.None;
            ErrorMessage = "";
            Exception = null;
        }
    }

    public class ActionResponseModel<T> : ResponseModel, ICloneable
    {
        public T Content { get; set; }
        public ActionResponseModel() : base()
        {
            Content = default(T);
        }
        public object Clone() { return this.MemberwiseClone(); }
    }
}