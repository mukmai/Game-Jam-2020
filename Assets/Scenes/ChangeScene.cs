using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public Button loseButton;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void NextScene()
    {
        SceneManager.LoadScene(1);
    }

    public void showLoseUI()
    {
        Debug.Log("setactive");
        loseButton.gameObject.SetActive(true);
    }
    
}
