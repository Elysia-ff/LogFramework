using System;
using System.Collections.Generic;

namespace LogFramework
{
    [Serializable]
    public class EnumKeyNotFoundException : KeyNotFoundException
    {
        public EnumKeyNotFoundException(LogBase log, Enum value)
            : base(string.Format("The given key ({0}.{1}) was not present in the dictionary of {2}.",
                value.GetType().ToString(), value.ToString(), log.GetType().Name))
        {
        }

        public EnumKeyNotFoundException(Enum value)
            : base(string.Format("The given key ({0}.{1}) was not present in the dictionary.",
                value.GetType().ToString(), value.ToString()))
        {
        }
    }
}
