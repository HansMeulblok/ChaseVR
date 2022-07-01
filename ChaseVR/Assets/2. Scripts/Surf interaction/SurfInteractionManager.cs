using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SurfInteractionManager : MonoBehaviour
{
    public static SurfInteractionManager Instance = null;

    // Array for the triggers for the hand interactions, 0 is left 1 is right
    //[HideInInspector]
    public GameObject[] handInteractionTriggers = new GameObject[2];
    //[HideInInspector]
    public MeshRenderer[] triggersMeshRenderers = new MeshRenderer[2];// meshRendererLeft, meshRendererRight;
    [HideInInspector]
    public Material surfTriggerCorrectLeft, surfTriggerIncorrectLeft, surfTriggerCorrectRight, surfTriggerIncorrectRight;
    private Vector3 handInteractionTriggersScale;

    [HideInInspector]
    public bool canTriggerLeft = true, canTriggerRight = true, canPause = true, isPlaying = false;

    public Material waveMaterial;
    public float timerTime;

    private Coroutine leftHandCoroutine, rightHandCoroutine;

    private bool leftDominant, rightDominant, tutorial;
    public float triggerMoveTime, triggerMoveSpeed;


    private SurfBoardMovement sbMove;

    public bool interaction = false, /*minorInteraction = false*/ allInteraction = false;


    private Vector3 steerMoveAmount;

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


    public float translationFactor;


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
        leftDominant = false;
        rightDominant = false;
        
        tutorial = true;

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

        sbMove.timeValue = translationFactor;

        steerMoveAmount = sbMove.transform.position;

        if (SceneManager.GetActiveScene().name != "SurfExperience")
        {
            ResumeSurfing();
            waveMaterial.SetVector("_SurfBoardPos", new Vector4(200f, -2f, 110f, 0));
        }
           
    }

    private void Update()
    {
        switch (stateBothHands)
        {
            case StateBothHands.BothHandsOutTrigger:

                if (canPause)
                {
                    if (SceneManager.GetActiveScene().name == "SurfExperience")
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
        
        switch (stateRightHand)
        {
            case StateRightHand.RightOutTrigger:

                ResetTriggerAlpha(1);

                if ((rightDominant || leftDominant) && tutorial)
                {
                    rightDominant = false;
                }

                if (!isPlaying || SceneManager.GetActiveScene().name != "SurfExperience")
                    canTriggerRight = true;

                if (SceneManager.GetActiveScene().name != "SurfExperience" &&
                    stateBothHands == StateBothHands.BothHandsOutTrigger)
                    SetTriggerMaterial(triggersMeshRenderers[1], surfTriggerIncorrectRight);
                
                if (SceneManager.GetActiveScene().name == "SurfExperience" && 
                    stateBothHands == StateBothHands.BothHandsOutTrigger && !isPlaying)
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
        
        if (isPlaying && SceneManager.GetActiveScene().name == "SurfExperience")
        {
            HeadSteer();
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

        triggerTransform.GetChild(0).gameObject.SetActive(false);

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

        if (SceneManager.GetActiveScene().name == "SurfExperience")
        {
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
        else
        {
            SceneManager.LoadScene("SurfExperience");
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

        sbMove.playing = isPlaying;
    }

    public void ResumeSurfing()
    {
        isPlaying = true;

        sbMove.playing = isPlaying;
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

        handInteractionTriggers[i].transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HeadSteer()
    {
        sbMove.transform.GetChild(2).rotation = Quaternion.Euler(SteerAngleUpAndDown() + 90f,
                                                                 -SteerAngleLeftAndRight() + 180f,
                                                                 -90f + (SteerAngleLeftAndRight() - 90f) * 1.5f);

        steerMoveAmount.x = sbMove.transform.position.x;
        steerMoveAmount.y = sbMove.transform.position.y;
        steerMoveAmount.z = Mathf.Clamp(sbMove.transform.position.z + (SteerAngleLeftAndRight() - 90f) * 0.005f, -70f, -20f);
        

        sbMove.transform.position = steerMoveAmount;
    }

    private float SteerAngleUpAndDown()
    {
        return Vector3.SignedAngle(Camera.main.transform.position - sbMove.transform.GetChild(0).position, sbMove.transform.GetChild(0).forward, Vector3.forward);
    }

    private float SteerAngleLeftAndRight()
    {
        float angle = Vector3.SignedAngle(Camera.main.transform.position - sbMove.transform.GetChild(0).position, sbMove.transform.GetChild(0).up, Vector3.up);

        if(angle < 0)
        {
            angle = -angle;
        }

        return angle;
    }
}
