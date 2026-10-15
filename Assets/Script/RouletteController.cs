using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class RouletteController : MonoBehaviour
{
    [Tooltip("輪盤分成幾格")]
    public int sectorCount = 8;

    [Tooltip("這次總共要轉幾圈的隨機範圍(差距至少要 1 圈,每一格機率才會平均)")]
    public Vector2 turnsRange = new Vector2(3f, 6f);

    [Tooltip("減速係數,越大停得越快")]
    public float friction = 1.5f;

    [Header("結果")]
    [Tooltip("每一格的內容,依順時針順序填寫,數量要和格數一樣")]
    public string[] sectorNames;

    [Tooltip("顯示結果的 UI 文字(可留空,只用 Console)")]
    public TMP_Text resultText;

    [Header("音效")]
    [Tooltip("旋轉時循環播放的聲音")]
    public AudioClip spinClip;

    [Tooltip("停下來時播放一次的聲音(可留空)")]
    public AudioClip stopClip;

    [Tooltip("轉速最快時的音調")]
    public float maxPitch = 1.2f;

    [Tooltip("快停下來時的最低音調")]
    public float minPitch = 0.6f;

    AudioSource audioSource;
    Quaternion startRotation;
    float rotSpeed;   
    float startSpeed;    
    bool spinning;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        startRotation = transform.rotation; 
    }

    public void StartSpin()
    {
        if (spinning) return;
        transform.rotation = startRotation; 

        float totalAngle = Random.Range(turnsRange.x, turnsRange.y) * 360f;
        rotSpeed = startSpeed = totalAngle * friction;
        spinning = true;

        if (spinClip != null)
        {
            audioSource.clip = spinClip;
            audioSource.loop = true;
            audioSource.pitch = maxPitch;
            audioSource.Play();
        }
    }

    void Update()
    {
    
        bool clicked = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
        bool touched = Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame;
        if (clicked || touched)
            StartSpin();

        if (!spinning) return;

       
        transform.Rotate(0f, 0f, -rotSpeed * Time.deltaTime);

   
        rotSpeed *= Mathf.Exp(-friction * Time.deltaTime);

   
        if (spinClip != null)
            audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, rotSpeed / startSpeed);

        if (rotSpeed < 1f)
        {
            rotSpeed = 0f;
            spinning = false;
            audioSource.Stop();
            if (stopClip != null) audioSource.PlayOneShot(stopClip);
            ReportResult();
        }
    }

    void ReportResult()
    {

        float angle = Mathf.Repeat(transform.eulerAngles.z - startRotation.eulerAngles.z, 360f);
        float sectorSize = 360f / sectorCount;
        int index = Mathf.FloorToInt(angle / sectorSize);           // 0 ~ sectorCount-1

        string result = (sectorNames != null && index < sectorNames.Length)
            ? sectorNames[index]
            : $"第 {index} 格(尚未填名稱)";

        Debug.Log($"要吃什麼 :{result}");
        if (resultText != null) resultText.text = result;
    }
}
