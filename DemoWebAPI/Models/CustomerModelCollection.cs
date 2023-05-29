using System;
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
        [Required(ErrorMessage = "請輸入身分證字號")]
        public string CustID { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [Required(ErrorMessage = "請輸入名稱")]
        public string CustFirstName { get; set; }

        /// <summary>
        /// 姓氏
        /// </summary>
        [Required(ErrorMessage = "請輸入姓氏")]
        public string CustLastName { get; set; }

        /// <summary>
        /// 全名
        /// </summary>
        [Required(ErrorMessage = "請輸入全名")]
        public string CustFullName { get; set; }

        /// <summary>
        /// 性別 M:男性/F:女性
        /// </summary>
        [Required(ErrorMessage = "請輸入性別")]
        [RegularExpression("^[MFmf]$", ErrorMessage = "性別只能是 M 或 F")]
        public string Sex { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        [Required(ErrorMessage = "請輸入電話號碼")]
        public string TelNo { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [RegularExpression(@"^(19|20)\d{2}/(0[1-9]|1[0-2])/([012]\d|3[01])$", ErrorMessage = "請輸入有效的生日，格式為 YYYY/MM/DD")]
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
        [Required(ErrorMessage = "請輸入Seq")]
        public int C_Seq { get; set; }
        /// <summary>
        /// 身分證字號
        /// </summary>
        [Required(ErrorMessage = "請輸入身分證字號")]
        public string CustID { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [Required(ErrorMessage = "請輸入名稱")]
        public string CustFirstName { get; set; }

        /// <summary>
        /// 姓氏
        /// </summary>
        [Required(ErrorMessage = "請輸入姓氏")]
        public string CustLastName { get; set; }

        /// <summary>
        /// 全名
        /// </summary>
        [Required(ErrorMessage = "請輸入全名")]
        public string CustFullName { get; set; }

        /// <summary>
        /// 性別 M:男性/F:女性
        /// </summary>
        [Required(ErrorMessage = "請輸入性別")]
        [RegularExpression("^[MFmf]$", ErrorMessage = "性別只能是 M 或 F")]
        public string Sex { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        [Required(ErrorMessage = "請輸入電話號碼")]
        public string TelNo { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [RegularExpression(@"^(19|20)\d{2}/(0[1-9]|1[0-2])/([012]\d|3[01])$", ErrorMessage = "請輸入有效的生日，格式為 YYYY/MM/DD")]
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