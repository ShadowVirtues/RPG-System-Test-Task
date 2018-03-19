using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum St { Health, Attack, Dodge, Speed, Armor, MRes }    //Stat names

public class Stats  //Class-container for all stats. In a full game, a character should get an instance of this class
{
    public const int Count = 6;    //Number of different stats

    private readonly Dictionary<St, Stat> stats;    //Dictionary with all the stats

    private readonly float[] defaultStatValues = { 4000, 1000, 10, 100, 15, 20 };   //Yeah

    public readonly Character character;    //Reference to actual character these stats are getting assigned to

    //Constructor fills all the UI Text fields, where values get output + the character these stats are getting assigned to
    public Stats(Text[] values, Text[] levels, Button[] lvlUpButtons, Character character)
    {
        this.character = character;

        stats = new Dictionary<St, Stat>(Count);    //Initializing dictionary

        for (int i = 0; i < Count; i++) //For the whole number of stats
        {
            //Create an instance of each single stat by initializing it with its constructor, and fill it into dictionary
            stats[(St)i] = new Stat(defaultStatValues[i], values[i], levels[i],lvlUpButtons[i], this);    
        }

    }

    public Stat this[St index]  //Indexer to be able to reference some stat from outside code (index is an enum element)
    {
        get { return stats[index]; }    //Return/fill the dictionary with a stat key
        set { stats[index] = value; }
    }

    //Function that checks if there are points available or not, to disable/enable lvlUp buttons
    public void CheckAvailablePoints()  //Gets invoked from 'Stat' class when leveling up
    {
        bool enable = character.valueUnspentPoints.Value != 0; //Create a bool from the unspent points value
      
        for (int i = 0; i < Count; i++)
        {
            stats[(St)i].lvlUpButton.interactable = enable;  //Enable/disable all buttons, depending on the bool value
        }

    }

    public void ResetPoints()   //Gets called from Character script (when pressing Reset Points button)
    {
        for (int i = 0; i < Count; i++)
        {
            stats[(St)i].SetLevel(0);   //Yeah
        }
    }

    public void LevelUp()   //Gets called from 'Stat' script to decrease/increase unspent/spent points value
    {
        //Would probably be nicer to do this directly in 'Stat' script, but exposing 'character' to 'Stat' class, on the contrary, wouldn't be nice D:
        character.valueUnspentPoints.Value--;
        character.valueSpentPoints.Value++;
    }

}

public class Stat   //Class representin a single stat
{
    private readonly Stats stats;   //Reference to 'big' stats container, to be able to run its functions

    private readonly float defaultValue;    //Default stat value

    public float Value { get; private set; }    //Actual stat value (outside code can only 'get' it, 'set'ting it gets performed purely from leveling up)
    private readonly Text valueText;            //UI Text representing the stat value
    private readonly Text valueLevel;           //UI Text representing the stat level value

    private int _level; //I hate those, but you can't make a non-autoproperty without them D:
    public int Level
    {
        get { return _level; }
        private set     //Setter that also sets UI Text values, when changing the actual stat value
        {
            _level = value; //So, change the internal level value
            Value = defaultValue * (1 + 0.2f * _level); //Set the actual stat value

            valueText.text = $"{Value} ({defaultValue}+{defaultValue * (0.2f * _level)})"; //Set UI Text of stat value to actual stat value + the hint ('base+bonus' value)
            valueLevel.text = _level.ToString();    //Also set UI Text of stat level value

            stats.CheckAvailablePoints();   //After setting everything, check if there are available points, and disable all lvlUp buttons if not
        }
        
    }

    public readonly Button lvlUpButton; //LvlUp Button that corresponds this stat. Public, because we need it in 'Stats' class

    //Constructor that takes all the stuff from big 'Stats' class + reference to same actual 'Stats' class
    public Stat(float defaultValue, Text valueText, Text valueLevel, Button lvlUpButton, Stats stats) 
    {
        this.defaultValue = defaultValue;
        this.valueText = valueText;
        this.valueLevel = valueLevel;   //Filling all the dependencies
        this.lvlUpButton = lvlUpButton;  
        this.stats = stats;

        this.lvlUpButton.onClick.AddListener(LevelUp);  //Adding listener to the button onClick event
    }

    private void LevelUp()   //This function is bound to the lvlUp button
    {
        stats.LevelUp();    //Increase/decrease unspent/spent points
        Level++;            //Increment the level, that by means of it being a 'property', changes all the needed stuff
    }

    public void SetLevel(int level) => Level = level;   //Setting the level manually (when starting the game)

}

public class ValueText  //Small class that just binds the value to UI Text
{
    private Text UIText;    //UI Text to bind value to

    private int _value;    //Hate these :)
    public int Value
    {
        get { return _value; }
        set
        {
            _value = value;    //When setting the property, also UI Text on screen gets set
            UIText.text = _value.ToString();
        }
    }

    public ValueText(Text text)   //Constructor assigns the UI Text to 'ValueText' instance
    {
        UIText = text;
    }
}