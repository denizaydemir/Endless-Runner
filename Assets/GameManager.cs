using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static List<Plane> PlaneList;
    public static List<GameObject> PlaneObjectList;
    public int Distance = 0;
    public float CurrentSpeed = 2f;
    public float SpeedIncreaseFactor;
    public int PlaneCountToIncreaseSpeed = 10;
    public int GlobalPlaneNumber = 5;
    public float EnemySpawnIntervalBegin = 2f;
    public float EnemySpawnIntervalEnd = 4f;

    public static Animator PlayerAnimator;

    public GameObject Plane0;
    public GameObject Plane1;
    public GameObject Plane2;
    public GameObject Plane3;
    public GameObject Plane4;
    public GameObject EnemyPrefab;
    public GameObject PlayerCamera;
    public GameObject TextDebug;

    private static GameObject _currentFrontRobot;
    private static GameObject _currentFrontRobotChild;

    private Rigidbody _rigidbody;
    private int _positionIndex;
    private int _passedPlaneCountModded;
    private int _planeScale = 10;
    private int _score;
    private bool _newEnemyCanSpawn;
    private bool _isAvailableToChangeDirection = true;
    private bool _isAvailableToAttack = true;
    private static int _highScore = 0;//pull from xml
    private static bool _isRobotAtFront = false;
    private static bool _isPlayerActive = true;
    private static bool _isPlayerDead = false;


    void Awake()
    {
        PlaneObjectList = new List<GameObject>(GlobalPlaneNumber);
        PlaneList = new List<Plane>();
        PlaneObjectList.Add(Plane0);
        PlaneObjectList.Add(Plane1);
        PlaneObjectList.Add(Plane2);
        PlaneObjectList.Add(Plane3);
        PlaneObjectList.Add(Plane4);
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        PlayerAnimator = gameObject.GetComponent<Animator>();
        SpeedIncreaseFactor = 1f / 5f;
        _positionIndex = 0;
        _passedPlaneCountModded = 0;
        _score = 0;
        _newEnemyCanSpawn = true;
        _isPlayerDead = false;
    }



    void Start()
    {
        InitiliazePlaneList();
        _isPlayerActive = true;
        HighScoreTextController.UpdateHighScoreText(_highScore);

    }

    void Update()
    {
        if (_isPlayerActive)
        {
            //Debug.Log(Input.inputString);
            if (transform.position.z > PlaneList[_positionIndex].ZPosition + 10)
            {
                PlaneList[_positionIndex].ZPosition += 50;
                Vector3 newPosition = new Vector3(0, 0, PlaneList[_positionIndex].ZPosition);
                PlaneList[_positionIndex].ThisGameObject.transform.position = newPosition;
                _positionIndex++;
                _passedPlaneCountModded++;
                _positionIndex = _positionIndex % (GlobalPlaneNumber);
                _score += 10; //10 points for griffindor
                ScoreTextController.UpdateScoreText(_score);
            }

            if (_passedPlaneCountModded >= PlaneCountToIncreaseSpeed)
            {
                IncreaseSpeed();
                EnemySpawnIntervalBegin *= 5f / 8f;
                EnemySpawnIntervalEnd *= 5f / 8f;
            }


            if (_newEnemyCanSpawn)
            {
                _newEnemyCanSpawn = false;
                //Debug.Log(EnemySpawnIntervalBegin);

                float randomTimeToSpawn = UnityEngine.Random.Range(EnemySpawnIntervalBegin, EnemySpawnIntervalEnd);
                StartCoroutine(SpawnEnemy(randomTimeToSpawn));
            }


#if UNITY_EDITOR
            if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && _isAvailableToChangeDirection)
            {
                StartCoroutine(MoveLeft());
            }

            if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && _isAvailableToChangeDirection)
            {
                StartCoroutine(MoveRight());
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }

            if ((Input.GetMouseButtonDown(0) && _isAvailableToAttack))
            {
                _isAvailableToAttack = false;
                StartCoroutine(AttackFront());
            }
#else
ScreenSwipeManager.DetectSwipe();
            if (ScreenSwipeManager.swipeDirection == Swipe.Left)
            {
                // do something...
                StartCoroutine(MoveLeft());
            }

            if (ScreenSwipeManager.swipeDirection == Swipe.Right)
            {
                // do something...
                StartCoroutine(MoveRight());
            }

            if (ScreenSwipeManager.swipeDirection == Swipe.Tap && _isAvailableToAttack)
            {
                // do something...
                _isAvailableToAttack = false;
                StartCoroutine(AttackFront());
            }


