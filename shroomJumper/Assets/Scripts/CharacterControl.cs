using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float speed = 10f;
    public float jumpStrength = 20f;
    private bool grounded;
    private CapsuleCollider2D col;
    private Vector2 moveVec;

    public GameObject Text;

    // Start is called before the first frame update
    void Start()
    {
        //Text.SetActive(false);
        col = GetComponent<CapsuleCollider2D>();//assign compenent to variable
    }

    // Update is called once per frame
    void Update()
    {
        moveVec.x = Input.GetAxisRaw("Horizontal") * speed; //x input 

		if (grounded)
		{
            moveVec.y = 0;

            if (Input.GetKey(KeyCode.Space))
            {
                moveVec.y = Mathf.Sqrt(2 * jumpStrength * Mathf.Abs(Physics2D.gravity.y));
            }
		}

        if (Input.GetKey(KeyCode.Space)&&(!grounded))
        {
            moveVec.y = Mathf.Sqrt(2 * jumpStrength * Mathf.Abs(Physics2D.gravity.y));
        }

        moveVec.y += Physics2D.gravity.y * Time.deltaTime;

        float speedClamp = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y));
        moveVec.y = Mathf.Clamp(moveVec.y, -speedClamp, speedClamp); //so does constsntly accelerate
        transform.Translate(moveVec * Time.deltaTime); //moves both axises

        grounded = false;
        /* custom collision detection */
        Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position, col.size, 0);
        foreach(Collider2D otherCol in collisions)
		{
            if (otherCol == col)
            {
                continue;
            }

            ColliderDistance2D colliderDistance = otherCol.Distance(col);

			if (colliderDistance.isOverlapped)
			{
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
                if(Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && moveVec.y < 0)
				{
                    grounded = true; 
				}
			}
            if((colliderDistance.pointA.y < colliderDistance.pointB.y) && moveVec.y > 0)
			{
                moveVec.y = 0;
			}
		}
      	
    }
}
