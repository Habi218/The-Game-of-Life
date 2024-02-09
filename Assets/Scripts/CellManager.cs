using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour {
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private float margin;
    [SerializeField] private int liveCellProbability;

    Cell[,] cellArray;

    Dictionary<Vector2, Cell> cellDictionary = new Dictionary<Vector2, Cell>();
    private float time = 0.0f;
    private bool hasWon = false;

    private int width => GameManager.BoardWidth;
    private int height => GameManager.BoardHeight;
    private float interpolationPeriod => GameManager.InterpolationPeriod;

    private void Awake() {
        GameManager.Generations = 0;
    }

    private void Start() {
        if(GameManager.UseArray) {
            GenerateBoardArray(width, height);
        } else {
            //GenerateBoard();
        }
    }

    private void Update() {
        HandleInput();

        if(GameManager.UseArray) {
            CountAndHandleArray();
        } else {
            //CountAndHandleGeneration();
        }
    }

    private void HandleInput() {
        if(GameManager.Instance.State == GameState.SetUp) {
            if(Input.GetKeyDown("r")) {
                ClearBoard();
            }
            if(Input.GetKeyDown("space") && GameManager.Population >= 3) {
                PauseBoard();
            }
        } else {
            if(Input.GetKeyDown("r")) {
                GameManager.Instance.UpdateGameState(GameState.SetUp);
            }
            if(Input.GetKeyDown("space")) {
                PauseBoard();
            }
        }

        if(Input.GetKeyDown("q")) {
            AdjustInterpolationPeriod(0.5f);
        }
        if(Input.GetKeyDown("e")) {
            AdjustInterpolationPeriod(2.0f);
        }
    }

    private void AdjustInterpolationPeriod(float factor) {
        float newInterpolationPeriod = interpolationPeriod / factor;
        if(newInterpolationPeriod >= 0.00625f && newInterpolationPeriod <= 0.4f) {
            GameManager.InterpolationPeriod = newInterpolationPeriod;
            GameManager.TimeMultiplier *= factor;
        }
    }

    private void CountAndHandleGeneration() {
        int population = 0;
        for(int z = 0; z < height; z++) {
            for(int x = 0; x < width; x++) {
                Cell cell = cellDictionary[new Vector2(x, z)];
                population = cell.State == 1 ? population + 1 : population;
                cell.NumNeighbors = CountNeighbors(x, z, cell);
            }
        }
        GameManager.Population = population;

        if(GameManager.Instance.State == GameState.Play) {
            time += Time.deltaTime;

            if(time >= interpolationPeriod) {
                UpdateGeneration();
                GameManager.Generations++;
                time = time - interpolationPeriod;
            }
            if(!hasWon) {
                CheckWinLoss();
            }
        }
    }

    private void CheckWinLoss() {
        if(GameManager.Population >= 50 || GameManager.Generations >= 50) {
            hasWon = true;
            GameManager.Instance.UpdateGameState(GameState.Victory);
        }
        if(GameManager.Population < 1) {
            GameManager.Instance.UpdateGameState(GameState.GameOver);
        }
    }

    public void PauseBoard() {
        GameState nextState = GameManager.Instance.State == GameState.Play ? GameState.Paused : GameState.Play;
        GameManager.Instance.UpdateGameState(nextState);
    }

    // Array functions

    private void GenerateBoardArray(int boardWidth, int boardHeight) {
        cellArray = new Cell[height,width];

        for(int z = 0; z < boardHeight; z++) {
            for(int x = 0; x < boardWidth; x++) {
                float xPos = (x*margin) - (boardWidth*margin/2) + (margin/2);
                float zPos = (z*margin) - (boardHeight*margin/2) + (margin/2);

                cellArray[z, x] = Instantiate(cellPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity, transform);
            }
        }
    }

    private void ClearArray() {
        for(int z = 0; z < width; z++) {
            for(int x = 0; x < height; x++) {
                cellArray[z, x].State = 0;
            }
        }
    }

    private void UpdateArray() {
        for(int z = 0; z < height; z++) {
            for(int x = 0; x < width; x++) {
                Cell cell = cellArray[z, x];
                
                if(cell.State == 1) {
                    if(cell.NumNeighbors < 2 || cell.NumNeighbors > 3) {
                        cell.State = 0;
                    }
                } else if(cell.State == 0) {
                    if(cell.NumNeighbors == 3) {
                        cell.State = 1;
                    }
                }
            }
        }
    }

    private void CountAndHandleArray() {
        int population = 0;
        for(int z = 0; z < height; z++) {
            for(int x = 0; x < width; x++) {
                Cell cell = cellArray[z, x];
                population = cell.State == 1 ? population + 1 : population;
                cell.NumNeighbors = CountArrayNeighbors(x, z, cell);
            }
        }
        GameManager.Population = population;

        if(GameManager.Instance.State == GameState.Play) {
            time += Time.deltaTime;

            if(time >= interpolationPeriod) {
                UpdateArray();
                GameManager.Generations++;
                time = time - interpolationPeriod;
            }
            if(!hasWon) {
                CheckWinLoss();
            }
        }
    }

    private int CountArrayNeighbors(int cellX, int cellZ, Cell cell) {
        int count = 0;

        foreach(Vector2Int offset in neighborOffsets) {
            int neighborX = cellX + offset.x;
            int neighborZ = cellZ + offset.y;

            if(neighborX >= 0 && neighborX < width && neighborZ >= 0 && neighborZ < height) {
                Cell neighborCell = cellArray[neighborZ, neighborX];
                if(neighborCell.State == 1) {
                    count++;
                }
            }
        }

        return count;
    }

    // Dictionary functions

    private void GenerateBoard() {
        for(int z = 0; z < height; z++) {
            for(int x = 0; x < width; x++) {
                float xPos = (x*margin) - (width*margin/2) + (margin/2);
                float zPos = (z*margin) - (height*margin/2) + (margin/2);
                cellDictionary[new Vector2(x, z)] = Instantiate(cellPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity, transform);
            }
        }
    }

    private void RandomBoard() {
        for(int z = 0; z < height; z++) {
            for(int x = 0; x < width; x++) {
                if(Random.Range(0, 100) <= liveCellProbability) {
                    cellDictionary[new Vector2(x, z)].State = 1;
                }
            }
        }
    }

    private void ClearBoard() {
        for(int z = 0; z < height; z++) {
            for(int x = 0; x < width; x++) {
                cellDictionary[new Vector2(x, z)].State = 0;
            }
        }
    }

    private void UpdateGeneration() {
        for(int z = 0; z < height; z++) {
            for(int x = 0; x < width; x++) {
                Cell cell = cellDictionary[new Vector2(x, z)];
                
                if(cell.State == 1) {
                    if(cell.NumNeighbors < 2 || cell.NumNeighbors > 3) {
                        cell.State = 0;
                    }
                } else if(cell.State == 0) {
                    if(cell.NumNeighbors == 3) {
                        cell.State = 1;
                    }
                }
            }
        }
    }

    private readonly Vector2Int[] neighborOffsets = new Vector2Int[] {
        new Vector2Int(-1, 1), new Vector2Int(0,1), new Vector2Int(1,1),
        new Vector2Int(-1, 0),                      new Vector2Int(1,0),
        new Vector2Int(-1, -1), new Vector2Int(0,-1), new Vector2Int(1,-1)
    };

    int CountNeighbors(int cellX, int cellZ, Cell cell) {
        int count = 0;

        foreach(Vector2Int offset in neighborOffsets) {
            int neighborX = cellX + offset.x;
            int neighborZ = cellZ + offset.y;

            if(neighborX >= 0 && neighborX < width && neighborZ >= 0 && neighborZ < height) {
                Cell neighborCell = cellDictionary[new Vector2(neighborX, neighborZ)];
                if(neighborCell.State == 1) {
                    count++;
                }
            }
        }

        return count;
    }

}