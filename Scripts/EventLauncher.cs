using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventLauncher : MonoBehaviour {
    //When a spell is loaded, EventLauncher will become active and ready to detect clicks
    Dictionary<Globals.energyTypes, float> energyStored;

    public void LoadEvent(Dictionary<Globals.energyTypes,float> _temp)
    {
        energyStored = new Dictionary<Globals.energyTypes, float>();
        foreach (KeyValuePair<Globals.energyTypes, float> kv in _temp)
        {
            energyStored.Add(kv.Key,kv.Value);
        }
       // Debug.LogError("LoadEvent called, current contents: " + Globals.PrintDictionary<Globals.energyTypes, float>(energyStored, "energyStored"));
    }

    public void LaunchEvent(Vector2 mousePos)
    {
        EventFactory.Instance.CreateEvent(mousePos,energyStored);
        //then position it at the mouse
        Destroy(this.gameObject);

    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
            LaunchEvent(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    /*
     * power = 10f;
        scale = 4f;
        directionVector = new Vector2(Random.Range(-10, 10),Random.Range(-10, 10));
        speed = 5f;
     * */
}
