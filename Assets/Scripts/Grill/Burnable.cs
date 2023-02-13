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

    public float burningTime = 0;
    public float grillingTime = 0;
    private State state;

    private void Start() {
        if (grillingTime <= 0) {
            state = State.Burning;
        } else {
            state = State.Grilling;
        }
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
    }

    private void Burnt() {
        Destroy(this);
    }
}