#endif
            




            if (_score >= _highScore)
            {
                _highScore = _score;
                HighScoreTextController.UpdateHighScoreText(_highScore);
            }

            MovePlayer();
            
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RestartPanelController.ToggleStaticPanel();
        }


    }

    public void MovePlayer()
    {
        _rigidbody.MovePosition(transform.position + transform.forward * Time.deltaTime * 10 * CurrentSpeed);
        float cameraNewZPos = transform.position.z - 6f;
        Vector3 newCameraPosition = new Vector3(0f, 9.13f, cameraNewZPos);
        PlayerCamera.transform.position = newCameraPosition;
    }


    public void IncreaseSpeed()
    {
        //Debug.Log("Current speed: " + CurrentSpeed);
        //Debug.Log("Speed factor: " + SpeedIncreaseFactor);
        CurrentSpeed += CurrentSpeed * SpeedIncreaseFactor;
        //Debug.Log("Current speeded: " + CurrentSpeed);
        _passedPlaneCountModded = _passedPlaneCountModded % PlaneCountToIncreaseSpeed;
        PlaneCountToIncreaseSpeed += 2 * PlaneCountToIncreaseSpeed;
    }

    public IEnumerator SpawnEnemy(float enemySpawnTime)
    {
        yield return new WaitForSeconds(enemySpawnTime);
        float randomXPosition = UnityEngine.Random.Range(-4.5f, 4.5f);
        Vector3 positionToSpawn = new Vector3(randomXPosition, 0f, transform.position.z + GlobalPlaneNumber * 10);
        GameObject newEnemy = Instantiate(EnemyPrefab, positionToSpawn, transform.rotation) as GameObject;
        var rotationVector = transform.rotation.eulerAngles;
        rotationVector.y = 270f;
        newEnemy.transform.rotation = Quaternion.Euler(rotationVector);
        newEnemy.transform.name = "Enemy";
        _newEnemyCanSpawn = true;
        Destroy(newEnemy, 6);
    }

    public void InitiliazePlaneList()
    {
        for (int i = 0; i < GlobalPlaneNumber; i++)
        {
            Plane planeToAdd = new Plane(i, i * 10 + 5, PlaneObjectList[i]);
            PlaneList.Add(planeToAdd);
        }
    }

    private IEnumerator MoveLeft()
    {
        float fixedX = transform.position.x;
        _isAvailableToChangeDirection = false;
        _rigidbody.AddForce(transform.right * -1200f);
        PlayerAnimator.SetBool("IsChangedDirectionLeft", true);
        yield return new WaitForSeconds(0.1f);
        _rigidbody.AddForce(transform.right * 1200f);
        PlayerAnimator.SetBool("IsChangedDirectionLeft", false);
        Vector3 fixedPosition = new Vector3(fixedX - 2.5f, transform.position.y, transform.position.z);
        transform.position = fixedPosition;
        _isAvailableToChangeDirection = true;
    }

    private IEnumerator MoveRight()
    {
        float fixedX = transform.position.x;
        _isAvailableToChangeDirection = false;
        _rigidbody.AddForce(transform.right * 1200f);
        PlayerAnimator.SetBool("IsChangedDirectionRight", true);
        yield return new WaitForSeconds(0.1f);
        _rigidbody.AddForce(transform.right * -1200f);
        PlayerAnimator.SetBool("IsChangedDirectionRight", false);
        Vector3 fixedPosition = new Vector3(fixedX + 2.5f, transform.position.y, transform.position.z);
        transform.position = fixedPosition;
        _isAvailableToChangeDirection = true;
    }

    private IEnumerator AttackFront()
    {
        if (_isRobotAtFront)
        {
            PassivateRobot();
            //additemdrop 
            DropRandomItem();
            _score += 20;
            ScoreTextController.UpdateScoreText(_score);
        }

        PlayerAnimator.SetBool("IsAttacked1", true);
        yield return new WaitForSeconds(0.2f);
        PlayerAnimator.SetBool("IsAttacked1", false);
        yield return new WaitForSeconds(0.8f);
        _isAvailableToAttack = true;
    }

    public static void DeadByRobot()
    {
        _isPlayerActive = false;
        _isPlayerDead = true;
        PlayerAnimator.SetBool("IsDead", true);
        //InventoryManager.InventoryToXML();
    }

    public static void SetRobotToKill(GameObject thisRobot, GameObject robotChild)
    {

        _isRobotAtFront = true;
        _currentFrontRobot = thisRobot;
        _currentFrontRobotChild = robotChild;
    }

    public static void ResetSelectedRobot()
    {
        _isRobotAtFront = false;
    }

    public void PassivateRobot()
    {
        _currentFrontRobot.GetComponent<BoxCollider>().enabled = false;
        _currentFrontRobotChild.GetComponent<BoxCollider>().enabled = false;
        _currentFrontRobot.GetComponent<Animator>().SetBool("IsEnemyDied", true);
        _isRobotAtFront = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
        ResetSelectedRobot();
        _isPlayerActive = true;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void DropRandomItem()
    {
        try
        {
            if (UnityEngine.Random.Range(0, 100) > 20)
            {
                DropCommonItem();
            }
            else
            {
                DropCraftableItem();
            }
        }
        catch (Exception e)
        {
            TextDebug.GetComponent<Text>().text = e.ToString();
        }
        
    }

    private void DropCommonItem()
    {
        int itemIndex = UnityEngine.Random.Range(0, ItemManager.CommonItemDropList.Count);
        //InventoryManager.ItemInventory.CommonItems.Add(ItemManager.CommonItemDropList[itemIndex]);
        InventoryManager.CommonDropAmountCheck(itemIndex);
        Debug.Log("Item Dropped:" + ItemManager.CommonItemDropList[itemIndex].Name);
    }

    private void DropCraftableItem()
    {
        int itemIndex = UnityEngine.Random.Range(0, ItemManager.CraftableItemDropList.Count);
        //InventoryManager.ItemInventory.CraftableItems.Add(ItemManager.CraftableItemDropList[itemIndex]);
        InventoryManager.CraftableDropAmountCheck(itemIndex);
        Debug.Log("Item Dropped:" + ItemManager.CraftableItemDropList[itemIndex].Name);
    }




    public static void EnablePlayerActions()
    {
        _isPlayerActive = true;
    }

    public static void DisablePlayerActions()
    {
        _isPlayerActive = false;
    }

    public static bool IsPlayerDead()
    {
        return _isPlayerDead;
    }

}
