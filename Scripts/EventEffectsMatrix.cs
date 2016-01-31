using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventEffectsMatrix : MonoBehaviour {

    #region singleton
    private static EventEffectsMatrix instance;

    private EventEffectsMatrix() { }

    public static EventEffectsMatrix Instance
   {
      get 
      {
         if (instance == null)
         {
             instance = new EventEffectsMatrix();
         }
         return instance;
      }
   }
    #endregion

    //Rows:  energySubTypes { fire = 2, water = 3,dark = 4,light = 5, critter = 6, beast = 7};
    //Cols: Forest = 0, Bog = 1,  Farmland = 2, Ranch = 3, Lake = 4, Mountain = 5
    
    const float l = -.01f;
    const float m = -.02f;
    const float h = -.05f;

    const float g = .01f;
    const float gg = .02f;
    const float ggg = .05f;

                                         //b0  //b1  //b2 //b3 //b4 //b5 
    float[,] eventAndBiomeMatrix = {    { h,   m,     m,   m,   0,   l   }, //e2
                                        {gg,   g,   ggg,   l, ggg,   m   }, //e3
                                        {m,   gg,     m,   h,   l,   l   },  //e4
                                        {ggg,  m,     g,   g,   g,   0   },  //e5
                                        {  g,  g,     h,   l, ggg,   0   },  //e6
                                        {  l,  0,     h,   h,   0,   l   }   //e7

                                   };

    Dictionary<int, float[]> eventAndPeopleMatrix = new Dictionary<int, float[]>() 
    {               //hp,hap,health
        {2,new float[3]{h,h,h}},
        {3,new float[3]{0,g,l}},
        {4,new float[3]{l,h,h}},
        {5,new float[3]{ggg,gg,ggg}},
        {6,new float[3]{l,m,h}},
        {7,new float[3]{h,l,0}}
    };
                                  //e2 e3 e4 e5 e6 e7
    float[] eventAndHouseMatrix = { h, l, 0, 0, 0, 0 };

    public float[] GetEventAndVillagerMultiplier(Globals.energySubTypes subType)
    {
        return eventAndPeopleMatrix[(int)subType];
    }

    public float GetEventAndBiomeMultiplier(Globals.energySubTypes subType,Globals.biome bioType)
    {
        float test = eventAndBiomeMatrix[(int)subType-2, (int)bioType];
        Debug.Log("test: " + test);
        return test;
    }

    public float GetEventAndHouseMultiplier(Globals.energySubTypes subType)
    {
        return eventAndHouseMatrix[(int)subType - 2];
    }
	
}
