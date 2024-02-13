using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class food : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _grid;
    [SerializeField] private Snake _snake;
    [SerializeField] private float _colorChangeDuration;
    
    private SpriteRenderer _spriteRenderer;

    private Vector3 _foodPosition;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        SnakeManager.OnEatFood += FoodRespawn;
    }
    private void OnDisable()
    {
        SnakeManager.OnEatFood -= FoodRespawn;
        StopAllCoroutines();
    }

    private void Start()
    {
        StartCoroutine(FoodColorDancing(_colorChangeDuration));
    }

    /// <summary>
    /// Respawn a food to the Random Valid Position
    /// after snake Just eat food
    /// </summary>
    void FoodRespawn()
    {
        do
        {
            RandomizePosition();
        } while (_snake.IsFoodOverLap(_foodPosition));

        transform.position = _foodPosition;
    }

    /// <summary>
    /// generate a random 3d Vector inside the Grid Bounds and save to _foodPosition
    /// </summary>
    private void RandomizePosition()
    {
        int x = Random.Range(Mathf.RoundToInt(_grid.bounds.min.x), Mathf.RoundToInt(_grid.bounds.max.x));
        int y = Random.Range(Mathf.RoundToInt(_grid.bounds.min.y), Mathf.RoundToInt(_grid.bounds.max.y));
    
        _foodPosition = new Vector3(x, y);
    }

    /// <summary>
    /// randomly get any color for small second then change to another color
    /// (basically it give disko effect to by changin its color)
    /// </summary>
    /// <returns></returns>
    IEnumerator FoodColorDancing(float timeInterval)
    {
        while(true)
        {
            _spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            yield return new WaitForSeconds(timeInterval);
        }
    }
}
