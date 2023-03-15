using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static string FormattedBigTimerString(int timeInSeconds)
    {
        string formattedBigTimerString = "";
        string hours;
        string minutes;
        string seconds;
        bool formatNeedsMinutes;
        bool formatNeedsHours;

        seconds = (timeInSeconds % 60).ToString("00");
        formatNeedsMinutes = Mathf.Floor(timeInSeconds / 60) > 0;
        formatNeedsHours = (timeInSeconds / 3600) > 0;

        if (formatNeedsMinutes)
        {
            if (!formatNeedsHours)
                minutes = Mathf.Floor(timeInSeconds / 60).ToString("00");
            else
            {
                minutes = (Mathf.Floor(timeInSeconds / 60) % 60).ToString("00");
                hours = Mathf.Floor((timeInSeconds / 60) / 60).ToString("00");
                formattedBigTimerString += hours + " : ";
            }
            formattedBigTimerString += minutes + " : ";
        }
        
        formattedBigTimerString += seconds;
        return formattedBigTimerString;
    }
}
