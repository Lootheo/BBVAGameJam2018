using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {
    public int xIndex;
    public int yIndex;
    Board m_board;
    public Animator anim;

    bool m_isMoving = false;
    public InterpType interpolation = InterpType.SmoothStep;
    public MatchValue matchValue;

    public enum MatchValue
    {
        Yellow,
        Blue,
        Magenta,
        Indigo,
        Green,
        Teal,
        Red,
        Cyan,
        Wild
    };
    public enum InterpType
    {
        Linear, 
        EaseOut,
        EaseIn,
        SmoothStep,
        SmootherStep
    };
	// Use this for initialization
	void Start () {
		
	}

    public void Init(Board board)
    {
        m_board = board;
    }

    public void SetCoord(int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }

    public void Move(int destX, int destY, float timeMove)
    {
        if(!m_isMoving)
        {
            StartCoroutine(MoveRoutine(new Vector3(destX, destY, 0), timeMove));
        }
    }

    IEnumerator MoveRoutine(Vector3 destination, float timeMove)
    {
        Vector3 startPos = transform.position;
        bool reachedDestination = false;
        float elapsedTime = 0f;
        m_isMoving = true;
        while (!reachedDestination)
        {
            if (Vector3.Distance(transform.position, destination)<0.01f)
            {
                reachedDestination = true;
                if (m_board!=null)
                {
                    m_board.PlacePiece(this, (int)destination.x, (int)destination.y);
                }
                break;
            }
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp(elapsedTime / timeMove, 0f, 1f);
            switch (interpolation)
            {
                case InterpType.Linear:
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;
                case InterpType.EaseOut:
                    t = 1 - Mathf.Cos(t * Mathf.PI * 0.5f);
                    break;
                case InterpType.EaseIn:
                    t = Mathf.Pow(t, 2);
                    break;
                case InterpType.SmoothStep:
                    t = Mathf.Pow(t, 2) * (3 - 2 * t);
                    break;
                case InterpType.SmootherStep:
                    t = Mathf.Pow(t, 3) * (t * (t * 6 - 15) + 10);
                    break;
                default:
                    break;
            }
            

            transform.position = Vector3.Lerp(startPos, destination, t);
            yield return null;
        }
        m_isMoving = false;
    }
}
