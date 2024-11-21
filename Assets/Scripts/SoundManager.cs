using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // Singleton Instance

    [Header("Sound Effects")]
    public AudioClip hitSound;
    public AudioClip hitArmorSound;
    public AudioClip itemPickupSound;    // ���§����Ѻ�������
    public AudioClip ultimateSound;      // ���§����Ѻ Ultimate Effect
    public AudioClip moveSound;          // ���§����Ѻ��á��Թ
    public AudioClip shootSound;
    public AudioClip deadSound;

    private AudioSource audioSource; // ��ǤǺ��� AudioSource

    private void Awake()
    {
        // ����� SoundManager �� Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ��������١��������������¹ Scene
        }
        else
        {
            Destroy(gameObject); // ��ͧ�ѹ������ҧ���
        }
    }

    private void Start()
    {
        // ���� AudioSource ����� SoundManager
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // ������§�����
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

    // ������§���µ��˹� (����Ѻ Spatial Sound)
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
