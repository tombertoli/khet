using UnityEngine;
using System.Collections;

public class BoardSetup : MonoBehaviour {
  private Board board;

  [SerializeField] private int width, height;
	// Use this for initialization
	void Start () {
    board = new Board(width, height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
