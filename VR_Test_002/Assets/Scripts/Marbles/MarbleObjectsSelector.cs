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
    private Transform posCurSelected = null;
    [SerializeField]
    private float scaleupCurSelected = 1.2f;
    [SerializeField]
    private bool removeColliders = true;

    //[SerializeField]
    //private GameObject[] prefabs = null;

    private int currentIndex = 0;
    private bool isVisible = false;
    private Transform[] instances = null;
    private int[] instancesSelRotations = null;
    private bool[] instancesSelUpDownFlip = null;
    private Transform[] instanceParents = null;

    private float moveS = 0f;

    private float curAngle = 0f;
    private float elasticDelta = 0f;
    private Vector3 midPos;

    private float anglePerObject = 0f;

    private bool isSnapping = false;
    private float snapAngle = 0f;
    private int oldIndexInFront = -1;

    private float scaleSelectedVisibleS = 0f;

    private Quaternion additionalRotation = Quaternion.identity;


    // Start is called before the first frame update
    void Start()
    {
        instances = new Transform[MarbleParts.Inst.MarblePartPrefabs.Length];
        instancesSelRotations = new int[MarbleParts.Inst.MarblePartPrefabs.Length];
        instancesSelUpDownFlip = new bool[MarbleParts.Inst.MarblePartPrefabs.Length];
        instanceParents = new Transform[MarbleParts.Inst.MarblePartPrefabs.Length];

        for (int i = 0; i < instances.Length; i++)
        {
            GameObject instParent = new GameObject("Parent");
            instParent.transform.parent = parentTrans;
            instanceParents[i] = instParent.transform;
            instanceParents[i].localPosition = Vector3.zero;
            instanceParents[i].localRotation = Quaternion.identity;
            instanceParents[i].localScale = new Vector3(1f, 1f, 1f);

            GameObject inst = Instantiate(MarbleParts.Inst.MarblePartPrefabs[i].prefabSelection, instanceParents[i]);
            instances[i] = inst.transform;
            instances[i].localPosition = Vector3.zero;
            instances[i].localRotation = Quaternion.identity;
            instances[i].localScale = new Vector3(objectsScale, objectsScale, objectsScale);
            if (removeColliders)
            {
                Utils.removeCollidersRec(instances[i].transform);
            }

            if (inst.GetComponent<VRInteractable>() != null)
            {
                inst.GetComponent<VRInteractable>().IsInteractable = false;
            }
            if (inst.GetComponent<Rigidbody>() != null)
            {
                DestroyImmediate(inst.GetComponent<Rigidbody>());
            }

            instancesSelRotations[i] = 0;
            instancesSelUpDownFlip[i] = false;
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

        if (IsPreviewVisible)
        {
            scaleSelectedVisibleS += Time.deltaTime / moveTime;
        }
        else
        {
            scaleSelectedVisibleS -= Time.deltaTime / moveTime;
        }
        scaleSelectedVisibleS = Mathf.Clamp(scaleSelectedVisibleS, 0f, 1f);

        float lerpS = moveCurve.Evaluate(moveS);

        if (oldIndexInFront != IndexInFront)
        {
            oldIndexInFront = IndexInFront;
            additionalRotation = Quaternion.identity;
            Debug.Log("Index In Front: " + IndexInFront.ToString());
        }

        if (lerpS >= 0.5f)
        {
            additionalRotation *= Quaternion.Euler(0f, Time.deltaTime * objectFrontRotationSpeed, 0f);
        }
        else
        {
            additionalRotation = Quaternion.identity;
        }


        //Debug.Log("In front: " + IndexInFront.ToString());

        for (int i = 0; i < instances.Length; i++)
        {
            float angleHere = curAngle + anglePerObject * i;
            Vector3 posOnCircle = midPos + (new Vector3(Mathf.Sin(angleHere), 0f, -Mathf.Cos(angleHere))) * circleRadius;
            Vector3 posInFront = midPos + new Vector3(0f, 0f, -circleRadius);

            if (i == IndexInFront)
            {
                instanceParents[i].localPosition = Vector3.Lerp(posCurSelected.localPosition, posOnCircle, lerpS);
            }
            else
            {
                instanceParents[i].localPosition = Vector3.Lerp(posInFront, posOnCircle, lerpS);
            }

            instanceParents[i].localRotation = Quaternion.Euler(0f, -angleHere * Mathf.Rad2Deg, 0f);

            if (i == IndexInFront)
            {
                instances[i].localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(objectsScale, objectsScale, objectsScale), scaleCurve.Evaluate(lerpS));
                instances[i].localRotation = Quaternion.Lerp(instances[i].localRotation, additionalRotation * Quaternion.Euler(instancesSelUpDownFlip[i] ? 180f : 0f, instancesSelRotations[i] * 90f, 0f), Time.deltaTime * rotateLerpSpeed);

                Vector3 scaleMenuOpen = Vector3.Lerp(instanceParents[i].localScale, new Vector3(scaleupInFront, scaleupInFront, scaleupInFront), Time.deltaTime * scaleupInFrontLerpSpeed);
                Vector3 scaleMenuClosed = Vector3.Lerp(Vector3.zero, new Vector3(scaleupCurSelected, scaleupCurSelected, scaleupCurSelected), moveCurve.Evaluate(scaleSelectedVisibleS));
                
                //instanceParents[i].localScale = Vector3.Lerp(instanceParents[i].localScale, new Vector3(scaleupInFront, scaleupInFront, scaleupInFront), Time.deltaTime * scaleupInFrontLerpSpeed);
                instanceParents[i].localScale = Vector3.Lerp(scaleMenuClosed, scaleMenuOpen, lerpS);
            }
            else
            {
                instances[i].localScale = new Vector3(objectsScale, objectsScale, objectsScale) * scaleCurve.Evaluate(lerpS);
                instances[i].localRotation = Quaternion.Lerp(instances[i].localRotation, Quaternion.identity * Quaternion.Euler(instancesSelUpDownFlip[i] ? 180f : 0f, instancesSelRotations[i] * 90f, 0f), Time.deltaTime * rotateLerpSpeed);
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

    private bool isPreviewVisible = true;
    public bool IsPreviewVisible
    {
        get
        {
            return isPreviewVisible;
        }
        set
        {
            bool wasPreviewVisible = isPreviewVisible;
            isPreviewVisible = value;

            if (isPreviewVisible != wasPreviewVisible)
            {

            }
        }
    }

    public void RotateSelectedObject(bool left)
    {
        if (left)
        {
            instancesSelRotations[IndexInFront]++;
            instancesSelRotations[IndexInFront] %= 4;
        }
        else
        {
            instancesSelRotations[IndexInFront]--;
            if (instancesSelRotations[IndexInFront] < 0)
            {
                instancesSelRotations[IndexInFront] = 3;
            }
        }
    }

    public void RotateSelectedObjectUpDown()
    {
        instancesSelUpDownFlip[IndexInFront] = !instancesSelUpDownFlip[IndexInFront];
    }

    public float SelectedPartRotation
    {
        get
        {
            return instancesSelRotations[IndexInFront] * 90f;
        }
    }


    private float cachedIndexCurAnlge = 0f;
    private int cachedIndexInFront = 0;
    public int IndexInFront
    {
        get
        {
            if (curAngle != cachedIndexCurAnlge)
            {
                float angleDiv = (curAngle / anglePerObject) + 0.5f + (curAngle < 0f ? -1f : 0f);
                int clamped = (int)angleDiv;
                if (clamped < 0)
                {
                    int flipped = -clamped;
                    clamped = instances.Length - flipped;
                }
                clamped = clamped % instances.Length;
                cachedIndexInFront = ((instances.Length) - clamped) % instances.Length;
                cachedIndexCurAnlge = curAngle;
            }

            return cachedIndexInFront;
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
