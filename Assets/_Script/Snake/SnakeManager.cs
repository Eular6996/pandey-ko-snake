using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SnakeManager : MonoBehaviour
{
    //Events
    public static Action AfterGameOver;
    public static Action OnEatFood;

    //Script Reference
    private Snake _snake;

    //Audio Reference
    [SerializeField] private AudioManager _audioManager;

    private void Awake()
    {
        _snake = GetComponent<Snake>();
    }

    private void Start()
    {
        _audioManager.PlayMusic(_audioManager.LEVEL1_MUSIC);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// update moveVector according to user Input
    /// </summary>
    /// <param name="context"></param>


    void FixedUpdate()
    {
        _snake.SnakeMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("obstacle"))
        {
            _audioManager.PlaySFX(_audioManager.SNAKE_DEATH_SFX);
            StartCoroutine(_snake.GameOver());
        }
        
        if(collision.gameObject.CompareTag("food"))
        {
            _audioManager.PlaySFX(_audioManager.EAT_FOOD_SFX);
            OnEatFood?.Invoke();
        }
    }

}
