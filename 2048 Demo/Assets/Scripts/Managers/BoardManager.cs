using UnityEngine;
using DG.Tweening;

public enum GameState
{
    Playing, GameOver
}

public class BoardManager : Singleton<BoardManager>
{
    [SerializeField] private int _size = 4;
    [SerializeField] private Transform _nodesParent;
    [SerializeField] private float _animationDuration = 0.125f;
    
    private GameState _gameState;
    private bool _hasAnyBlockMoved = false;
    private readonly int[] _randomNumbers = { 2, 4 };

    private void Start()
    {
        _gameState = GameState.Playing;

        for (int i = 0; i < 2; i++)
        {
            CreateRandomNumber();
        }
    }

    private void Update()
    {
        if (_gameState.Equals(GameState.Playing))
        {
            if (InputManager.Instance.GetInput("Left"))
            {
                MoveAllToLeft();
            }
            else if (InputManager.Instance.GetInput("Up"))
            {
                MoveAllToUp();
            }
            else if (InputManager.Instance.GetInput("Right"))
            {
                MoveAllToRight();
            }
            else if (InputManager.Instance.GetInput("Down"))
            {
                MoveAllToDown();
            }
        }
    }

    private void MoveAllToLeft()
    {
        for (int y = 0; y < _size; y++) // for each row [0,1,2,3]
        {
            for (int x = 1; x < _size; x++) // for each column except the leftmost [1,2,3]
            {
                var currentNode = GetNodeFromBoard(x, y);

                if (currentNode.childCount.Equals(0)) // node does not have a number inside
                    continue;

                Transform targetNode = null;

                for (int z = x - 1; z >= 0; z--)
                {
                    var iterNode = GetNodeFromBoard(z, y);

                    if (!iterNode.childCount.Equals(0)) // node has a number inside
                    {
                        if (AreTheNumbersEqual(currentNode, iterNode) && !iterNode.GetChild(0).GetComponent<Number>().justCombined)
                        {
                            targetNode = iterNode;
                            Move(currentNode, targetNode, true);
                            break;
                        }
                        else // iter is not equal, go one right
                        {
                            targetNode = GetNodeFromBoard(z + 1, y);
                            if (Mathf.Abs(x - z) > 1) // only if it's not adjacent
                            {
                                MoveToEmptyNode(currentNode, targetNode);
                                break;
                            }
                        }
                        break;
                    }
                }

                if (targetNode == null) // if there is no target, select the leftmost
                {
                    targetNode = GetNodeFromBoard(0, y);
                    MoveToEmptyNode(currentNode, targetNode);
                }
            }
        }

        CreateNumberAndCheckForGameOver();
    }

    private void MoveAllToUp()
    {
        for (int x = 0; x < _size; x++) // for each column [0,1,2,3]
        {
            for (int y = _size - 2; y >= 0; y--) // for each row except the uppermost [2,1,0]
            {
                var currentNode = GetNodeFromBoard(x, y);

                if (currentNode.childCount.Equals(0)) // node does not have a number inside
                    continue;

                Transform targetNode = null;

                for (int z = y + 1; z < _size; z++)
                {
                    var iterNode = GetNodeFromBoard(x, z);

                    if (!iterNode.childCount.Equals(0)) // node has a number inside
                    {
                        if (AreTheNumbersEqual(currentNode, iterNode) && !iterNode.GetChild(0).GetComponent<Number>().justCombined)
                        {
                            targetNode = iterNode;
                            Move(currentNode, targetNode, true);
                            break;
                        }
                        else // iter is not equal, go one down
                        {
                            targetNode = GetNodeFromBoard(x, z - 1);
                            if (Mathf.Abs(y - z) > 1) // only if it's not adjacent
                            {
                                MoveToEmptyNode(currentNode, targetNode);
                                break;
                            }
                        }
                        break;
                    }
                }

                if (targetNode == null) // if there is no target, select the uppermost
                {
                    targetNode = GetNodeFromBoard(x, _size - 1);
                    MoveToEmptyNode(currentNode, targetNode);
                }
            }
        }

        CreateNumberAndCheckForGameOver();
    }

    private void MoveAllToRight()
    {
        for (int y = 0; y < _size; y++) // for each row [0,1,2,3]
        {
            for (int x = _size - 2; x >= 0; x--) // for each column except the rightmost [2,1,0]
            {
                var currentNode = GetNodeFromBoard(x, y);

                if (currentNode.childCount.Equals(0)) // node does not have a number inside
                    continue;

                Transform targetNode = null;

                for (int z = x + 1; z < _size; z++)
                {
                    var iterNode = GetNodeFromBoard(z, y);

                    if (!iterNode.childCount.Equals(0)) // node has a number inside
                    {
                        if (AreTheNumbersEqual(currentNode, iterNode) && !iterNode.GetChild(0).GetComponent<Number>().justCombined)
                        {
                            targetNode = iterNode;
                            Move(currentNode, targetNode, true);
                            break;
                        }
                        else // iter is not equal, go one left
                        {
                            targetNode = GetNodeFromBoard(z - 1, y);
                            if (Mathf.Abs(x - z) > 1) // only if it's not adjacent
                            {
                                MoveToEmptyNode(currentNode, targetNode);
                                break;
                            }
                        }
                        break;
                    }
                }

                if (targetNode == null) // if there is no target, select the rightmost
                {
                    targetNode = GetNodeFromBoard(_size - 1, y);
                    MoveToEmptyNode(currentNode, targetNode);
                }
            }
        }

        CreateNumberAndCheckForGameOver();
    }

