using UnityEngine;
using System.Collections;

public class TitlePageScript : MonoBehaviour {
    public GameObject persistentScript;
    public void StartGame()
    {
        DontDestroyOnLoad(persistentScript);
        UnityEngine.SceneManagement.SceneManager.LoadScene("VillageMap");
    }
}
