using UnityEngine;

public class playsound : MonoBehaviour
{
    private AudioSource m_AudioSource;

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();

    }
    private void Start()
    {
        m_AudioSource.Play();
    }
}
