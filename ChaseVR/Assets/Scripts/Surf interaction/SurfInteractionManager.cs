using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfInteractionManager : MonoBehaviour
{
    public static SurfInteractionManager Instance = null;

    // Array for the triggers for the hand interactions, 0 is left 1 is right
    [HideInInspector]
    public GameObject[] handInteractionTriggers = new GameObject[2];
    [HideInInspector]
    public MeshRenderer[] triggersMeshRenderers = new MeshRenderer[2];// meshRendererLeft, meshRendererRight;
    [HideInInspector]
    public Material surfTriggerCorrectLeft, surfTriggerIncorrectLeft, surfTriggerCorrectRight, surfTriggerIncorrectRight;
    private Vector3 handInteractionTriggersScale;

    [HideInInspector]
    public bool canTriggerLeft = true, canTriggerRight = true, canPause = true, isPlaying = false;

    public Material waveMaterial;
    public float timerTime;

    private Coroutine leftHandCoroutine, rightHandCoroutine;


    private SurfBoardMovement sbMove;

    public bool interaction = false, /*minorInteraction = false*/ allInteraction = false;

    public enum StateBothHands
    {
        BothHandsOutTrigger,
        BothHandsInTrigger
    }

    public enum StateLeftHand
    {
        LeftOutTrigger,
        LeftInTrigger
    }

    public enum StateRightHand
    {
        RightInTrigger,
        RightOutTrigger
    }

    [HideInInspector]
    public StateBothHands stateBothHands;
    [HideInInspector]
    public StateLeftHand stateLeftHand;
    [HideInInspector]
    public StateRightHand stateRightHand;

    [InspectorButton("ResumeSurfing")]
    public bool clickResume;

    [InspectorButton("PauseSurfing")]
    public bool clickPause;


    public float testValue;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {        
        if (interaction)
        {
            for (int i = 0; i < handInteractionTriggers.Length; i++)
            {
                triggersMeshRenderers[i] = handInteractionTriggers[i].gameObject.GetComponent<MeshRenderer>();
            }

            surfTriggerCorrectLeft = Resources.Load("Materials/SurfTriggerCorrectLeft", typeof(Material)) as Material;
            surfTriggerIncorrectLeft = Resources.Load("Materials/SurfTriggerIncorrectLeft", typeof(Material)) as Material;

            surfTriggerCorrectRight = Resources.Load("Materials/SurfTriggerCorrectRight", typeof(Material)) as Material;
            surfTriggerIncorrectRight = Resources.Load("Materials/SurfTriggerIncorrectRight", typeof(Material)) as Material;


            stateBothHands = StateBothHands.BothHandsOutTrigger;
        }
        else
        {
            for (int i = 0; i < triggersMeshRenderers.Length; i++)
            {
                handInteractionTriggers[i].gameObject.SetActive(false);
            }

            stateBothHands = StateBothHands.BothHandsInTrigger;
            ResumeSurfing();
        }
        

        sbMove = FindObjectOfType<SurfBoardMovement>();
    }

    private void Update()
    {

        //waveMaterial.SetFloat("_DeltaTimeSpeedValue", waveMaterial.GetFloat("_TimeValue") * waveMaterial.GetFloat("_DeltaSpeed"));

        switch (stateBothHands)
        {
            case StateBothHands.BothHandsOutTrigger:

                if (canPause)
                {
                    PauseSurfing();
                    canPause = false;
                }

                break;

            case StateBothHands.BothHandsInTrigger:

                if (isPlaying)
                canPause = true;

                break;

            default:

                PauseSurfing();

                break;
        }

        switch (stateLeftHand)
        {
            case StateLeftHand.LeftOutTrigger:

                ResetTriggerAlpha(0);

                if (!isPlaying)
                    canTriggerLeft = true;

                if (stateBothHands == StateBothHands.BothHandsOutTrigger && !isPlaying)
                    SetTriggerMaterial(triggersMeshRenderers[0], surfTriggerIncorrectLeft);
                
                if (leftHandCoroutine != null)
                    StopCoroutine(leftHandCoroutine);

                break;

            case StateLeftHand.LeftInTrigger:

                if (canTriggerLeft)
                {
                    SetTriggerMaterial(triggersMeshRenderers[0], surfTriggerCorrectLeft);
                    leftHandCoroutine = StartCoroutine(TriggerTimer(timerTime, handInteractionTriggers[0].transform));

                    canTriggerLeft = false;
                }

                break;

            default:

                SetTriggerMaterial(triggersMeshRenderers[0], surfTriggerIncorrectLeft);

                break;
        }


        switch (stateRightHand)
        {
            case StateRightHand.RightOutTrigger:

                ResetTriggerAlpha(1);

                if (!isPlaying)
                    canTriggerRight = true;

                if (stateBothHands == StateBothHands.BothHandsOutTrigger && !isPlaying)
                    SetTriggerMaterial(triggersMeshRenderers[1], surfTriggerIncorrectRight);

                if (rightHandCoroutine != null)
                    StopCoroutine(rightHandCoroutine);

                break;

            case StateRightHand.RightInTrigger:

                if (canTriggerRight)
                {
                    SetTriggerMaterial(triggersMeshRenderers[1], surfTriggerCorrectRight);
                    rightHandCoroutine = StartCoroutine(TriggerTimer(timerTime, handInteractionTriggers[1].transform));

                    canTriggerRight = false;
                }

                break;

            default:

                SetTriggerMaterial(triggersMeshRenderers[1], surfTriggerIncorrectRight);

                break;
        }

    }


    public IEnumerator TriggerTimer(float timerTime, Transform triggerTransform)
    {
        float t = 0;
        Color color;

        if (triggerTransform.gameObject.name.Contains("Left"))
        {
            color = surfTriggerCorrectLeft.color;
        }
        else
        {
            color = surfTriggerIncorrectRight.color;
        }


        //color = triggerTransform.GetComponent<MeshRenderer>().material.color;

        while (t < timerTime) 
        {
            color.r = 0.1f;
            color.g = 1f;
            color.b = 0f;

            color.a = Mathf.Lerp(0.5f, 0, (t / timerTime));

            triggerTransform.GetComponent<MeshRenderer>().material.color = color;

            t += Time.deltaTime;


            yield return null;
        }

        if (stateBothHands == StateBothHands.BothHandsInTrigger && !isPlaying)
        {
            ResumeSurfing();

            if (!allInteraction)
            {
                for (int i = 0; i < triggersMeshRenderers.Length; i++)
                {
                    handInteractionTriggers[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void SetTriggerMaterial(MeshRenderer meshRenderer, Material triggerMat)
    {
        if (meshRenderer.material != triggerMat)
        {
            meshRenderer.material = triggerMat;
        }
    }

    public void PauseSurfing()
    {
        isPlaying = false;

        waveMaterial.SetFloat("_DeltaSpeed", 0.2f);
        waveMaterial.SetFloat("_Translation", -3f);
    }

    public void ResumeSurfing()
    {
        isPlaying = true;
        
        waveMaterial.SetFloat("_DeltaSpeed", 1.8f);
        waveMaterial.SetFloat("_Translation", -2f);

    }

    public void ResetTriggerAlpha(int i)
    {
        if (handInteractionTriggers[i].GetComponent<MeshRenderer>().material == surfTriggerCorrectLeft ||
            handInteractionTriggers[i].GetComponent<MeshRenderer>().material == surfTriggerCorrectRight)
        {
            Color color = handInteractionTriggers[i].GetComponent<MeshRenderer>().material.color;

            color.a = 0.5f;

            handInteractionTriggers[i].GetComponent<MeshRenderer>().material.color += color;
        }
    }
}
