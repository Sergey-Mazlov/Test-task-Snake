using UnityEngine;
public class Wall : MonoBehaviour
{
    public Transform snakeTail;
    public int distance;

    private Material _material;

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        /*if (snakeTail.position.z <= distance)
        {
            (Color first, Color second) = GameManager.Instance.SetRandomColors();
            _material.color = first;
        }*/
    }
}
