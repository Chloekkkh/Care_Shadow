using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunState : Basic_State
{
    private float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;
    public bool isFirstTime = true;
    

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    // enemy action mode
    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            if (isFirstTime)
            {
                shotTimer = enemy.fireRate - 1f;
                isFirstTime = false;
            }
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;

            if (shotTimer > enemy.fireRate)
            {
                Shoot();
            }
            // Keep the enemy looking at the player when the enemy sees the player
            // enemy_StateMechina.ChangeState(new GunState());
            // Quaternion quaternion = Quaternion.LookRotation((Main_manager.instance.normal_camera.transform.position - enemy.transform.position).normalized);
            // enemy.transform.rotation = quaternion;



            //change the quaternion of enemy
            enemy.transform.LookAt(PlayerMovement.instance.transform.position);

            // Move in a pattern A
            if (moveTimer > Random.Range(3, 5))
            {
                // This method is ineffective
                // float xOffSet = Mathf.Sin(Time.deltaTime * 2f);
                // enemy.transform.position = new Vector3(enemy.transform.position.x + xOffSet, enemy.transform.position.y , enemy.transform.position.z) ;

                enemy.NavMeshAgent.SetDestination(enemy.transform.position + (Random.insideUnitSphere) * 2);

                moveTimer = 0;
            }
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 2f)
            {
                enemy_StateMachine.ChangeState(new PatrolState());
            }
        }
    }

    public void Shoot()
    {/*
        // 1. Find the firing position
        Transform firePosition = enemy.firePosition;

        if (firePosition != null)
        {
            // 2. Instantiate a bullet at the firing position
            GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, firePosition.position, enemy.transform.rotation);

            // 3. Find the direction to shoot
            Vector3 fireDirection = (enemy.player.transform.position - firePosition.position).normalized;

            // 4. Give the bullet a force in the shooting direction and specify a random angle
           // bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-3, 3), Vector3.up) * fireDirection * enemy.fireForce;
            bullet.GetComponent<Rigidbody>().velocity =  fireDirection * enemy.fireForce;



            shotTimer = 0;
        }
        */
        Transform firePosition = enemy.firePosition;

        Vector3 directionToPlayer = (enemy.player.transform.position - firePosition.position).normalized; // 基本方向

        // 实例化并发射中间的飞镖
        GameObject middleDart = GameObject.Instantiate(Resources.Load("Prefabs/Dart") as GameObject, firePosition.position, Quaternion.LookRotation(directionToPlayer));
        middleDart.GetComponent<Rigidbody>().AddForce(directionToPlayer * enemy.fireForce, ForceMode.VelocityChange);

        // 实例化并发射左边的飞镖（30度偏差）
        Quaternion leftRotation = Quaternion.Euler(0, 60, 0) * Quaternion.LookRotation(directionToPlayer);
        GameObject leftDart =GameObject.Instantiate(Resources.Load("Prefabs/Dart") as GameObject, firePosition.position, leftRotation);
        leftDart.GetComponent<Rigidbody>().AddForce(leftRotation * Vector3.forward * enemy.fireForce, ForceMode.VelocityChange);

        // 实例化并发射右边的飞镖（-30度偏差）
        Quaternion rightRotation = Quaternion.Euler(0, -60, 0) * Quaternion.LookRotation(directionToPlayer);
        GameObject rightDart =GameObject.Instantiate(Resources.Load("Prefabs/Dart") as GameObject, firePosition.position, rightRotation);
        rightDart.GetComponent<Rigidbody>().AddForce(rightRotation * Vector3.forward * enemy.fireForce, ForceMode.VelocityChange);

        AudioManager.instance.ShootAudio();

        shotTimer = 0;
    }
}
