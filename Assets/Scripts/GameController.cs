using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static int numObjects = 1;

    static GameController Instance;
    [SerializeField]
    Button buttonGameObject;
    [SerializeField]
    Button buttonDOTS;
    [SerializeField]
    Button buttonGameObjectSphere;
    [SerializeField]
    Button buttonDOTSSphere;
    [SerializeField]
    InputField inputfieldObjectCount;
    
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        Instance = this;

        buttonGameObject.onClick.AddListener(OnClickGameObject);
        buttonDOTS.onClick.AddListener(OnClickDOTS);
        buttonGameObjectSphere.onClick.AddListener(OnClickGameObjectSphere);
        buttonDOTSSphere.onClick.AddListener(OnClickDOTSSphere);
        inputfieldObjectCount.onValueChanged.AddListener(OnInputFieldObjectsCount);

        inputfieldObjectCount.text = numObjects.ToString();
    }

    void OnClickGameObject()
    {
        SceneManager.LoadScene("GameObjects");
    }

    void OnClickDOTS()
    {
        SceneManager.LoadScene("DOTS");
    }

    void OnClickGameObjectSphere()
    {
        SceneManager.LoadScene("GameObjectSphere");
    }

    void OnClickDOTSSphere()
    {
        SceneManager.LoadScene("DOTSSphere");
    }

    void OnInputFieldObjectsCount( string newInput)
    {
        int result;
        if ( int.TryParse(newInput, out result) )
        {
            numObjects = result;
        }
        else
        {
            inputfieldObjectCount.text = numObjects.ToString();
        }
    }
}
