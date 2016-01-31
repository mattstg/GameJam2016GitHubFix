using UnityEngine;
using System.Collections;

public class PhysicalIngredient : MonoBehaviour {

	//Ingredient carried by which
    Globals.product ingredient;

    public void InitializeIngredient(Globals.product _ingredient)
    {
        ingredient = _ingredient;
        SetIngredientGraphic();
    }

    private void SetIngredientGraphic()
    {
        Sprite toLoad = Resources.Load<Sprite>(ingredient.ToString());
        if (toLoad)
            GetComponent<SpriteRenderer>().sprite = toLoad;
        else
        {
            //Debug.LogError("Sprite " + ingredient.ToString() + " not found");
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("DefaultIngr");
        }
    }

    public Globals.product getIngredient()
    {
        return ingredient;
    }

    public void Dropped()
    {
        RaycastHit2D[] allHit = Physics2D.RaycastAll(transform.position, -Vector2.up,0);
        foreach(RaycastHit2D hit in allHit)
        {
            if (hit.transform.CompareTag("Cauldron"))
            {
                hit.transform.gameObject.GetComponent<Cauldron>().AddIngredient(ingredient);
                Destroy(gameObject);
                break;
            }
        }
    }
}
