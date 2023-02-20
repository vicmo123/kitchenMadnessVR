using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    private int MAX_NB_STARS = 5;
    private int current_nb_stars = 5;

    public int Current_nb_stars { get => current_nb_stars; set => current_nb_stars = value; }
}
