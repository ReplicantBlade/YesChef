using System;

public sealed class Utilities
{
    private static Utilities instance = null;
    private static readonly object padlock = new object();

    private Utilities(){}

    public static Utilities Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new Utilities();
                }
                return instance;
            }
        }
    }
    public string ToTimeString(float seconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(seconds);
        return seconds > 60f ? string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds) : string.Format("{1:D2}", t.Minutes, t.Seconds);
    }
}