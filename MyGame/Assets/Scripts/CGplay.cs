using UnityEngine;
using System.Collections;

public class CGplay : MonoBehaviour
{
    public MovieTexture movie;
    public AudioSource audio;
    public MovieTexture logo;
    public AudioSource logoaudio;
    private bool islogo = true;
    private bool movieend = false;
    private GameObject movietx;
    // Use this for initialization
    void Start()
    {
        movie.Play();
        audio.Play();
        movietx = GameObject.Find("CG");
    }

    // Update is called once per frame
    void Update()
    {
        /*if (movieend) {
            if (islogo) {
                islogo = false;
                logo.Play ();
                logoaudio.Play ();	
                StartCoroutine ("wait");
            }
            if (logoaudio.time > logo.duration) {
                Application.LoadLevel ("Login");
            }
        }
        if (audio.time > movie.duration) {
            movieend = true;
        }*/
        if (audio.time > movie.duration && movieend == false)
        {
            movieend = true;
            loadScene();
        }
    }
    IEnumerator wait()
    {
        yield return 0;
        movietx.SetActive(false);
    }
    public void onclick(){
        movieend = true;
        loadScene();
	}

    public void loadScene()
    {
        Application.LoadLevel("Login");
    }
}
