using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame()
    {
        SelectButton(true);
        SceneManager.LoadScene("Scenes/Game");
    }

    public void EndGame()
    {
        SelectButton(false);
        Application.Quit();
    }

    private void SelectButton(bool toggle)
    {
        if(toggle)
        {
            GetComponentInChildren<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sounds/SelectButton"));
        }
        else
        {
            GetComponentInChildren<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sounds/BackButton"));
        }
    }

    public void HoverButton()
    {
        GetComponentInChildren<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sounds/HoverButton"));
    }
}
