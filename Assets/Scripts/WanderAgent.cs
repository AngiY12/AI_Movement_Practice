using UnityEngine;
using UnityEngine.AI;
public class WanderAgent : MonoBehaviour{
 [Header("Wander Configuration")]
 public float wanderRadius = 10f; // Radio de búsqueda para nuevo destino
 public float wanderTimer = 3f; // Tiempo entre cambios de destino
 public float stoppingDistance = 0.5f; // Distancia para considerar que llegó
 private NavMeshAgent agent;
 private float timer;
 void Start() {
 agent = GetComponent<NavMeshAgent>();
 timer = wanderTimer;
 }
 void Update() {
 timer += Time.deltaTime;
 // Si el agente llegó al destino o pasó el timer, buscar nuevo destino
 if (timer >= wanderTimer || agent.remainingDistance < stoppingDistance) {
 SetRandomDestination();
 timer = 0;
 }
 }
 void SetRandomDestination() {
 // Generar un punto aleatorio dentro del radio
 Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
 randomDirection += transform.position;

 // Proyectar el punto al NavMesh para asegurar que es caminable
 NavMeshHit hit;
 if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas)) {
 agent.SetDestination(hit.position);
 }
 }
 // Visualizar el radio de deambulación
 void OnDrawGizmosSelected() {
 Gizmos.color = Color.green;
 Gizmos.DrawWireSphere(transform.position, wanderRadius);
 }
}