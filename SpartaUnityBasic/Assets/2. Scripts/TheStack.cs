using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStack : MonoBehaviour
{
    private const float BoundSize = 3.5f;
    private const float MovingBoundsSize = 3f;
    private const float StackMovingSpeed = 5.0f;
    private const float BlockMovingSpeed = 3.5f;
    private const float ErrorMargin = 0.1f;
    private const string BestScoreKey = "BestScroe";
    private const string BestComboKey = "BestCombo";

    public GameObject originBlock = null;

    private Vector3 prevBlockPosition;
    private Vector3 desiredPosition;
    private Vector3 stackBounds = new Vector2(BoundSize, BoundSize);

    private Transform lastBlock = null;

    private float blockTransition = 0f;
    private float secondaryPosition = 0f;

    private int bestScore = 0;
    public int BestScore => bestScore;

    private int bestCombo = 0;
    public int BestCombo => bestCombo;

    private int stackCount = -1;
    public int Score => stackCount;

    private int comboCount = 0;
    public int Combo => comboCount;

    private int maxCombo = 0;
    public int MaxCombo => maxCombo;


    public Color prevColor;
    public Color nextColor;

    private Camera mainCamera;

    private bool isMovingX = true;

    private bool isGameOver;

    void Start()
    {
        if (originBlock == null)
        {
            Debug.Log("Origin Block is Null");
            return;
        }

        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        bestCombo = PlayerPrefs.GetInt(BestComboKey, 0);

        mainCamera = Camera.main;
        prevBlockPosition = Vector3.down;
        Spawn_Block();
    }

    void Update()
    {
        if (isGameOver)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            if (PlaceBlock())
            {
                Spawn_Block();
            }
            else
            {
                Debug.Log("Game Over");
                UpdateScore();
                isGameOver = true;
                GameOverEffect();
            }
        }

        MoveBlock();
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * StackMovingSpeed);
    }

    private void UpdateScore()
    {
        if (bestScore < stackCount)
        {
            Debug.Log("최고 점수 갱신");
            bestScore = stackCount;
            bestCombo = maxCombo;

            PlayerPrefs.SetInt(BestScoreKey, bestScore);
            PlayerPrefs.SetInt(BestComboKey, bestCombo);
        }
    }

    private bool PlaceBlock()
    {
        Vector3 lastPosition = lastBlock.localPosition;
        if (isMovingX)
        {
            float deltaX        = prevBlockPosition.x - lastPosition.x;
            bool  isNegativeNum = (deltaX < 0) ? true : false;

            deltaX = Mathf.Abs(deltaX);
            if (deltaX > ErrorMargin)
            {
                stackBounds.x -= deltaX;
                if (stackBounds.x <= 0)
                {
                    return false;
                }

                float middle = (prevBlockPosition.x + lastPosition.x) / 2;
                lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

                Vector3 tempPosition = lastBlock.localPosition;
                tempPosition.x = middle;
                lastBlock.localPosition = lastPosition = tempPosition;

                float rubbleHalfScale = deltaX / 2f;
                CreateRubble(new Vector3(isNegativeNum
                        ? lastPosition.x + stackBounds.x / 2 + rubbleHalfScale
                        : lastPosition.x - stackBounds.x / 2 - rubbleHalfScale, lastPosition.y, lastPosition.z),
                    new Vector3(deltaX, 1, stackBounds.y));

                comboCount = 0;
            }
            else
            {
                CheckCombo();
                lastBlock.localPosition = prevBlockPosition + Vector3.up;
            }
        }
        else
        {
            float deltaZ        = prevBlockPosition.z - lastPosition.z;
            bool  isNegativeNum = (deltaZ < 0) ? true : false;

            deltaZ = Mathf.Abs(deltaZ);
            if (deltaZ > ErrorMargin)
            {
                stackBounds.z -= deltaZ;
                if (stackBounds.z <= 0)
                    return false;

                float middle = (prevBlockPosition.z + lastPosition.z) / 2;
                lastBlock.localScale = new Vector3(stackBounds.z, 1, stackBounds.z);

                Vector3 tempPosition = lastBlock.localPosition;
                tempPosition.z = middle;
                lastBlock.localPosition = lastPosition = tempPosition;

                float rubbleHalfScale = deltaZ / 2f;
                CreateRubble(new Vector3(isNegativeNum
                        ? lastPosition.x + stackBounds.x / 2 + rubbleHalfScale
                        : lastPosition.x - stackBounds.x / 2 - rubbleHalfScale, lastPosition.y, lastPosition.z),
                    new Vector3(deltaZ, 1, stackBounds.y));
            }
            else
            {
                lastBlock.localPosition = prevBlockPosition + Vector3.up;
            }
        }

        secondaryPosition = (isMovingX) ? lastBlock.localPosition.x : lastBlock.localPosition.z;

        return true;
    }

    private void CheckCombo()
    {
        comboCount++;
        if (comboCount > maxCombo)
        {
            maxCombo = comboCount;
        }

        if ((comboCount % 5) == 0)
        {
            Debug.Log("5Combo Success");
            stackBounds += new Vector3(0.5f, 0.5f);
            stackBounds.x =
                (stackBounds.x > BoundSize) ? BoundSize : stackBounds.x;
            stackBounds.y =
                (stackBounds.y > BoundSize) ? BoundSize : stackBounds.y;
        }
    }

    private void CreateRubble(Vector3 pos, Vector3 scale)
    {
        GameObject go = Instantiate(lastBlock.gameObject, transform);

        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        go.transform.localRotation = Quaternion.identity;

        go.AddComponent<Rigidbody>();
        go.name = "Rubble";
    }

    private void MoveBlock()
    {
        blockTransition += Time.deltaTime * BlockMovingSpeed;
        float movePosition = Mathf.PingPong(blockTransition, BoundSize) - BoundSize / 2;

        if (isMovingX)
        {
            lastBlock.localPosition = new Vector3(movePosition * MovingBoundsSize, stackCount, secondaryPosition);
        }
        else
        {
            lastBlock.localPosition = new Vector3(secondaryPosition, stackCount, -movePosition * MovingBoundsSize);
        }
    }

    private bool Spawn_Block()
    {
        if (lastBlock != null)
        {
            prevBlockPosition = lastBlock.localPosition;
        }

        GameObject newBlock = null;
        Transform  newTrans = null;

        newBlock = Instantiate(originBlock);

        if (newBlock == null)
        {
            Debug.Log("NewBlock is null");
            return false;
        }

        ColorChange(newBlock);
        newTrans = newBlock.transform;
        newTrans.parent = this.transform;
        newTrans.localPosition = prevBlockPosition + Vector3.up;
        newTrans.localRotation = Quaternion.identity;
        newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

        stackCount++;

        desiredPosition = Vector3.down * stackCount;
        blockTransition = 0;
        lastBlock = newTrans;

        return true;
    }

    Color GetRandomColor()
    {
        float r = Random.Range(100f, 250f) / 255f;
        float g = Random.Range(100f, 250f) / 255f;
        float b = Random.Range(100f, 250f) / 255f;

        return new Color(r, g, b);
    }

    void ColorChange(GameObject go)
    {
        Color apllyColor = Color.Lerp(prevColor, nextColor, (stackCount % 11) / 10f);

        Renderer rn = go.GetComponent<Renderer>();
        if (rn == null)
        {
            Debug.Log("Renderer is NULL!");
            return;
        }

        rn.material.color = apllyColor;
        mainCamera.backgroundColor = apllyColor - new Color(0.1f, 0.1f, 0.1f);

        if (apllyColor.Equals(nextColor))
        {
            prevColor = nextColor;
            nextColor = GetRandomColor();
        }
    }

    void GameOverEffect()
    {
        int childCount = transform.childCount;

        for (int i = 1; i < 20; i++)
        {
            if (childCount < i)
                break;

            GameObject go = transform.GetChild(childCount - i).gameObject;

            if (go.name.Equals("Rubble"))
            {
                continue;
            }

            Rigidbody rb = go.AddComponent<Rigidbody>();

            rb.AddForce(
                (Vector3.up * Random.Range(0, 10f)
                 + Vector3.right * (Random.Range(0, 10f) - 5f))
                * 100f);
        }
    }
}