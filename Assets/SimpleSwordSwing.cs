using UnityEngine;

public class SimpleSwordSwing : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRot;

    public Vector3 attackOffset = new Vector3(0f, 0f, 0.35f);
    public Vector3 attackRotation = new Vector3(8f, 0f, -12f);
    public float speed = 18f;

    private bool attacking = false;
    private bool returning = false;

    void Start()
    {
        startPos = transform.localPosition;
        startRot = transform.localRotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !attacking && !returning)
        {
            attacking = true;
        }

        if (attacking)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos + attackOffset, Time.deltaTime * speed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, startRot * Quaternion.Euler(attackRotation), Time.deltaTime * speed);

            if (Vector3.Distance(transform.localPosition, startPos + attackOffset) < 0.03f)
            {
                attacking = false;
                returning = true;
            }
        }

        if (returning)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime * speed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, startRot, Time.deltaTime * speed);

            if (Vector3.Distance(transform.localPosition, startPos) < 0.03f)
            {
                transform.localPosition = startPos;
                transform.localRotation = startRot;
                returning = false;
            }
        }
    }
}