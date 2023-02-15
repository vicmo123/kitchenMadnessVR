using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartGameUi : MonoBehaviour
{
    Camera cam;
    [HideInInspector] public UnityEvent StartButtonClickedEvent;
    [SerializeField, Range(1, 10)] private float velocity;
    bool startGame = false;

    private void Awake()
    {
        StartButtonClickedEvent = new UnityEvent();
        StartButtonClickedEvent.AddListener(StartGame);
        cam = Camera.main;
    }

    private void Update()
    {
        if (startGame)
        {
            if (transform.position.x > -15)
                transform.position = new Vector3((transform.position.x - velocity * Time.deltaTime), transform.position.y, transform.position.z);
            else
                gameObject.SetActive(false);
        }
        else
        {
            CheckMouseClick();
        }
    }

    private void CheckMouseClick()
    {
        //For testing until vr is ready
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 100f;
        mousePos = cam.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(transform.position, mousePos - transform.position, Color.blue);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.gameObject.CompareTag("UI"))
                {
                    StartButtonClickedEvent.Invoke();
                }
            }
        }
    }

    public void StartGame()
    {
        SoundManager.ButtonClick.Invoke();
        startGame = true;
    }

    public void ResetUi()
    {
        startGame = false;
        transform.position = new Vector3(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Right Hand") || other.gameObject.CompareTag("Left Hand"))
        {
            StartButtonClickedEvent.Invoke();
        }
    }
}
