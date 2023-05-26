using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoWebAPI.Enums
{
    public enum ResultCode : short
    {
        None = -1,
        Success = 0,
        Fail = 998,
        WrongArgument = 997,
        NoData = 996,
        AuthenticateFail = 995
    }

}