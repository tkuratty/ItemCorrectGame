using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
  private Rigidbody rb;
  private float movementX;
  private float movementY;
  public float speed = 0;
  public GameObject gameMaster;

  // Start is called before the first frame update
  void Start()
  {
    // Set camera offset
    GameObject mainCamera = GameObject.Find("Main Camera");
    mainCamera.GetComponent<CameraController>().player = this.gameObject;
    mainCamera.GetComponent<CameraController>().SetOffset();
    // get rigid body
    rb = GetComponent<Rigidbody>();
  }
  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "Pickups")
    {
      other.gameObject.SetActive(false);
      this.gameMaster.GetComponent<GameMaster>().AddPoint(1);
    }
  }
  void OnMove(InputValue movementValue)
  {
    Vector2 movementVector = movementValue.Get<Vector2>();
    movementX = movementVector.x;
    movementY = movementVector.y;
  }
  void FixedUpdate()
  {
    if (gameMaster.GetComponent<GameMaster>().DisableInput)
    {
      // stop player if game is under disabled the input
      rb.velocity = Vector3.zero;
    }
    else
    {
      Vector3 movement = new Vector3(movementX, 0f, movementY);
      rb.AddForce(movement * speed);
    }
  }
}
