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

    [Header("End Sequence Actions")]
    public Animator playerAnimator;  

    private int collectedCount = 0;
    private Coroutine currentTextRoutine;

    public CharacterMovement3D charactermovement;
    public CameraCon2 cameracontroller;
    public Transform teleportTarget;

    public GameObject pastPlayer;
    public RuntimeAnimatorController pastPlayerAnimator;

    [Header("Past Player Movement")]
    public float pastPlayerMoveSpeed = 1.5f;
    public bool pastPlayerReady = false;


    public bool endCutScene = false;

    void Start()
    {
        pastPlayer.SetActive(false);
        if (statusText != null)
        {
            Color c = statusText.color;
            c.a = 0;
            statusText.color = c;
            statusText.gameObject.SetActive(true);
            StartCoroutine(IntroSequence());
        }
    }

    IEnumerator IntroSequence()
    {
        yield return StartCoroutine(FlashText("Welcome to Oneiros, the City of Dreams"));
        yield return StartCoroutine(FlashText("Press [Tab] for Skills"));
        yield return StartCoroutine(FlashText("What was that? I woke up by a statue in... a garden?"));
        yield return StartCoroutine(FlashText("No, I've been in that garden before... I think"));
        yield return StartCoroutine(FlashText("..."));
        yield return StartCoroutine(FlashText("I'm out of time. I need to collect and reseal all the Remnants before it's too late."));
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
                    ExecuteEndSequenceActions();
                }
            }
        }
    }

    void ExecuteEndSequenceActions()
    {
        player.position = teleportTarget.position;
        player.rotation = teleportTarget.rotation;

        if (playerAnimator != null)
        {
            playerAnimator.speed = 0f; 
        }

        if (cameracontroller != null)
        {
            cameracontroller.canMoveCamera = false;
        }

        if (charactermovement != null)
        {
            charactermovement.canMove = false;
        }

        StartCoroutine(PastPlayerSequence());
        
    }

    void CollectFragment(int index)
    {
        GameObject fragment = fragments[index];
        fragments.RemoveAt(index);
        Destroy(fragment);

        collectedCount++;

        if (statusText != null)
        {
            if (currentTextRoutine != null)
                StopCoroutine(currentTextRoutine);

            if (collectedCount >= totalTarget)
            {
                allFragmentsCollected = true;
                currentTextRoutine = StartCoroutine(FinalSequence());
            }
            else
            {
                currentTextRoutine = StartCoroutine(
                    FlashText($"{collectedCount}/{totalTarget} Remnants Collected")
                );
            }
        }
    }

    IEnumerator FinalSequence()
    {
        yield return StartCoroutine(
            FlashText($"{collectedCount}/{totalTarget} Remnants Collected")
        );

        yield return StartCoroutine(
            FlashText("I'll change the past and reseal the box... it should have never been opened")
        );
    }

    IEnumerator FlashText(string message)
    {
        statusText.text = message;
        Color textColor = statusText.color;

        // Fade In
        while (textColor.a < 1.0f)
        {
            textColor.a += Time.deltaTime * fadeSpeed;
            statusText.color = textColor;
            yield return null;
        }

        yield return new WaitForSeconds(displayDuration);

        // Fade Out
        while (textColor.a > 0.0f)
        {
            textColor.a -= Time.deltaTime * fadeSpeed;
            statusText.color = textColor;
            yield return null;
        }
    }

    IEnumerator EnablePastPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        endCutScene = true;
    }

    IEnumerator PastPlayerSequence()
    {
        Animator anim = pastPlayer.GetComponent<Animator>();

        if (anim != null && pastPlayerAnimator != null)
        {
            anim.runtimeAnimatorController = pastPlayerAnimator;
        }

        pastPlayer.SetActive(true);

        // Wait before moving
        //yield return new WaitForSeconds(4f);

        pastPlayerReady = true;

        // Move for 4 seconds
        yield return StartCoroutine(MovePastPlayerForward());

        endCutScene = true;
    }
    IEnumerator MovePastPlayerForward()
    {
        float duration = 8f;
        float timer = 0f;

        while (timer < duration)
        {
            pastPlayer.transform.position += pastPlayer.transform.forward * pastPlayerMoveSpeed * Time.deltaTime;

            timer += Time.deltaTime;
            yield return null;
        }

        // Movement finished
        endCutScene = true;
    }
}