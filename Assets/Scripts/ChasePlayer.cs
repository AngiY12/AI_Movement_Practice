using UnityEngine;
using UnityEngine.AI;
public class ChasePlayer : MonoBehaviour{
 [Header("Chase Configuration")]
 public Transform playerTarget;
 public float chaseRange = 15f;
 public float attackRange = 2f;
 public float updatePathInterval = 0.5f;
 private NavMeshAgent agent;
 private float pathUpdateTimer;
 void Start() {
 agent = GetComponent<NavMeshAgent>();

 if (playerTarget == null) {
 // Buscar automáticamente al jugador si no está asignado
 GameObject player = GameObject.FindGameObjectWithTag("Player");
 if (player != null)
 playerTarget = player.transform;
 }
 }
 void Update() {
 if (playerTarget == null) return;
 float distance = Vector3.Distance(transform.position, playerTarget.position);
 // Actualizar el path periódicamente para mejor rendimiento
 pathUpdateTimer += Time.deltaTime;
 if (pathUpdateTimer >= updatePathInterval) {
 pathUpdateTimer = 0;
 if (distance <= chaseRange) {
 agent.SetDestination(playerTarget.position);
 } else {
 // Si está fuera del rango, detener el movimiento
 if (agent.hasPath)
 agent.ResetPath();
 }
 }
 // Detectar si está en rango de ataque
 if (distance <= attackRange) {
 Debug.Log("Enemy attacking player!");
 // Aquí se puede llamar a la lógica de ataque
 }
 }
 // Visualizar rangos en la escena
 void OnDrawGizmosSelected() {
 Gizmos.color = Color.yellow;
 Gizmos.DrawWireSphere(transform.position, chaseRange);

 Gizmos.color = Color.red;
 Gizmos.DrawWireSphere(transform.position, attackRange);
 }
}