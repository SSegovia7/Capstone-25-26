using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] public string sceneName;

    public void LoadScene() 
    {

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
            Debug.Log("loading scene" + sceneName);
        }
        else 
        {
            Debug.Log("erorr loading failed");
        }
    }
}
