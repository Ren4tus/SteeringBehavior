using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MySceneManager : MonoBehaviour
{
    private static MySceneManager instance = null;
    private int sceneNumber = 0;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static MySceneManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            sceneNumber = (sceneNumber+1)%6;
            string sceneName = "DemoScene" + sceneNumber.ToString();
            SceneManager.LoadScene(sceneName);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (sceneNumber == 0) sceneNumber = 5;
            else sceneNumber = sceneNumber - 1;

            string sceneName = "DemoScene" + sceneNumber.ToString();
            SceneManager.LoadScene(sceneName);
        }

    }
}
