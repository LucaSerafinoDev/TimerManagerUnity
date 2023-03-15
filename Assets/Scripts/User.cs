using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string name;
    public int currentTimer;
    public int maxTimer;

    public User(string name, int currentTimer, int maxTimer)
    {
        this.name = name;
        this.currentTimer = currentTimer;
        this.maxTimer = maxTimer;
    }
}
