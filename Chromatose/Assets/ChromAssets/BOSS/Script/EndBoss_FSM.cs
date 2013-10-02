using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
 
[RequireComponent(typeof(Rigidbody))]
public class EndBoss_FSM : MonoBehaviour{
	
#region Variables
	//Variables Generales de l'Ennemi
    public GameObject player;
    private FSMSystem fsm;
	private ChromatoseManager chromanager;
	private EndBoss_DataBase data;
#endregion	

	
#region StateMachine CreateRoot
    public void SetTransition(Transition t) { fsm.PerformTransition(t); }
	
    public void Start()
    {
		player = GameObject.FindGameObjectWithTag("avatar");
		chromanager = ChromatoseManager.manager;
		data = this.gameObject.GetComponent<EndBoss_DataBase>();
        MakeFSM();
    }

    public void FixedUpdate()
    {
        fsm.CurrentState.Reason(player, gameObject, chromanager, data);
        fsm.CurrentState.Act(player, gameObject, chromanager, data);

    }
	
    private void MakeFSM()
    {
		Boss_IdleState boss_Idle = new Boss_IdleState();
			boss_Idle.AddTransition(Transition.tBoss_ToDeath, StateID.Boss_Death);
			boss_Idle.AddTransition(Transition.tBoss_PlayerAimed, StateID.Boss_AttackPlayer);
		Boss_AttackPlayerState boss_AttackPlayer = new Boss_AttackPlayerState();
		
		Boss_BlowState boss_Blow = new Boss_BlowState();
		
		Boss_ForwardState boss_Forward = new Boss_ForwardState();
		
		Boss_ReturnToPlaceState boss_ReturnToPlace = new Boss_ReturnToPlaceState();
		
		Boss_DeathState boss_Death = new Boss_DeathState();
		
        fsm = new FSMSystem();
		fsm.AddState(boss_Idle);
		fsm.AddState(boss_AttackPlayer);
		fsm.AddState(boss_Blow);
		fsm.AddState(boss_Forward);
		fsm.AddState(boss_ReturnToPlace);
		fsm.AddState(boss_Death);

    }
}
#endregion
 

#region Idle State
public class Boss_IdleState : FSMState
{
    public Boss_IdleState() 
    { 

    }
    public override void Reason(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
			//Verifie si on a depasser le nb de round allouer
		if(data.CheckRoundNb()){
			npc.GetComponent<EndBoss_FSM>().SetTransition(Transition.tBoss_ToDeath);
				Debug.Log("BOSS - Transition - Idle->Death");
		}
		if(data.canStart){
			npc.GetComponent<EndBoss_FSM>().SetTransition(Transition.tBoss_PlayerAimed);
				Debug.Log("BOSS - Transition - Idle->Attack");
		}
    }
     public override void Act(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
 
    }	 
}
#endregion

#region AttackPlayer State
public class Boss_AttackPlayerState : FSMState
{
    public Boss_AttackPlayerState() 
    { 

    }
    public override void Reason(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
			//Verifie si on a depasser le nb de round allouer
		if(data.CheckRoundNb()){
			npc.GetComponent<EndBoss_FSM>().SetTransition(Transition.tBoss_ToDeath);
				Debug.Log("BOSS - Transition - Idle->Death");
		}
    }
     public override void Act(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
 
    }	 
}
#endregion

#region Blow State
public class Boss_BlowState : FSMState
{
    public Boss_BlowState() 
    { 

    }
    public override void Reason(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {

    }
     public override void Act(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
 
    }	 
}
#endregion

#region Forward State
public class Boss_ForwardState : FSMState
{
    public Boss_ForwardState() 
    { 

    }
    public override void Reason(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {

    }
     public override void Act(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
 
    }	 
}
#endregion

#region ReturnToPlace State
public class Boss_ReturnToPlaceState : FSMState
{
    public Boss_ReturnToPlaceState() 
    { 

    }
    public override void Reason(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {

    }
     public override void Act(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
 
    }	 
}
#endregion

#region Death State
public class Boss_DeathState : FSMState
{
    public Boss_DeathState() 
    { 

    }
    public override void Reason(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {

    }
     public override void Act(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
 
    }	 
}
#endregion