    private void MoveAllToDown()
    {
        for (int x = 0; x < _size; x++) // for each column [0,1,2,3]
        {
            for (int y = 1; y < _size; y++) // for each row except the undermost [1,2,3]
            {
                var currentNode = GetNodeFromBoard(x, y);

                if (currentNode.childCount.Equals(0)) // node does not have a number inside
                    continue;

                Transform targetNode = null;

                for (int z = y - 1; z >= 0; z--)
                {
                    var iterNode = GetNodeFromBoard(x, z);

                    if (!iterNode.childCount.Equals(0)) // node has a number inside
                    {
                        if (AreTheNumbersEqual(currentNode, iterNode) && !iterNode.GetChild(0).GetComponent<Number>().justCombined)
                        {
                            targetNode = iterNode;
                            Move(currentNode, targetNode, true);
                            break;
                        }
                        else // iter is not equal, go one up
                        {
                            targetNode = GetNodeFromBoard(x, z + 1);
                            if (Mathf.Abs(y - z) > 1) // only if it's not adjacent
                            {
                                MoveToEmptyNode(currentNode, targetNode);
                                break;
                            }
                        }
                        break;
                    }
                }

                if (targetNode == null) // if there is no target, select the undermost
                {
                    targetNode = GetNodeFromBoard(x, 0);
                    MoveToEmptyNode(currentNode, targetNode);
                }
            }
        }

        CreateNumberAndCheckForGameOver();
    }

    private void CreateNumberAndCheckForGameOver()
    {
        if (_hasAnyBlockMoved && !_gameState.Equals(GameState.GameOver))
        {
            Invoke(nameof(CreateRandomNumber), _animationDuration);
            _hasAnyBlockMoved = false;
        }
    }
    
    private void Move(Transform currentNode, Transform targetNode, bool merge)
    {
        _hasAnyBlockMoved = true;

        var movingBlock = currentNode.GetChild(0);
        Transform targetBlock = null;
        int newValue = int.Parse(movingBlock.name) * 2;
        movingBlock.SetParent(transform);

        if (targetNode.childCount > 0) // node has a number inside
        {
            targetBlock = targetNode.GetChild(0);
            targetBlock.SetParent(transform);

            if (merge) // move and merge
            {
                GameObject newBlock = NumberPool.Instance.GetPooledObject(newValue, targetNode, false);
                newBlock.GetComponent<Number>().justCombined = true;

                movingBlock.DOMove(targetNode.position, _animationDuration).OnComplete(() =>
                {
                    UIManager.Instance.OnInput(newValue);
                    NumberPool.Instance.SetPooledObject(movingBlock);
                    NumberPool.Instance.SetPooledObject(targetBlock);

                    newBlock.GetComponent<Number>().justCombined = false;
                    newBlock.SetActive(true);
                    newBlock.transform.DOPunchScale(Vector3.one * 0.15f, _animationDuration, 0, 0f);
                });
            }
            else // just move
            {
                movingBlock.DOMove(targetNode.position, _animationDuration).OnComplete(() => {
                    movingBlock.SetParent(targetNode);
                });
            }
        }
    }

    private void MoveToEmptyNode(Transform currentNode, Transform targetNode)
    {
        _hasAnyBlockMoved = true;

        var movingBlock = currentNode.GetChild(0);
        movingBlock.SetParent(targetNode);
        movingBlock.DOMove(targetNode.position, _animationDuration);
    }

    private void CreateRandomNumber()
    {
        if (IsTableFilled())
        {
            return;
        }

        int randomNumber = _randomNumbers[Random.Range(0, _randomNumbers.Length)];
        bool hasCreated = false;

        while (!hasCreated)
        {
            int randomX = Random.Range(0, _size);
            int randomY = Random.Range(0, _size);

            Transform randomBlock = GetNodeFromBoard(randomX, randomY);

            if (randomBlock.childCount.Equals(0)) // node does not have a number inside
            {
                hasCreated = true;
                GameObject newBlock = NumberPool.Instance.GetPooledObject(randomNumber, randomBlock, true);
                newBlock.transform.DOScale(Vector3.one, _animationDuration).From(Vector3.zero);
            }
        }

        if (IsGameOver())
        {
            _gameState = GameState.GameOver;
            UIManager.Instance.GameOver();
        }
    }

    private bool AreTheNumbersEqual(Transform firstNode, Transform secondNode)
    {
        Transform firstBlock = firstNode.GetChild(0);
        Transform secondBlock = secondNode.GetChild(0);

        return firstBlock.name.Equals(secondBlock.name);
    }

    private bool IsGameOver()
    {
        if (!IsTableFilled())
        {
            return false;
        }

        for (int x = 0; x < _size; x++) // start from first column
        {
            for (int y = 0; y < _size; y++) // start from first row
            {
                Transform currentNode = GetNodeFromBoard(x, y);

                if (x + 1 < _size) // check for right node
                {
                    Transform rightNode = GetNodeFromBoard(x + 1, y);

                    if (AreTheNumbersEqual(currentNode, rightNode)) // if the numbers are equal, it means there are still moves
                    {
                        return false;
                    }
                }
                if (y + 1 < _size) // check for up node
                {
                    Transform upNode = GetNodeFromBoard(x, y + 1);

                    if (AreTheNumbersEqual(currentNode, upNode)) // if the numbers are equal, it means there are still moves
                    {
                        return false;
                    }
                }
            }
        }

        // no more moves, game is over
        return true;
    }

    private bool IsTableFilled()
    {
        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                if (GetNodeFromBoard(x, y).childCount.Equals(0)) // if any node has a number inside, then the game is not over
                {
                    return false;
                }
            }
        }

        return true;
    }

    private Transform GetNodeFromBoard(int x, int y)
    {
        return _nodesParent.transform.GetChild(y * _size + x);
    }
}
