using System;
using UnityEngine;

public class CheckpointForch : MonoBehaviour
{
    public PauseManager PauseManager;
    public ScoreTimerManager ScoreTimerManager;
    public openweb openweb;         // Drag object ที่มี openweb.cs เข้ามา
    public string ch = "ch1_1";     // ตั้งชื่อ chapter ใน Inspector เช่น ch2_3
    public GameObject activateEffectObject;
    public AudioSource soundSource;

    private bool isActivated = false;
    private void OnTriggerEnter(Collider other)
    {
        if (isActivated) return;
        ScoreTimerManager.plus();
        if (other.CompareTag("Player"))
        {
            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
            if (respawn != null)
            {
                respawn.UpdateCheckpoint(transform.position);
                Debug.Log("📍 Player touched checkpoint at: " + transform.position);
            }

            if (activateEffectObject != null)
            {
                var particle = activateEffectObject.GetComponent<ParticleSystem>();
                if (particle != null) particle.Play();
            }

            if (soundSource != null)
            {
                soundSource.Play();
            }

            if (openweb != null && !string.IsNullOrEmpty(ch))
            {
                openweb.OpenChapter(ch); // ✅ เรียกตาม key เช่น "ch1_3"
                PauseManager.PauseGame1();
            }

            isActivated = true;
        }
    }
}