using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour {

    public GameManager gm;
    public UnityEngine.Experimental.Rendering.Universal.Light2D star_light;
    SpriteRenderer[] sprites;

    // Start is called before the first frame update
    void Start() {
        gm = GameManager.instance;
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        star_light.intensity = Mathf.Clamp(Mathf.Abs(gm.global_light.intensity - 1), 0, 1);
        foreach (SpriteRenderer sprite in sprites) {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, star_light.intensity);
        }
    }
}
