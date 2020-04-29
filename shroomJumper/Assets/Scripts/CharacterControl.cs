using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CharacterControl : MonoBehaviour
{
    Scene currentScene;
    string sceneName;

    public float walkAccel = 20f; //acceleration while grounded
    public float jumpAccel = 10f; //acceleration while airborne
    public float speed = 8f; // max speed
    public float jumpStrength = 1f; //the height jumped regardless of gravity
    public float groundDecel = 70f; //deceleration that occurs when no input is made

    private bool grounded;
    private CapsuleCollider2D col;
    private Vector2 moveVec;

    public GameObject Text;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //Text.SetActive(false);
        col = GetComponent<CapsuleCollider2D>();//assign compenent to variable
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
    }

    // Update is called once per frame
    void Update()
    {

        float moveInput = Input.GetAxisRaw("Horizontal"); //x input 

        if (grounded)
        {
            moveVec.y = 0;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveVec.y = Mathf.Sqrt(2 * jumpStrength * Mathf.Abs(Physics2D.gravity.y));
            }
        }


        //The corresponding acceleration/deceleration is chosen based on if grounded
        float accel = grounded ? walkAccel : jumpAccel;
        float decel = grounded ? groundDecel : 0;



        if (moveInput != 0)
        {
            moveVec.x = Mathf.MoveTowards(moveVec.x, speed * moveInput, (2.5f + (accel * Time.deltaTime)));
        }
        else
        {
            moveVec.x = Mathf.MoveTowards(moveVec.x, 0, decel * Time.deltaTime);
        }

        moveVec.y += Physics2D.gravity.y * Time.deltaTime;

        //float speedClamp = Mathf.Sqrt(4 * Mathf.Abs(Physics2D.gravity.y));
        //moveVec.y = Mathf.Clamp(moveVec.y, -speedClamp, speedClamp); //so does constsntly accelerate

        transform.Translate(moveVec * Time.deltaTime);

        grounded = false;
        /* custom collision detection */
        Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position, col.size, 0);
        foreach (Collider2D otherCol in collisions)
        {
            if (otherCol == col)
            {
                continue;
            }

            ColliderDistance2D colliderDistance = otherCol.Distance(col);

            if (colliderDistance.isOverlapped && ! otherCol.isTrigger)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && moveVec.y < 0)
                {
                    grounded = true;
                }
            }
            if ((colliderDistance.pointA.y < colliderDistance.pointB.y) && moveVec.y > 0)
            {
                moveVec.y = 0;
            }
        }
        
        
        if(player.transform.position.y < -20f){
            SceneManager.LoadScene(sceneName);
        }
    }
}
