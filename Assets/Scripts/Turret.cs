using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    [SerializeField] private ParticleSystem muzzleEffect;
    [SerializeField] private Material[] levelMaterials;
    [SerializeField] private MeshRenderer bodyMesh;
    public int damage = 1;
    private float attackTimer = 1f;
    public float attackTime = 1f;
    [SerializeField] GameObject target;
    private Transform targetPos;
    private Vector3 lookAtPos = new Vector3(21f, 2f, -12f);
    [SerializeField] Transform baseT;
    [SerializeField] Transform barrelT;
    private float rotationSpeed = 15f;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private GameObject currentEnemy;


    // Start is called before the first frame update
    void Start() {

    }


    // Update is called once per frame
    void Update() {
        LookAtTarget();
        if (attackTimer < attackTime) {
            attackTimer += Time.deltaTime;
        }

        if (currentEnemy != null) {

            if (attackTimer >= attackTime) {
                Attack();
                attackTimer = 0;
            }
        }
        else {
            NextTarget();
        }
    }


    private void LookAtTarget() {
        if (targetPos != null) {
            lookAtPos = targetPos.position;
        }
        // ROTATE BASE (Y-Axis)
        Vector3 baseDirection = lookAtPos - baseT.position;
        baseDirection.y = 0f; // ignore vertical component for base rotation
        if (baseDirection != Vector3.zero) {
            Quaternion baseTargetRotation = Quaternion.LookRotation(baseDirection);
            baseT.rotation = Quaternion.Slerp(baseT.rotation, baseTargetRotation, rotationSpeed * Time.deltaTime);
        }

        // ROTATE BARREL (X-Axis)
        Vector3 barrelDirection = lookAtPos - barrelT.position;
        Quaternion barrelTargetRotation = Quaternion.LookRotation(barrelDirection);
        // Only rotate around the local X-axis (pitch)
        Vector3 localBarrelEuler = barrelT.localEulerAngles;
        float pitch = barrelTargetRotation.eulerAngles.x;
        // Normalize pitch to avoid gimbal lock issues
        if (pitch > 180) pitch -= 360;
        localBarrelEuler.x = Mathf.LerpAngle(localBarrelEuler.x, pitch, rotationSpeed * Time.deltaTime);
        barrelT.localEulerAngles = localBarrelEuler;
    }


    private void Attack() {
        //Debug.Log("Attacking");
        currentEnemy.GetComponent<Enemy>().ReceiveDamage(damage);
        muzzleEffect.Play();
    }


    private void NextTarget() {
        enemies.RemoveAll(e => e == null);
        if (enemies.Count > 0) {
            currentEnemy = enemies[0];
            targetPos = currentEnemy.transform;
            //Debug.Log("Changed targets");
        }
        else {
            currentEnemy = null;
            targetPos = null;
            //Debug.Log("No targets");
        }
    }


    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy") {
            enemies.Add(other.gameObject);
            if (currentEnemy == null) {
                currentEnemy = other.gameObject;
                targetPos = currentEnemy.transform;
            }
            //Debug.Log("Enemy entered");
        }
    }


    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Enemy") {
            enemies.Remove(other.gameObject);
            //Debug.Log("Enemy exited");
            if (currentEnemy == other.gameObject) {
                NextTarget();
            }
        }
    }

    public void UpdateBody(int lvl) {
        bodyMesh.material = levelMaterials[lvl];
    }
}
