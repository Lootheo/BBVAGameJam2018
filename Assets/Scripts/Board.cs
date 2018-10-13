using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour {
    public int width;
    public int height;

    public int borderSize;

    public GameObject tilePrefab;

    public GameObject[] piecePrefabs;

    public float swapTime = 0.2f;


    TileClass[,] m_allTiles;
    Piece[,] m_allPieces;

    TileClass m_clickedTile;
    TileClass m_targetTile;
    bool m_playerInputEnabled = true;

	// Use this for initialization
	void Start ()
    {
        m_allTiles = new TileClass[width, height];
        m_allPieces = new Piece[width, height];
        SetupTiles();
        SetupCamera();
        FillBoard(5, 1f);
        //HighlightMatches();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetupTiles()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                tile.name = "Tile (" + i + "," + j + ")";
                m_allTiles[i, j] = tile.GetComponent<TileClass>();
                tile.transform.SetParent(transform);
                m_allTiles[i, j].Init(i, j, this);
            }
        }
    }


    void SetupCamera()
    {
        Camera.main.transform.position = new Vector3((float)(width-1) / 2f, (float)(height-1) / 2f, -10f);
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float verticalSize = (float)height / 2f + (float)borderSize;
        float horizontalSize = ((float)width / 2f + (float)borderSize)/aspectRatio;
        Camera.main.orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize;
        /*if(verticalSize > horizontalSize)
        {
            Camera.main.orthographicSize = verticalSize;
        }
        else
        {
            Camera.main.orthographicSize = horizontalSize;
        }*/
    }

    GameObject GetRandomPiece()
    {
        int randIndex = Random.Range(0, piecePrefabs.Length);
        if (piecePrefabs[randIndex]==null)
        {
            Debug.Log("No tenemos nada en este prefab index");
        }
        return piecePrefabs[randIndex];
    }

    public void PlacePiece(Piece piece, int x, int y)
    {
        if (piece == null)
        {
            Debug.Log("No tenemos ninguna pieza");
            return;
        }
        piece.transform.position = new Vector3(x, y, 0);
        piece.transform.rotation = Quaternion.identity;
        if(IsWithinBounds(x,y))
        {
            m_allPieces[x, y] = piece;
        }
        piece.SetCoord(x, y);
    }

    bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

    void FillBoard(int falseYOffset = 0, float moveTime = 0.5f)
    {
        List<Piece> addedPieces = new List<Piece>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (m_allPieces[i,j] == null)
                {
                    if (falseYOffset==0)
                    {
                        Piece piece = FillRandomAt(i, j);
                        addedPieces.Add(piece);
                    }
                    else
                    {
                        Piece piece = FillRandomAt(i, j, falseYOffset, moveTime);
                        addedPieces.Add(piece);
                    }
                }
            }
        }
        int maxIterations = 50;
        int iteration = 0;
        bool isFilled = false;
        while (!isFilled)
        {
            List<Piece> matches = FindAllMatches();
            if(matches.Count == 0)
            {
                isFilled = true;
                break;
            }
            else
            {
                matches = matches.Intersect(addedPieces).ToList();
                if (falseYOffset==0)
                {
                    ReplaceWithRandom(matches);
                }
                else
                {
                    ReplaceWithRandom(matches, falseYOffset, moveTime);
                }
            }
            if (iteration>maxIterations)
            {
                isFilled = true;
                break;
            }
            iteration++;
        }
    }

    Piece FillRandomAt(int x, int y, int falseYOffset = 0, float moveTime = 0.5f)
    {
        GameObject randomPiece = Instantiate(GetRandomPiece(), Vector3.zero, Quaternion.identity) as GameObject;
        if (randomPiece != null)
        {
            randomPiece.GetComponent<Piece>().Init(this);
            PlacePiece(randomPiece.GetComponent<Piece>(), x, y);
            if (falseYOffset!=0)
            {
                randomPiece.transform.position = new Vector3(x, y + falseYOffset, 0);
                randomPiece.GetComponent<Piece>().Move(x, y, moveTime);
            }
            randomPiece.transform.parent = transform;
        }
        return randomPiece.GetComponent<Piece>();
    }

    List<Piece> FindAllMatches()
    {
        List<Piece> combinedMatches = new List<Piece>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var matches = FindMatchesAt(i, j);
                combinedMatches = combinedMatches.Union(matches).ToList();
            }
        }
        return combinedMatches;
    }

    void ReplaceWithRandom(List<Piece> gamePieces, int falseYOffset = 0, float moveTime = 0.1f)
    {
        foreach (Piece piece in gamePieces)
        {
            ClearPieceAt(piece.xIndex, piece.yIndex);
            if (falseYOffset==0)
            {
                FillRandomAt(piece.xIndex, piece.yIndex);
            }
            else
            {
                FillRandomAt(piece.xIndex, piece.yIndex, falseYOffset, moveTime);
            }
        }
    }

    public void ClickTile(TileClass tile)
    {
        if (m_clickedTile == null)
        {
            m_clickedTile = tile;
            Debug.Log("Clicked tile " + tile.name);
        }
    }

    public void DragToTile(TileClass tile)
    {
        if (m_clickedTile != null && IsNextTo(tile, m_clickedTile))
        {
            m_targetTile = tile;
        }
    }

    public void ReleaseTile()
    {
        if (m_clickedTile!=null && m_targetTile!=null)
        {
            SwitchTiles(m_clickedTile, m_targetTile);
        }
        m_clickedTile = null;
        m_targetTile = null;
    }

    void SwitchTiles(TileClass clickedTile, TileClass targetTile)
    {
        StartCoroutine(SwitchTilesRoutine(clickedTile, targetTile));

    }

    IEnumerator SwitchTilesRoutine(TileClass clickedTile, TileClass targetTile)
    {
        if(m_playerInputEnabled)
        {
            Piece clickedPiece = m_allPieces[clickedTile.xIndex, clickedTile.yIndex];
            Piece targetPiece = m_allPieces[targetTile.xIndex, targetTile.yIndex];
            if (clickedPiece != null && targetPiece != null)
            {
                clickedPiece.Move(targetTile.xIndex, targetTile.yIndex, swapTime);
                targetPiece.Move(clickedTile.xIndex, clickedTile.yIndex, swapTime);
                yield return new WaitForSeconds(swapTime);


                List<Piece> clickedPieceMatches = FindMatchesAt(clickedTile.xIndex, clickedTile.yIndex);
                List<Piece> targetPieceMatches = FindMatchesAt(targetTile.xIndex, targetTile.yIndex);

                if (targetPieceMatches.Count == 0 && clickedPieceMatches.Count == 0)
                {
                    clickedPiece.Move(clickedTile.xIndex, clickedTile.yIndex, swapTime);
                    targetPiece.Move(targetTile.xIndex, targetTile.yIndex, swapTime);
                }
                else
                {
                    yield return new WaitForSeconds(swapTime);
                    ClearAndRefillBoard(clickedPieceMatches.Union(targetPieceMatches).ToList());
                }

            }
        }
    }

    bool IsNextTo(TileClass start, TileClass end)
    {
        if (Mathf.Abs(start.yIndex- end.yIndex)==1 && start.xIndex == end.xIndex)
        {
            return true;
        }
        if (Mathf.Abs(start.xIndex - end.xIndex)==1 && start.yIndex == end.yIndex)
        {
            return true;
        }
        return false;
    }

    List<Piece> FindMatches(int startX, int startY, Vector2 searchDirection, int minLength = 3)
    {
        List<Piece> matches = new List<Piece>();
        Piece startPiece = null;
        if (IsWithinBounds(startX, startY))
        {
            startPiece = m_allPieces[startX, startY];
        }
        if (startPiece!=null)
        {
            matches.Add(startPiece);
        }
        else
        {
            return null;
        }
        int nextX;
        int nextY;
        int maxValue = (width > height) ? width : height;
        for (int i = 1; i < maxValue -1; i++)
        {
            nextX = startX + (int)Mathf.Clamp(searchDirection.x,-1,1) * i;
            nextY = startY + (int)Mathf.Clamp(searchDirection.y, -1, 1) * i;

            if (!IsWithinBounds(nextX, nextY))
            {
                break;
            }
            Piece nextPiece = m_allPieces[nextX, nextY];
            if (nextPiece == null)
            {
                break;
            }
            else
            {
                if (nextPiece.matchValue == startPiece.matchValue && !matches.Contains(nextPiece))
                {
                    matches.Add(nextPiece);

                }
                else
                {
                    break;
                }
            }
        }
        if (matches.Count >= minLength)
        {
            return matches;
        }
        return null;

    }

    List<Piece> FindVerticalMatches(int startX, int startY, int minLength = 3)
    {
        List<Piece> upwardMatches = FindMatches(startX, startY, new Vector2(0, 1), 2);
        List<Piece> downwardMatches = FindMatches(startX, startY, new Vector2(0, -1), 2);
        if (upwardMatches==null)
        {
            upwardMatches = new List<Piece>();
        }
        if (downwardMatches==null)
        {
            downwardMatches = new List<Piece>();
        }
        var combinedMatches = upwardMatches.Union(downwardMatches).ToList();

        return (combinedMatches.Count >= minLength) ? combinedMatches : null;
    }

    List<Piece> FindHorizontalMatches(int startX, int startY, int minLength = 3)
    {
        List<Piece> rightMatches = FindMatches(startX, startY, new Vector2(1, 0), 2);
        List<Piece> leftMatches = FindMatches(startX, startY, new Vector2(-1, 0), 2);
        if (rightMatches == null)
        {
            rightMatches = new List<Piece>();
        }
        if (leftMatches == null)
        {
            leftMatches = new List<Piece>();
        }
        var combinedMatches = rightMatches.Union(leftMatches).ToList();

        return (combinedMatches.Count >= minLength) ? combinedMatches : null;
    }

    void HighlightMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                HighlightMatchesAt(i, j);
            }
        }
    }

    void HighlightPieces(List<Piece> gamePieces)
    {
        foreach (Piece piece in gamePieces)
        {
            if (piece!=null)
            {
                piece.anim.SetTrigger("shake");
                ParticleSystem emitter = m_allTiles[piece.xIndex, piece.yIndex].emitter;
                emitter.Play();
            }
        }
    }

    private void HighlightMatchesAt(int x, int y)
    {
        List<Piece> combinedMatches = FindMatchesAt(x, y);
        if (combinedMatches.Count > 0)
        {
            foreach (Piece piece in combinedMatches)
            {
                piece.anim.SetTrigger("shake");
            }
        }
    }

    private List<Piece> FindMatchesAt(int i, int j, int minLength = 3)
    {
        List<Piece> horizMatches = FindHorizontalMatches(i, j, minLength);
        List<Piece> vertMatches = FindVerticalMatches(i, j, minLength);
        if (horizMatches == null)
        {
            horizMatches = new List<Piece>();
        }
        if (vertMatches == null)
        {
            vertMatches = new List<Piece>();
        }
        var combinedMatches = horizMatches.Union(vertMatches).ToList();
        return combinedMatches;
    }

    List<Piece> FindMatchesAt(List<Piece> gamePieces, int minLength = 3)
    {
        List<Piece> matches = new List<Piece>();
        foreach (Piece piece in gamePieces)
        {
            matches = matches.Union(FindMatchesAt(piece.xIndex, piece.yIndex, minLength)).ToList();
        }
        return matches;
    }

    void ClearPieceAt(int x, int y)
    {
        Piece pieceToClear = m_allPieces[x, y];
        if (pieceToClear!=null)
        {
            m_allPieces[x, y] = null;
            Destroy(pieceToClear.gameObject);
        }
    }

    void ClearBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                ClearPieceAt(i, j);
            }
        }
    }

    void ClearPieceAt(List<Piece> gamePieces)
    {
        foreach (Piece piece in gamePieces)
        {
            if (piece!=null)
            {
                ClearPieceAt(piece.xIndex, piece.yIndex);
            }
        }
    }

    List<Piece> CollapseColumn(int column, float collapseTime = 0.1f)
    {
        List<Piece> movingPieces = new List<Piece>();
        for (int i = 0; i < height -1 ; i++)
        {
            if (m_allPieces[column, i] == null)
            {
                for (int j = i+1; j < height; j++)
                {
                    if (m_allPieces[column,j]!=null)
                    {
                        m_allPieces[column, j].Move(column, i, collapseTime * (j-i));

                        m_allPieces[column, i] = m_allPieces[column, j];
                        m_allPieces[column, i].SetCoord(column, i);

                        if (!movingPieces.Contains(m_allPieces[column, i]))
                        {
                            movingPieces.Add(m_allPieces[column, i]);
                        }
                        m_allPieces[column, j] = null;
                        break;
                    }
                }
            }
        }
        return movingPieces;
    }

    List<Piece> CollapseColumn(List<Piece> gamePieces)
    {
        List<Piece> movingPieces = new List<Piece>();

        List<int> columnsToCollapse = GetColumns(gamePieces);
        foreach (int column in columnsToCollapse)
        {
            movingPieces = movingPieces.Union(CollapseColumn(column)).ToList();
        }

        return movingPieces;
    }

    List<int> GetColumns(List<Piece> gamePieces)
    {
        List<int> columns = new List<int>();
        foreach (Piece piece in gamePieces)
        {
            if (!columns.Contains(piece.xIndex))
            {
                columns.Add(piece.xIndex);
            }
        }
        return columns;
    }

    void ClearAndRefillBoard(List<Piece> gamePieces)
    {
        StartCoroutine(ClearAndRefillBoardRoutine(gamePieces));
    }

    IEnumerator ClearAndRefillBoardRoutine(List<Piece> gamePieces)
    {
        m_playerInputEnabled = false;
        List<Piece> matches = gamePieces;
        do
        {
            yield return StartCoroutine(ClearAndCollapseRoutine(matches));
            yield return null;
            yield return StartCoroutine(RefillRoutine());
            matches = FindAllMatches();
            yield return new WaitForSeconds(0.5f);
        }
        while (matches.Count!=0); 
        
        m_playerInputEnabled = true;
    }

    IEnumerator ClearAndCollapseRoutine(List<Piece> gamePieces)
    {
        List<Piece> movingPieces = new List<Piece>();
        List<Piece> matches = new List<Piece>();

        HighlightPieces(gamePieces);
        yield return new WaitForSeconds(1f);

        bool isFinished = false;
        ClearPieceAt(gamePieces);
        CollapseColumn(gamePieces);
        /*while (!isFinished)
        {
            ClearPieceAt(gamePieces);
            yield return new WaitForSeconds(0.25f);
            movingPieces = CollapseColumn(gamePieces);
            while (!IsCollapsed(gamePieces))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            matches = FindMatchesAt(movingPieces);
            if (matches.Count == 0)
            {
                isFinished = true;
                break;
            }
            else
            {
                yield return StartCoroutine(ClearAndCollapseRoutine(matches));
            }
        }*/
        yield return null;
    }

    IEnumerator RefillRoutine()
    {
        FillBoard(5, 0.5f);
        yield return null;
    }

    bool IsCollapsed(List<Piece> gamePieces)
    {
        foreach (Piece piece in gamePieces)
        {
            if (piece!=null)
            {
                if (piece.transform.position.y - (float)piece.yIndex > 0.001f)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
