/*
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
 
[RequireComponent(typeof(Rigidbody))]
public class scr_Exemple_FSM : MonoBehaviour
{
	
	//Variables Generales de l'Ennemi
    public GameObject player;
    public Transform[] path;
    private FSMSystem fsm;
	private ChromatoseManager chromanager;
	private EndBoss_DataBase data;
	
 	//Methode obligatoire, ne pas toucher
    public void SetTransition(Transition t) { fsm.PerformTransition(t); }
	
	//Vous pouvez initialiser vos propres methodes ici
 
	
	//Vous pouvez instancier des valeur a des variables pour l'ennemis en general dans ce Start-ci
    public void Start()
    {
        MakeFSM();
    }
 
	//Update() de l'ennemi
    public void FixedUpdate()
    {
		//Laissez ces 2 lignes intacts
        fsm.CurrentState.Reason(player, gameObject, chromanager, data);
        fsm.CurrentState.Act(player, gameObject, chromanager, data);
		
		///Placez votre code que vous desirez mettre dans le FixedUpdate en-dessous
		///Mais plus vous mettez de code ici, plus votre script de viendra lourd et la
		///StateMachine de plus en plus inutile face a l'optimisation du script
		///Exemple;
		if (Input.GetKeyDown(KeyCode.A)){
			//Effectue la transition Exemple_SawPlayer
			fsm.PerformTransition(Transition.Exemple_SawPlayer);
		}
		
		
    }
	
	//Exemple d'utilisation d'un trigger
	public void OnTriggerEnter(Collider zoneTrigger)
	{
		if (zoneTrigger.tag == "TriggerBox"){
			//Effectue la transition Exemple_SawPlayer
			fsm.PerformTransition(Transition.Exemple_SawPlayer);
			
		}
	}
	
 
	//Le NPC a 2 States; Exemple_FollowPath et Exemple_ChasePlayer
	//Si, lorsque le NPC se trouve dans le State Exemple_FollowPath, et que
	//la transition Exemple_SawPlayer est appeller On passe au State Exemple_ChasePlayer
	//Si, lorsque le NPC se trouve dans le State Exemple_ChasePlayer, et que
	//la transition Exemple_LostPlayer est appeller On repasse au State Exemple_FollowPath
    private void MakeFSM()
    {
		//Exemple d'associer des paires dans la Machine Instancier
		
		//Association de la paire Exemple_FollowPathState, representant la paire exemple_Follow dans la machine
        Exemple_FollowPathState exemple_Follow = new Exemple_FollowPathState(path);
        exemple_Follow.AddTransition(Transition.Exemple_SawPlayer, StateID.Exemple_ChasingPlayer);
 
		//Association de la paire Exemple_ChasePlayerState, representant la paire exemple_Chase dans la machine
        Exemple_ChasePlayerState exemple_Chase = new Exemple_ChasePlayerState();
        exemple_Chase.AddTransition(Transition.Exemple_LostPlayer, StateID.Exemple_FollowingPath);
 
		//Instantiation de la StateMachine et de ces paires
        fsm = new FSMSystem();
        fsm.AddState(exemple_Follow);
        fsm.AddState(exemple_Chase);
    }
}
 
//Class representant le State Exemple_FollowPathState decoulant de FSMState
public class Exemple_FollowPathState : FSMState
{
	//Variables unique a cette Class
    private int currentWayPoint;
    private Transform[] waypoints;
 
	//Instantiation des variables a l'interieur de Exemple_FollowPathState
    public Exemple_FollowPathState(Transform[] wp) 
    { 
        waypoints = wp;
        currentWayPoint = 0;
        stateID = StateID.Exemple_FollowingPath;
    }
 
	//Methode Reason, Decide si le State doit passer a une transition
    public override void Reason(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
        // Si le joueur est a 15 metres ou moins du npc, appelle la transition Exemple_SawPlayer
        RaycastHit hit;
        if (Physics.Raycast(npc.transform.position, npc.transform.forward, out hit, 15F))
        {
            if (hit.transform.gameObject.tag == "Player")
                npc.GetComponent<scr_Exemple_FSM>().SetTransition(Transition.Exemple_SawPlayer);
        }
    }
 
	//Methode Act, Effectue les actions, les controlles, les evenements, les communications
    public override void Act(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
		
        // Suit les waypoint
		// Trouve la direction du Waypoint
        Vector3 vel = npc.rigidbody.velocity;
        Vector3 moveDir = waypoints[currentWayPoint].position - npc.transform.position;
 
        if (moveDir.magnitude < 1)
        {
            currentWayPoint++;
            if (currentWayPoint >= waypoints.Length)
            {
                currentWayPoint = 0;
            }
        }
        else
        {
            vel = moveDir.normalized * 10;
 
            // Se tourne vers le prochain Waypoint
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
                                                      Quaternion.LookRotation(moveDir),
                                                      5 * Time.deltaTime);
            npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);
 
        }
 
        // Applique la Velocite
        npc.rigidbody.velocity = vel;
		}     
} // Fin de la Class du State Exemple_FollowPathState
 


//Class representant le State Exemple_ChasePlayerState decoulant de FSMState
public class Exemple_ChasePlayerState : FSMState
{
	//Instantiation des variables a l'interieur de Exemple_ChasePlayerState
    public Exemple_ChasePlayerState()
    {
        stateID = StateID.Exemple_ChasingPlayer;
    }
 
    public override void Reason(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
        // Si le joueur se trouve a plus de 30 metres du NPC, effectue la transition Exemple_LostPlayer
        if (Vector3.Distance(npc.transform.position, player.transform.position) >= 30)
            npc.GetComponent<scr_Exemple_FSM>().SetTransition(Transition.Exemple_LostPlayer);
    }
 
    public override void Act(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
        // Trouve la direction du Joueur 		
        Vector3 vel = npc.rigidbody.velocity;
        Vector3 moveDir = player.transform.position - npc.transform.position;
 
        // Se tourne vers le Joueur
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
                                                  Quaternion.LookRotation(moveDir),
                                                  5 * Time.deltaTime);
        npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);
 
        vel = moveDir.normalized * 10;
 
        // Applique la velocite
        npc.rigidbody.velocity = vel;
    }
 
} // Fin de la Class du State Exemple_ChasePlayerState*/