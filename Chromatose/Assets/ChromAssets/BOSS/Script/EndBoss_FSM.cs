using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class EndBoss_FSM : MonoBehaviour
{
	
#region Variables
	//Variables Generales de l'Ennemi
    public GameObject player;
    public static FSMSystem fsm;
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
			boss_AttackPlayer.AddTransition(Transition.tBoss_ToDeath, StateID.Boss_Death);
			boss_AttackPlayer.AddTransition(Transition.tBoss_GoToBlow, StateID.Boss_Blow);
		
		Boss_BlowState boss_Blow = new Boss_BlowState();
			boss_Blow.AddTransition(Transition.tBoss_ToDeath, StateID.Boss_Death);
			boss_Blow.AddTransition(Transition.tBoss_ReadyToForward, StateID.Boss_Forward);
		
		Boss_ForwardState boss_Forward = new Boss_ForwardState();
			boss_Forward.AddTransition(Transition.tBoss_ReturnIdle, StateID.Boss_Idle);
		
		Boss_ReturnToPlaceState boss_ReturnToPlace = new Boss_ReturnToPlaceState();
			boss_ReturnToPlace.AddTransition(Transition.tBoss_ReturnIdle, StateID.Boss_Idle);
		
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
		stateID = StateID.Boss_Idle;
    }
    public override void Reason(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
		if(data.CheckRound()){
			npc.GetComponent<EndBoss_FSM>().SetTransition(Transition.tBoss_ToDeath);
			data.Die();
			Debug.Log("BossTransition : IDLE->DEATH");
			return;
		}
		if(data.PlayerInCombatZone){
			npc.GetComponent<EndBoss_FSM>().SetTransition(Transition.tBoss_PlayerAimed);
			Debug.Log("BossTransition : IDLE->ATTACK");
		}
    }
     public override void Act(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
 		data.PlayAnim("bossIdle");
    }	 
}
#endregion

#region AttackPlayer State
public class Boss_AttackPlayerState : FSMState
{
	
		//Counter Variables
	private float _ShooterCounter;
	
    public Boss_AttackPlayerState() 
    { 
		stateID = StateID.Boss_AttackPlayer;
    }
    public override void Reason(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
		if(data.CheckRound()){
			npc.GetComponent<EndBoss_FSM>().SetTransition(Transition.tBoss_ToDeath);
			Debug.Log("BossTransition : ATTACK->DEATH");
			return;
		}
		if(data.CheckRedColRecu()){
			npc.GetComponent<EndBoss_FSM>().SetTransition(Transition.tBoss_GoToBlow);
			Debug.Log("BossTransition : ATTACK->BLOW");
		}
    }
     public override void Act(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
 		_ShooterCounter += Time.deltaTime;
		if(_ShooterCounter >= data.fireRate){
			switch(data.round){
			case 0:
				data.FullLineShoot();
				break;
			case 1:
				data.VShapeShoot();
				break;
			case 2:
				data.RandomShoot(true);
				break;
			}			
			_ShooterCounter = 0;
		}
		
		data.PlayAnim("bossAttack");
    }	 
}
#endregion

#region Blow State
public class Boss_BlowState : FSMState
{
    public Boss_BlowState() 
    { 
		stateID = StateID.Boss_Blow;
    }
    public override void Reason(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
		if(data.CheckRound()){
			npc.GetComponent<EndBoss_FSM>().SetTransition(Transition.tBoss_ToDeath);
			Debug.Log("BossTransition : BLOW->DEATH");
			return;
		}
		if(data.CheckCanForward()){
			npc.GetComponent<EndBoss_FSM>().SetTransition(Transition.tBoss_ReadyToForward);
			Debug.Log("BossTransition : BLOW->ToFORWARD");
		}
    }
     public override void Act(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
 		data.PlayAnim("bossHurt");
    }	 
}
#endregion

#region Forward State
public class Boss_ForwardState : FSMState
{
    public Boss_ForwardState() 
    { 
		stateID = StateID.Boss_Forward;
    }
    public override void Reason(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {

    }
     public override void Act(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
		data.Forward();
		data.PlayAnim("bossIdle");
    }	 
}
#endregion

#region ReturnToPlace State
public class Boss_ReturnToPlaceState : FSMState
{
    public Boss_ReturnToPlaceState() 
    { 
		stateID = StateID.Boss_ReturnBossPlace;
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
		stateID = StateID.Boss_Death;
    }
    public override void Reason(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {

    }
     public override void Act(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data)
    {
 		data.PlayAnim("bossDie");
    }	 
}
#endregion
