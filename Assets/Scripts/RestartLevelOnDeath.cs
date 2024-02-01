using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevelOnDeath : MonoBehaviour
{
    public float delayBeforeRestart = 2;    // How many seconds after death to wait before restarting level
    private IEnumerator WaitThenRestart()
    {
        yield return new WaitForSeconds(delayBeforeRestart);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // this doesnt work but the idea is to introduce a delay to play death noise
        // StartCoroutine(WaitThenRestart());
    }
}
