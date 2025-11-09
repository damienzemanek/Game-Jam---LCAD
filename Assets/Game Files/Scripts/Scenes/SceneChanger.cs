using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public float delay = 0f;
    public void LoadScene(int scene) => SceneManager.LoadScene(scene);

    public void LoadSceneAfterDelay(int scene) => this.DelayedCall(() => SceneManager.LoadScene(scene), delay);

    public void LoadScene(string scene) => SceneManager.LoadScene(scene);

}
