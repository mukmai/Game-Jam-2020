using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public Button loseButton;
    [SerializeField] private GameObject explodePrefab;
    public GameObject tv;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BeginGame()
    {
        GameObject explode = Instantiate(explodePrefab, tv.transform.position, Quaternion.identity);
        explode.transform.localScale = Vector3.one * 12;

        StartCoroutine(waitLoad());
    }

    IEnumerator waitLoad()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(tv);
        yield return new WaitForSeconds(2.6f);
        SceneManager.LoadScene(1);
        yield return null;
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
