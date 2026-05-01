using UnityEngine;

public class IKConstraintLeg : MonoBehaviour
{
    [Header("Raycast Settings")]
    public LayerMask groundLayer;
    public float rayDistance = 2f;
    
    [Header("Offsets")]
    public float sideOffset = 0.5f;
    public float forwardOffset = 0.5f;

    [Header("Step Settings")]
    public float stepDistance = 0.5f;
    public float stepSpeed = 5f;
    public float stepHeight = 0.3f;

    [SerializeField] Transform leftLegConstraint;
    [SerializeField] Transform rightLegConstraint;
    
    [HideInInspector] public Vector3 leftLegTarget;
    [HideInInspector] public Vector3 rightLegTarget;

    private bool isLeftStepping;
    private float leftLerp;
    private Vector3 leftStartPos;
    private Vector3 leftEndPos;

    private bool isRightStepping;
    private float rightLerp;
    private Vector3 rightStartPos;
    private Vector3 rightEndPos;

    void Start()
    {
        leftEndPos = leftLegConstraint.position;
        rightEndPos = rightLegConstraint.position;
    }

    void Update()
    {
        Vector3 leftOrigin = transform.position - (transform.right * sideOffset) + (transform.forward * forwardOffset);
        Vector3 rightOrigin = transform.position + (transform.right * sideOffset) + (transform.forward * forwardOffset);

        if (Physics.Raycast(leftOrigin, Vector3.down, out RaycastHit leftHit, rayDistance, groundLayer))
        {
            leftLegTarget = leftHit.point;
        }

        if (Physics.Raycast(rightOrigin, Vector3.down, out RaycastHit rightHit, rayDistance, groundLayer))
        {
            rightLegTarget = rightHit.point;
        }

        if (!isLeftStepping && !isRightStepping && Vector3.Distance(leftEndPos, leftLegTarget) > stepDistance)
        {
            isLeftStepping = true;
            leftStartPos = leftEndPos;
            leftEndPos = leftLegTarget;
            leftLerp = 0f;
        }

        if (!isRightStepping && !isLeftStepping && Vector3.Distance(rightEndPos, rightLegTarget) > stepDistance)
        {
            isRightStepping = true;
            rightStartPos = rightEndPos;
            rightEndPos = rightLegTarget;
            rightLerp = 0f;
        }

        if (isLeftStepping)
        {
            leftLerp += Time.deltaTime * stepSpeed;
            Vector3 currentPos = Vector3.Lerp(leftStartPos, leftEndPos, leftLerp);
            currentPos.y += Mathf.Sin(leftLerp * Mathf.PI) * stepHeight;
            leftLegConstraint.position = currentPos;

            if (leftLerp >= 1f) isLeftStepping = false;
        }
        else 
        {
            leftLegConstraint.position = leftEndPos;
        }

        if (isRightStepping)
        {
            rightLerp += Time.deltaTime * stepSpeed;
            Vector3 currentPos = Vector3.Lerp(rightStartPos, rightEndPos, rightLerp);
            currentPos.y += Mathf.Sin(rightLerp * Mathf.PI) * stepHeight;
            rightLegConstraint.position = currentPos;

            if (rightLerp >= 1f) isRightStepping = false;
        }
        else
        {
            rightLegConstraint.position = rightEndPos;
        }
    }
}