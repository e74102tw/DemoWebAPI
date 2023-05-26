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
    public ActionResponseModel<List<CustomerModel>> GetAll ()
    {
        string guid = Guid.NewGuid().ToString();
        // 設定目前執行的 Function 名稱
        string m_Function = string.Format("{0}/{1}", this.GetType().Name, MethodBase.GetCurrentMethod().Name);
        // 初始化回傳的物件
        ActionResponseModel<List<CustomerModel>> resp = new ActionResponseModel<List<CustomerModel>>();
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
            List<CustomerModel> qryCustomers = db.Query<CustomerModel>(sQuery).ToList();
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

    // GET: api/Customer/5
    public ActionResponseModel<CustomerModel> Get(int Seq)
    {
        string guid = Guid.NewGuid().ToString();
        // 設定目前執行的 Function 名稱
        string m_Function = string.Format("{0}/{1}", this.GetType().Name, MethodBase.GetCurrentMethod().Name);
        // 初始化回傳的物件
        ActionResponseModel<CustomerModel> resp = new ActionResponseModel<CustomerModel>();
        try
        {
            // 寫入開始訊息到 Log
            LogHelper.LogProvider.Info(string.Format("{0} START", m_Function));
            // 記錄 Request 的相關資訊到 Log
            LogHelper.LogProvider.Info(string.Format("{0} REQ: {1}", m_Function, Request.GetClientIP()));
            LogHelper.LogProvider.DumpTrace(Enum_DumpSendType.Request, m_ControllerName, m_Function, m_Function, guid, $"Seq:{Seq}");
            ResponseModel responseModelTemp = new ResponseModel();
            DB_DemoDB db = new DB_DemoDB();
            string sQuery = "SELECT * FROM TB_Customer WHERE C_Seq = @C_Seq";
            CustomerModel qryCustomer = db.Query<CustomerModel>(sQuery, new { C_Seq = Seq }).FirstOrDefault();
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
    public ActionResponseModel<CustomerModel> Post([FromBody] CustomerModel customer)
    {
        string guid = Guid.NewGuid().ToString();
        // 設定目前執行的 Function 名稱
        string m_Function = string.Format("{0}/{1}", this.GetType().Name, MethodBase.GetCurrentMethod().Name);
        // 初始化回傳的物件
        ActionResponseModel<CustomerModel> resp = new ActionResponseModel<CustomerModel>();
        try
        {
            // 寫入開始訊息到 Log
            LogHelper.LogProvider.Info(string.Format("{0} START", m_Function));
            // 記錄 Request 的相關資訊到 Log
            LogHelper.LogProvider.Info(string.Format("{0} REQ: {1}", m_Function, Request.GetClientIP()));
            LogHelper.LogProvider.DumpTrace(Enum_DumpSendType.Request, m_ControllerName, m_Function, m_Function, guid, customer);
            ResponseModel responseModelTemp = new ResponseModel();
            DB_DemoDB db = new DB_DemoDB();
            string sQuery = "INSERT INTO TB_Customer (CustID, CustFirstName, CustLastName, CustFullName, Sex, TelNo, BirthDate)"
                            + " VALUES(@CustID, @CustFirstName, @CustLastName, @CustFullName, @Sex, @TelNo, @BirthDate);"
                            + " SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = db.Query<int>(sQuery, customer).Single();
            customer.C_Seq = id;
            // 回傳成功訊息
            resp.ResultCode = (int)ResultCode.Success;
            resp.ErrorMessage = "";
            resp.Exception = null;
            resp.Content = customer;
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

    // PUT: api/Customer/5
    public ActionResponseModel<CustomerModel> Put(int Seq, [FromBody] CustomerModel customer)
    {
        string guid = Guid.NewGuid().ToString();
        // 設定目前執行的 Function 名稱
        string m_Function = string.Format("{0}/{1}", this.GetType().Name, MethodBase.GetCurrentMethod().Name);
        // 初始化回傳的物件
        ActionResponseModel<CustomerModel> resp = new ActionResponseModel<CustomerModel>();
        try
        {
            // 寫入開始訊息到 Log
            LogHelper.LogProvider.Info(string.Format("{0} START", m_Function));
            // 記錄 Request 的相關資訊到 Log
            LogHelper.LogProvider.Info(string.Format("{0} REQ: {1}", m_Function, Request.GetClientIP()));
            customer.C_Seq = Seq;
            LogHelper.LogProvider.DumpTrace(Enum_DumpSendType.Request, m_ControllerName, m_Function, m_Function, guid, customer);
            ResponseModel responseModelTemp = new ResponseModel();
            DB_DemoDB db = new DB_DemoDB();
            
            string sQuery = "UPDATE TB_Customer SET CustID = @CustID, CustFirstName = @CustFirstName, CustLastName = @CustLastName, CustFullName = @CustFullName, Sex = @Sex, TelNo = @TelNo, BirthDate = @BirthDate WHERE C_Seq = @C_Seq";
            db.Execute(sQuery, customer);
            // 回傳成功訊息
            resp.ResultCode = (int)ResultCode.Success;
            resp.ErrorMessage = "";
            resp.Exception = null;
            resp.Content = customer;
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

    // DELETE: api/Customer/5
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
            ResponseModel responseModelTemp = new ResponseModel();
            DB_DemoDB db = new DB_DemoDB();
            string sQuery = "DELETE FROM TB_Customer WHERE C_Seq = @C_Seq";
            db.Execute(sQuery, new { C_Seq = Seq });
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
