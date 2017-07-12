using UnityEngine;
using System.Collections;

public class UnderlineSetup : MonoBehaviour {
  [SerializeField] private GameObject go;
  
	void Start() {
    for (int i = 0; i < BoardSetup.b.Width; i++) {
      for (int j = 0; j < BoardSetup.b.Height; j++) {
        Underline u = BoardSetup.b.GetUnderline(new Point(i, j));;
        
        Vector3 position = new Vector3(
                             i + transform.position.x,
                             transform.position.y,
                             j + transform.position.z);
        
        GameObject instance = (GameObject)Instantiate(go, position, Quaternion.identity);
        Renderer r = instance.GetComponent<Renderer>();
        
        instance.transform.parent = transform;
        
        if (u == Underline.RedHorus) r.material.color = Color.red;
        else if (u == Underline.SilverAnkh) r.material.color = Color.grey;          
      }
    }
	}
}