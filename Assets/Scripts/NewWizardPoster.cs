using UnityEngine;
using UnityEngine.UI;

public class NewWizardPoster : MonoBehaviour
{
    public GameObject wizardPrefab;
    public int cost = 100;

    public float leftEdge = -4.5f;
    public float cellWidth = 1.23f;
    public int columns = 8;

    public float[] laneY =
    {
        3f, 1.75f, 0.5f, -1f, -2.25f, -3.5f
    };

    private GameObject draggedInstance;
    private SpriteRenderer draggedSprite;
    private Collider2D draggedCollider;

    private bool isDragging = false;
    private bool waitingForRelease = false;

    private Image img;
    private Button btn;

    void Start()
    {
        img = GetComponent<Image>();
        btn = GetComponent<Button>();
    }

    void Update()
    {
        UpdateAffordability();

        if (draggedInstance == null) return;

        if (waitingForRelease)
        {
            if (Input.GetMouseButtonUp(0))
            {
                waitingForRelease = false;
                isDragging = true;
            }
            return;
        }

        if (isDragging)
        {
            Vector3 pos = SnapToLaneGrid(GetMouseWorldPos());
            draggedInstance.transform.position = pos;

            bool valid = IsInsideGrid(pos) && !IsTileOccupied(pos);

            if (draggedSprite != null)
            {
                draggedSprite.color = valid ?
                    new Color(0f,1f,0f,0.6f) :
                    new Color(1f,0f,0f,0.6f);
            }

            if (Input.GetMouseButtonUp(0))
                PlaceWizard();
        }
    }

    void UpdateAffordability()
    {
        bool canAfford = GameManager.instance.money >= cost;

        if (img != null)
            img.color = canAfford ? Color.white : Color.gray;

        if (btn != null)
            btn.interactable = canAfford;
    }

    public void BuyWizard()
    {
        if (GameManager.instance.money < cost)
            return;

        GameManager.instance.SpendMoney(cost);

        draggedInstance = Instantiate(wizardPrefab, GetMouseWorldPos(), Quaternion.identity);

        draggedSprite = draggedInstance.GetComponent<SpriteRenderer>();
        draggedCollider = draggedInstance.GetComponent<Collider2D>();

        if (draggedSprite != null)
            draggedSprite.color = new Color(1f,1f,1f,0.5f);

        if (draggedCollider != null)
            draggedCollider.enabled = false;

        waitingForRelease = true;
        isDragging = false;
    }

    void PlaceWizard()
    {
        Vector3 pos = draggedInstance.transform.position;

        if (IsInsideGrid(pos) && !IsTileOccupied(pos))
        {
            draggedSprite.color = Color.white;
            draggedCollider.enabled = true;
        }
        else
        {
            GameManager.instance.money += cost;
            GameManager.instance.UpdateMoneyUI();
            Destroy(draggedInstance);
        }

        draggedInstance = null;
        isDragging = false;
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0;
        return mouse;
    }

    Vector3 SnapToLaneGrid(Vector3 position)
    {
        int column = Mathf.RoundToInt((position.x - leftEdge) / cellWidth);
        column = Mathf.Clamp(column, 0, columns - 1);

        float x = leftEdge + column * cellWidth;
        float y = GetClosestLane(position.y);

        return new Vector3(x, y, 0);
    }

    float GetClosestLane(float yPos)
    {
        float closest = laneY[0];
        float min = Mathf.Abs(yPos - closest);

        foreach (float lane in laneY)
        {
            float d = Mathf.Abs(yPos - lane);
            if (d < min)
            {
                min = d;
                closest = lane;
            }
        }

        return closest;
    }

    bool IsInsideGrid(Vector3 pos)
    {
        float maxX = leftEdge + (columns - 1) * cellWidth;
        return pos.x >= leftEdge && pos.x <= maxX;
    }

    bool IsTileOccupied(Vector3 pos)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(pos, 0.2f);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Wizard"))
                return true;
        }

        return false;
    }
}