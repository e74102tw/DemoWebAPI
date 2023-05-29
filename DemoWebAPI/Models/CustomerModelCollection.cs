﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DemoWebAPI.Models
{
    public class CustomerPostRequestModel
    {
        /// <summary>
        /// 身分證字號
        /// </summary>
        [Required]
        public string CustID { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        [Required]
        public string CustFirstName { get; set; }
        /// <summary>
        /// 姓氏
        /// </summary>
        [Required]
        public string CustLastName { get; set; }
        /// <summary>
        /// 全名
        /// </summary>
        [Required]
        public string CustFullName { get; set; }
        /// <summary>
        /// 性別 M:男性/F:女性
        /// </summary>
        [Required]
        public string Sex { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        public string TelNo { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public string BirthDate { get; set; }
    }
    public class CustomerPostResponseModel
    {
        /// <summary>
        /// 序號
        /// </summary>
        public int C_Seq { get; set; }
    }
    public class CustomerPutRequestModel
    {
        /// <summary>
        /// 序號
        /// </summary>
        [Required]
        public int C_Seq { get; set; }
        /// <summary>
        /// 身分證字號
        /// </summary>
        public string CustID { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string CustFirstName { get; set; }
        /// <summary>
        /// 姓氏
        /// </summary>
        public string CustLastName { get; set; }
        /// <summary>
        /// 全名
        /// </summary>
        public string CustFullName { get; set; }
        /// <summary>
        /// 性別 M:男性/F:女性
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        public string TelNo { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public string BirthDate { get; set; }
    }
    public class CustomerPutResponseModel
    {
        /// <summary>
        /// 序號
        /// </summary>
        public int C_Seq { get; set; }
    }
}