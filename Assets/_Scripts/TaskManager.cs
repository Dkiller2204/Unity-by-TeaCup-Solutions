﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{

    public List<Task> tasks = new List<Task>();
    public List<Task> activeTasks = new List<Task>();
    //public TMP_Text title, subtasks, title1, subtasks2;
    public List<TMP_Text> taskTitles = new List<TMP_Text>();
    public List<TMP_Text> taskDescriptions = new List<TMP_Text>();
    public List<Image> StickySprites = new List<Image>();

    public GameObject sound;
    public AudioManager AM;
    public GameObject EndScreen;

    private bool triggerEnd = false;
    private bool[] FadeMask_Sticky = { false, false };
  

    // Use this for initialization
    void Start()
    {
        //connecting to audiomnaer through tag
        sound = GameObject.FindGameObjectWithTag("AudioManager");
        AM = sound.GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
   
        int remove = -1;

        HandleTaskHUD();

        checkWin(); // returns -1 if gameOver

        if (checkWin() == -1 && !triggerEnd)
        {
            triggerEnd = true;
            StartCoroutine(EndGame(3));
        }

        //loop through each active task, stop if we hit completed task
        for (int i = 0; i < activeTasks.Capacity; i++)
        {
           
            //if we find an active task that is complete
            if (activeTasks[i].IsCompleted)
            {
                //random number between 1 and 4 (1 inclusive and 5 exclusive)
                int randomNum = Random.Range(1, 5);
                if(randomNum == 1)
                {
                    AM.Play("taskCompleteMessageOne");
                }
                if (randomNum == 2)
                {
                    AM.Play("taskCompleteMessageTwo");
                }
                if (randomNum == 3)
                {
                    AM.Play("taskCompleteMessageThree");
                }
                if (randomNum == 4)
                {
                    AM.Play("taskCompleteMessageFour");
                }

                //store the value that we are going to remove
                remove = i;
                break; //get out of here!
            }

        }

        //if we found something completed, remove it from the list
        if (remove != -1)
        {

            //for every task that exists
            foreach (Task nextTask in tasks)
            {

                //if task is not completed yet
                if (!nextTask.IsCompleted && !AlreadyActive(nextTask))
                {

                    //overwrite the task we set to remove with the next task
                    activeTasks[remove] = nextTask;
                    remove = -1; //set back to show an overwrite took place
                    break;
                }
            }

            //break takes us here.
            //if remove still NOT EQUAL -1, then we have no other uncompleted task
            if (remove != -1)
            {

                //trigger the sticky to fade out
                FadeMask_Sticky[remove] = true;

            }//end if remove != -1

        }

    }

    int checkWin()
    {

        //if all tasks are completed, game win.a

        foreach (Task task in tasks)
        {
            if (!task.IsCompleted)
            {
                return 0;
            }
        }

        return -1;
    }



    /* 
	    Handles the telling the StickyNotes what to draw.
		We always write activetask[0] to sNote_0
		and 			activetask[1] to sNote_1

		When we have completed all tasks we set
		their alpha to transparent but technically 
		still draw them.
	*/
    void HandleTaskHUD()
    {

        HandleStickyFade();

        //draw active tasks
        taskTitles[0].text = activeTasks[0].description;
        taskDescriptions[0].text = activeTasks[0].subtasksToString();

        taskTitles[1].text = activeTasks[1].description;
        taskDescriptions[1].text = activeTasks[1].subtasksToString();


    }


    void HandleStickyFade()
    {

        //for every sticky sprite
        for (int i = 0; i < StickySprites.Count; i++)
        {

            //if its marked to be faded AND not fully faded yet
            if (FadeMask_Sticky[i] && StickySprites[i].color.a > 0)
            {

                //fade the spirtes and text items
                //StickySprites[i].color.a -= 0.1;
                //taskTitles [i].color.a -= 0.1;
                //taskDescriptions [i].color.a -= 0.1;

            }

        }
    }

    //check if task is already in activeTask
    bool AlreadyActive(Task inputTask)
    {

        //loop through active list
        foreach (Task activeTask in activeTasks)
        {

            //if they're the same
            if (activeTask == inputTask)
            {
                return true;
            }

        }
        return false;
    }

    IEnumerator EndGame(int seconds)
    {
        EndScreen.SetActive(true);
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(0);
    }



}
