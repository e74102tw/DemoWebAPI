using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using System;
using DemoWebAPI.Controllers;
using DemoWebAPI.Models;
using DemoWebAPI.Library;
using System.Data.Common;
using System.Data.Odbc;
using DemoWebAPI.Enums;
using DemoWebAPI;
using Newtonsoft.Json;
using System.Reflection;

public class CustomerController : BaseApiController
{
    // GET: api/Customer
    public ActionResponseModel<List<CustomerDataModel>> GetAllCustomers ()
    {
        string guid = Guid.NewGuid().ToString();
        // 設定目前執行的 Function 名稱
        string m_Function = string.Format("{0}/{1}", this.GetType().Name, MethodBase.GetCurrentMethod().Name);
        // 初始化回傳的物件
        ActionResponseModel<List<CustomerDataModel>> resp = new ActionResponseModel<List<CustomerDataModel>>();
        try
        {
            // 寫入開始訊息到 Log
            LogHelper.LogProvider.Info(string.Format("{0} START", m_Function));
            // 記錄 Request 的相關資訊到 Log
            LogHelper.LogProvider.Info(string.Format("{0} REQ: {1}", m_Function, Request.GetClientIP()));
            LogHelper.LogProvider.DumpTrace(Enum_DumpSendType.Request, m_ControllerName, m_Function, m_Function, guid, "");
            ResponseModel responseModelTemp = new ResponseModel();
            DB_DemoDB db = new DB_DemoDB();
            string sQuery = "SELECT * FROM TB_Customer";
            List<CustomerDataModel> qryCustomers = db.Query<CustomerDataModel>(sQuery).ToList();
            if (qryCustomers == null)
            {
                resp.ResultCode = (int)ResultCode.NoData;
                resp.ErrorMessage = $"查無資料";
                resp.Exception = null;
                resp.Content = null;
            }
            else
            {
                // 回傳成功訊息
                resp.ResultCode = (int)ResultCode.Success;
                resp.ErrorMessage = "";
                resp.Exception = null;
                resp.Content = qryCustomers;
            }
        }
        catch (CustomAcitonException exAction)
        {
            // 回傳自訂錯誤訊息
            resp.ResultCode = (int)exAction.AcitonResult;
            resp.ErrorMessage = exAction.Message;
            resp.Exception = null;
            resp.Content = null;
        }
        catch (Exception ex)
        {
            // 回傳錯誤訊息
            resp.ResultCode = (int)ResultCode.Fail;
            resp.ErrorMessage = ex.Message;
            resp.Exception = ex;
            resp.Content = null;
        }
        // 記錄 Response 的相關資訊到 Log
        LogHelper.LogProvider.DumpTrace(Enum_DumpSendType.Response, m_ControllerName, m_Function, m_Function, guid, resp);
        LogHelper.LogProvider.Info(string.Format("{0} ClientIP:{1} END", m_Function, Request.GetClientIP()));
        // 回傳結果
        return resp;
    }

    // GET: api/Customer/{Seq}
    public ActionResponseModel<CustomerDataModel> Get(int Seq)
    {
        string guid = Guid.NewGuid().ToString();
        // 設定目前執行的 Function 名稱
        string m_Function = string.Format("{0}/{1}", this.GetType().Name, MethodBase.GetCurrentMethod().Name);
        // 初始化回傳的物件
        ActionResponseModel<CustomerDataModel> resp = new ActionResponseModel<CustomerDataModel>();
        try
        {
            // 寫入開始訊息到 Log
            LogHelper.LogProvider.Info(string.Format("{0} START", m_Function));
            // 記錄 Request 的相關資訊到 Log
            LogHelper.LogProvider.Info(string.Format("{0} REQ: {1}", m_Function, Request.GetClientIP()));
            LogHelper.LogProvider.DumpTrace(Enum_DumpSendType.Request, m_ControllerName, m_Function, m_Function, guid, $"Seq:{Seq}");
            if (Seq <= 0)
                throw new CustomAcitonException(ResultCode.WrongArgument, @"參數錯誤:Seq");
            DB_DemoDB db = new DB_DemoDB();
            string sQuery = "SELECT * FROM TB_Customer WHERE C_Seq = @C_Seq";
            CustomerDataModel qryCustomer = db.Query<CustomerDataModel>(sQuery, new { C_Seq = Seq }).FirstOrDefault();
            if (qryCustomer == null)
            {
                resp.ResultCode = (int)ResultCode.NoData;
                resp.ErrorMessage = $"查無資料,C_Seq:{Seq}";
                resp.Exception = null;
            }
            else
            {
                // 回傳成功訊息
                resp.ResultCode = (int)ResultCode.Success;
                resp.ErrorMessage = "";
                resp.Exception = null;
                resp.Content = qryCustomer;
            }
        }
        catch (CustomAcitonException exAction)
        {
            // 回傳自訂錯誤訊息
            resp.ResultCode = (int)exAction.AcitonResult;
            resp.ErrorMessage = exAction.Message;
            resp.Exception = null;
            resp.Content = null;
        }
        catch (Exception ex)
        {
            // 回傳錯誤訊息
            resp.ResultCode = (int)ResultCode.Fail;
            resp.ErrorMessage = ex.Message;
            resp.Exception = ex;
            resp.Content = null;
        }
        // 記錄 Response 的相關資訊到 Log
        LogHelper.LogProvider.DumpTrace(Enum_DumpSendType.Response, m_ControllerName, m_Function, m_Function, guid, resp);
        LogHelper.LogProvider.Info(string.Format("{0} ClientIP:{1} END", m_Function, Request.GetClientIP()));
        // 回傳結果
        return resp;
    }

