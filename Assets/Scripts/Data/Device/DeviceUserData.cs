using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceUserData
{
    public static List<DeviceUserData> GenerateRandomUserList(int minUsers, int maxUsers)
    {
        List<DeviceUserData> users = new();
        int userCount = Random.Range(minUsers, maxUsers + 1);
        for(int i = 0; i < userCount; i++)
        {
            users.Add(new()
            {
                Username = DataListManager.Instance.UsernameList.SelectRandomItem(),
                Password = DataListManager.Instance.PasswordList.SelectRandomItem(),
                IsAdmin = false
            });
        }
        users.Add(new()
        {
            Username = "root",
            Password = DataListManager.Instance.PasswordList.SelectRandomItem(),
            IsAdmin = true
        });
        return users;
    }

    public string Username { get; private set; }
    public string Password { get; private set; }
    public bool IsAdmin { get; private set; }
}
