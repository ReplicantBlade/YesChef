using System;
using System.Collections.Generic;
using Random = System.Random;

public sealed class Utilities
{
    private static Utilities _instance;
    private static readonly object Padlock = new();
    private Utilities(){}
    public static Utilities Instance
    {
        get
        {
            lock (Padlock)
            {
                if (_instance != null) return _instance;
                _instance = new Utilities();
                return _instance;
            }
        }
    }
    public string ToTimeString(float seconds)
    {
        var t = TimeSpan.FromSeconds(seconds);
        return seconds > 60f ? $"{t.Minutes:D2}:{t.Seconds:D2}" : $"{t.Seconds:D2}";
    }
    public T GetWeightedRandomItem<T>(List<T> items, List<int> weights)
    {
        var totalWeight = 0;
        foreach (var weight in weights)
        {
            totalWeight += weight;
        }
        var randomIndex = new Random().Next(0, totalWeight);
        for (var i = 0; i < items.Count; i++)
        {
            if (randomIndex < weights[i])
            {
                return items[i];
            }
            randomIndex -= weights[i];
        }
        return default(T);
    }
}