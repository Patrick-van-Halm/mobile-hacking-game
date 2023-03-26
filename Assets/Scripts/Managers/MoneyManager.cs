using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : SingletonMonoBehaviour<MoneyManager>
{
    [field: SerializeField] public int Money { get; private set; }
    [SerializeField] private IntEvent _onMoneyChanged;

    public void Spend(int amount)
    {
        if (amount < 0) return;
        if (Money - amount < 0) return;
        Money -= amount;
        _onMoneyChanged.Invoke(Money);
    }

    public void Earn(int amount)
    {
        if(amount < 0) return;
        Money += amount;
        _onMoneyChanged.Invoke(Money);
    }
}
