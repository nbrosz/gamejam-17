using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_Continue : MonoBehaviour {
    public GameObject btn_Cont;
    public GameObject btn_Quit;
    public GameObject btn_toMenu;

	// Use this for initialization
	void Start () {
        Button cont = btn_Cont.GetComponent<Button>();
        cont.onClick.AddListener(ReturnToGame);
	}
	
    //Continue the Game
    void ReturnToGame()
    {
        Time.timeScale = 1;
        btn_Cont.SetActive(false);
        btn_Quit.SetActive(false);
        btn_toMenu.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
