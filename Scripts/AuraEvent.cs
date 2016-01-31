using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AuraEvent : MonoBehaviour {

    Transform parentTransform;
    bool isChildOrb = false;
    List<Vector2> localPoints;
    int i = 0;
    float speed;

    public void ParentSubOrbs(float amount)
    {
        for(int i = 0; i < amount; ++i)
        {
            GameObject go = Instantiate(Resources.Load("SpellEvents/AuraEvent"),transform.position,Quaternion.identity) as GameObject;
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector2(0,0);
            go.transform.localScale = new Vector2(.25f, .25f);
            go.GetComponent<AuraEvent>().SetAsChild(transform);
        }
    }

    public void SetAsChild(Transform auraParent)
    {
        parentTransform = auraParent;
        localPoints = new List<Vector2>();
        float rad = Random.Range(0, 2 * Mathf.PI);
        localPoints.Add(new Vector2(1.5f*Mathf.Cos(rad), 1.5f*Mathf.Sin(rad)));
        localPoints.Add(new Vector2(Mathf.Cos(rad + Mathf.PI), Mathf.Sin(rad + Mathf.PI)));
        isChildOrb = true;
        speed = Random.Range(2, 5);
    }

    public void ColorKidsBlack()
    {
        SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer s in sr)
        {
            s.color = Color.black;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isChildOrb)
        {
            transform.position = Vector2.MoveTowards(transform.position, Globals.AddVec(localPoints[i],parentTransform.position), speed * Time.deltaTime);
            if (Globals.CompareVec(transform.position, Globals.AddVec(localPoints[i], parentTransform.position)))
            {
                i = (i + 1) % 2;
                GetComponent<SpriteRenderer>().sortingOrder = (i == 0)?0:2;
            }
        }
	}

}
