using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrayerCircle : MonoBehaviour {

    public float prayerSpeed = 3f;
    List<Worshipper> worshippers;
    

    public void Initialize(int size)
    {
        worshippers = new List<Worshipper>();
        transform.localScale = new Vector2(8 + size * 4.5f, 8 + size * 4.5f);
        for (int i = 0; i < Globals.PeoplePerCircleLevel[size]; ++i)
        {
           GameObject go = Instantiate(Resources.Load("Worshipper"),new Vector2(transform.localScale.x/2,0),Quaternion.identity) as GameObject;
           go.transform.SetParent(transform);
           go.GetComponent<Worshipper>().Initialize(i * 360 / (Globals.PeoplePerCircleLevel[size]), transform.localScale.x / 2, Globals.worshipperStates.Dance);
           worshippers.Add(go.GetComponent<Worshipper>());
        }
        foreach (Worshipper w in worshippers)
            w.transform.localScale = new Vector3(1 / (8 + size * 4.5f), 1 / (8 + size * 4.5f), 1);
        gameObject.AddComponent<PolygonCollider2D>();
        gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
    }

    public void ChangeWorshipperState()
    {
        foreach (Worshipper w in worshippers)
            w.ChangeState();
    }
}

