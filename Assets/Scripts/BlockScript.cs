using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour {
    public enum Type { Normal, Locking }

    public GameManager gm;
    public Type block_type = Type.Normal;
    [SerializeField] AudioSource block_clip;
    [SerializeField] Rigidbody2D block_rb;
    [SerializeField] GameObject left_side;
    [SerializeField] GameObject right_side;
    [SerializeField] SpriteRenderer sprite;
    bool landed = false;
    bool fell = false;
    bool going_left = true;
    float movement = 0.1f;

    // Start is called before the first frame update
    void Start() {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        right_side = GameObject.Find("BlockStart");
        left_side = GameObject.Find("BlockEnd");
        block_rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        if (!landed && Input.GetKeyDown(KeyCode.Mouse0)) {
            dropDown();
        }
    }

    void FixedUpdate() {
        if (!fell) {
            block_rb.position += going_left ? new Vector2(-movement, 0) : new Vector2(movement, 0);
        }
        if (block_rb.position.x <= left_side.transform.position.x) {
            going_left = false;
        }
        else if (block_rb.position.x >= right_side.transform.position.x) {
            going_left = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        block_clip.Play();
        if (!landed) {
            gm.updateScore();
            landed = true;
            gm.rigid_stack.Add(block_rb);
            gm.generate_new = true;
            if (block_type == Type.Locking) {
                gm.freezeRigids();
            }
        }
    }

    void dropDown() {
        block_rb.bodyType = RigidbodyType2D.Dynamic;
        fell = true;
    }

    public void setLocking() {
        block_type = Type.Locking;
        sprite.color = new Color(0, 1, 0);
    }
}
