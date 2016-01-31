using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Listener  {
    Dictionary<string, float> valueDifference; //Stores percent difference since last value
    enum mood { dire, bad, okay, good, great }

    public Listener()
    {
        valueDifference = new Dictionary<string, float>();
    }

    public void RecordString()
    {

    }

    public void RecordValue(string name, float oldValue, float newValue)
    {
        valueDifference.Add(name, newValue / oldValue);
    }

    public void RecordInitialBiomeHp(string name, float value)
    {
        try
        {
            valueDifference.Add(name, value);
        }
        catch
        {
            Debug.Log("exception for: " + name + " v: " + value);
        }
    }

    public void RecordFinalBiomeHp(string name, float value)
    {
        valueDifference[name] = value / valueDifference[name];
    }

    public List<string> GetAllSignificantUpdates()
    {
        
        List<string> toReturn = new List<string>();
        foreach (KeyValuePair<string, float> kv in valueDifference)
        {
            string relation = (kv.Value < .75f)?"dropped":(kv.Value > 1.25f)?"increased":""; 
            if(relation != "")
            {
                toReturn.Add(kv.Key + " has " + relation + " to " + kv.Value*100 + " % of it's value");
            } 
        }
        return toReturn;
    }

    public void ClearList()
    {
        valueDifference.Clear();
    }
}
