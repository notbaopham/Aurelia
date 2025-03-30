using UnityEngine;
using TMPro;
using System.Collections;
using Mono.Cecil;

public class TypingDialogue : MonoBehaviour
{
    public TextMeshPro dialogue;
    public string[] lines;
    public float textSpeed;
    public float fadeDuration = 2f; // Time to fade out
    public float timeUntilStart;
    private int index;
    private AudioSource typing;
    private AudioClip[] typingSounds;

    void Start()
    {
        dialogue.text = string.Empty;
        typing = GetComponentInChildren<AudioSource>();
        typingSounds = Resources.LoadAll<AudioClip>("Audio/TypeSounds");
        StartDialogue();
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        yield return new WaitForSeconds(timeUntilStart);

        foreach (char c in lines[index].ToCharArray())
        {
            dialogue.text += c;
            int randomIndex = Random.Range(0, typingSounds.Length);
            typing.PlayOneShot(typingSounds[randomIndex]);
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeOutText());
    }

    IEnumerator FadeOutText()
    {
        Color originalColor = dialogue.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            dialogue.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        dialogue.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}