    // POST: api/Customer
    public ActionResponseModel<CustomerPostResponseModel> Post([FromBody] CustomerPostRequestModel model)
    {
        string guid = Guid.NewGuid().ToString();
        // 設定目前執行的 Function 名稱
        string m_Function = string.Format("{0}/{1}", this.GetType().Name, MethodBase.GetCurrentMethod().Name);
        // 初始化回傳的物件
        ActionResponseModel<CustomerPostResponseModel> resp = new ActionResponseModel<CustomerPostResponseModel>();
        CustomerPostResponseModel detail = new CustomerPostResponseModel();
        try
        {
            // 寫入開始訊息到 Log
            LogHelper.LogProvider.Info(string.Format("{0} START", m_Function));
            // 記錄 Request 的相關資訊到 Log
            LogHelper.LogProvider.Info(string.Format("{0} REQ: {1}", m_Function, Request.GetClientIP()));
            LogHelper.LogProvider.DumpTrace(Enum_DumpSendType.Request, m_ControllerName, m_Function, m_Function, guid, model);
            // 驗證 Model 是否符合規範
            ResponseModel responseModelTemp = new ResponseModel();
            Methods.BaseValidation(ModelState, model, responseModelTemp);
            if (responseModelTemp.ResultCode == (int)ResultCode.WrongArgument)
                throw new CustomAcitonException(ResultCode.WrongArgument, responseModelTemp.ErrorMessage);
            // 初始化 Model 屬性的預設值
            model.InitializePropertyDefaultValues();
            DB_DemoDB db = new DB_DemoDB();
            string sQuery = "INSERT INTO TB_Customer (CustID, CustFirstName, CustLastName, CustFullName, Sex, TelNo, BirthDate)"
                            + " VALUES(@CustID, @CustFirstName, @CustLastName, @CustFullName, @Sex, @TelNo, @BirthDate);"
                            + " SELECT CAST(SCOPE_IDENTITY() as int)";
            var parameters = new
            {
                CustID = model.CustID,
                CustFirstName = model.CustFirstName,
                CustLastName = model.CustLastName,
                CustFullName = model.CustFullName,
                Sex = model.Sex.ToUpper(),
                TelNo = model.TelNo,
                BirthDate = model.BirthDate
            };
            var id = db.Query<int>(sQuery, parameters).Single();
            detail.C_Seq = id;
            // 回傳成功訊息
            resp.ResultCode = (int)ResultCode.Success;
            resp.ErrorMessage = "";
            resp.Exception = null;
            resp.Content = detail;
        }
        catch (CustomAcitonException exAction)
        {
            // 回傳自訂錯誤訊息
            resp.ResultCode = (int)exAction.AcitonResult;
            resp.ErrorMessage = exAction.Message;
            resp.Exception = null;
            resp.Content = null;
        }
        catch (Exception ex)
        {
            // 回傳錯誤訊息
            resp.ResultCode = (int)ResultCode.Fail;
            resp.ErrorMessage = ex.Message;
            resp.Exception = ex;
            resp.Content = null;
        }
        // 記錄 Response 的相關資訊到 Log
        LogHelper.LogProvider.DumpTrace(Enum_DumpSendType.Response, m_ControllerName, m_Function, m_Function, guid, resp);
        LogHelper.LogProvider.Info(string.Format("{0} ClientIP:{1} END", m_Function, Request.GetClientIP()));
        // 回傳結果
        return resp;
    }

