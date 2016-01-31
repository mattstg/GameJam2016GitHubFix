using UnityEngine;
using System.Collections;


public class CharacterController : MonoBehaviour {
    
    public float characterSpeed = 3f;
    public Vector2 fowardVector;
    public Cauldron cauldron;
    Rigidbody2D rigidbody2D;
    PhysicalIngredient physIngr;

    public void Start()
    {
        fowardVector = new Vector2(-1, 0);
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        float deltaTime = Time.deltaTime;
        
        if(Input.anyKey)
            HandleMovement(deltaTime);

        if (Input.GetKeyDown(KeyCode.F))
            if (physIngr)
                DropIngredient();
            else
                Interact();
        if (Input.GetKeyDown(KeyCode.Q))
            IssueChoirCommand();
	}

    void HandleMovement(float deltaTime)
    {
        Vector2 moveDir = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            moveDir += new Vector2(-characterSpeed * deltaTime, 0);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
             moveDir += new Vector2(0, characterSpeed * deltaTime);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
             moveDir += new Vector2(characterSpeed * deltaTime, 0);
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
             moveDir += new Vector2(0,-characterSpeed * deltaTime);
        if(moveDir != new Vector2(0,0))
         Move(moveDir);
    }

    void Move(Vector2 moveDir)
    {
        fowardVector = moveDir.normalized;
        Vector2 toMove = new Vector2(transform.position.x + moveDir.x, transform.position.y + moveDir.y);                      //to improve, add operator overload instead?
        rigidbody2D.MovePosition(toMove);
    }

    void DropIngredient()
    {
       physIngr.transform.SetParent(null);
       physIngr.transform.position = new Vector2(transform.position.x + fowardVector.x, transform.position.y + fowardVector.y);
       physIngr.GetComponent<BoxCollider2D>().enabled = true;
       physIngr.Dropped();
       physIngr = null;
    }

    void IssueChoirCommand()
    {
        RaycastHit2D[] allHit = Physics2D.RaycastAll(transform.position, -Vector2.up,0);
        foreach(RaycastHit2D hit in allHit)
            if (hit.transform.CompareTag("Choir"))
                 hit.transform.gameObject.GetComponent<PrayerCircle>().ChangeWorshipperState();
    }

    void Interact()
    {
        RaycastHit2D[] allHit = Physics2D.RaycastAll(new Vector2(transform.position.x + fowardVector.x,transform.position.y + fowardVector.y), -Vector2.up,0);
        if(allHit.Length == 0)
            allHit = Physics2D.RaycastAll(transform.position, -Vector2.up,0);
        foreach(RaycastHit2D hit in allHit)
        {
            if(hit.transform.CompareTag("Item"))
            {
                PickUpIngredient(hit.transform);
                break;
            }
        }
    }

    void PickUpIngredient(Transform ingredientTransform)
    {
        physIngr = ingredientTransform.GetComponent<PhysicalIngredient>();
        physIngr.GetComponent<BoxCollider2D>().enabled = false;
        physIngr.transform.SetParent(transform);
        physIngr.transform.localPosition = new Vector2(0, .16f);
    }

    
}
