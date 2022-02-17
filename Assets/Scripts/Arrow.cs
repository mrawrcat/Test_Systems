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
        /*
        */
        float hitDetection = 1f;
        BaseEnemy closestBaseEnemy = BaseEnemy.GetClosestEnemy(transform.position, hitDetection);
        if (closestBaseEnemy != null)
        {
            if(Vector3.Distance(transform.position, closestBaseEnemy.transform.position) < 1f)
            {
                Debug.Log("hit enemy #" + closestBaseEnemy.GetIndexPositionInActiveBaseEnemyList());
                closestBaseEnemy.DamageTaken(50);
                Destroy(gameObject);
                //gameObject.SetActive(false);

            }
        }
    }

    public static Quaternion LookAtTarget(Vector2 rotation)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            Debug.Log("arrow hit ground");
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            Debug.Log("arrow hit ground");
            gameObject.SetActive(false);
        }
    }
}
