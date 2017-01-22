﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour {
    public string levelName;

    public void LoadLevel() {
        SceneManager.LoadScene(levelName);
    }
}
