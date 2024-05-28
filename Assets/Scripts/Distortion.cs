using UnityEngine;

public class Distortion : MonoBehaviour
{
    public GameObject distortionEffect;
    public float distance = 0.1f;

    Animator anim;

    void Start()
    {
        distortionEffect.SetActive(false);
        anim = distortionEffect.GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            distortionEffect.SetActive(true);
            var col = other.GetContact(0);
            var pos = col.point + col.normal * distance;
            distortionEffect.transform.position = new Vector3(pos.x, pos.y, distortionEffect.transform.position.z);
            anim.SetTrigger("Distort");
        }
    }
}
