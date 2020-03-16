using UnityEngine;
using UnityEngine.UI;

public class LoseWindowManager : MonoBehaviour
{
    public static LoseWindowManager Instance = null;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
    }

    public void Show()
    {
        GetComponent<Animator>().Play("Open");
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
