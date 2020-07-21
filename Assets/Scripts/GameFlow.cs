using UnityEngine;
using Fungus;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameFlow : MonoBehaviour
{
    public Flowchart flowChart;
    public GameObject endMenu;
    public GameObject progressCircle;
    public List<int> youmuConfessionNums;

    public delegate void BaseContinue();

    private void Awake()
    {
        youmuConfessionNums = new List<int>() {0, 1, 2, 3, 4, 5};
    }

    public void StartFirstTopic()
    {
        List<string> topics = TopicManager.instance.availableTopics;
        int topicIndex = Random.Range(0, topics.Count);
        ConfessionBackOut(topics[topicIndex]);
    }

    public void ChangeTopic(string topic)
    {
        TopicManager.instance.ClearTopicRoll();
        flowChart.SetStringVariable("NextBlock", topic);
    }

    public void HandleContinue(Command sayCommand, BaseContinue bc)
    {
        string nextBlock = flowChart.GetStringVariable("NextBlock");
        if (!"Empty".Equals(nextBlock))
        {
            flowChart.StopAllBlocks();
            flowChart.ExecuteBlock(nextBlock);
            flowChart.SetStringVariable("NextBlock", "Empty");
            if (flowChart.GetStringVariable("ConvoStage").Equals("MidConvo"))
            {
                flowChart.SetBooleanVariable("BadTransition", true);
            }
            else if (flowChart.GetStringVariable("ConvoStage").Equals("ConfessionApproach"))
            {
                // good transition
                flowChart.SetStringVariable("ConvoStage", "MidConvo");
                flowChart.SetIntegerVariable("CommandIndex", 0);
                flowChart.SetStringVariable("CurrentBlock", nextBlock);
                TopicManager.instance.RegisterTopic(nextBlock);
                TopicManager.instance.ClearTopicRoll();
                TopicManager.instance.ActivateTopicRoll();
            }
        }
        else
        {
            if (flowChart.GetBooleanVariable("BadTransition"))
            {
                flowChart.StopAllBlocks();
                flowChart.ExecuteBlock("AbruptTopicChange");
                flowChart.SetBooleanVariable("BadTransition", false);
            } else {
                if (!sayCommand.ParentBlock.BlockName.Equals("AbruptTopicChange"))
                {
                    // Normal flow
                    flowChart.SetIntegerVariable("CommandIndex", (sayCommand.CommandIndex + 1));
                }
                bc();
            }
        }
    }

    public void ConfessionBackOut(string newTopic)
    {
        flowChart.StopAllBlocks();
        flowChart.ExecuteBlock(newTopic);
        flowChart.SetStringVariable("ConvoStage", "MidConvo");
        flowChart.SetIntegerVariable("CommandIndex", 0);
        TopicManager.instance.RegisterTopic(newTopic);
        flowChart.SetStringVariable("CurrentBlock", newTopic);
        flowChart.SetStringVariable("NextBlock", "Empty");
        TopicManager.instance.ClearTopicRoll();
        TopicManager.instance.ActivateTopicRoll();
        GameObject.FindObjectOfType<Timer>().EnableTimer();
        progressCircle.SetActive(true);
    }

    public void DisplayEndMenu()
    {
        endMenu.SetActive(true);
    }

    public void TryAgain()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void DestroyAudioSource()
    {
        Destroy(GameObject.FindGameObjectWithTag("AudioSource"));
    }

    public void ReturnToCurrentBlock() {
        flowChart.ExecuteBlock(flowChart.FindBlock(flowChart.GetStringVariable("CurrentBlock")), flowChart.GetIntegerVariable("CommandIndex"));
    }

    public void IgnoredConfession()
    {
        TopicManager.instance.ActivateTopicRoll();
        flowChart.SetBooleanVariable("Lock", true);
    }

    public void YoumuConfessionReached()
    {
        GameObject.FindObjectOfType<Timer>().DisableTimer();
        progressCircle.SetActive(false);
        TopicManager.instance.ClearTopicRoll();
    }

    public void SetYoumuConfessionNum()
    {
        int youmuConfessionNum = youmuConfessionNums[Random.Range(0, youmuConfessionNums.Count)];
        flowChart.SetIntegerVariable("YoumuConfessionNum", youmuConfessionNum);
        youmuConfessionNums.Remove(youmuConfessionNum);
    }
}
