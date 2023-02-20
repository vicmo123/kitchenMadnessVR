using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnable : MonoBehaviour
{
    private enum State
    {
        Grilling,
        Burning,
        Destroying
    }

    public float destroyingTime = .5f;
    public float burningTime = 3;
    public float grillingTime = 0;

    GameObject firePrefab;

    float initialGrillingTime;
    float startDestroying;

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
        }
        else {
            state = State.Grilling;

            rend.material = material1;
        }

        initialGrillingTime = grillingTime;
        firePrefab = Resources.Load<GameObject>("Prefabs/Burning/Fire");
    }

    public void OnGrill(Burner burner) {
        if (state == State.Grilling) {
            grillingTime -= burner.grillingMultiplier * Time.deltaTime;
            if (grillingTime <= 0) {
                Grilled();
            }
        }
        else if (state == State.Burning) {
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
        Instantiate(firePrefab, this.gameObject.transform.position, firePrefab.transform.rotation);


        //SoundManager.CatchFire?.Invoke();
        state = State.Destroying;
        startDestroying = Time.time;

        //burntDelegate.Invoke();
    }

    private void Destroyed() {
        if (Time.time >= startDestroying + destroyingTime) {
            Destroy(this.gameObject);
        }
    }

    void Update() {
        if (state == State.Grilling) {
            float lerp = (initialGrillingTime - grillingTime) / initialGrillingTime;
            rend.material.Lerp(material1, material2, lerp);
        }
        if (state == State.Destroying) {
            Destroyed();
        }
    }
}
