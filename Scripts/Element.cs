using UnityEngine;
using System.Collections;

public class Element {

    public int power;
    public Globals.energyTypes energyType;

    public Element(int _power, Globals.energyTypes _et)
    {
        power = _power;
        energyType = _et;
    }

    public override string ToString()
    {
        return "Element[p,et] = [" + power + "," + energyType + "]";
    }
   

}
