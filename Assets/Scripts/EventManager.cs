using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit(); 
    }
    public void Reload()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("PlayGround");
    }
}