    // PUT: api/Customer
    public ActionResponseModel<CustomerPutResponseModel> Put([FromBody] CustomerPutRequestModel model)
    {
        string guid = Guid.NewGuid().ToString();
        // 設定目前執行的 Function 名稱
        string m_Function = string.Format("{0}/{1}", this.GetType().Name, MethodBase.GetCurrentMethod().Name);
        // 初始化回傳的物件
        ActionResponseModel<CustomerPutResponseModel> resp = new ActionResponseModel<CustomerPutResponseModel>();
        CustomerPutResponseModel detail = new CustomerPutResponseModel();
        try
        {
            // 寫入開始訊息到 Log
            LogHelper.LogProvider.Info(string.Format("{0} START", m_Function));
            // 記錄 Request 的相關資訊到 Log
            LogHelper.LogProvider.Info(string.Format("{0} REQ: {1}", m_Function, Request.GetClientIP()));
            LogHelper.LogProvider.DumpTrace(Enum_DumpSendType.Request, m_ControllerName, m_Function, m_Function, guid, model);
            // 驗證 Model 是否符合規範
            ResponseModel responseModelTemp = new ResponseModel();
            Methods.BaseValidation(ModelState, model, responseModelTemp);
            if (responseModelTemp.ResultCode == (int)ResultCode.WrongArgument)
                throw new CustomAcitonException(ResultCode.WrongArgument, responseModelTemp.ErrorMessage);
            // 初始化 Model 屬性的預設值
            model.InitializePropertyDefaultValues();

            DB_DemoDB db = new DB_DemoDB();           
            string sQuery = "UPDATE TB_Customer SET CustID = @CustID, CustFirstName = @CustFirstName, CustLastName = @CustLastName, CustFullName = @CustFullName, Sex = @Sex, TelNo = @TelNo, BirthDate = @BirthDate WHERE C_Seq = @C_Seq";
            var parameters = new
            {
                C_Seq = model.C_Seq,
                CustID = model.CustID,
                CustFirstName = model.CustFirstName,
                CustLastName = model.CustLastName,
                CustFullName = model.CustFullName,
                Sex = model.Sex.ToUpper(),
                TelNo = model.TelNo,
                BirthDate = model.BirthDate
            };

            if (db.Execute(sQuery, parameters) <= 0)
                throw new CustomAcitonException(ResultCode.Fail, "更新失敗!");
            detail.C_Seq = model.C_Seq;
            // 回傳成功訊息
            resp.ResultCode = (int)ResultCode.Success;
            resp.ErrorMessage = "";
            resp.Exception = null;
            resp.Content = detail;
        }
        catch (CustomAcitonException exAction)
        {
            // 回傳自訂錯誤訊息
            resp.ResultCode = (int)exAction.AcitonResult;
            resp.ErrorMessage = exAction.Message;
            resp.Exception = null;
            resp.Content = null;
        }
        catch (Exception ex)
        {
            // 回傳錯誤訊息
            resp.ResultCode = (int)ResultCode.Fail;
            resp.ErrorMessage = ex.Message;
            resp.Exception = ex;
            resp.Content = null;
        }
        // 記錄 Response 的相關資訊到 Log
        LogHelper.LogProvider.DumpTrace(Enum_DumpSendType.Response, m_ControllerName, m_Function, m_Function, guid, resp);
        LogHelper.LogProvider.Info(string.Format("{0} ClientIP:{1} END", m_Function, Request.GetClientIP()));
        // 回傳結果
        return resp;
    }

    // DELETE: api/Customer/{Seq}
    public ActionResponseModel<int> Delete(int Seq)
    {
        string guid = Guid.NewGuid().ToString();
        // 設定目前執行的 Function 名稱
        string m_Function = string.Format("{0}/{1}", this.GetType().Name, MethodBase.GetCurrentMethod().Name);
        // 初始化回傳的物件
        ActionResponseModel<int> resp = new ActionResponseModel<int>();
        try
        {
            // 寫入開始訊息到 Log
            LogHelper.LogProvider.Info(string.Format("{0} START", m_Function));
            // 記錄 Request 的相關資訊到 Log
            LogHelper.LogProvider.Info(string.Format("{0} REQ: {1}", m_Function, Request.GetClientIP()));
            LogHelper.LogProvider.DumpTrace(Enum_DumpSendType.Request, m_ControllerName, m_Function, m_Function, guid, $"Seq:{Seq}");
            if (Seq <= 0)
                throw new CustomAcitonException(ResultCode.WrongArgument, @"參數錯誤:Seq");
            DB_DemoDB db = new DB_DemoDB();
            string sQuery = "DELETE FROM TB_Customer WHERE C_Seq = @C_Seq";           
            if (db.Execute(sQuery, new { C_Seq = Seq }) <= 0)
                throw new CustomAcitonException(ResultCode.Fail, "刪除失敗!");
            // 回傳成功訊息
            resp.ResultCode = (int)ResultCode.Success;
            resp.ErrorMessage = "";
            resp.Exception = null;
            resp.Content = Seq;
        }
        catch (CustomAcitonException exAction)
        {
            // 回傳自訂錯誤訊息
            resp.ResultCode = (int)exAction.AcitonResult;
            resp.ErrorMessage = exAction.Message;
            resp.Exception = null;
            resp.Content = 0;
        }
        catch (Exception ex)
        {
            // 回傳錯誤訊息
            resp.ResultCode = (int)ResultCode.Fail;
            resp.ErrorMessage = ex.Message;
            resp.Exception = ex;
            resp.Content = 0;
        }
        // 記錄 Response 的相關資訊到 Log
        LogHelper.LogProvider.DumpTrace(Enum_DumpSendType.Response, m_ControllerName, m_Function, m_Function, guid, resp);
        LogHelper.LogProvider.Info(string.Format("{0} ClientIP:{1} END", m_Function, Request.GetClientIP()));
        // 回傳結果
        return resp;
    }
}
