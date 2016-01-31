using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//magic values everyhwere, times running low

public class EventBrain : MonoBehaviour {
    enum stormType{Fire,Water};
    enum critterType{Cow = 1,Wolf = 2,Frogs = 3,Insect = 4};
    enum auraType{Light,Dark};
	//going to have set/get for radius and vector
    List<Transform> objectsToIgnore;
	public Vector2 directionVector; //vector which keeps track of movement direction and speed
	public float scale; //radius defining the sive of the collision box
	public float totalPower = 0; //power rating of Event
	public float speed;
    List<Globals.energySubTypes> activeSubTypes = new List<Globals.energySubTypes>();
    
    
	//power will effect radius and movementVector
	//decays over time

	// Use this for initialization
    
    public void Initialize(Dictionary<Globals.energySubTypes, float> initialEnergies)
    {
        foreach (Globals.energySubTypes subType in System.Enum.GetValues(typeof(Globals.energySubTypes))) //since used to fill all values, need to fill missing ones to zero or will crash
        {
            if (!initialEnergies.ContainsKey(subType))
                initialEnergies.Add(subType, 0);
        }
        foreach (KeyValuePair<Globals.energySubTypes, float> kv in initialEnergies) //since used to fill all values, need to fill missing ones to zero or will crash
        {
            totalPower += kv.Value;
        }

        if (initialEnergies[Globals.energySubTypes.ether] > 0)
        {
            float powerMultiplier = 1 + initialEnergies[Globals.energySubTypes.ether]/15f;
            totalPower*= powerMultiplier;
        }
        else
            speed = (initialEnergies[Globals.energySubTypes.air] <= 0) ? 1f : initialEnergies[Globals.energySubTypes.air];

        // Globals.energySubTypes.beast; Globals.energySubTypes.critter; Globals.energySubTypes.dark; Globals.energySubTypes.ether; Globals.energySubTypes.fire; Globals.energySubTypes.light; Globals.energySubTypes.water;
        
        
        if (initialEnergies[Globals.energySubTypes.fire] >= 4f)
            CreateStorm(stormType.Fire);
        else if (initialEnergies[Globals.energySubTypes.water] >= 4f)
            CreateStorm(stormType.Water);

        if (initialEnergies[Globals.energySubTypes.light] >= 4f)
            CreateAuraEvent(auraType.Light);
        else if (initialEnergies[Globals.energySubTypes.dark] >= 4f)
            CreateAuraEvent(auraType.Dark);

        if (initialEnergies[Globals.energySubTypes.critter] >= 15f)
            CreateCritterEvent(critterType.Insect);
        else if (initialEnergies[Globals.energySubTypes.critter] >= 4f)
            CreateCritterEvent(critterType.Frogs);        
        else if (initialEnergies[Globals.energySubTypes.beast] >= 15f)
            CreateCritterEvent(critterType.Wolf);
        else if (initialEnergies[Globals.energySubTypes.beast] >= 4f)
            CreateCritterEvent(critterType.Cow);

        scale = totalPower/15;
        scale = (scale < .25f) ? .25f : scale;
        directionVector = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
    }

    public void Initialize(float pow, float spd, float scle, Vector2 dirVec)
    {
		totalPower = pow;
		speed = spd;
		scale = scle;
		directionVector = dirVec;
	}
	
	// Update is called once per frame
	void Update () {
		//refresh power, some decay or gain
		if (totalPower < Globals.minimumPower) {
			Destroy(gameObject);
		}
		refreshPower();
		//need to refreshRadius
		//and with new radius scale sprite and scale radius of circle collider
		//refreshSize();
		refreshScale ();

		//then need to refresh movementVector
		refreshDirectionVector();
		//Debug.Log ("movementVector = " + directionVector);

		//then need to apply movementVector with Delta Time
		moveEvent();
		}

	public void refreshScale (){
        
		scale *= (1 - (Globals.eventDecay * Time.deltaTime));
		//GetComponent<Transform>().localScale = new Vector3(scale,scale,scale);
		transform.localScale = new Vector3(scale,scale,scale);
	}

    private void CreateCritterEvent(critterType _critterType)
    {
        float crittersToMake = (int)_critterType*totalPower / 10f;
        crittersToMake = (crittersToMake < 1) ? 1 : crittersToMake;
        for (int i = 0; i < crittersToMake; ++i)
        {
            GameObject go = Instantiate(Resources.Load("SpellEvents/CritterEvent"), this.transform.position, Quaternion.identity) as GameObject;
            go.transform.SetParent(this.transform);
            go.GetComponent<SpriteRenderer>().sortingOrder = i;
            go.transform.localScale /= (int)_critterType;
            go.transform.position = Globals.AddVec(go.transform.position,new Vector2(Random.Range(-.1f, .1f), Random.Range(-.1f, .1f)));
        }

        switch (_critterType)
        {
            case critterType.Insect:
                CreateInsectEvent();
                break;
            case critterType.Frogs:
                CreateFrogEvent();
                break;
            case critterType.Cow:
                CreateCowEvent();
                break;
            case critterType.Wolf:
                CreateWolfEvent();
                break;
        }
    }

    private void CreateInsectEvent()
    {
        activeSubTypes.Add(Globals.energySubTypes.critter);
        Critters[] critters = GetComponentsInChildren<Critters>();
        foreach (Critters s in critters)
        {
            s.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        }
    }

    private void CreateFrogEvent()
    {
        activeSubTypes.Add(Globals.energySubTypes.critter);
        Critters[] critters = GetComponentsInChildren<Critters>();
        foreach (Critters s in critters)
        {
            s.GetComponent<SpriteRenderer>().color = new Color(0, .75f, 0);
        }
    }

