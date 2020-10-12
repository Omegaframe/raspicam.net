using System;

namespace MMALSharp
{
    internal class Parameter
    {
        public int ParamValue { get; set; }

        public Type ParamType { get; set; }

        public string ParamName { get; set; }

        public Parameter(int paramVal, Type paramType, string paramName)
        {
            ParamValue = paramVal;
            ParamType = paramType;
            ParamName = paramName;
        }
    }
}
