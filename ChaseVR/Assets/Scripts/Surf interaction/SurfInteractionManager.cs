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
    public Material surfTriggerCorrect, surfTriggerIncorrect;
    private Vector3 handInteractionTriggersScale;

    [HideInInspector]
    public bool canTriggerLeft = true, canTriggerRight = true, canPause = true;


    public Material waveMaterial;
    public float timerTime;

    private Coroutine leftHandCoroutine, rightHandCoroutine;

    public enum StateBothHands
    {
        BothHandsOutTrigger,
        BothHandsInTrigger
    }

    public enum StateLeftHand
    {
        LeftInTrigger,
        LeftOutTrigger
    }

    public enum StateRightHand
    {
        RightInTrigger,
        RightOutTrigger
    }

    public StateBothHands stateBothHands;
    public StateLeftHand stateLeftHand;
    public StateRightHand stateRightHand;

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
        for (int i = 0; i < triggersMeshRenderers.Length; i++)
        {
            triggersMeshRenderers[i] = handInteractionTriggers[i].gameObject.GetComponent<MeshRenderer>();
        }

        surfTriggerCorrect = Resources.Load("Materials/SurfTriggerCorrect", typeof(Material)) as Material;
        surfTriggerIncorrect = Resources.Load("Materials/SurfTriggerIncorrect", typeof(Material)) as Material;

        stateBothHands = StateBothHands.BothHandsOutTrigger;
    }

    private void Update()
    {
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

                canPause = true;

                break;

            default:

                PauseSurfing();

                break;
        }

        switch (stateLeftHand)
        {
            case StateLeftHand.LeftOutTrigger:

                SetTriggerMaterial(triggersMeshRenderers[0], surfTriggerIncorrect);
                ResetTriggerScale(0);
                StopCoroutine(leftHandCoroutine);

                break;

            case StateLeftHand.LeftInTrigger:

                SetTriggerMaterial(triggersMeshRenderers[0], surfTriggerCorrect);

                if (canTriggerLeft)
                {
                    leftHandCoroutine = StartCoroutine(TriggerTimer(timerTime, handInteractionTriggers[0].transform));

                    canTriggerLeft = false;
                }

                break;

            default:

                SetTriggerMaterial(triggersMeshRenderers[0], surfTriggerIncorrect);

                break;
        }


        switch (stateRightHand)
        {
            case StateRightHand.RightOutTrigger:

                SetTriggerMaterial(triggersMeshRenderers[1], surfTriggerIncorrect);
                ResetTriggerScale(1);
                StopCoroutine(rightHandCoroutine);

                break;

            case StateRightHand.RightInTrigger:

                SetTriggerMaterial(triggersMeshRenderers[1], surfTriggerCorrect);

                if (canTriggerRight)
                {
                    rightHandCoroutine = StartCoroutine(TriggerTimer(timerTime, handInteractionTriggers[1].transform));

                    canTriggerRight = false;
                }

                break;

            default:

                SetTriggerMaterial(triggersMeshRenderers[1], surfTriggerIncorrect);

                break;
        }

    }


    public IEnumerator TriggerTimer(float timerTime, Transform triggerTransform)
    {
        float t = 0;
        Color color;
        
        while (t < timerTime) 
        {
            color = triggerTransform.GetComponent<MeshRenderer>().material.color;

            color.a = Mathf.Lerp(150, 0, (t / timerTime));

            triggerTransform.GetComponent<MeshRenderer>().material.color = color;

                //new Vector4(255, 3, 0, Mathf.Lerp(triggerTransform.GetComponent<MeshRenderer>().material.color.a, 0, (t / timerTime)));

            t += Time.deltaTime;

            Debug.Log("time test" + t);

            yield return null;
        }

        if (stateBothHands == StateBothHands.BothHandsInTrigger)
        {
            ResumeSurfing();
        }

        Debug.Log("time test end");
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
        //transform.localScale = Vector3.one;

        waveMaterial.SetFloat("_DeltaSpeed", 0.2f);
        canTriggerLeft = true;
        canTriggerRight = true;
    }

    public void ResumeSurfing()
    {
        waveMaterial.SetFloat("_TimeValue", waveMaterial.GetFloat("_TimeValue") - 2.5f);
        waveMaterial.SetFloat("_DeltaSpeed", 1.5f);
    }

    public void ResetTriggerScale(int i)
    {
        //handInteractionTriggers[i].transform.localScale = Vector3.one;
        handInteractionTriggers[i].GetComponent<MeshRenderer>().material.color = new Vector4(255, 3, 0, 150);

    }
}
