using UnityEngine;
using System.Collections;

public class CameraSwitch : MonoBehaviour {

    public Camera camera1;
    public Camera camera2;
    public Canvas CanvasObject;

    private bool Active1, Active2 = true;

    private float timer;
    private float timerLimit = 7.0f;//f stands for float

	// Use this for initialization
	void Start () 
    {
        timer = timerLimit;

        camera1.GetComponent<Camera>().enabled = true;
        camera2.GetComponent<Camera>().enabled = false;
        CanvasObject.GetComponent<Canvas>().enabled = false;
        Active1 = true;
	
	}
	
	// Update is called once per frame
	void Update () {
        //print(timer);
        timer -= Time.deltaTime;
        if (timer <= 7.0f && Active1 == true)
        {
            camera1.GetComponent<Camera>().enabled = true;
            camera2.GetComponent<Camera>().enabled = false;

            camera1.GetComponent<Animation>().Play();
            print("working!1");

            Active1 = false;
            Active2 = true;
        }


        if (timer <= 1.0f && Active2 == true)
        {
            CanvasObject.GetComponent<Canvas>().enabled = true;
            

            camera1.GetComponent<Camera>().enabled = false;
            camera2.GetComponent<Camera>().enabled = true;

            camera2.GetComponent<Animation>().Play();
            print("working!2");

            Active2 = false;
            //Active3 = true;
        }
	}
}
