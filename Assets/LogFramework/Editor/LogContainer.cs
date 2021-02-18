using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LogFramework
{
    public partial class LogContainer
    {
        private delegate bool Validate(LogBase log);
        private Dictionary<Type, IAddEventHandler> eventDic = new Dictionary<Type, IAddEventHandler>();

        private List<Data> logList = new List<Data>();
        public List<Data> LogList { get { return logList; } }

        public LogContainer()
        {
            eventDic.Clear();
            eventDic.Add(typeof(EnumLogAttribute), new AddEventHandler<EnumLogAttribute>(AddEnumLog));
            eventDic.Add(typeof(EnumEnumLogAttribute), new AddEventHandler<EnumEnumLogAttribute>(AddEnumEnumLog));
            eventDic.Add(typeof(BoolLogAttribute), new AddEventHandler<BoolLogAttribute>(AddBoolLog));
            eventDic.Add(typeof(TableLogAttribute), new AddEventHandler<TableLogAttribute>(AddTableLog));
            eventDic.Add(typeof(IntRangeLogAttribute), new AddEventHandler<IntRangeLogAttribute>(AddIntRangeLog));
            eventDic.Add(typeof(LogAttribute), new AddEventHandler<LogAttribute>(AddLog));
        }

        public void MakeLogList()
        {
            logList.Clear();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                var enumerable = GetTypesWithLogAttribute(assembly);
                foreach (Type t in enumerable)
                {
                    MakeLogList(t);
                }
            }
        }

        private IEnumerable<Type> GetTypesWithLogAttribute(Assembly assembly)
        {
            var types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.GetCustomAttributes(typeof(LogAttribute), true).Length > 0)
                    yield return type;
            }
        }

        private void MakeLogList(Type classType)
        {
            var customAttributes = classType.GetCustomAttributes(true);
            foreach (object attribute in customAttributes)
            {
                Type attributeType = attribute.GetType();
                if (eventDic.ContainsKey(attributeType))
                {
                    eventDic[attributeType].Invoke(classType, attribute);
                }
            }
        }

        private void CreateLogInstance(Type classType, string attributeDescription, Validate validate, params object[] args)
        {
            try
            {
                LogBase newT = Activator.CreateInstance(classType, args) as LogBase;
                if (validate == null || validate(newT))
                {
                    Data data = new Data(attributeDescription, newT);
                    logList.Add(data);
                }
            }
            catch (TargetInvocationException e) when (e.InnerException is EnumKeyNotFoundException)
            {
                // EnumKeyNotFoundException is expected in this method
                // because LogContainer tries to create the logs with all possible arguments
                // so it's harmless; ignore it.
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        private void AddLog(Type classType, LogAttribute attribute)
        {
            CreateLogInstance(classType, attribute.Desc, null, null);
        }

        private void AddEnumLog(Type classType, EnumLogAttribute attribute)
        {
            Type t = attribute.EnumType;
            foreach (string n in Enum.GetNames(t))
            {
                object i = Enum.Parse(t, n);
                CreateLogInstance(classType, attribute.Desc, null, i);
            }
        }

        private void AddEnumEnumLog(Type classType, EnumEnumLogAttribute attribute)
        {
            HashSet<string> hash = new HashSet<string>();
            Type t_1 = attribute.EnumType_1;
            Type t_2 = attribute.EnumType_2;
            foreach (string n_1 in Enum.GetNames(t_1))
            {
                object i = Enum.Parse(t_1, n_1);
                foreach (string n_2 in Enum.GetNames(t_2))
                {
                    object k = Enum.Parse(t_2, n_2);
                    CreateLogInstance(classType, attribute.Desc, (l) => !hash.Contains(l.LogName), i, k);
                }
            }
        }

        private void AddBoolLog(Type classType, BoolLogAttribute attribute)
        {
            CreateLogInstance(classType, attribute.Desc, null, false);
            CreateLogInstance(classType, attribute.Desc, null, true);
        }

        private void AddTableLog(Type classType, TableLogAttribute attribute)
        {
            int[] idxes = GetTableIdxes(attribute.TableEnum);
            for (int i = 0; i < idxes.Length; ++i)
            {
                object idx = idxes[i];

                CreateLogInstance(classType, attribute.Desc, null, idx);
            }
        }

        private int[] GetTableIdxes(TableEnumType tableEnum)
        {
            switch (tableEnum)
            {
                case TableEnumType.SampleTable:
                    return Sample.SampleTable.Table.RowList.
                        Where(t => !string.IsNullOrEmpty(t.log_name)).
                        Select(t => t.idx).
                        ToArray();
            }

            throw new NotImplementedException(tableEnum.ToString());
        }

        private void AddIntRangeLog(Type classType, IntRangeLogAttribute attribute)
        {
            for (int i = attribute.Min; i <= attribute.Max; ++i)
            {
                CreateLogInstance(classType, attribute.Desc, null, i);
            }
        }
    }
}
