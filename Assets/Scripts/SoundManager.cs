using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // Singleton Instance

    [Header("Sound Effects")]
    public AudioClip hitSound;
    public AudioClip hitArmorSound;
    public AudioClip itemPickupSound;    // เสียงสำหรับเก็บไอเท็ม
    public AudioClip ultimateSound;      // เสียงสำหรับ Ultimate Effect
    public AudioClip moveSound;          // เสียงสำหรับการกดเดิน
    public AudioClip shootSound;
    public AudioClip deadSound;

    private AudioSource audioSource; // ตัวควบคุม AudioSource

    private void Awake()
    {
        // ทำให้ SoundManager เป็น Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ทำให้ไม่ถูกทำลายเมื่อเปลี่ยน Scene
        }
        else
        {
            Destroy(gameObject); // ป้องกันการสร้างซ้ำ
        }
    }

    private void Start()
    {
        // เพิ่ม AudioSource ให้ตัว SoundManager
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // เล่นเสียงทั่วไป
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SoundManager: AudioClip is null.");
        }
    }

    // เล่นเสียงด้วยตำแหน่ง (สำหรับ Spatial Sound)
    public void PlaySoundAtPosition(AudioClip clip, Vector3 position)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position);
        }
        else
        {
            Debug.LogWarning("SoundManager: AudioClip is null.");
        }
    }
}
