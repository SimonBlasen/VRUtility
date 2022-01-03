using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private Transform body;
    [SerializeField]
    private IKFootSolver otherFoot;
    [SerializeField]
    private float speed = 5f, stepDistance = 0.3f, stepLength = 0.3f, stepHeight = 0.3f;
    [SerializeField]
    private Vector3 footPosOffset, footRotOffset;
    [SerializeField]
    private Vector3 constFootRotation;

    private float _footSpacing, _lerp;
    private Vector3 _oldPos, _currentPos, _newPos;
    private Vector3 _oldNorm, _currentNorm, _newNorm;
    private Transform _bodyFlat;
    private Quaternion _constFootRot;

    // Start is called before the first frame update
    void Start()
    {
        GameObject bodyFlatGameobject = new GameObject("Body Flat");
        _bodyFlat = bodyFlatGameobject.transform;

        _footSpacing = transform.localPosition.x;
        _currentPos = _newPos = _oldPos = transform.position;
        _currentNorm = _newNorm = _oldNorm = transform.up;
        _lerp = 1f;
        _constFootRot = Quaternion.Euler(constFootRotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _currentPos;

        Vector3 bodyForward = body.up;
        bodyForward.y = 0f;
        bodyForward.Normalize();
        //bodyFlat.forward = bodyForward;
        //Vector3 bodyRight = Vector3.Cross(Vector3.up, bodyForward);
        _bodyFlat.rotation = Quaternion.LookRotation(bodyForward, Vector3.up);
        _bodyFlat.position = body.position;


        transform.rotation = Quaternion.LookRotation(_constFootRot * _bodyFlat.forward, _constFootRot * _currentNorm);

        Ray ray = new Ray(_bodyFlat.position + (_bodyFlat.right * _footSpacing) + Vector3.up * 2f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 10f, groundLayer.value))
        {
            if (Vector3.Distance(_newPos, hit.point) > stepDistance && !otherFoot.IsMoving() && !IsMoving())
            {
                _lerp = 0f;
                int direction = _bodyFlat.InverseTransformPoint(hit.point).z > _bodyFlat.InverseTransformPoint(_newPos).z ? 1 : -1;
                _newPos = hit.point + (_bodyFlat.forward * direction * stepLength) + footPosOffset;
                _newNorm = hit.normal + footRotOffset;
            }
        }

        if (_lerp < 1f)
        {
            Vector3 tempPos = Vector3.Lerp(_oldPos, _newPos, _lerp);
            tempPos.y += Mathf.Sin(_lerp * Mathf.PI) * stepHeight;

            _currentPos = tempPos;
            _currentNorm = Vector3.Lerp(_oldNorm, _newNorm, _lerp);

            _lerp += Time.deltaTime * speed;
        }
        else
        {
            _oldPos = _newPos;
            _oldNorm = _newNorm;
        }
    }

    public bool IsMoving()
    {
        return _lerp < 1f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_newPos, 0.1f);
    }
}
