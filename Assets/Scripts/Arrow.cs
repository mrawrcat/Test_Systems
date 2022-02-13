using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public static void Create_Arrow(Vector3 spawnPos, Vector3 targetPos, float speed)
    {
        Transform arrowTransfrom = Instantiate(GameResources.instance.arrow, spawnPos, Quaternion.identity);
        Arrow arrow = arrowTransfrom.GetComponent<Arrow>();
        arrow.SetUp(spawnPos, targetPos, speed);
    }

    private Vector3 targetPosition;
    private Vector3 spawnPosition;
    //private float startTime;
    private float journeyTime;

    private void SetUp(Vector3 spawnPos, Vector3 targetPos, float speed)
    {
        this.journeyTime = speed;
        this.spawnPosition = spawnPos;
        this.targetPosition = new Vector3(targetPos.x, -4, 0); //-4 is where the ground is
    }


    private void Update()
    {
        float distance = targetPosition.x - spawnPosition.x;
        float nextX = Mathf.MoveTowards(transform.position.x, targetPosition.x, journeyTime * Time.deltaTime);
        float baseY = Mathf.Lerp(spawnPosition.y, targetPosition.y, (nextX - spawnPosition.x) / distance);
        float height = 2 * (nextX - spawnPosition.x) * (nextX - targetPosition.x) / (-.25f * distance * distance);

        Vector3 movePos = new Vector3(nextX, baseY + height, transform.position.z);
        transform.rotation = LookAtTarget(movePos - transform.position);
        transform.position = movePos;
    }

    public static Quaternion LookAtTarget(Vector2 rotation)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDmg_By_Ally<int> objToDmg = collision.gameObject.GetComponent<IDmg_By_Ally<int>>();
        if (objToDmg != null)
        {
            Debug.Log("arrow hit ally");
            //objToDmg.TakeDmg(50); this is the IDmgByAlly
            objToDmg.DamageTaken(50); //this is the IDmg_By_Ally
            gameObject.SetActive(false);
        }

        if (collision.collider.tag == "Ground")
        {
            Debug.Log("arrow hit ground");
            gameObject.SetActive(false);
        }
    }
}
