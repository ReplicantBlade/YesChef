using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
                _instance.GetAllResources();
                return _instance;
            }
        }
    }
    private readonly Dictionary<string, List<object>> _resources = new();
    private readonly List<Ingredient> _inventory = new();
    
    public string ToTimeString(float seconds)
    {
        var t = TimeSpan.FromSeconds(seconds);
        return seconds > 60f ? $"{t.Minutes:D2}:{t.Seconds:D2}" : $"{t.Seconds:D2}";
    }
    private void GetAllResources()
    {
        if (_resources.Count > 0) return;
        var resourcesPath = Application.dataPath + "/Resources/";
        var folders = System.IO.Directory.GetDirectories(resourcesPath);
        foreach (var folder in folders)
        {
            var s = folder.Split("/").Last().Trim();
            _resources.Add(s, new List<object>(Resources.LoadAll(s ,typeof(Ingredient))));
        }
    }
    public List<Ingredient> GetAvailableIngredients()
    {
        if (_inventory.Count > 0) return _inventory;
        foreach (var ingredient in _resources["Inventory"])
        {
            _inventory.Add((Ingredient)ingredient);
        }
        return _inventory;
    }
}