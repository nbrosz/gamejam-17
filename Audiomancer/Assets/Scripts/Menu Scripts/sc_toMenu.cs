using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sc_toMenu : MonoBehaviour
{
    public Button btn_toMenu;

    // Use this for initialization
    void Start()
    {
        Button main = btn_toMenu.GetComponent<Button>();
        main.onClick.AddListener(RestartGame);
    }

    // Start Game
    void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }

    // Update is called once per frame
    void Update()
    {

    }
}