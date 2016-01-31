using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldLoader : MonoBehaviour {

    List<Vector2> houseTransforms = new List<Vector2>();

    public void LoadAll(Population pop,float curHouses)
    {
        LoadAllHouses(curHouses);
        LoadAllPeople(pop);
        LoadAllBiomes();

    }

    private void LoadAllHouses(float amt)
    {
        foreach (Vector2 spot in houseTransforms)
            Instantiate(Resources.Load("House"), spot, Quaternion.identity);

        float errorCounts = 0;
        float extraToCreate = amt - houseTransforms.Count;
        if (extraToCreate > 0)
        {
            Vector2 creationSpot; 
            RaycastHit2D[] allLeft;
            RaycastHit2D[] allRight;
            bool success = false;

            for (int i = 0; i <extraToCreate; ++i)
            {
                do
                {
                    creationSpot = new Vector2(Random.Range(-Globals.mapRadiusXforHousePlacement, Globals.mapRadiusXforHousePlacement), Random.Range(-Globals.mapRadiusYforHousePlacement, Globals.mapRadiusYforHousePlacement));
                    allLeft = Physics2D.RaycastAll(creationSpot - new Vector2(-.2f, -.5f), -Vector2.up, 1f);
                    allRight = Physics2D.RaycastAll(creationSpot - new Vector2(-.2f, -.5f), -Vector2.up, 1f);
                    if (allLeft.Length > 0 || allRight.Length > 0)
                    {
                        success = false;
                        errorCounts++;
                    }
                    else
                    {
                        success = true;
                        GameObject go = Instantiate(Resources.Load("House"), creationSpot, Quaternion.identity) as GameObject;
                        houseTransforms.Add(go.transform.position);
                    }
                    if (errorCounts > 8)
                    {
                        Debug.LogError("Surrendered house builds");
                        break; //it'll build the house next iteration
                    }

                } while (!success);
            }
        }
    }

    private void LoadAllPeople(Population pop)
    {
        float errorCounter = 0;
        float totalError = 0;
        bool recklessPlacement = false;
        
        for (int i = 0; i < pop.currentPopulation; ++i)
        {
            Vector2 creationSpot = new Vector2(Random.Range(-Globals.mapRadiusX, Globals.mapRadiusX), Random.Range(-Globals.mapRadiusY, Globals.mapRadiusY));
            RaycastHit2D[] allHit = Physics2D.RaycastAll(creationSpot, -Vector2.up,0);
            if (allHit.Length == 0 || recklessPlacement)
            {
                GameObject go = Instantiate(Resources.Load("Villager"), creationSpot, Quaternion.identity) as GameObject;
                go.GetComponent<Villager>().Initialize(pop.averagePercentLifePoints, pop.averageHappiness, pop.averageHealthiness);
            } else {
                errorCounter++;
                totalError++;
                if (errorCounter > 10) //too much wait, that vilagers toast, next one
                {
                    errorCounter = 0;
                    ++i;
                }
                if (totalError > 40)  //too much thrashing, just place them ontop other objects and hope unity forces them out
                {
                    recklessPlacement = true;
                }                
            }            
        }        
    }

    private void LoadAllBiomes()
    {
        PhysicalBiome[] go = GameObject.FindObjectsOfType<PhysicalBiome>();
        foreach (PhysicalBiome b in go)
            b.LoadBiome();
    }

    private void CreateHuman()
    {

    }

    public void RemoveHouse(Vector2 houseToRemove)
    {
        houseTransforms.Remove(houseToRemove);
    }
    
}
