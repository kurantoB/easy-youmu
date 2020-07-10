using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopicManager : MonoBehaviour
{
    public static TopicManager instance;
    // Array with all topic GameObjects that can spawn
    // Uhh I was too lazy to prevent spawning of Real talk topics, so I just made a separate array for topics Patchouli can say
    public List<GameObject> availableTopics;
    public GameObject[] realTalkTopics;

    public float topicSpawnTime;
    private float topicTimeLeft;

    bool spawningTopics = true;

    // Canvas where topic buttons are drawn
    public GameObject parentCanvas;

    private GameObject spawnedTopic;

    public List<string> visitedTopics;

    private List<string> scrollingTopics = new List<string>();

    public GameFlow gameFlow;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            // A unique case where the Singleton exists but not in this scene
            if (instance.gameObject.scene.name == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        topicTimeLeft = topicSpawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Timer code
        if (spawningTopics)
        {
            topicTimeLeft -= Time.deltaTime;
            if (topicTimeLeft <= 0)
            {
                // Instantiate random GameObjects in array, set parent to canvas so it'll render
                GameObject tentativeTopic;
                while ((tentativeTopic = availableTopics[UnityEngine.Random.Range(0, availableTopics.Count)]).GetComponent<TopicButton>().goToTopic.Equals(gameFlow.GetFlowchartCurrentBlock())
                    || scrollingTopics.Contains(tentativeTopic.GetComponent<TopicButton>().goToTopic)) ;
                spawnedTopic = Instantiate(tentativeTopic, new Vector3(200, 200, 0), Quaternion.identity, parentCanvas.transform);
                spawnedTopic.SetActive(true);
                scrollingTopics.Add(spawnedTopic.GetComponent<TopicButton>().goToTopic);
                // Reset timer
                topicTimeLeft = topicSpawnTime;
            }
        }
    }

    public void RemoveFromScrolling(string topic)
    {
        scrollingTopics.Remove(topic);
    }

    // Keeps track of all topics player has visited
    public void Visited(string topicName)
    {
        visitedTopics.Add(topicName);
    }

    // Checks if topic has been visited before by comparing topics with list of visited topics
    public bool IsAlreadyVisited(string topic)
    {
        foreach (string topicItem in visitedTopics)
        {
            if (topicItem == topic)
            {
                // return if topic has been visited
                Debug.Log(topic + " is an old topic");
                return true;
            }
        }
        // return if topic has not been visited
        Debug.Log(topic + " is a new topic");
        return false;
    }

    // Clear the scrolling topic list
    public void ClearTopicRoll()
    {
        foreach (GameObject topic in GameObject.FindGameObjectsWithTag("TopicButton"))
        {
            Destroy(topic);
        }
        spawningTopics = false;
        scrollingTopics.Clear();
    }

    // Activate the scrolling topic list
    public void ActivateTopicRoll()
    {
        spawningTopics = true;
    }

    public string GetNextTopic()
    {
        string nextTopic = availableTopics[UnityEngine.Random.Range(0, 7)].name;
        bool isOldTopic = true;

        if (IsAlreadyVisited(nextTopic))
        {
            Debug.Log(nextTopic + " was picked, old topic");
            nextTopic = GetNextTopic();
        }
        else
        {
            if (nextTopic == "Magic Real" || nextTopic == "Becoming a Youkai Real")
            {
                Debug.Log(nextTopic + " was picked, but Patchy can't choose it");
                nextTopic = GetNextTopic();
            }
            isOldTopic = false;
        }
        // topic initiated by Patchy - can't be already visited, can't be real-talk
        return nextTopic;
    }


}
