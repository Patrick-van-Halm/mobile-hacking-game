using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeviceUserData
{
    public static List<DeviceUserData> GenerateRandomUserList(int minUsers, int maxUsers)
    {
        List<DeviceUserData> users = new();
        int userCount = UnityEngine.Random.Range(minUsers, maxUsers + 1);
        for(int i = 0; i < userCount; i++)
        {
            users.Add(new(DataListManager.Instance.UsernameList.SelectRandomItem(), DataListManager.Instance.PasswordList.SelectRandomItem(), false));
        }
        users.Add(new("root", DataListManager.Instance.PasswordList.SelectRandomItem(), true));
        return users;
    }

    public DeviceUserData(string username, string password, bool isAdmin)
    {
        Username = username;
        Password = password;
        IsAdmin = isAdmin;
    }

    [field:SerializeField] public string Username { get; private set; }
    [field: SerializeField] public string Password { get; private set; }
    [field: SerializeField] public bool IsAdmin { get; private set; }
}
