using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public int score = 0;
    public int locking_mod = 1;
    public bool generate_new = true;
    public float block_speed;
    public float block_speed_inc;
    public bool reverse_block = false;
    public bool game_going = true;
    public List<Rigidbody2D> rigid_stack;
    public List<GameObject> stars;
    public UnityEngine.Experimental.Rendering.Universal.Light2D global_light;
    public string final_text;
    [SerializeField] Camera main_camera;
    [SerializeField] GameObject block;
    [SerializeField] GameObject dog_block;
    [SerializeField] TextMeshProUGUI score_field;
    [SerializeField] GameObject block_pos;
    [SerializeField] [Range(0, 3)] float camera_movement;
    [SerializeField] [Range(0, 0.1f)] float camera_speed;
    [SerializeField] float space_height;
    [SerializeField] GameObject blocker;
    [SerializeField] SpriteRenderer bg;
    [SerializeField] Button b;
    [SerializeField] TextMeshProUGUI final_score;
    public Animator shake;
    bool moving_up;
    float target_height;
    float block_rotation = 0f;
    int counter = 0;
    int total_counter = 0;

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        score_field.text = score.ToString();
        b.onClick.AddListener(Restart);
    }

    // Update is called once per frame
    void Update() {
    }

    private void FixedUpdate() {
        float colour_mod = main_camera.transform.position.y / space_height;
        float b = Mathf.Clamp(1 - colour_mod, 0.1f, 1);
        float g = Mathf.Clamp(0.25f - colour_mod, 0.1f, 1);
        if (moving_up) {
            global_light.intensity = b;

            if (main_camera.transform.position.y < target_height) {
                main_camera.transform.position = main_camera.transform.position + new Vector3(0, camera_speed, 0);
            }

            bg.color = new Color(0, g, b);
        }

        if (generate_new && game_going) {
            GameObject next_block = block;
            if (counter % locking_mod == 0) {
                next_block = dog_block;
                BlockScript bs = next_block.GetComponent<BlockScript>();
                bs.setLocking();
                reverse_block = !reverse_block;
            }

            Instantiate(next_block, block_pos.transform.position, Quaternion.Euler(new Vector3(0, 0, next_block == dog_block ? 0f : block_rotation)));
            counter++;
            total_counter++;
            generate_new = false;
            moving_up = true;

            block_rotation += 180f;
        }
    }

    public void moveCamera() {
        if (total_counter > 3) {
            target_height = main_camera.transform.position.y + camera_movement;
        }
    }

    public void updateScore() {
        score += 1;
        score_field.text = score.ToString();
    }

    public void speedUp() {
        block_speed += block_speed_inc;
        locking_mod += 2;
        counter = 1;
    }

    public void endGame() {
        block_speed = 0f;
        game_going = false;
        blocker.SetActive(true);
        score_field.color = new Color(1, 1, 1);
        final_score.text = final_text + "\nScore:" + score.ToString();
    }

    private void Restart() {
        SceneManager.LoadScene(0);
    }
}
