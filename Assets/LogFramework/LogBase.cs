using System;
using System.Text;
using UnityEngine;

namespace LogFramework
{
    [Serializable]
    public abstract class LogBase
    {
        private static readonly StringBuilder sb = new StringBuilder();

        public string LogName { get; protected set; }
        public LogFlagType LogFlag { get; protected set; }

        public LogBase()
        {
            // TODO (LogFramework) ::
            // if you need user datas in the log, collect here.
            // then, add them to ToString() too for better debugging.
            // but you'd better collect them on the server side.
            //
            // ex)
            // loginTime = LoginSystem.Instance.GetLoginTime();
            // uid = LoginSystem.Instance.GetUid();
        }

        public override string ToString()
        {
            sb.Remove(0, sb.Length);
            sb.AppendFormat("logFlag : {0}\n", LogFlag.ToString());

            return sb.ToString();
        }

        public string ToJSON()
        {
            string json = JsonUtility.ToJson(this);
            return json;
        }
    }
}