    private void CreateCowEvent()
    {
        activeSubTypes.Add(Globals.energySubTypes.beast);
        Critters[] critters = GetComponentsInChildren<Critters>();
        foreach (Critters s in critters)
        {
            s.GetComponent<SpriteRenderer>().color = new Color(.5f, .2f, 0);
        }
    }

    private void CreateWolfEvent()
    {
        activeSubTypes.Add(Globals.energySubTypes.beast);
        Critters[] critters = GetComponentsInChildren<Critters>();
        foreach (Critters s in critters)
        {
            float randGrey = Random.Range(.40f, .60f);
            s.GetComponent<SpriteRenderer>().color = new Color(randGrey, randGrey, randGrey);
        }
    }

    private void CreateAuraEvent(auraType _auraType)
    {
        
        int kids = (int)(totalPower / 4);
        GameObject go = Instantiate(Resources.Load("SpellEvents/AuraEvent"), this.transform.position, Quaternion.identity) as GameObject;
        go.transform.SetParent(this.transform);
        go.transform.localPosition = new Vector2(0, 0);
        go.GetComponent<AuraEvent>().ParentSubOrbs(kids);

        if (_auraType == auraType.Light)
            CreateLightEvent();
        else
            CreateDarkEvent();
    }

    private void CreateLightEvent()
    {
        activeSubTypes.Add(Globals.energySubTypes.light);
    }
    private void CreateDarkEvent()
    {
        activeSubTypes.Add(Globals.energySubTypes.dark);
        GetComponentInChildren<AuraEvent>().ColorKidsBlack();
    }

    private void CreateStorm(stormType _stormType)
    {
        float stormsToMake = totalPower/10f;
        stormsToMake = (stormsToMake<1)?1:stormsToMake;
        for(int i = 0; i < stormsToMake; ++i)
        {
            GameObject go = Instantiate(Resources.Load("SpellEvents/Storm"),this.transform.position,Quaternion.identity) as GameObject;
            go.transform.SetParent(this.transform);
            go.GetComponent<SpriteRenderer>().sortingOrder = i;
        }

        if(_stormType == stormType.Fire)
            CreateFireStorm();
        else
            CreateWaterStorm();
    }

    private void CreateFireStorm()
    {
        activeSubTypes.Add(Globals.energySubTypes.fire);
        Storm[] storms = GetComponentsInChildren<Storm>();
        foreach (Storm s in storms)
        {
            s.GetComponent<SpriteRenderer>().color = new Color(1, Random.Range(0, .33f), 0);
        }
        /*float stormsToMake = power/20f;   //when fix fire bug
        stormsToMake = (stormsToMake<1)?1:stormsToMake;
        for(int i = 0; i < stormsToMake; ++i)
        {
            GameObject go = Instantiate(Resources.Load("SpellEvents/FireParticle"), this.transform.position, Quaternion.identity) as GameObject;
            go.transform.SetParent(this.transform);
        }*/
    }


    private void CreateWaterStorm()
    {
        activeSubTypes.Add(Globals.energySubTypes.water);
        Storm[] storms = GetComponentsInChildren<Storm>();
        foreach (Storm s in storms)
        {
            s.GetComponent<SpriteRenderer>().color = new Color(Random.Range(.55f, .1f), 1, 1);
        }
        /*float stormsToMake = power/20f;   //when fix fire bug
        stormsToMake = (stormsToMake<1)?1:stormsToMake;
        for(int i = 0; i < stormsToMake; ++i)
        {
            GameObject go = Instantiate(Resources.Load("SpellEvents/SnowParticle"), this.transform.position, Quaternion.identity) as GameObject;
            go.transform.SetParent(this.transform);
        }*/
    }

	public void moveEvent(){
		GetComponent<Rigidbody2D>().MovePosition(Globals.AddVec(transform.position, directionVector * Time.deltaTime));
	}

	public void refreshPower(){
		//some decay per update, probably %loss until dissolve at some base power.
		totalPower *= (1 - (Globals.eventDecay * Time.deltaTime));
	}

	public float randomPosFloat(float multiplier){
		return (Random.Range(1, 10) * multiplier);
	}
		
	public void refreshDirectionVector(){
		/// NEEDS TO BE MORE RANDOM ///
		//going to alter direction vector slightly based on random variables
		directionVector.x += Random.Range(-Globals.eventSwerveVariance,Globals.eventSwerveVariance);
		directionVector.y += Random.Range(-Globals.eventSwerveVariance,Globals.eventSwerveVariance);
		directionVector.Normalize ();
	}

	public void refreshSpeed(){
		speed *= (1 - (Globals.eventDecay * Time.deltaTime));
	}

    public void OnTriggerStay2D(Collider2D coli)
    {
        if (coli.CompareTag("Villager"))
        {
            Villager v = coli.GetComponent<Villager>();
            foreach (Globals.energySubTypes est in activeSubTypes)
                v.InteractWithEvent(est, Time.deltaTime*totalPower / activeSubTypes.Count);
        }
        if (coli.CompareTag("Biome"))
        {
            PhysicalBiome b = coli.GetComponent<PhysicalBiome>();
            foreach (Globals.energySubTypes est in activeSubTypes)
                b.InteractWithEvent(est, Time.deltaTime * totalPower / activeSubTypes.Count);
        }
        if (coli.CompareTag("House"))
        {
            House b = coli.GetComponent<House>();
            foreach (Globals.energySubTypes est in activeSubTypes)
                b.InteractWithEvent(est, Time.deltaTime * totalPower / activeSubTypes.Count);
        }
    }

   
}