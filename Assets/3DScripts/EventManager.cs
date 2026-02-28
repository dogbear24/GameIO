using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    [Header("Settings")]
    public Transform player;                
    public List<GameObject> fragments = new List<GameObject>(); 
    public float collectionDistance = 2f;   
    public TextMeshProUGUI statusText;     
    
    [Header("Chest Settings")]
    public Transform chest;
    public float chestInteractionDistance = 3f;

    [Header("Fade Settings")]
    public float fadeSpeed = 1.5f;     
    public float displayDuration = 2f; 

    [Header("Results")]
    public bool allFragmentsCollected = false;
    public bool endSequence = false;
    public int totalTarget = 3; 

    private int collectedCount = 0;

    void Start()
    {
        if (statusText != null)
        {
            Color c = statusText.color;
            c.a = 0;
            statusText.color = c;
            statusText.gameObject.SetActive(true); 
            StartCoroutine(FlashText($"Was that all a dream...I remember coming to collect pieces of the calamity"));
        }
    }

    public void RegisterFragment(GameObject newFragment)
    {
        if (!fragments.Contains(newFragment))
        {
            fragments.Add(newFragment);
        }
    }

    void Update()
    {
        if (!allFragmentsCollected)
        {
            for (int i = fragments.Count - 1; i >= 0; i--)
            {
                if (fragments[i] == null || !fragments[i].activeInHierarchy) 
                    continue;

                float distance = Vector3.Distance(player.position, fragments[i].transform.position);

                if (distance <= collectionDistance)
                {
                    CollectFragment(i);
                }
            }
        }
        else if (!endSequence)
        {
            if (chest != null)
            {
                float distanceToChest = Vector3.Distance(player.position, chest.position);
                if (distanceToChest <= chestInteractionDistance)
                {
                    endSequence = true;
                }
            }
        }
    }

    void CollectFragment(int index)
    {
        GameObject fragment = fragments[index];
        fragments.RemoveAt(index); 
        Destroy(fragment); 

        collectedCount++;

        if (statusText != null)
        {
            StopAllCoroutines(); 
            
            if (collectedCount >= totalTarget)
            {
                allFragmentsCollected = true;
                StartCoroutine(FinalSequence());
            }
            else
            {
                StartCoroutine(FlashText($"{collectedCount}/{totalTarget} Fragments Collected"));
            }
        }
    }

    IEnumerator FinalSequence()
    {
        yield return StartCoroutine(FlashText($"{collectedCount}/{totalTarget} Fragments Collected"));
        if (collectedCount >= totalTarget)
        {
            yield return StartCoroutine(FlashText("I'll change the past and reseal the box...it should have never been opened"));
        }
    }

    IEnumerator FlashText(string message)
    {
        statusText.text = message;
        Color textColor = statusText.color;
        
        while (textColor.a < 1.0f) {
            textColor.a += Time.deltaTime * fadeSpeed;
            statusText.color = textColor;
            yield return null;
        }

        yield return new WaitForSeconds(displayDuration);

        while (textColor.a > 0.0f) {
            textColor.a -= Time.deltaTime * fadeSpeed;
            statusText.color = textColor;
            yield return null;
        }
    }
}