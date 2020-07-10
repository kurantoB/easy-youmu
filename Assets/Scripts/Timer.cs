using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float maxTime;
    private float timeLeft;
    private bool timerActive = false;

    public Image progressCircle;

    public delegate void MessageExit();
    private MessageExit msgExit;

    void Update()
    {
        if (!timerActive)
        {
            return;
        }

        timeLeft -= Time.deltaTime;
        progressCircle.fillAmount = timeLeft / maxTime;

        if (timeLeft <= 0)
        {
            msgExit();
            timerActive = false;
        }
    }

    public void timerReset(float messageTime, MessageExit msgExit)
    {
        this.msgExit = msgExit;
        maxTime = messageTime;
        timeLeft = messageTime;
        timerActive = true;
    }
}
