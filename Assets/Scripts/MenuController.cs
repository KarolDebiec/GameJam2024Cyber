using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public List<GameObject> runtimeUIElements;
    public GameObject menu;
    public void ShowMenu()
    {
        foreach(GameObject element in runtimeUIElements)
        {
            element.SetActive(false);
        }
        menu.SetActive(true);
    }
    public void HideMenu()
    {
        foreach (GameObject element in runtimeUIElements)
        {
            element.SetActive(true);
        }
        menu.SetActive(false);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}
