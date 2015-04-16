using System;
using System.Diagnostics;
using System.Reflection;

namespace LingSubPlayer.Common.Logging
{
    public class Log : ILog
    {
        private InternalLog internalLog;

        public IInternalLog Write {
            get
            {
                if (internalLog != null)
                {
                    return internalLog;
                }

                return (internalLog = new InternalLog(GetCallingClassName()));
            }
        }

        private string GetCallingClassName()
        {
            int skipFrames = 2;
            Type declaringType;
            string name;
            do
            {
                MethodBase method = new StackFrame(skipFrames, false).GetMethod();
                declaringType = method.DeclaringType;
                if (declaringType == (Type)null)
                {
                    return method.Name;
                }

                while (declaringType.IsNested && declaringType.ReflectedType != null && declaringType.Name.Contains("<"))
                {
                    declaringType = declaringType.ReflectedType;
                }

                ++skipFrames;
                name = declaringType.FullName;
            }
            while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));
            return name;
        }
    }
}