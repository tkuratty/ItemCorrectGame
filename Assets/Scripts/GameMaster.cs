using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameMaster : MonoBehaviour
{
  private int totalPickUpCount = 12;
  public TextMeshProUGUI countText;
  public TextMeshProUGUI remainingText;
  public GameObject winTextObject;
  public GameObject pickUpObj;
  public GameObject playerObj;

  private int count;
  private int point;
  private bool pauseGame;
  public GameObject pickupItems;

  void Awake()
  {
    point = 0;
    pauseGame = false;
  }
  // Start is called before the first frame update
  void Start()
  {
    winTextObject.SetActive(false);
    StageGenerate();
    UpdateInfoTexts();
    PutPlayer();
  }

  IEnumerator WaitStart()
  {
    pauseGame = true;
    TextMeshProUGUI textObj = winTextObject.GetComponent<TextMeshProUGUI>();
    yield return new WaitForSeconds(1.0f);
    for (int i = 3; i > 0; i--)
    {
      textObj.text = i.ToString();
      yield return new WaitForSeconds(1.0f);
    }
    textObj.text = "GO!";
    yield return new WaitForSeconds(1.0f);
    winTextObject.SetActive(false);
    textObj.text = "Try Next!!";
    pauseGame = false;
    yield break;
  }
  void UpdateInfoTexts()
  {
    this.countText.text = "Score: " + this.point.ToString();
    this.remainingText.text = "Remaining: " +
        (this.totalPickUpCount - this.count).ToString();
  }
  void GameRestart()
  {
    winTextObject.SetActive(true);
    StageGenerate();
    UpdateInfoTexts();
    StartCoroutine("WaitStart");
  }
  void PutPlayer()
  {
    GameObject g = Instantiate(playerObj, pickupItems.transform);
    g.name = "Player";
    g.GetComponent<PlayerController>().gameMaster = this.gameObject;

  }
  public void AddPoint(int pointAdd)
  {
    this.point += pointAdd;
    this.count++;
    UpdateInfoTexts();
    // Clear stage & restart
    if (count >= totalPickUpCount)
    {
      GameRestart();
    }
  }

  void StageGenerate()
  {
    count = 0;

    List<int> posXs = new List<int>();
    List<int> posZs = new List<int>();
    // -8 to 9
    for (int i = 2; i <= 19; i++)
    {
      posXs.Add(i);
      posZs.Add(i);
    }
    for (int i = 1; i <= totalPickUpCount; i++)
    {
      GameObject g = Instantiate(pickUpObj, pickupItems.transform);
      g.name = "pickup" + i.ToString();
      // Generate posision X and Z (not duplicated)
      int idx = Random.Range(0, posXs.Count);
      int posX = posXs[idx] - 10;
      posXs.RemoveAt(idx);
      idx = Random.Range(0, posZs.Count);
      int posZ = posZs[idx] - 10;
      posZs.RemoveAt(idx);

      g.transform.position = new Vector3(posX, 0.5f, posZ);
    }
  }
  public bool DisableInput
  {
    get { return this.pauseGame; }
  }
}