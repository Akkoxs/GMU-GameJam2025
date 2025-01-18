using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float skinWidth = 0.15f;
    BoxCollider2D pCollider;
    RaycastOrigins raycastOrigins;

    float horizontalRayCount = 4;
    float verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRaycastOrigins();
        CalculateRaySpacing();
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = pCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = pCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / ( horizontalRaySpacing - 1);
        verticalRaySpacing = bounds.size.x / ( verticalRaySpacing - 1);
    }
    
    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
