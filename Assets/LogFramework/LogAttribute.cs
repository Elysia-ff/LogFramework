using System;

namespace LogFramework
{
    // TODO (LogFramework) ::
    // If you have some log classes that don't correspond to the attributes already exist,
    // define here your attributes then, assign them to LogContainer.eventDic.
    // They must inherit LogAttribute!

    /// <summary>
    /// Default Attribute. All the other attributes must inherit this.
    /// The log class that has this attribute has no arguments in the constructor. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class LogAttribute : Attribute
    {
        public string Desc { get; private set; }

        public LogAttribute(string desc)
        {
            Desc = desc;
        }
    }

    /// <summary>
    /// The log class that has this attribute has a constructor(EnumType).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class EnumLogAttribute : LogAttribute
    {
        public Type EnumType { get; private set; }

        public EnumLogAttribute(string desc, Type enumType)
            : base(desc)
        {
            EnumType = enumType;
        }
    }

    /// <summary>
    /// The log class that has this attribute has a constructor(EnumType, EnumType).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class EnumEnumLogAttribute : LogAttribute
    {
        public Type EnumType_1 { get; private set; }
        public Type EnumType_2 { get; private set; }

        public EnumEnumLogAttribute(string desc, Type enumType_1, Type enumType_2)
            : base(desc)
        {
            EnumType_1 = enumType_1;
            EnumType_2 = enumType_2;
        }
    }

    /// <summary>
    /// The log class that has this attribute has a constructor(Bool).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class BoolLogAttribute : LogAttribute
    {
        public BoolLogAttribute(string desc)
            : base(desc)
        {
        }
    }

    /// <summary>
    /// The log class that has this attribute has a constructor(TableIdxes).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableLogAttribute : LogAttribute
    {
        public TableEnumType TableEnum { get; private set; }

        public TableLogAttribute(string desc, TableEnumType tableEnum)
            : base(desc)
        {
            TableEnum = tableEnum;
        }
    }

    /// <summary>
    /// The log class that has this attribute has a constructor(IntRange).
    /// </summary>
    public class IntRangeLogAttribute : LogAttribute
    {
        public int Min { get; private set; }
        public int Max { get; private set; }

        public IntRangeLogAttribute(string desc, int min, int max)
            : base(desc)
        {
            Min = min;
            Max = max;
        }
    }
}
