using System.Collections.Generic;

namespace LogFramework
{
    // TODO (LogFramework) ::
    // Define here your own logs!
    // Don't forget to delete sample logs.

    #region Sample Logs
    [Log("Button is clicked")]
    public class LogButtonClick : LogBase
    {
        private static readonly string logName = "log_button_click";

        public LogButtonClick()
            : base()
        {
            LogName = logName;
            LogFlag = LogFlagType.General;
        }
    }

    [Log("Button is clicked 2")]
    public class LogButtonClick2 : LogBase
    {
        private static readonly string logName = "log_button_click_2";

        public LogButtonClick2()
            : base()
        {
            LogName = logName;
            LogFlag = LogFlagType.General;
        }
    }

    [EnumLog("Dropdown is clicked", typeof(Sample.SampleScript.MyEnum))]
    public class LogDropDown : LogBase
    {
        private static readonly Dictionary<Sample.SampleScript.MyEnum, string> data = new Dictionary<Sample.SampleScript.MyEnum, string>()
        {
            { Sample.SampleScript.MyEnum.My_1, "log_drop_down_1" },
            { Sample.SampleScript.MyEnum.My_2, "log_drop_down_2" },
            { Sample.SampleScript.MyEnum.My_3, "log_drop_down_3" }
        };

        public LogDropDown(Sample.SampleScript.MyEnum myEnum)
            : base()
        {
            if (!IsValid(myEnum))
                throw new EnumKeyNotFoundException(myEnum);

            LogName = data[myEnum];
            LogFlag = LogFlagType.General;
        }

        public static bool IsValid(Sample.SampleScript.MyEnum myEnum)
        {
            return data.ContainsKey(myEnum);
        }
    }

    [EnumEnumLog("Two enum value are selected", typeof(Sample.SampleScript.MyEnum), typeof(Sample.SampleScript.YourEnum))]
    public class LogDualDropDown : LogBase
    {
        public LogDualDropDown(Sample.SampleScript.MyEnum myEnum, Sample.SampleScript.YourEnum yourEnum)
            : base()
        {
            LogName = string.Format("log_drop_down_{0}+{1}", myEnum.ToString(), yourEnum.ToString());
            LogFlag = LogFlagType.Special;
        }
    }

    [BoolLog("Toggle status has changed")]
    public class LogToggle : LogBase
    {
        private static readonly string onLogName = "log_toggle_on";
        private static readonly string offLogName = "log_toggle_off";

        public LogToggle(bool isChecked)
            : base()
        {
            LogName = isChecked ? onLogName : offLogName;
            LogFlag = LogFlagType.General;
        }
    }
    
    [TableLog("table selected", TableEnumType.SampleTable)]
    public class LogTable : LogBase
    {
        public LogTable(int idx)
            : base()
        {
            Sample.SampleTable.Row table = Sample.SampleTable.Table.Find_idx(idx);
            LogName = table.log_name;
            LogFlag = LogFlagType.Special;
        }

        public static bool IsValid(int idx)
        {
            Sample.SampleTable.Row table = Sample.SampleTable.Table.Find_idx(idx);
            return !string.IsNullOrEmpty(table.log_name);
        }
    }

    [IntRangeLog("Slider value changed", 5, 10)]
    public class LogSlider : LogBase
    {
        private static readonly string logFormat = "log_slider_{0}";

        public LogSlider(int v)
            : base()
        {
            LogName = string.Format(logFormat, v);
            LogFlag = LogFlagType.General;
        }
    }
    #endregion
}
