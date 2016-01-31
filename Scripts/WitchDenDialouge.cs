using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
// just for fun
public class WitchDenDialouge : MonoBehaviour {
   /* public delegate void functionCall();
    struct EventTime
    {
        bool occured;
        float atTime;
        functionCall fc;
        public EventTime(float _atTime, functionCall myMethodName)
        {
            fc = myMethodName;
            occured = false;
            atTime = _atTime;
        }
    }*/
    List<string> statusReport;
    public Transform listener;
    public Text dialouge;
    public Transform dialougeImage;
    float totalTime = 0;
    float walkFoward = 2f;
    bool boxIsOpen = false;
    bool dialougeFinished = false;

	void Start () {
        Listener theListener = GameObject.FindObjectOfType<VillageCenter>().TheListener;
        statusReport = theListener.GetAllSignificantUpdates();
        Debug.Log("status report is " + statusReport.Count + " large");
        theListener.ClearList();
	}

    void Update()
    {
        if (!boxIsOpen)
        {
            totalTime += Time.deltaTime;
            if (totalTime < 3)
                listener.position = Globals.AddVec(listener.position, new Vector2(2 * Time.deltaTime, 0));
            else if (totalTime > 4)
            {
                CreateGuiBox();
                boxIsOpen = true;
            }
        }
        else if (boxIsOpen && !dialougeFinished)
        {
            if (Input.anyKeyDown)
            {
                KeyPressed();
            }
        }
        else if (dialougeFinished)
        {
            listener.position = Globals.AddVec(listener.position, new Vector2(-2 * Time.deltaTime, 0));
            if(listener.position.x < -8)
                dialougeImage.gameObject.SetActive(false);
        }

    }

    public void CreateGuiBox()
    {
        dialougeImage.gameObject.SetActive(true);
        if (statusReport.Count <= 0)
            dialouge.text = "It was a quiet night it would seem";
        else if (statusReport.Count >= 0 && statusReport.Count <= 4)
            dialouge.text = "There were some issues today I should warn you about";
        else if (statusReport.Count >= 4)
            dialouge.text = "There were alot of problems today! We need your aid";

    }

    public void KeyPressed()
    {
        if (statusReport.Count <= 0)
        {
            dialougeFinished = true;
            dialouge.text = "Nothing else to report, Thats all for now, thank you Felesmanka";
        }
        else
        {
            dialouge.text = statusReport[0];
            statusReport.RemoveAt(0);
        }
    }
}
