using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleObjectsSelector : MonoBehaviour
{
    [SerializeField]
    private Transform parentTrans = null;
    [SerializeField]
    private float circleRadius = 1f;
    [SerializeField]
    private AnimationCurve moveCurve = null;
    [SerializeField]
    private AnimationCurve scaleCurve = null;
    [SerializeField]
    private float moveTime = 0.4f;
    [SerializeField]
    private float scrollSpeed = 0.4f;
    [SerializeField]
    private float elasticDeccelration = 0.4f;
    [SerializeField]
    private float snapSpeed = 0.4f;
    [SerializeField]
    private float clampedMaxSnapSpeed = 0.4f;
    [SerializeField]
    private float objectsScale = 1f;
    [SerializeField]
    private float rotateLerpSpeed = 1f;
    [SerializeField]
    private float objectFrontRotationSpeed = 0.4f;
    [SerializeField]
    private float scaleupInFront = 1.2f;
    [SerializeField]
    private float scaleupInFrontLerpSpeed = 1.2f;

    [SerializeField]
    private GameObject[] prefabs = null;

    private int currentIndex = 0;
    private bool isVisible = false;
    private Transform[] instances = null;
    private Transform[] instanceParents = null;

    private float moveS = 0f;

    private float curAngle = 0f;
    private float elasticDelta = 0f;
    private Vector3 midPos;

    private float anglePerObject = 0f;

    private bool isSnapping = false;
    private float snapAngle = 0f;
    private int oldIndexInFront = -1;

    private Quaternion additionalRotation = Quaternion.identity;


    // Start is called before the first frame update
    void Start()
    {
        instances = new Transform[prefabs.Length];
        instanceParents = new Transform[prefabs.Length];

        for (int i = 0; i < instances.Length; i++)
        {
            GameObject instParent = new GameObject("Parent");
            instParent.transform.parent = parentTrans;
            instanceParents[i] = instParent.transform;
            instanceParents[i].localPosition = Vector3.zero;
            instanceParents[i].localRotation = Quaternion.identity;
            instanceParents[i].localScale = new Vector3(1f, 1f, 1f);

            GameObject inst = Instantiate(prefabs[i], instanceParents[i]);
            instances[i] = inst.transform;
            instances[i].localPosition = Vector3.zero;
            instances[i].localRotation = Quaternion.identity;
            instances[i].localScale = new Vector3(objectsScale, objectsScale, objectsScale);


        }

        anglePerObject = Mathf.PI * 2f / instances.Length;

        midPos = new Vector3(0f, 0f, circleRadius);
    }

    // Update is called once per frame
    void Update()
    {
        if (isVisible && moveS < 1f)
        {
            moveS += Time.deltaTime / moveTime;
            moveS = Mathf.Clamp(moveS, 0f, 1f);
        }
        else if (!isVisible && moveS > 0f)
        {
            moveS -= Time.deltaTime / moveTime;
            moveS = Mathf.Clamp(moveS, 0f, 1f);
        }

        float lerpS = moveCurve.Evaluate(moveS);

        if (oldIndexInFront != IndexInFront)
        {
            oldIndexInFront = IndexInFront;
            additionalRotation = Quaternion.identity;
            Debug.Log("Index In Front: " + IndexInFront.ToString());
        }
        additionalRotation *= Quaternion.Euler(0f, Time.deltaTime * objectFrontRotationSpeed, 0f);


        Debug.Log("In front: " + IndexInFront.ToString());

        for (int i = 0; i < instances.Length; i++)
        {
            float angleHere = curAngle + anglePerObject * i;
            Vector3 posOnCircle = midPos + (new Vector3(Mathf.Sin(angleHere), 0f, -Mathf.Cos(angleHere))) * circleRadius;
            Vector3 posInFront = midPos + new Vector3(0f, 0f, -circleRadius);


            instanceParents[i].localPosition = Vector3.Lerp(posInFront, posOnCircle, lerpS);
            instanceParents[i].localRotation = Quaternion.Euler(0f, -angleHere * Mathf.Rad2Deg, 0f);
            instances[i].localScale = new Vector3(objectsScale, objectsScale, objectsScale) * scaleCurve.Evaluate(lerpS);

            if (i == IndexInFront)
            {
                instances[i].localRotation = Quaternion.Lerp(instances[i].localRotation, additionalRotation, Time.deltaTime * rotateLerpSpeed);
                instanceParents[i].localScale = Vector3.Lerp(instanceParents[i].localScale, new Vector3(scaleupInFront, scaleupInFront, scaleupInFront), Time.deltaTime * scaleupInFrontLerpSpeed);
            }
            else
            {
                instances[i].localRotation = Quaternion.Lerp(instances[i].localRotation, Quaternion.identity, Time.deltaTime * rotateLerpSpeed);
                instanceParents[i].localScale = Vector3.Lerp(instanceParents[i].localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * scaleupInFrontLerpSpeed);
            }

            //instances[i].localPosition = midPos + (new Vector3(Mathf.Sin(angleHere), 0f, -Mathf.Cos(angleHere))) * curRadius;
        }

        float elasticMovement = 0f;
        float snapDelta = 0f;

        if (!HoldDownTrackpad && elasticDelta != 0f && !isSnapping)
        {
            elasticMovement = elasticDelta * scrollSpeed;

            elasticDelta = Mathf.MoveTowards(elasticDelta, 0f, Time.deltaTime * elasticDeccelration);
        }

        if (!HoldDownTrackpad)
        {
            float angleDiv = (curAngle / anglePerObject) + 0.5f + (curAngle < 0f ? -1f : 0f);
            int clamped = (int)angleDiv;
            if (clamped < 0)
            {
                int flipped = -clamped;
                clamped = instances.Length - flipped;
            }
            clamped = clamped % instances.Length;

            float goalAngle = clamped * anglePerObject;

            while (curAngle + Mathf.PI < goalAngle)
            {
                curAngle += Mathf.PI * 2f;
            }
            while (curAngle - Mathf.PI > goalAngle)
            {
                curAngle -= Mathf.PI * 2f;
            }

            snapDelta = Mathf.Lerp(curAngle, goalAngle, snapSpeed * Time.deltaTime) - curAngle;

            snapDelta = Mathf.Clamp(snapDelta, -clampedMaxSnapSpeed, clampedMaxSnapSpeed);
        }

        if (Mathf.Abs(elasticMovement) > Mathf.Abs(snapDelta))
        {
            curAngle += elasticMovement;
        }
        else
        {
            curAngle += snapDelta;
            elasticDelta = 0f;
        }
    }

    public int IndexInFront
    {
        get
        {
            float angleDiv = (curAngle / anglePerObject) + 0.5f + (curAngle < 0f ? -1f : 0f);
            int clamped = (int)angleDiv;
            if (clamped < 0)
            {
                int flipped = -clamped;
                clamped = instances.Length - flipped;
            }
            clamped = clamped % instances.Length;
            return ((instances.Length) - clamped) % instances.Length;
        }
    }

    private bool isTrackpadDown = false;

    public bool HoldDownTrackpad
    {
        get
        {
            return isTrackpadDown;
        }
        set
        {
            bool wasDown = isTrackpadDown;
            isTrackpadDown = value;

            if (isTrackpadDown != wasDown)
            {
                if (isTrackpadDown == false)
                {
                    elasticDelta = averagedDelta();
                }
            }
        }
    }


    private float[] lastDeltas = new float[16];
    private int lastDeltaIndex = 0;

    public void MoveDelta(float delta)
    {
        lastDeltas[lastDeltaIndex] = delta;
        lastDeltaIndex++;
        lastDeltaIndex = lastDeltaIndex % lastDeltas.Length;


        curAngle += delta * scrollSpeed;
    }

    private float averagedDelta()
    {
        float sum = 0f;
        for (int i = 0; i < lastDeltas.Length; i++)
        {
            sum += lastDeltas[i];
        }

        return sum / lastDeltas.Length;
    }

    public void Init()
    {

    }

    public void Show()
    {
        isVisible = true;
    }

    public void Hide()
    {
        isVisible = false;
    }
}
