using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using NativeWebSocket;

public class apisound : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    WebSocket websocket;
    bool isUpdating = false;

    float lastUserChangeTime = 0f;
    float updateDelay = 0.3f; // ป้องกันเด้งกลับ

    void Start()
    {
        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        StartCoroutine(SyncWithServer());
        ConnectWebSocket();
    }

    public void OnMusicChanged(float val)
    {
        lastUserChangeTime = Time.time;
        Debug.Log("🎵 Music Slider: " + val + " → " + MapVolume(val));
        if (isUpdating) return;
        audioMixer.SetFloat("MusicVolume", MapVolume(val));
        StartCoroutine(SendVolumeToServer());
    }

    public void OnSFXChanged(float val)
    {
        lastUserChangeTime = Time.time;
        Debug.Log("🔊 SFX Slider: " + val + " → " + MapVolume(val));
        if (isUpdating) return;
        audioMixer.SetFloat("SFXVolume", MapVolume(val));
        StartCoroutine(SendVolumeToServer());
    }

    IEnumerator SendVolumeToServer()
    {
        var json = JsonUtility.ToJson(new
        {
            music = musicSlider.value,
            sfx = sfxSlider.value
        });

        UnityWebRequest req = UnityWebRequest.Put("http://localhost:8000/volume", json);
        req.method = "POST";
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();
    }

    IEnumerator SyncWithServer()
    {
        UnityWebRequest req = UnityWebRequest.Get("http://localhost:8000/volume");
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success)
        {
            var data = JsonUtility.FromJson<VolumeData>(req.downloadHandler.text);

            // ✅ ถ้าเพิ่งเปลี่ยนเองภายใน 0.3 วิ → ข้าม
            if (Time.time - lastUserChangeTime < updateDelay) yield break;

            isUpdating = true;
            musicSlider.value = data.music;
            sfxSlider.value = data.sfx;
            audioMixer.SetFloat("MusicVolume", MapVolume(data.music));
            audioMixer.SetFloat("SFXVolume", MapVolume(data.sfx));
            isUpdating = false;
        }
    }

    async void ConnectWebSocket()
    {
        websocket = new WebSocket("ws://localhost:8000/ws");

        websocket.OnMessage += (bytes) =>
        {
            var json = System.Text.Encoding.UTF8.GetString(bytes);
            var data = JsonUtility.FromJson<VolumeData>(json);

            // ✅ ถ้าเพิ่งเปลี่ยนเอง → ข้ามการ sync จาก server
            if (Time.time - lastUserChangeTime < updateDelay) return;

            isUpdating = true;
            musicSlider.value = data.music;
            sfxSlider.value = data.sfx;
            audioMixer.SetFloat("MusicVolume", MapVolume(data.music));
            audioMixer.SetFloat("SFXVolume", MapVolume(data.sfx));
            isUpdating = false;
        };

        await websocket.Connect();
    }

    float MapVolume(float value)
    {
        if (value <= 0.0001f)
            return -80f; // เงียบสุด
        return Mathf.Log10(value) * 20f; // แปลงเป็น dB
    }

    [System.Serializable]
    public class VolumeData
    {
        public float music;
        public float sfx;
    }

    void Update()
    {
        websocket?.DispatchMessageQueue();
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
