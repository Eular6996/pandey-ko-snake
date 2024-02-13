using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Snake : MonoBehaviour
{
    private Vector2 _moveVector;
    private SnakeControls _snakeControls;

    /// <summary>
    /// List of Segment of Snake
    /// </summary>
    List<GameObject> _segment = new List<GameObject>();
    
    [SerializeField] float _reloadTimeAfterGameOver;
    [SerializeField] GameObject _snakeDeathEffect;
    [SerializeField] public Vector3 _headInitialPosition;
    [SerializeField] Transform _segmentPrefab;


    

    private void Awake()
    {
        _snakeControls = new SnakeControls();
        _segment.Add(this.gameObject);
    }
    private void OnEnable()
    {
        _snakeControls.Enable();
        _snakeControls.Board.Movement.performed += OnMovementInput;

        SnakeManager.AfterGameOver += RemoveAllSegments;
        SnakeManager.AfterGameOver += ResetHeadPosition;

        SnakeManager.OnEatFood += GrowSnake;
    }
    private void OnDisable()
    {
        _snakeControls.Disable();
        _snakeControls.Board.Movement.performed -= OnMovementInput;

        SnakeManager.AfterGameOver -= RemoveAllSegments;
        SnakeManager.AfterGameOver -= ResetHeadPosition;

        SnakeManager.OnEatFood -= GrowSnake;
    }

    /// <summary>
    /// Update MoveVector
    /// </summary>
    /// <param name="context"></param>
    public void OnMovementInput(InputAction.CallbackContext context)
    {
        Vector2 contextVector = context.ReadValue<Vector2>();
        if (contextVector == -_moveVector
            || contextVector == _moveVector)
        {
            return;
        }

        _moveVector = contextVector;
    }

    /// <summary>
    /// Move Snake
    /// </summary>
    public void SnakeMovement()
    {
        //segment follow their front segment
        for (int i = _segment.Count - 1; i > 0; i--)
        {
            _segment[i].transform.position = _segment[i - 1].transform.position;
        }

        //Head Movement
        int x = Mathf.RoundToInt(this.transform.position.x) + (int)_moveVector.x;
        int y = Mathf.RoundToInt(this.transform.position.y) + (int)_moveVector.y;
       

        this.transform.position = new Vector3(x, y, 0);
        _snakeDeathEffect.transform.position = transform.position;
    }


    /// <summary>
    /// When Snake Collide With Obstacle Game Over
    /// </summary>
    /// <returns></returns>
    public IEnumerator GameOver()
    {
        //Disable control for certain Movement
        _moveVector = Vector2.zero;
        _snakeControls.Disable();
        _snakeDeathEffect.SetActive(true);


        //pause For Movement
        yield return new WaitForSeconds(_reloadTimeAfterGameOver);

        //Enable control and disable Effect
        _snakeDeathEffect.SetActive(false);
        _snakeControls.Enable();

        //Trigger all event after game Over
        SnakeManager.AfterGameOver?.Invoke();
    }

    /// <summary>
    /// Reset Head Position to its Initial Position 
    /// </summary>
    void ResetHeadPosition()
    {
        this.transform.position = _headInitialPosition;
    }

    /// <summary>
    /// Destroy All the segments of snake Expect Head
    /// </summary>
    private void RemoveAllSegments()
    {
        for (int i = _segment.Count - 1; i > 0; i--)
        {
            Destroy(_segment[i]);
            _segment.Remove(_segment[i]);
        }
    }

    /// <summary>
    /// Add Segment to Snake
    /// </summary>
    public void GrowSnake()
    {
        GameObject segment = Instantiate(_segmentPrefab).gameObject;
        segment.transform.position = _segment[_segment.Count - 1].transform.position;
        _segment.Add(segment);
    }

    /// <summary>
    /// return true when food is generate into the position of Snake segments 
    /// </summary>
    /// <returns></returns>
    public bool IsFoodOverLap(Vector3 foodPosition)
    {
        for(int i = _segment.Count - 1; i >= 0; i--)
        {
            if(foodPosition == _segment[i].transform.position)
            {
                Debug.Log("Food Overlap into the snake");
                return true;
            }
        }
        return false;
    }
}
