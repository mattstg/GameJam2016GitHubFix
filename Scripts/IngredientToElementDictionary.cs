using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class IngredientToElementDictionary  {

	 #region singleton
    private static IngredientToElementDictionary instance;

    private IngredientToElementDictionary() { RandomizeDictionary(); }

    public static IngredientToElementDictionary Instance
   {
      get 
      {
         if (instance == null)
         {
             instance = new IngredientToElementDictionary();
         }
         return instance;
      }
   }
    #endregion

    Dictionary<Globals.product, List<Element>> ingToElemDict = new Dictionary<Globals.product, List<Element>>();

    public void RandomizeDictionary()
    {
        foreach (Globals.product ingr in System.Enum.GetValues(typeof(Globals.product)))
        {
            ingToElemDict.Add(ingr, CreateElementList());
        }

        //foreach (KeyValuePair<Globals.product, List<Element>> kv in ingToElemDict)
        //    foreach (Element e in kv.Value)
         //       Debug.Log(kv.Key + " has element[P,A,E]  [" + e.power + "," + e.attributeNumber + "," + e.activeEvent + "]");
    }

    public List<Element> ElementsFromIngredient(Globals.product ingredient)
    {
        return ingToElemDict[ingredient];
    }

    private List<Element> CreateElementList()
    {
        List<Element> toReturn = new List<Element>();
        int numOfElems1 = Random.Range(1, Globals.MaxElementsPerIngredient+1);
        int numOfElems2 = Random.Range(1, Globals.MaxElementsPerIngredient + 1);
        int numberOfElems = (numOfElems1 <= numOfElems2) ? numOfElems1 : numOfElems2; //weighted random for smaller value
        for (int i = 0; i < numberOfElems; ++i)
        {
            toReturn.Add(CreateRandomElement());
        }
        return toReturn;
    }

    private Element CreateRandomElement()
    {        
        return new Element(Random.Range(-4, 5), (Globals.energyTypes)Random.Range(0, (int)4));
    }
}
