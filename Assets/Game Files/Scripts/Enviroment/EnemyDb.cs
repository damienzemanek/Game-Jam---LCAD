using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DependencyInjection;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDb : MonoBehaviour, IDependencyProvider
{
    GroceryList list;
    [SerializeReference] public DungeonData dungeonData;
    
    [Provide] EnemyDb Provide() => this;

    bool linear { get => (dungeonData == null) ? false: dungeonData.linear; }

    [ShowInInspector] public int amountOfEncounters { get => (
               dungeonData == null 
            && dungeonData.enemyPrefabs != null
            && dungeonData.enemyPrefabs.Count > 0) 
            ? 0 : dungeonData.enemyPrefabs.Count; 
    }


    [SerializeField, ShowIf("linear"), ReadOnly] public int currentEnemy;

    [TitleGroup("Completed Level")]
    [SerializeField] Button completeLevelButton;

    [TitleGroup("Audio Play")]
    [SerializeField] public AudioPlay playerAudioplay;
    [SerializeField] public AudioClip uiClickSound;

    [TitleGroup("Background Music")]
    [SerializeField] public AudioPlay bgMusicAudioPlay;
    [ShowInInspector, ReadOnly] public AudioClip bgMusic => (dungeonData == null) ? null : dungeonData.bgMusic;

    [TitleGroup("Scene Changer")]
    [SerializeField] SceneChanger sceneChanger;
    [SerializeField] int completedSceneIndex = 2;

    [TitleGroup("Fade")]
    [SerializeField] FadeScreen fadeScreen;

    private void Awake()
    {
        if (completeLevelButton == null) this.Error("compelted level button not referenced");
        if (playerAudioplay == null) this.Error("playeraudio play not referenced");
        if (sceneChanger == null) this.Error("scene changer not referenced");
        if (fadeScreen == null) this.Error("fade screen not referenced");
        if (bgMusicAudioPlay == null) this.Error("bg audio play not referenced");


        if (!dungeonData.final)
        {
            completeLevelButton.onClick.AddListener(() => sceneChanger.LoadSceneAfterDelay(completedSceneIndex));
            completeLevelButton.onClick.AddListener(UpdateProgressionWithItem);
        }
        else
            completeLevelButton.onClick.AddListener(() => sceneChanger.LoadSceneAfterDelay(dungeonData.finalSceneIndex));


        completeLevelButton.onClick.AddListener(fadeScreen.FadeToFullyOpaque);
        completeLevelButton.onClick.AddListener(call: () => playerAudioplay.Play(uiClickSound));
        completeLevelButton.onClick.AddListener(DebugPrint);
    }

    private void Start()
    {
        list = GroceryList.Instance;
        currentEnemy = 0;

        bgMusicAudioPlay.SetLooping(true);
        bgMusicAudioPlay.Play(bgMusic);

        if (dungeonData.enemyPrefabs == null) this.Error("Enemy db needs prefabs, set them");
    }

    public GameObject GetEnemyPrefab()
    {
        if (!dungeonData.linear)
            return dungeonData.enemyPrefabs.Rand().gameObject;

        if (dungeonData.enemyPrefabs.Count == 0) { this.Error("Enemy list empty"); return null; }

        if (currentEnemy >= dungeonData.enemyPrefabs.Count)
            currentEnemy = 0;

        return dungeonData.enemyPrefabs[currentEnemy++].gameObject;
    }

    [Button]
    public void UpdateProgressionWithItem()
    {
        GroceryItem match = list.items.FirstOrDefault(i => i.type == dungeonData.giveType);
        if (match != null)
        {
            match.have = true;
            this.Log($"Updated {match.type} to {match.have}");
        }

    }

    void DebugPrint()
    {
        this.Log("Pressed Compelte button");
    }
}
