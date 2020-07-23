using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TopicManager : MonoBehaviour
{
    public static TopicManager instance;
    // Array with all topic GameObjects that can spawn
    // Uhh I was too lazy to prevent spawning of Real talk topics, so I just made a separate array for topics Patchouli can say
    public List<string> availableTopics;
    private List<int> availableIndices = new List<int>();
    private Dictionary<string, int> topicToIndex = new Dictionary<string, int>();
    public GameObject topicPrefab;

    public float topicSpawnTime;
    public int scrollSpeed;
    public int spawnX;
    public int spawnY;
    public float deleteThreshold;
    private float topicTimeLeft;

    bool spawningTopics = true;

    // Canvas where topic buttons are drawn
    public GameObject parentCanvas;

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
        for (int i = 0; i < availableTopics.Count; i++)
        {
            availableIndices.Add(i);
            topicToIndex.Add(availableTopics[i], i);
        }
    }

    public void TopicAvailable(string goToTopic)
    {
        if (spawningTopics)
        {
            availableIndices.Add(topicToIndex[goToTopic]);
        }
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
                // Reset timer
                topicTimeLeft = topicSpawnTime;
                if (availableIndices.Count == 0)
                {
                    return;
                }
                int availableIndicesIndex = Random.Range(0, availableIndices.Count);
                string spawnTopic = availableTopics[availableIndices[availableIndicesIndex]];
                GameObject topicButtonInst = Instantiate(topicPrefab, new Vector3(spawnX, spawnY, 0), Quaternion.identity, parentCanvas.transform);
                topicButtonInst.GetComponent<TopicButton>().Init(scrollSpeed, spawnTopic, deleteThreshold);
                availableIndices.RemoveAt(availableIndicesIndex);
            }
        }
    }

    // Clear the scrolling topic list
    public void ClearTopicRoll()
    {
        foreach (GameObject topic in GameObject.FindGameObjectsWithTag("TopicButton"))
        {
            Destroy(topic);
            TopicAvailable(topic.GetComponent<TopicButton>().getGoToTopic());
        }
        spawningTopics = false;
    }

    public void RegisterTopic(string newTopic)
    {
        availableIndices.Remove(topicToIndex[newTopic]);
    }

    // Activate the scrolling topic list
    public void ActivateTopicRoll()
    {
        spawningTopics = true;
    }

    public bool IsLastTopic()
    {
        return availableIndices.Count == 0;
    }

}
