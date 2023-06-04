using UnityEngine;

public class FOVMeshRenderer : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private bool drawGizmos;
    [Min(3)]
    [SerializeField] private int segmentsCount;

    private Mesh _mesh;
    private int[] _newTriangles;
    private Vector3[] _noRaycastVertices;
    private Vector3[] _raycastVertices;
    private Transform _followTransform;
    private Transform _transformThis;
    private float _distance;
    private int _segmentsPlusOne;

    private void Start()
    {
        _segmentsPlusOne = segmentsCount + 1;
    }

    public void UpdateVisualization()
    {
        _transformThis.position = _followTransform.position;
        _transformThis.rotation = _followTransform.rotation;
        UpdateMesh();
    }
    
    private void UpdateMesh()
    {
        Vector3 transformPosition = _transformThis.position;
        
        for (int i = 0; i < _segmentsPlusOne; i++)
        {
            int iPlusOne = i + 1;
            if (Physics.Raycast(transformPosition,
                    _transformThis.TransformPoint(_noRaycastVertices[iPlusOne]) - transformPosition,
                    out RaycastHit hit, _distance, hitMask))
            {
                _raycastVertices[iPlusOne] = _transformThis.InverseTransformPoint(hit.point);
            }
            else
                _raycastVertices[iPlusOne] = _noRaycastVertices[iPlusOne];
        }
        
        _mesh.vertices = _raycastVertices;
    }

    public void UpdateInfo(Transform target, float angle, float distance)
    {
        _followTransform = target;
        _distance = distance;
        _transformThis = transform;

        float segmentAngle = angle / segmentsCount;
        float segmentCountMedium = segmentsCount / 2f;
        Vector3 forwardVector = Vector3.forward * distance;
        Vector3 transformUp = _transformThis.up;

        _noRaycastVertices = new Vector3[segmentsCount + 2];
        _raycastVertices = new Vector3[segmentsCount + 2];
        _noRaycastVertices[0] = Vector3.zero;
        _raycastVertices[0] = Vector3.zero;
        for (int i = 0; i < segmentsCount + 1; i++)
        {
            _noRaycastVertices[i + 1] = Quaternion.AngleAxis(segmentAngle * (segmentCountMedium - i), transformUp) * forwardVector;
        }

        _newTriangles = new int[(segmentsCount + 1) * 3];
        for (int i = 0; i < segmentsCount; i++)
        {
            _newTriangles[i*3] = i + 2;
            _newTriangles[i*3+1] = i + 1;
            _newTriangles[i*3+2] = 0;
        }
        
        _mesh = new Mesh
        {
            vertices = _noRaycastVertices,
            triangles = _newTriangles
        };

        meshFilter.mesh = _mesh;
    }

    private void OnDrawGizmos()
    {
        if(!drawGizmos) return;
        
        Gizmos.color = Color.red;
        for (int i = 0; i < segmentsCount + 2; i++)
        {
            Gizmos.DrawSphere(transform.position + _noRaycastVertices[i], .2f);
        }
    }
}
