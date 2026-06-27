using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DevConsole : MonoBehaviour
{
    [SerializeField] bool showInputConsole = false;
    [SerializeField] bool showFps = false;
    [SerializeField] bool shouldFollow = true;
    [SerializeField] bool autoFire = false;
    [SerializeField] TMP_InputField inputField;

    [SerializeField] TMP_Text fpsTxt;

    [SerializeField] AutoFire fireBot;
    [SerializeField] bool isPaused = false;
    [SerializeField] UnityEvent<bool> OnPausedTriggered;

    float pollingTime = 1f;
    float time;
    int frameCount;

    float nextPressedTime = 0f;
    float msBetweenPress = 0.15f;

    private void Start()
    {
        isPaused = false;

        showInputConsole = false;
        inputField.gameObject.SetActive(showInputConsole);

        showFps = true;
        fpsTxt.gameObject.SetActive(showFps);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.isPressed)
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0f : 1f;
            OnPausedTriggered?.Invoke(isPaused);
        }
        if (Time.time > nextPressedTime)
        {
            nextPressedTime = Time.time + msBetweenPress;
            ProcessKeyPress();
        }
        if(showFps) FpsUpdate();
    }

    private void ProcessKeyPress()
    {
        // Show Dev Console InputField
        if (Keyboard.current.backquoteKey.isPressed)
        {
            ShowConsoleInputField();
        }

        // TODO: More keys
    }

    private void ShowConsoleInputField()
    {
        showInputConsole = !showInputConsole;
        inputField.gameObject.SetActive(showInputConsole);
    }


    public void ProcessCMD(string cmd)
    {
        Debug.Log($"CMD: {cmd}");

        if (cmd.Equals("fps"))
        {
            HandleFps();
        }

        if (cmd.Equals("auto"))
        {
            autoFire = !autoFire;
            fireBot.enabled = autoFire;
        }

        if (cmd.Equals("sf"))
        {
            shouldFollow = !shouldFollow;
            foreach (var enemy in FindObjectsByType<Enemy>())
            {
                enemy.ShouldFollowCMD = shouldFollow;
            }
        }
    }

    private void HandleFps()
    {
        showFps = !showFps;
        fpsTxt.gameObject.SetActive(showFps);
    }

    void FpsUpdate()
    {
        time += Time.deltaTime;

        frameCount++;

        if (time >= pollingTime)
        {
            int fremeRate = Mathf.RoundToInt(frameCount / time);
            fpsTxt.text = $"{fremeRate.ToString("D3")} FPS";

            time -= pollingTime;
            frameCount = 0;
        }
    }
}
