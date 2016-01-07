using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {

    public Texture2D fadeOutTexture; // the texture that will overlay the scene. this could be a black image or a loaded graphic
    public float fadeSpeed = 0.8f; //fade speed

    private int drawDepth = -1000; //the texture's order in the draw hierarchy: a low means it renders on top
    private float alpha = 1.0f; // the texture's alpha value between 0 and 1
    private int fadeDir = -1; // the direction to fade: in = -1 or out = 1

    void OnGUI()
    {
        //fadde out/in alpha value using a direction, a speed and time.deltatime to convert the operatioon to seconds
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        //force (clamp) the number between 0 and 1 because GUI.color uses alpha values between 0 and 1
        alpha = Mathf.Clamp01(alpha);

        //set colour of our GUI (in this case our texture). All Colour values remain the same & the Alpha is set to the alpha varible
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha); //set the Alpha value
        GUI.depth = drawDepth;// make the black texture render on top (drawn last)
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture); //draw the texture to fit the etire screen area
    }

    //set fadeDir to the direction parameter making the same fade in if -1 and out if 1
    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed); //return fadespeed var so it's easy to time the Application.LoadLevel(); 
    }

    //OnLevelWasLoaded os called when a level is loaded. It takes loaded index (int) as a paramter so you can limit fade in to certain scenes
    void OnLevelWasLoaded()
    {
        //alpha = 1;   //use this if the alpha is not set to 1 by dealut
        BeginFade(-1); // call the fade in function
    }

    }