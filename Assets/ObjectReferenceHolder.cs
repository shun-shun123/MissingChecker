using UnityEngine;

public class ObjectReferenceHolder : MonoBehaviour
{
    [SerializeField, Tooltip("これはMissingになる参照")]
    private GameObject willMissingObjectReference;
}
