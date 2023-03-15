using DG.Tweening;
using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsersTimerManager : MonoBehaviour
{
    public HorizontalSelector usersHorizontalSelector;
    public ProgressBar userTimerProgressBar;
    public ListView usersTimersListView;
    public List<User> usersList = new List<User>();
    public int userTimerStartingValue;


    private void Start()
    {
        float usersCount = PlayerPrefs.GetInt("usersCount", 0);
        userTimerStartingValue = PlayerPrefs.GetInt("userTimerStartingValue", 1);
        string userName = "";
        int userCurrentTimer = 0;
        int userMaxTimer = 0;
        for (int i = 0; i < usersCount; i++)
        {
            userName = PlayerPrefs.GetString("user" + i, "User");
            userMaxTimer = PlayerPrefs.GetInt("userMaxTimer" + i, userTimerStartingValue);
            if (PlayerPrefs.GetInt("userTimerChanged") == 1)
                userCurrentTimer = userMaxTimer;
            else
                userCurrentTimer = PlayerPrefs.GetInt("userCurrentTimer" + i, userMaxTimer);
            usersList.Add(new User(userName, userCurrentTimer, userMaxTimer));
        }
        if (usersCount <= 0)
            return;
        PopulateUsersTimerMenu();
    }

    void PopulateUsersTimerMenu()
    {
        foreach (User user in usersList)
        {
            usersHorizontalSelector.CreateNewItem(user.name);

            usersTimersListView.rowCount = ListView.RowCount.Two;
            ListView.ListItem userTimer = new ListView.ListItem();
            userTimer.row0 = new ListView.ListRow();
            userTimer.row0.rowText = user.name;
            userTimer.row1 = new ListView.ListRow();
            userTimer.row1.rowText = "" + user.currentTimer;
            usersTimersListView.listItems.Add(userTimer);

        }
        usersHorizontalSelector.SetupSelector();
        usersHorizontalSelector.UpdateUI();

        usersTimersListView.InitializeItems();

        userTimerProgressBar.maxValue = usersList[0].maxTimer;
        userTimerProgressBar.ChangeValue(usersList[0].currentTimer);
        userTimerProgressBar.textPercent.text = "" + Utilities.FormattedBigTimerString(usersList[0].currentTimer);

    }

    public void UpdateUserTimer()
    {
        userTimerProgressBar.maxValue = usersList[usersHorizontalSelector.index].maxTimer;
        userTimerProgressBar.ChangeValue(usersList[usersHorizontalSelector.index].currentTimer);
        userTimerProgressBar.textPercent.text = "" + Utilities.FormattedBigTimerString(usersList[usersHorizontalSelector.index].currentTimer); 
    }

    public void UpdateUsersList()
    {
        for (int i = 0; i < usersTimersListView.listItems.Count; i++)
        {
            usersTimersListView.listItems[i].row1.rowText = Utilities.FormattedBigTimerString(usersList[i].currentTimer);
        }

        usersTimersListView.InitializeItems();
    }

    private void OnApplicationQuit()
    {
        for(int i = 0; i < usersList.Count; i++)
        {
            PlayerPrefs.SetInt("userCurrentTimer" + i, usersList[i].currentTimer);
        }
    }
}
