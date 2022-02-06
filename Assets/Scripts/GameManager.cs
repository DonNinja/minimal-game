using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public int score = 0;
    public bool generate_new = true;
    public List<Rigidbody2D> rigid_stack;
    public Camera main_camera;
    [SerializeField] [Range(0, 10)] int locking_mod;
    [SerializeField] GameObject block_type;
    [SerializeField] TextMeshProUGUI score_field;
    [SerializeField] GameObject block_pos;
    float block_rotation = 0f;
    int counter = 0;

    // Start is called before the first frame update
    void Start() {
        score_field.text = score.ToString();
    }

    // Update is called once per frame
    void Update() {
    }

    private void FixedUpdate() {
        if (generate_new) {
            GameObject new_obj = Instantiate(block_type, block_pos.transform.position, Quaternion.Euler(new Vector3(0, 0, block_rotation)));
            counter++;
            generate_new = false;

            if (counter % locking_mod == 0) {
                BlockScript bs = new_obj.GetComponent<BlockScript>();
                bs.setLocking();
            }

            block_rotation += 180f;
        }
    }

    public void updateScore() {
        score += 1;
        score_field.text = score.ToString();
    }

    public void freezeRigids() {
        for (int i = 0; i < rigid_stack.Count; i++) {
            rigid_stack[i].bodyType = RigidbodyType2D.Static;
        }
        rigid_stack.Clear();
    }
}
