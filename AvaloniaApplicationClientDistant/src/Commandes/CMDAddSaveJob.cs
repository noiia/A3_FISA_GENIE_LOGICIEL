﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AvaloniaApplicationClientDistant.Commandes;

public class CMDAddSaveJob : CMD
{
    private string _Destination;

    private string _Name;
    private string _Source;
    private string _Type;

    public CMDAddSaveJob(string name, string source, string destination, string type) : base("AddSaveJob")
    {
        _Name = name;
        _Source = source;
        _Destination = destination;
        _Type = type;
    }


    public string Source
    {
        get => _Source;
        set => _Source = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Destination
    {
        get => _Destination;
        set => _Destination = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Type
    {
        get => _Type;
        set => _Type = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Name
    {
        get => _Name;
        set => _Name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override string toString()
    {
        var json = new JObject();
        json.Add("commande", command);
        json.Add("name", _Name);
        json.Add("source", _Source);
        json.Add("destination", _Destination);
        json.Add("type", _Type);
        var jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
}