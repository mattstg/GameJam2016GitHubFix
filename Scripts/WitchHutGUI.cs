using UnityEngine;
using System.Collections;

public class WitchHutGUI : MonoBehaviour {

    public void GoToRitualPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("RitualScene");
    }
}
