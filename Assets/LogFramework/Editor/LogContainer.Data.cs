namespace LogFramework
{
    public partial class LogContainer
    {
        public class Data
        {
            public string Desc { get; private set; }
            public LogBase Log { get; private set; }

            public Data(string desc, LogBase log)
            {
                Desc = desc;
                Log = log;
            }

            public override string ToString()
            {
                return string.Format("{0} ({1}) : {2}", Log.LogName, Log.LogFlag, Desc);
            }
        }
    }
}
