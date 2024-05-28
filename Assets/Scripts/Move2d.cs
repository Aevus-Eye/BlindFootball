using UnityEngine;
using UnityEngine.InputSystem;

public class Move2d : MonoBehaviour
{
    Rigidbody2D rb;

    public float speed = 1;

    private Vector2 movement_input;
    public int player_touch_id = 0;
    private string player_on_bg = "";

    // Start is called before the first frame update
    void Start() => rb = GetComponent<Rigidbody2D>();

    void FixedUpdate() => rb.velocity = speed * Time.fixedDeltaTime * movement_input;

    void Update()
    {
        // get touch input and move ball to that position
        if (Input.touchCount > player_touch_id)
        {
            Touch touch = Input.GetTouch(player_touch_id);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            LayerMask layerMask = LayerMask.GetMask("BG");
            if (Physics.Raycast(ray, out RaycastHit hit, layerMask: layerMask, maxDistance: 1000f))
            {
                if (player_on_bg == ""){
                    player_on_bg = hit.collider.gameObject.name;
                    var target_touch1 = hit.point;
                    rb.position = target_touch1;
                }
                var target_touch = hit.point;
                rb.MovePosition(target_touch);
                return;
            }
        }

        if (player_on_bg != "")
        {
            player_on_bg = "";
            rb.position = new Vector2(10, 10);
        }
    }

    // 'Move' input action has been triggered.
    public void OnMove(InputValue value) => movement_input = value.Get<Vector2>();
}
