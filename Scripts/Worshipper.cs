using UnityEngine;
using System.Collections;

public class Worshipper : MonoBehaviour {
    Globals.worshipperStates currentState;
    public float currentDegree = 0;
    public float rotationSpeed = 5f;
    public float distanceFromCenter = 0;

    public void Initialize(float _currentDegree, float _distanceFromCenter,Globals.worshipperStates startState)
    {
        distanceFromCenter = _distanceFromCenter;
        currentDegree = _currentDegree;
        currentState = startState;
    }

    public void Update()
    {
        float deltaTime = Time.deltaTime;
        switch (currentState)
        {
            case Globals.worshipperStates.Dance:
                MoveAroundCircle(deltaTime);
                break;
            case Globals.worshipperStates.Chant:
                break;
            default:
                Debug.LogError("Not handled worshipper state: " + currentState);
                break;
        }
    }

    public void MoveAroundCircle(float deltaTime)
    {
        currentDegree += rotationSpeed * deltaTime;
        transform.position = new Vector2(distanceFromCenter * Mathf.Cos(Mathf.PI * currentDegree / 180), distanceFromCenter * Mathf.Sin(Mathf.PI * currentDegree / 180));
    }

    public void ChangeState()
    {
        int curState = (int)currentState;
        curState = (curState + 1) % (int)Globals.worshipperStates.COUNT;
        currentState = (Globals.worshipperStates)curState;
    }
	
}
