using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{ 
	float step = 2f;     // 移動速度
	Vector3 target;      // 入力受付時、移動後の位置を算出して保存 
	Vector3 prevPos;     // 何らかの理由で移動できなかった場合、元の位置に戻すため移動前の位置を保存
    Quaternion targetrotation;
    Vector3 rotationAngles;
    public Text friendText;
    public Text clearText;
    public Text missText;
    private int friend;
 
	// Use this for initialization
	void Start () {
		target = transform.position;
        targetrotation = transform.rotation;
        rotationAngles = targetrotation.eulerAngles;
        friend = 100;
        SetCountText();
        clearText.text = " ";
        missText.text = " ";
	}
 
	// Update is called once per frame
	void Update () {
	
		if (transform.position == target) {
			SetTargetPosition ();
		}
		Move ();
        Rotate();
	}
	void SetTargetPosition(){
 
		prevPos = target;
 
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
            rotationAngles.y = rotationAngles.y + 90.0f;
            targetrotation = Quaternion.Euler(rotationAngles);
			return;
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
            rotationAngles.y = rotationAngles.y - 90.0f;
            targetrotation = Quaternion.Euler(rotationAngles);
			return;
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			target = transform.position + transform.forward * 2f;
			return;
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
            rotationAngles.y = rotationAngles.y + 180.0f;
            targetrotation = Quaternion.Euler(rotationAngles);
			return;
		}
	} 
	void Move(){
		transform.position = Vector3.MoveTowards (transform.position, target, step * Time.deltaTime);
	}
    void Rotate(){
        transform.rotation = targetrotation;
    }
    void OnCollisionEnter(Collision other){
        if(other.gameObject.CompareTag("Wall")){
            target = prevPos;
        }
    }
    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Goal")){
            clearText.text = "Clear!!";
            Invoke("ChangeScene", 1.5f);
        }else if(other.gameObject.CompareTag("3Branch")){
            friend -= 10;
            SetCountText();
        }else if(other.gameObject.CompareTag("4Branch")){
            friend -= 20;
            SetCountText();
        }
    }
    void SetCountText(){
        friendText.text = "Friend: " + friend.ToString();
        if(friend <= 0){
            missText.text = "友達がみんないなくなってしまった．．．";
            Invoke("ChangeScene", 1.5f);
        }
    }
    void ChangeScene(){
        if(friend > 0){
            SceneManager.sceneLoaded += ClearSceneLoaded;
            SceneManager.LoadScene("ClearResult");
        }else if(friend <= 0){
            SceneManager.sceneLoaded += MissSceneLoaded;
            SceneManager.LoadScene("MissResult");
        }
    }
    private void ClearSceneLoaded(Scene next, LoadSceneMode mode){
        var scoreManager = GameObject.Find("ClearResultManager").GetComponent<ClearResult>();
        SceneManager.sceneLoaded -= ClearSceneLoaded;
    }
    private void MissSceneLoaded(Scene next, LoadSceneMode mode){
        var scoreManager = GameObject.Find("MissResultManager").GetComponent<MissResult>();
        SceneManager.sceneLoaded -= MissSceneLoaded;
    }
}
