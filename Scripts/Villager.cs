using UnityEngine;
using System.Collections;


public class Villager : MonoBehaviour {

    public float hp; //range from 0 - 1 (is multiplied by Globals.maximumVillagerHealthPoints)
    public float happiness; //range from 0 - 1
    public float healthiness; //range from 0 - 1
    public float speed = .5f;
    public float counter = 0;
    Vector2 moveDir = new Vector2(0, 0);


    public void Initialize(float _hp, float _happiness, float _healthiness)
    {
        hp = _hp + Random.Range(-Globals.PopulationMutationRate,Globals.PopulationMutationRate);
        happiness = _happiness + Random.Range(-Globals.PopulationMutationRate,Globals.PopulationMutationRate);
        healthiness = _healthiness + Random.Range(-Globals.PopulationMutationRate, Globals.PopulationMutationRate);
        hp = limit(hp);
        happiness = limit(happiness);
        healthiness = limit(healthiness); 
        float moveX = (Random.Range(-1f, 1f) * speed);
        float moveY = (Random.Range(-1f, 1f) * speed);
        moveDir = new Vector2(moveX, moveY);
    }

    public void Update()
    {
        float dt = Time.deltaTime;
        counter += dt;
        if (counter >= Globals.villageUpdateCounterMax - .5f)
        {
            float moveX = (Random.Range(-1f, 1f) * speed);
            float moveY = (Random.Range(-1f, 1f) * speed);
            moveDir = new Vector2(moveX, moveY);
            counter = 0;
        }
        Wander(dt);
        if (hp <= 0)
        {
            Debug.Log("Villager has died");
            GameObject.Destroy(this.gameObject);
        }
    }
    
    public void UpdateStats()
    {
        float dif = (hp + happiness + healthiness - (Globals.contentThreshold * 3)) * Globals.contentExcessMultiplier;
        //Debug.Log("Difference in stats will be: " + dif);
        hp += dif;
        happiness += dif;
        healthiness += dif;

        hp = limit(hp);
        happiness = limit(happiness);
        healthiness = limit(healthiness); 
    }

    public void InteractWithEvent(Globals.energySubTypes eventType, float power)
    {
        float[] multiplier = EventEffectsMatrix.Instance.GetEventAndVillagerMultiplier(eventType);
        hp += power * multiplier[0] * Globals.worldDamageReduction;
        happiness += power * multiplier[1] * Globals.worldDamageReduction;
        healthiness += power * multiplier[2] * Globals.worldDamageReduction;
        limit2(ref hp);
        limit2(ref happiness);
        limit2(ref healthiness);

        Debug.Log("Villager interacted with " + eventType + " event, [hp,hapiness,healthiness] = [ " + hp + "," + happiness + "," + healthiness + "]");
    }

    public void Wander(float dt)
    {
        GetComponent<Rigidbody2D>().MovePosition(Globals.AddVec(transform.position, moveDir * dt * speed)); ///why at center
    }

    private float limit(float value)
    {
        if (value < 0)
            return 0;
        if (value > 1)
            return 1;
        return value;
    }

    private void limit2(ref float value)
    {
        if (value < 0)
            value = 0;
        if (value > 1)
            value = 1;
    }
}
