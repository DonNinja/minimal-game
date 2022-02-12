using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogScript : MonoBehaviour {
    GameManager gm;

    [SerializeField] AudioSource bark;
    [SerializeField] UnityEngine.Experimental.Rendering.Universal.Light2D dog_light;

    private void Awake() {
        Vector3 l_scale = gameObject.transform.localScale;
        float x_scale = l_scale.x;
        float mult = Random.Range(1, 100) % 2 == 0 ? 1 : -1;
        x_scale *= mult;
        gameObject.transform.localScale = new Vector3(x_scale, l_scale.y, l_scale.z);
    }

    private void Start() {
        gm = GameManager.instance;
    }

    void Update() {
        dog_light.intensity = Mathf.Clamp(Mathf.Abs(gm.global_light.intensity - 1), 0, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (gm.game_going) {
            gm.final_text = "A dog has been crushed!";
            bark.Play();
            gm.endGame();
        }
    }
}
