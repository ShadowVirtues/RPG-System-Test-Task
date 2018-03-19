using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] private string initialString;  //Initial string to get stats from (3,1,2:1,0,1,0,0,0)

    //=================================

    public ValueText valueLevel;
    public ValueText valueUnspentPoints;    //Values representing levels and points
    public ValueText valueSpentPoints;      //Made into custom classes to bind those to UI Text's

    [SerializeField] private Text textLevel;  
    [SerializeField] private Text textUnspentPoints;    //Yeah, binding previous values to these UI Text's
    [SerializeField] private Text textSpentPoints;
    
    [SerializeField] private Text[] textsStatValue;
    [SerializeField] private Text[] textsStatLevel;     //Arrays of different stat UI elements of the scene, to easily assign them from Inspector
    [SerializeField] private Button[] buttonsLevelUp;

    public Stats stats;     //Stats of THIS character

    private void InitValues()   //Initializing all the custom classes
    {
        valueLevel = new ValueText(textLevel);
        valueUnspentPoints = new ValueText(textUnspentPoints);  //Bind values to UI Text's
        valueSpentPoints = new ValueText(textSpentPoints);

        //Constructing a 'Stats' instance for this character, passing the actual character reference in as well (this)
        stats = new Stats(textsStatValue, textsStatLevel, buttonsLevelUp, this);    

    }

    void Start()
    {
        InitValues();

        //=========Parsing Initial String==========

        int colonPos = initialString.IndexOf(":");  //Get the colon position in a string

        string levelsString = initialString.Substring(0, colonPos);     //Subdivide the string into two
        string statLevelsString = initialString.Substring(colonPos + 1);

        Debug.Log($"{levelsString}    {statLevelsString}"); //Test that we actually did it properly :D

        string[] subLevels = levelsString.Split(',');           //Get the values 'in-between commas'
        string[] subStatLevels = statLevelsString.Split(',');

        valueLevel.Value = int.Parse(subLevels[0]);
        valueUnspentPoints.Value = int.Parse(subLevels[1]); //Setting character level variables
        valueSpentPoints.Value = int.Parse(subLevels[2]);

        for (int i = 0; i < Stats.Count; i++)   //Setting stat levels depending on initial string values
        {
            stats[(St)i].SetLevel(int.Parse(subStatLevels[i]));     //Setting all the UI Text's on screen is included in this function
        }
        
    }

    public void ResetPoints()   //Bound to UI Button
    {
        valueUnspentPoints.Value = valueLevel.Value;    //Unspent points equals to level number when resetting
        valueSpentPoints.Value = 0;                     //No spent points
        stats.ResetPoints();                            //Reset all the stat levels to 0
    }

    public void LevelUp()   //Bound to UI Button
    {
        valueLevel.Value++;             //Increment level value
        valueUnspentPoints.Value++;     //Increment unspent points value
        stats.CheckAvailablePoints();   //This will basically enable the stat lvlUp buttons if they were disabled
    }












}
