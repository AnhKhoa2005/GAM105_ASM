using System.ComponentModel;
using System.ComponentModel.Design;
using Edgar.Unity;
using Unity.VisualScripting;
using UnityEngine;

public class BG_loop : MonoBehaviour
{
    [HelpBox("Gán cinemachinecamera vào Cam")]
    [SerializeField] Transform Cam;
    [HelpBox("Gán player vào player")]
    [SerializeField] Transform player;
    [HelpBox("Nhân width các BG với số lượng vừa đủ rồi điền số đó vào đây")]
    [SerializeField] float times;
    [HelpBox("Chỉ đọc, không được sửa")]
    [SerializeField] private float[] distance;
    [SerializeField] SpriteRenderer[] BG;
    Vector3 movement = Vector3.zero;

    void Update()
    {
        //Cho camera di chuyển theo trục y của player
        Vector3 BGposition = this.transform.position;
        BGposition.y = Cam.position.y;
        this.transform.position = BGposition;

        //Lấy hướng của player
        movement.x = Input.GetAxisRaw("Horizontal");

        for (int i = 0; i < BG.Length; i++)
        {
            BG[i].transform.Translate(movement * (-i) * Time.deltaTime); //Cho BG di chuyển ngược lại với player

            //Nếu khoảng cách từ player đến tâm BG lớn hơn chiều dài ban đầu của BG thì dịch chuyển BG đến player
            if (Mathf.Abs(player.transform.position.x - BG[i].transform.position.x) >= distance[i] / times)
            {
                Vector3 _BGposition = BG[i].transform.position;
                _BGposition.x = player.transform.position.x;
                BG[i].transform.position = _BGposition;
            }
        }
    }

    void OnDrawGizmos()
    {
        //Tự gán các các sprite là con GameObject BG này vào mảng BG
        BG = GetComponentsInChildren<SpriteRenderer>();
        distance = new float[BG.Length];
        for (int i = 0; i < BG.Length; i++)
        {
            BG[i].sortingLayerName = "Background";
            BG[i].sortingOrder = i;
            distance[i] = BG[i].bounds.size.x;
        }
    }
}
