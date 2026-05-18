using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIController : MonoBehaviour
{
    [SerializeField] List<GameObject> tutorialUIList;
    private int currentIndex = 0;

    private static TutorialUIController instance;

    public static TutorialUIController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TutorialUIController>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<TutorialUIController>();
                    singletonObject.name = typeof(TutorialUIController).ToString() + " (Singleton)";
                }
            }
            return instance;
        }
    }

    private void Start()
    {
        
    }

    public void StartTutorial()
    {
        tutorialUIList[0].SetActive(true);
    }

    public void TutorialButtonPressed()
    {
        tutorialUIList[currentIndex].SetActive(false);
        currentIndex += 1;
        if (currentIndex < tutorialUIList.Count)
        {
            tutorialUIList[currentIndex].SetActive(true);
        }

    }



}
