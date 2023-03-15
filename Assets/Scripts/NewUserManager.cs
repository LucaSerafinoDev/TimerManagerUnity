using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NewUserManager : MonoBehaviour
{
    public GameObject newUserPanelPrefab;
    public Transform usersListContainer;

    public CustomInputField newUserInputField;
    public List<NewUserName> usersList = new List<NewUserName>();

    public CustomInputField bigTimerValueInputFieldHrs;
    public CustomInputField bigTimerValueInputFieldMins;
    public CustomInputField bigTimerValueInputFieldSecs;

    public CustomInputField userTimerValueInputFieldHrs;
    public CustomInputField userTimerValueInputFieldMins;
    public CustomInputField userTimerValueInputFieldSecs;

    public TextMeshProUGUI infoTextTimers;
    public TextMeshProUGUI infoTextUsers;


    public WindowManager newUserWindowManager;


    public static NewUserManager Instance { get; private set; }


    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        float bigTimerValue = PlayerPrefs.GetInt("bigTimerValue", 0);
        if (bigTimerValue > 0)
        {
            string hours;
            string minutes;
            string seconds;
            bool formatNeedsMinutes;
            bool formatNeedsHours;

            seconds = (bigTimerValue % 60).ToString("00");
            formatNeedsMinutes = Mathf.Floor(bigTimerValue / 60) > 0;
            formatNeedsHours = Mathf.Floor((bigTimerValue / 60) / 60) > 0;
            if (formatNeedsMinutes)
            {
                if (!formatNeedsHours)
                    minutes = Mathf.Floor(bigTimerValue / 60).ToString("00");
                else
                {
                    minutes = (Mathf.Floor(bigTimerValue / 60) % 60).ToString("00");
                    hours = Mathf.Floor((bigTimerValue / 60) / 60).ToString("00");
                    bigTimerValueInputFieldHrs.AnimateIn();
                    bigTimerValueInputFieldHrs.inputText.text = hours;
                }
                bigTimerValueInputFieldMins.AnimateIn();
                bigTimerValueInputFieldMins.inputText.text = minutes;
            }
            bigTimerValueInputFieldSecs.AnimateIn();
            bigTimerValueInputFieldSecs.inputText.text = seconds;
        }

        float userTimerValue = PlayerPrefs.GetInt("userTimerStartingValue", 0);
        if (userTimerValue > 0)
        {
            string hours;
            string minutes;
            string seconds;
            bool formatNeedsMinutes;
            bool formatNeedsHours;

            seconds = (userTimerValue % 60).ToString("00");
            formatNeedsMinutes = Mathf.Floor(userTimerValue / 60) > 0;
            formatNeedsHours = Mathf.Floor((userTimerValue / 60) / 60) > 0;
            if (formatNeedsMinutes)
            {
                if (!formatNeedsHours)
                    minutes = Mathf.Floor(userTimerValue / 60).ToString("00");
                else
                {
                    minutes = (Mathf.Floor(userTimerValue / 60) % 60).ToString("00");
                    hours = Mathf.Floor((userTimerValue / 60) / 60).ToString("00");
                    userTimerValueInputFieldHrs.AnimateIn();
                    userTimerValueInputFieldHrs.inputText.text = hours;
                }
                userTimerValueInputFieldMins.AnimateIn();
                userTimerValueInputFieldMins.inputText.text = minutes;
            }
            userTimerValueInputFieldSecs.AnimateIn();
            userTimerValueInputFieldSecs.inputText.text = seconds;
        }

        float usersCount = PlayerPrefs.GetInt("usersCount", 0);
        if (usersCount > 0)
        {
            for (int i = 0; i < usersCount; i++)
            {
                GameObject userNameInList = Instantiate(newUserPanelPrefab, usersListContainer);
                NewUserName newUserName = userNameInList.GetComponent<NewUserName>();
                newUserName.userName = PlayerPrefs.GetString("user" + i, "User");
                newUserName.userNameText.text = (i + 1) + ") " + newUserName.userName;
                usersList.Add(userNameInList.GetComponent<NewUserName>());
                newUserInputField.inputText.text = "";
            }
        }
    }

    public void OnClickAddUser()
    {
        string username = newUserInputField.inputText.text;
        if (!string.IsNullOrEmpty(username))
        {
            SetInfoUsersMessage("Click ok to terminate the setup", Color.white);
            GameObject userNameInList = Instantiate(newUserPanelPrefab, usersListContainer);
            NewUserName newUserName = userNameInList.GetComponent<NewUserName>();
            newUserName.userName = username;
            newUserName.userNameText.text = (usersList.Count + 1) + ") " + username;
            usersList.Add(userNameInList.GetComponent<NewUserName>());
            newUserInputField.inputText.text = "";
        }
    }


    int bigTimerHrsValue = 0;
    int bigTimerMinsValue = 0;
    int bigTimerSecsValue = 0;
    int bigTimerTotalValue = 0;
    int userTimerHrsValue = 0;
    int userTimerMinsValue = 0;
    int userTimerSecsValue = 0;
    int userTimerTotalValue = 0;
    public void FinishSetup()
    {
        bool isInputCorrect = true;
        int.TryParse(bigTimerValueInputFieldHrs.inputText.text, out bigTimerHrsValue);
        int.TryParse(bigTimerValueInputFieldMins.inputText.text, out bigTimerMinsValue);
        int.TryParse(bigTimerValueInputFieldSecs.inputText.text, out bigTimerSecsValue);

        if (bigTimerHrsValue > 0 || bigTimerMinsValue > 0 || bigTimerSecsValue > 0)
        {
            bigTimerTotalValue = bigTimerHrsValue * 60 * 60 + bigTimerMinsValue * 60 + bigTimerSecsValue;
        }
        else
        {
            newUserWindowManager.OpenFirstTab();
            SetInfoTimerMessage("Remember to set main timer", Color.red);
            isInputCorrect = false;
        }

        string msg = "Remember to set user timer";
        int.TryParse(userTimerValueInputFieldHrs.inputText.text, out userTimerHrsValue);
        int.TryParse(userTimerValueInputFieldMins.inputText.text, out userTimerMinsValue);
        int.TryParse(userTimerValueInputFieldSecs.inputText.text, out userTimerSecsValue);

        if (userTimerHrsValue > 0 || userTimerMinsValue > 0 || userTimerSecsValue > 0)
        {
            userTimerTotalValue = userTimerHrsValue * 60 * 60 + userTimerMinsValue * 60 + userTimerSecsValue;
            if (isInputCorrect)
                SetInfoTimerMessage("Set main timer and user timer", Color.white);
        }
        else
        {
            newUserWindowManager.OpenFirstTab();
            if (!isInputCorrect)
                msg = "Remember to set both timers";
            SetInfoTimerMessage(msg, Color.red);
            isInputCorrect = false;
        }


        if (usersList.Count <= 0)
        {
            SetInfoUsersMessage("Minimum users: 1", Color.red);
            isInputCorrect = false;
        }

        if (!isInputCorrect)
            return;


        if (bigTimerTotalValue > 0)
            PlayerPrefs.SetInt("bigTimerValue", bigTimerTotalValue);

        if (userTimerTotalValue > 0)
        {
            if (userTimerTotalValue != PlayerPrefs.GetInt("userTimerStartingValue"))
                PlayerPrefs.SetInt("userTimerChanged", 1);
            else
                PlayerPrefs.SetInt("userTimerChanged", 0);
            PlayerPrefs.SetInt("userTimerStartingValue", userTimerTotalValue);
        }

        PlayerPrefs.SetInt("usersCount", usersList.Count);
        for (int i = 0; i < usersList.Count; i++)
        {
            PlayerPrefs.SetString("user" + i, usersList[i].userName);
        }

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void SetInfoTimerMessage(string message, Color color)
    {
        infoTextTimers.text = message;
        infoTextTimers.color = color;
    }

    public void SetInfoUsersMessage(string message, Color color)
    {
        infoTextUsers.text = message;
        infoTextUsers.color = color;
    }

    public void FormatNewUserList()
    {
        int index = 1;
        foreach (Transform newUserPanel in usersListContainer)
        {
            NewUserName newUserName = newUserPanel.GetComponent<NewUserName>();
            newUserName.userNameText.text = index + ") " + newUserName.userName;
            index++;
        }
    }

    public void SwitchPage()
    {
        newUserWindowManager.NextWindow();
    }
}
