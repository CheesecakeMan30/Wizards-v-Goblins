using UnityEngine;

public class WizardPoster : MonoBehaviour
{
    [Header("Wizard")]
    public GameObject wizardPrefab;
    public int cost = 100;

    [Header("Grid")]
    public float leftEdge = -4.5f;
    public float cellWidth = 1.23f;
    public int columns = 8;

    [Header("Lanes")]
    public float[] laneY = new float[]
    {
        3f, 1.75f, 0.5f, -1f, -2.25f, -3.5f
    };

    [Header("Placement")]
    public float shootDelay = 0.2f;

    private GameObject draggedInstance;
    private bool isDragging = false;
    private SpriteRenderer draggedSprite;
    private Collider2D draggedCollider;
    private Wizard shootScript;
    private Collider2D detectionCollider;

    void OnMouseDown()
    {
        if (!GameManager.instance.SpendMoney(cost))
            return;

        draggedInstance = Instantiate(wizardPrefab, GetMouseWorldPos(), Quaternion.identity);

        draggedSprite = draggedInstance.GetComponent<SpriteRenderer>();
        draggedCollider = draggedInstance.GetComponent<Collider2D>();
        shootScript = draggedInstance.GetComponent<Wizard>();

        // ✅ FIX: get detection AFTER instantiate
        WizardDetection detection = draggedInstance.GetComponentInChildren<WizardDetection>();
        if (detection != null)
            detectionCollider = detection.GetComponent<Collider2D>();

        // 👻 Ghost look
        if (draggedSprite != null)
            draggedSprite.color = new Color(1f, 1f, 1f, 0.5f);

        // 🚫 Disable body collider
        if (draggedCollider != null)
            draggedCollider.enabled = false;

        // 🚫 Disable detection collider (prevents shooting completely)
        if (detectionCollider != null)
            detectionCollider.enabled = false;

        // 🚫 Disable shooting via flag (BEST METHOD)
        if (shootScript != null)
            shootScript.canShoot = false;

        isDragging = true;
    }

    void Update()
    {
        if (isDragging && draggedInstance != null)
        {
            Vector3 mousePos = GetMouseWorldPos();
            Vector3 snappedPos = SnapToLaneGrid(mousePos);

            draggedInstance.transform.position = snappedPos;

            bool valid = IsInsideGrid(snappedPos) && !IsTileOccupied(snappedPos);

            if (draggedSprite != null)
            {
                if (valid)
                    draggedSprite.color = new Color(0f, 1f, 0f, 0.6f);
                else
                    draggedSprite.color = new Color(1f, 0f, 0f, 0.6f);
            }
        }
    }

    void OnMouseUp()
    {
        if (!isDragging || draggedInstance == null) return;

        isDragging = false;

        Vector3 snappedPos = SnapToLaneGrid(draggedInstance.transform.position);

        if (IsInsideGrid(snappedPos) && !IsTileOccupied(snappedPos))
        {
            draggedInstance.transform.position = snappedPos;

            // Restore visuals
            if (draggedSprite != null)
                draggedSprite.color = Color.white;

            // Enable collider AFTER placement
            if (draggedCollider != null)
                draggedCollider.enabled = true;

            // Re-enable detection AFTER placement
            if (detectionCollider != null)
                detectionCollider.enabled = true;

            // 🔥 FORCE detection immediately
            if (shootScript != null)
                shootScript.DetectExistingEnemies();

            // ⏱ Start shooting after delay
            if (shootScript != null)
                StartCoroutine(EnableShootingAfterDelay());
        }
        else
        {
            GameManager.instance.money += cost;
            GameManager.instance.UpdateMoneyUI();
            Destroy(draggedInstance);
        }
    }

    System.Collections.IEnumerator EnableShootingAfterDelay()
    {
        yield return new WaitForSeconds(shootDelay);

        if (shootScript != null)
            shootScript.canShoot = true;
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0f;
        return mouse;
    }

    Vector3 SnapToLaneGrid(Vector3 position)
    {
        int column = Mathf.RoundToInt((position.x - leftEdge) / cellWidth);
        column = Mathf.Clamp(column, 0, columns - 1);

        float x = leftEdge + column * cellWidth;
        float y = GetClosestLane(position.y);

        return new Vector3(x, y, 0f);
    }

    float GetClosestLane(float yPos)
    {
        float closest = laneY[0];
        float minDist = Mathf.Abs(yPos - closest);

        foreach (float lane in laneY)
        {
            float dist = Mathf.Abs(yPos - lane);
            if (dist < minDist)
            {
                minDist = dist;
                closest = lane;
            }
        }

        return closest;
    }

    bool IsInsideGrid(Vector3 pos)
    {
        float minX = leftEdge;
        float maxX = leftEdge + (columns - 1) * cellWidth;

        return pos.x >= minX - 0.01f && pos.x <= maxX + 0.01f;
    }

    bool IsTileOccupied(Vector3 pos)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(pos, 0.2f);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Wizard") && hit.gameObject != draggedInstance)
            {
                return true;
            }
        }

        return false;
    }
}