using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Person
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public string PlayerId { get; private set; }
    [field: SerializeField] public string Faction { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string EmailAddress { get; private set; }

    public Person(string id, string playerId, string faction, string name, string emailAddress)
    {
        Id = id;
        PlayerId = playerId;
        Faction = faction;
        Name = name;
        EmailAddress = emailAddress;
    }
}
