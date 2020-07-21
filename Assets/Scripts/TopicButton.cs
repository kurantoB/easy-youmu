using UnityEngine;
using Fungus;
using TMPro;

public class TopicButton : MonoBehaviour

{
    // Upon spawning, button will move upwards until it hits a certain height, where it will delete itself
    private RectTransform rectTransform;
    private int speed;
    private string goToTopic;
    public TMP_Text tmproText;
    public float deleteThreshold;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Init(int speed, string goToTopic, float deleteThreshold)
    {
        this.speed = speed;
        this.goToTopic = goToTopic;
        this.tmproText.text = goToTopic;
        this.deleteThreshold = deleteThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition = new Vector2(transform.position.x, transform.position.y + Time.deltaTime * speed);

        if (transform.position.y >= deleteThreshold)
        {
            Destroy(gameObject);
            TopicManager.instance.TopicAvailable(goToTopic);
        }
    }

    public string getGoToTopic()
    {
        return goToTopic;
    }

    public void topicButtonClicked()
    {
        TopicManager.instance.ClearTopicRoll();
        if (GameObject.FindObjectOfType<Flowchart>().GetBooleanVariable("Lock"))
        {
            GameObject.FindObjectOfType<Flowchart>().SetBooleanVariable("Lock", false);
            GameObject.FindObjectOfType<GameFlow>().ConfessionBackOut(goToTopic);
        } else
        {
            GameObject.FindObjectOfType<Flowchart>().SetStringVariable("NextBlock", goToTopic);
        }
    }
}
