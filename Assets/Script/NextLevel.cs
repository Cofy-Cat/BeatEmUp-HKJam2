using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public int sceneBuildIndex;
    public bool clearEnemies;

    private void onTriggerEnter2d(Collider2D other){
        Debug.Log("Entered Trigger");

        if(other.tag == "Player" && clearEnemies){
            Debug.Log($"Switching Scene to {sceneBuildIndex}");
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        }
    }
}
