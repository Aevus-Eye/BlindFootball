using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveAndHideTester : MonoBehaviour
{
    AudioPanner audioPanner;
    MeshRenderer[] meshRenderer;
    LineRenderer lineRenderer;

    bool isHiding = false;

    Vector3 start, end;

    // Start is called before the first frame update
    void Start()
    {
        audioPanner = GetComponent<AudioPanner>();
        meshRenderer = GetComponentsInChildren<MeshRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isHiding)
            {
                foreach (var r in meshRenderer)
                    r.enabled = true;
                isHiding = false;
            }
            else
            {
                foreach (var r in meshRenderer)
                    r.enabled = false;
                isHiding = true;
                float x = Random.Range(-audioPanner.radius_x, audioPanner.radius_x);
                float y = Random.Range(-audioPanner.radius_y, audioPanner.radius_y);
                transform.position = new Vector3(x, y, transform.position.z);
                lineRenderer.positionCount = 0;
            }
        }

        // check mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // raycast
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                foreach (var r in meshRenderer)
                    r.enabled = true;
                isHiding = false;

                start = transform.position;
                end = hit.point;

                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, start);
                lineRenderer.SetPosition(1, end);
                
                Debug.Log($"Distance: {Vector3.Distance(start, end):F2}");
            }
        }
    }
}
