using DemoWebAPI.Enums;
using System;

namespace DemoWebAPI.Library
{
    internal class CustomException : Exception
    {
        public CustomException(string value)
            : base(value)
        {
        }
        public CustomException(string value, params object[] args)
            : base(string.Format(value, args))
        {
        }
    }

    internal class CustomAcitonException : Exception
    {
        private ResultCode m_Result = ResultCode.None;
        public ResultCode AcitonResult { get { return m_Result; } }

        public CustomAcitonException(ResultCode _result, string _value)
            : base(_value)
        {
            m_Result = _result;
        }
        public CustomAcitonException(ResultCode _result, string _value, params object[] _args)
            : base(string.Format(_value, _args))
        {
            m_Result = _result;
        }
    }
}
