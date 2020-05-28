using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FireConnection : MonoBehaviour
{
    private DatabaseReference Manas_e,Modi_e,Trump_e;
    long Manas_l, Modi_l, Trump_l;
    public GameObject Photo, Mood;
    public GameObject[] buttons; 
    public Sprite[] pho,mod; 
    public GameObject selector;
    public Camera arCamera;
    int phot_len, Mood_len;
    Animator manas_anim, modi_anim, trump_anim; 
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://moodapplication-be743.firebaseio.com/");
        Manas_e = FirebaseDatabase.DefaultInstance.GetReference("Manas");
        Modi_e = FirebaseDatabase.DefaultInstance.GetReference("Modi");
        Trump_e = FirebaseDatabase.DefaultInstance.GetReference("Trump");
        Photo.SetActive(false);
        Mood.SetActive(false);
        modi_anim = buttons[0].GetComponent<Animator>();
        manas_anim = buttons[1].GetComponent<Animator>();
        trump_anim = buttons[2].GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        select_option(); 
    }
    public void get_Value()
    {
        Manas_e.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Wait
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snap = task.Result;
                Manas_l = (long)snap.Value;
            }
        });
        Modi_e.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Do n
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snap_n = task.Result;
                Modi_l = (long)snap_n.Value;
            }
        });
        Trump_e.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snap_p = task.Result;
                Trump_l = (long)snap_p.Value; 
            }

        });
    }
    
    public string get_Emotions(long x)
    {
        string answer = "";
        switch (x)
        {
            case 0:answer = "Angry";
                Mood.GetComponent<SpriteRenderer>().sprite = mod[0];
                break;
            case 1:answer = "Disgust";
                Mood.GetComponent<SpriteRenderer>().sprite = mod[0];
                break;
            case 2:answer = "Scared";
                Mood.GetComponent<SpriteRenderer>().sprite = mod[2];
                break;
            case 3:answer = "Happy";
                Mood.GetComponent<SpriteRenderer>().sprite = mod[1];
                break;
            case 4:answer = "Sad";
                Mood.GetComponent<SpriteRenderer>().sprite = mod[2];
                break;
            case 5:answer = "Surprised";
                Mood.GetComponent<SpriteRenderer>().sprite = mod[3];
                break;
            case 6:answer = "Neutral";
                Mood.GetComponent<SpriteRenderer>().sprite = mod[2];
                break; 
        }
        return answer; 
    }

    void select_option()
    {
        get_Value();
        // creates a ray from the screen point origin 
        Ray ray = arCamera.ScreenPointToRay(selector.transform.position);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Photo.SetActive(true);
            Mood.SetActive(true);
            if (hit.collider.tag == "Modi")
            {
                //
                manas_anim.SetBool("Manas_Anim", false);
                trump_anim.SetBool("Trump_Anim", false);
                modi_anim.SetBool("Modi_Anim", true);
                Photo.GetComponent<SpriteRenderer>().sprite = pho[0];
                string Modi = get_Emotions(Modi_l);
            }
            else if(hit.collider.tag == "Manas")
            {
                //
                
                trump_anim.SetBool("Trump_Anim", false);
                modi_anim.SetBool("Modi_Anim", false);
                manas_anim.SetBool("Manas_Anim", true);
                Photo.GetComponent<SpriteRenderer>().sprite = pho[1];
                string Manas = get_Emotions(Manas_l);
            }
            else if(hit.collider.tag == "Trump")
            {
                //
                
                modi_anim.SetBool("Modi_Anim", false);
                manas_anim.SetBool("Manas_Anim", false);
                trump_anim.SetBool("Trump_Anim", true);
                Photo.GetComponent<SpriteRenderer>().sprite = pho[2];
                string Trump = get_Emotions(Trump_l);
            }
        }
    }

    
}
