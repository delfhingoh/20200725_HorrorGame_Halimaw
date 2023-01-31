using UnityEngine;

public class ObjectOutlineScript : MonoBehaviour
{
    [SerializeField] private float outlineWidth;

    private Renderer rend;
    private float defaultWidth;
    private float width;

    private bool isOutlineOn;

    private void Start()
    {
        if (outlineWidth == 0f)
            outlineWidth = 0.02f;

        rend = this.gameObject.GetComponent<Renderer>();
        defaultWidth = 0f;
        width = defaultWidth;

        isOutlineOn = false;
    }

    private void Update()
    {
        rend.material.SetFloat("_OutlineWidth", width);

        if (GetIsOutlineOn())
            width = outlineWidth;
        else
            width = defaultWidth;
    }

    public bool GetIsOutlineOn()
    {
        return isOutlineOn;
    }

    public void SetIsOutlineOn(bool _isOutlineOn)
    {
        isOutlineOn = _isOutlineOn;
    }
}
