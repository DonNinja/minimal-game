using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour {
    public enum Type { Normal, Locking }

    GameManager gm;
    public Type block_type = Type.Normal;
    [SerializeField] AudioSource block_clip;
    [SerializeField] Rigidbody2D block_rb;
    [SerializeField] GameObject left_side;
    [SerializeField] GameObject right_side;
    [SerializeField] AudioSource bark;
    bool landed = false;
    bool fell = false;
    bool going_left = true;
    float movement;
    float max_rot = 20f;
    //* Mathf.Deg2Rad
    Vector2 init_rot;

    private void Awake() {
        gm = GameManager.instance;
        Vector3 l_scale = gameObject.transform.localScale;
        float x_scale = l_scale.x;
        float mult = gm.reverse_block ? 1 : -1;
        x_scale *= mult;
        gameObject.transform.localScale = new Vector3(x_scale, l_scale.y, l_scale.z);
    }

    // Start is called before the first frame update
    void Start() {
        init_rot = gameObject.transform.up;
        right_side = GameObject.Find("BlockStart");
        left_side = GameObject.Find("BlockEnd");
        block_rb = gameObject.GetComponent<Rigidbody2D>();
        movement = gm.block_speed;
    }

    // Update is called once per frame
    void Update() {
        if (!landed && Input.GetKeyDown(KeyCode.Mouse0) && gm.game_going) {
            dropDown();
        }

        if (!gm.game_going) {
            movement = 0;
        }
    }

    void FixedUpdate() {
        if (!fell) {
            block_rb.position += going_left ? new Vector2(-movement, 0) : new Vector2(movement, 0);
        }
        if (block_rb.position.x <= left_side.transform.position.x) {
            block_rb.position = right_side.transform.position;
        }

        float curr_rot = Vector2.Angle(init_rot, gameObject.transform.up);

        if (curr_rot > max_rot || curr_rot < -max_rot) {
            gm.final_text = "The tower has fallen!";
            gm.endGame();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (gm.game_going) {
            block_clip.Play();
            if (!landed) {
                gm.updateScore();
                gm.moveCamera();
                landed = true;
                gm.shake.SetBool("Landed", true);

                gm.generate_new = true;
                if (block_type == Type.Locking) {
                    gm.speedUp();
                }
            }
        }
    }

    void dropDown() {
        block_rb.bodyType = RigidbodyType2D.Dynamic;
        fell = true;
    }

    public void setLocking() {
        block_type = Type.Locking;
    }
}
