using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagScript : MonoBehaviour
{

    Scene currentScene;
    int buildIndex;
    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        buildIndex = currentScene.buildIndex;
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.name == "player"){
            SceneManager.LoadScene(buildIndex + 2);
        }
    }
}
