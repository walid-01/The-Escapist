using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    [SerializeField] private GameObject[] toIgnore;
    private void Start()
    {
        foreach (GameObject obj in toIgnore)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), obj.GetComponent<Collider>());
        }        
    }
}
