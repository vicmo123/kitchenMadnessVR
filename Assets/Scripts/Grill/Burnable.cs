using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnable : MonoBehaviour
{
    private enum State
    {
        Grilling,
        Burning
    }

    public float burningTime = 3;
    public float grillingTime = 10;

    float initialGrillingTime;
    float initialBurningTime;

    private State state;

    public Material material1;
    public Material material2;
    public Renderer rend;

    public delegate void StateDelegate();
    [HideInInspector] public StateDelegate grilledDelegate;
    [HideInInspector] public StateDelegate burntDelegate;

    private void Start() {
        if (grillingTime <= 0) {
            state = State.Burning;
        } else {
            state = State.Grilling;
        }

        rend.material = material1;

        initialGrillingTime = grillingTime;
        initialBurningTime = burningTime;
    }

    public void OnGrill(Burner burner) {
        if (state == State.Grilling) {
            grillingTime -= burner.grillingMultiplier * Time.deltaTime;
            if (grillingTime <= 0) {
                Grilled();
            }
        } else if (state == State.Burning) {
            burningTime -= burner.grillingMultiplier * Time.deltaTime;
            if (burningTime <= 0) {
                Burnt();
            }
        }
    }

    private void Grilled() {
        state = State.Burning;

        grilledDelegate.Invoke();
    }

    private void Burnt() {
        Destroy(this);

        burntDelegate.Invoke();
    }

    void Update() {
        float lerp = (initialGrillingTime - grillingTime) / initialGrillingTime;
        rend.material.Lerp(material1, material2, lerp);

        //float burnLevel = 255 * burningTime / initialBurningTime;
        //rend.material.color = new Color(burnLevel, burnLevel, burnLevel);
    }
}
