using UnityEngine;
using System.Collections;

public class PhysicalBiome : MonoBehaviour {

    public Globals.biome bioType;
    float hp;

    public void LoadBiome()
    {
        hp = GameObject.FindObjectOfType<VillageCenter>().GetBiomeHp((int)bioType);
    }

    public void InteractWithEvent(Globals.energySubTypes est, float power)
    {
        float effectMultiplier = EventEffectsMatrix.Instance.GetEventAndBiomeMultiplier(est, bioType);
        Debug.Log("Biome going to take: " + (power * effectMultiplier) + "dmg");
        GameObject.FindObjectOfType<VillageCenter>().BiomeModHp((int)bioType,power*effectMultiplier);
    }


}
