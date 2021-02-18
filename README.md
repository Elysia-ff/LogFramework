# LogFramework
A log framework for Unity  
This will help you to send the log (for analysis maybe) to your server.  
And provides a tool that can search the logs by their name.

# What should I do?
## First, open Sample/SampleScene.  
You'll see..

![image](https://user-images.githubusercontent.com/45890606/108326117-35577f00-720d-11eb-8ccd-ff5ad50e80d5.png)

Click 'Play' and click those buttons whatever you want.  
then, in the Console, you can see two lines.
```
Log sent : log_button_click
logFlag : General
// this may be different by what you've chosen
```
And a error log (Ignore this error for now)
```
Cannot connect to destination host
```
 
## Let's see how it works.  
See [SampleScript.cs](https://github.com/Elysia-ff/LogFramework/blob/main/Assets/LogFramework/Sample/SampleScript.cs)  
By calling `LogSystem.Instance.Send(new <YourLogType>(some_arguments));`, you can send the log.  
Now, we're gonna make a new log. see [LogData.cs](https://github.com/Elysia-ff/LogFramework/blob/main/Assets/LogFramework/LogData.cs)  
The only one thing you have to do is to make a new class inherits LogBase then set its LogName and LogFlag in the constructor.  
Do not forget to add a attribute corresponding your new log!  

What attribute does correspond to my log? see [LogAttribute.cs](https://github.com/Elysia-ff/LogFramework/blob/main/Assets/LogFramework/LogAttribute.cs)  
And its name means what arguments does constructor of the log need.

Lastly, Replace `<your log server address>` with your server address in [LogSystem.cs](https://github.com/Elysia-ff/LogFramework/blob/3a8a83267b21399e8246bee50f5fed1c810a81b7/Assets/LogFramework/LogSystem.cs#L49)  
The connect error above has now gone.

After you make a lot of logs.  
You might forget what the log is really meaning.  
Open Window -> Log Finder

![image](https://user-images.githubusercontent.com/45890606/108326030-1bb63780-720d-11eb-903d-beee3d3e35c2.png)

This window will make all possible logs and filter them.  
For example, [sample table.csv](https://github.com/Elysia-ff/LogFramework/blob/main/Assets/LogFramework/Sample/sample%20table.csv) has 5 rows 
but only 3 rows have valid log_name fields.  
So Log Finder will show you those 3 rows only. `log_table_idx_1, log_table_idx_2 and log_table_idx_5 in the image`  

## But I have a log class that doesn't correspond with existing attributes.
Let's make a new attribute named FloatRangeLogAttribute for example.
Go to [LogAttribute.cs](https://github.com/Elysia-ff/LogFramework/blob/3a8a83267b21399e8246bee50f5fed1c810a81b7/Assets/LogFramework/LogAttribute.cs), 
define it.  
```
public class FloatRangeLogAttribute : LogAttribute
{
    public float Min { get; private set; }
    public float Max { get; private set; }
    public float Delta { get; private set; }

    public FloatRangeLogAttribute(string desc, float min, float max, float delta)
        : base(desc)
    {
        Min = min;
        Max = max;
        Delta = delta;
    }
}
```
Then, Assign it to [LogContainer.eventDic](https://github.com/Elysia-ff/LogFramework/blob/3a8a83267b21399e8246bee50f5fed1c810a81b7/Assets/LogFramework/Editor/LogContainer.cs#L12)  
```
eventDic.Add(typeof(FloatRangeLogAttribute), new AddEventHandler<FloatRangeLogAttribute>(AddFloatRangeLog));
```

`AddFloatRangeLog` looks like..  
```
private void AddFloatRangeLog(Type classType, FloatRangeLogAttribute attribute)
{
    for (float i = attribute.Min; i <= attribute.Max; i += attribute.Delta)
    {
        CreateLogInstance(classType, attribute.Desc, null, i);
    }
}
```

Now, you can make a new log that has float.
```
[FloatRangeLog("Let's make a new attribute", 1.0f, 2.0f, 0.1f)]
public class LogNew : LogBase
{
    private static readonly string logFormat = "I'm a new log {0}";

    public LogNew(float v)
        : base()
    {
        LogName = string.Format(logFormat, v);
        LogFlag = LogFlagType.General;
    }
}
```

Then Log Finder will show you the new logs.  
![image](https://user-images.githubusercontent.com/45890606/108339828-40191080-721b-11eb-8a67-5824c32e28c0.png)

## For more information
Search `// TODO (LogFramework) ::` or See `View -> Task List` on Visual Studio.
