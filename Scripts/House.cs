using UnityEngine;
using System.Collections;

public class House : MonoBehaviour {

    float hp = 1;

    public void InteractWithEvent(Globals.energySubTypes est, float power)
    {
        hp += EventEffectsMatrix.Instance.GetEventAndHouseMultiplier(est);
        Globals.limit(ref hp,0,1);
        if (hp == 0)
        {
            GameObject.FindObjectOfType<VillageCenter>().DestroyHouse(transform.position);
        }
    }
}
