using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] public  GameManager gameManager;
    public void MoveToScene(string sceneName)
    {
      SceneManager.LoadScene(sceneName);
    }
    
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "GameScene")
        {
            if (gameManager.IsBothDead())
            {
                SceneManager.LoadScene("LoseScene");
            }
        }
    }

}
