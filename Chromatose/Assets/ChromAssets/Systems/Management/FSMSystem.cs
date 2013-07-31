using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
/*****************************************************************************************************************************************
Comment l'Utiliser:
	1. Placez les Labels des Transitions et des State de la StateMachine dans l'enum correspondant
	
	2. Ecrire les nouvelles classes en inheritant de FSMState et associes chacun a une paires (Transition-State)
		Ces paires representent le State S2 etant accessible lorsque l'on se trouve dans le State S1, une 
		transition T est lancer et le State S1 passe au State S2. 
		NE PAS OUBLIER : C'est une "Deterministic" StateMachine, on ne peut pas avoir une meme transition menant a 2 State different.
		
	   La Methode Reason est utiliser pour determiner quelle transition doit etre lancer. Vous pouvez ecrire la transition a d'autre
	   endroit selon les besoins. 
	   
		   Ex; Pour le piege de feu ; 
		   Executer la transition depuis void OnTriggerEnter(){ fsm.PerformTransition(Transition.AicardGeler); }
	   
	   La Methode Act est la ou le code des action du NPC est supposer s'executer si il est dans ce State. Vous pouvez ecrire la transition a d'autre
	   endroit selon les besoins. 
	   
		   Ex; Pour le Mode Puzzle de l'avatar
		   Executer la transition depuis void OnTriggerStay(){ fsm.PerformTransition(Transition.ModePuzzle); }
		   ou
		   Ex2; Pour un certains temps ou distance
		   Executer la transition depuis void Update() { if (npc.transform.position.x > 10) { fsm.PerformTransition(Transition.FaitKekChose) }
	   
	   On peut aussi l'appeller de la meme facon a partir d'une coroutine ou d'a peu pres toute les autres facons obscures que vous pourriez trouvez.
	   Dans ces derniers cas, laissez simplement les methodes Reason & Act vides (elle doivent cependant imperativement etre presente dans la classe du State).
	   
	3. Creer une instance de la Class FSMSystem et ajouter les States (voir exemple du Template)
	
	4. Appeller les methodes Reason & Act (elle s'y trouvent deja) ainsi que toute les methodes dont vous aurez besoin pour faire la transition
		et rendre les NPC operationnels dans le jeu depuis FixedUpdate(){}.
	    
*******************************************************************************************************************************************/
 
 
/// <summary>
/// Placez les labels pour les Transition dans cet Enum, 
/// dans la section assigner pour vos ennemis. 
/// Laisser NullTransition = 0 en place, FSMSystem l'utilise
/// Utiliser des noms specifique comme; BouleDePics_SawPlayer afin d'eviter les melanges entre AI
/// </summary>
public enum Transition
{
    NullTransition = 0, // La StateMachine utilise cette transition pour representer une transition non-existante dans votre systeme.
	
	tBoss_PlayerAimed = 1,
	//tBoss_GoToWaiting = 2,  <-- TO BE DELETED
	tBoss_GoToBlow = 3,
	tBoss_ReadyToForward = 4,
	tBoss_GoToReturn = 5,
	tBoss_ToDeath = 6,
	tBoss_ReturnIdle = 7,
	
	tShavatar_PathFinded = 11,
	
	//Pour Exemple; 301 a 302
	Exemple_SawPlayer = 301,
	Exemple_LostPlayer = 302,
	
}
 
/// <summary>
/// Placez les labels pour les States dans cet Enum, 
/// dans la section assigner pour vos ennemis. 
/// Laisser NullTransition = 0 en place, FSMSystem l'utilise
/// Utiliser des noms specifique comme; BouleDePics_FollowingPath afin d'eviter les melanges entre AI
/// Separez chaque States par une virgule
/// </summary>
public enum StateID
{
    NullStateID = 0, // La StateMachine utilise ce State pour representer un State non-existante dans votre systeme.
	
	Boss_Idle = 1,
	Boss_AttackPlayer = 2,
	//Boss_WaitToAttack = 3,  <-- TO BE DELETED
	Boss_Blow = 4,
	Boss_Forward = 5,
	Boss_ReturnBossPlace = 6,
	Boss_Death = 7,
	
	Shavatar_Idle = 11,
	Shavatar_FollowPath = 12,
	
	
	//Pour Exemple; 301 a 302
	Exemple_FollowingPath = 301,
	Exemple_ChasingPlayer = 302,
}
 
/// <summary>
/// Cette Classe est la representante des States dans la StateMachine.
/// Chaque States est une reference avec une paire (Transition-State) qui dit
/// quel state doit etre appeller si une transition est appeller depuis le State courant
/// 
/// La Methode Reason est utiliser pour determiner quel transition doit etre effectuer
/// La Methode Act est la ou le code des action courante de ce State vont
/// </summary>
public abstract class FSMState
{
    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
    protected StateID stateID;
    public StateID ID { get { return stateID; } }
 
    public void AddTransition(Transition trans, StateID id)
    {
        // Verifie si les arguments sont invalides
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed for a real transition");
            return;
        }
 
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSMState ERROR: NullStateID is not allowed for a real ID");
            return;
        }
 
        //Verifie si la transition est deja dans le mapping
        if (map.ContainsKey(trans))
        {
            Debug.LogError("FSMState ERROR: State " + stateID.ToString() + " already has transition " + trans.ToString() + 
                           "Impossible to assign to another state");
            return;
        }
 
        map.Add(trans, id);
    }
 
    /// <summary>
    /// Cette method efface une pair Transition-State du mapping
    /// Affiche l'erreur si la Transition ne se trouve pas dans le mapping
    /// </summary>
    public void DeleteTransition(Transition trans)
    {
        // Verifie NullTransition
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed");
            return;
        }
 
        // Verifie si il se trouve dans le mapping avant de l'effacer
        if (map.ContainsKey(trans))
        {
            map.Remove(trans);
            return;
        }
        Debug.LogError("FSMState ERROR: Transition " + trans.ToString() + " passed to " + stateID.ToString() + 
                       " was not on the state's transition list");
    }
 
    /// <summary>
    /// Cette methode retourne le nouveau State que la StateMachine doit passer
    /// si ce State recoit une transition
    /// </summary>
    public StateID GetOutputState(Transition trans)
    {
        // Verifie si le mapping a une transition
        if (map.ContainsKey(trans))
        {
            return map[trans];
        }
        return StateID.NullStateID;
    }
 
    /// <summary>
    /// Cette methode set les parametres du State avant d'y entrer
    /// Il est appeller en premier lors du passage a un nouveau State
    /// </summary>
    public virtual void DoBeforeEntering() { }
 
    /// <summary>
    /// Cette methode re-set les parametres avant de quitter le State present
    /// Il est appeller en dernier avant de passer a un nouveau State
    /// </summary>
    public virtual void DoBeforeLeaving() { } 
 
    /// <summary>
    /// Cette methode decide si le State doit passer a un nouveau State sur la liste
    /// des States possibles. On peut y stocker de nombreux liens vers d'autres States
    /// Voyez cette methode comme un losange dans un FlowChart. WinkWink 
    /// </summary>
    public abstract void Reason(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data);
 
    /// <summary>
    /// Cette methode controle le NPC les action et evenements du NPC dans le Jeu
    /// Chaque action, mouvement, communication, etc... devrait se retrouver ici
    /// npc est la reference de la l'objet controller par la Class
    /// </summary>
    public abstract void Act(GameObject player, GameObject npc, ChromatoseManager chromanager, EndBoss_DataBase data);
 
} // class FSMState
 
 
/// <summary>
/// La Class FSMSystem represente la Class de la StateMachine instancier
/// Elle a une liste avec les States du NPC ainsi que les methodes necessaire au passage
/// de State en State, ou encore ceux pour ajouter-effacer des States dans le mapping
/// </summary>
public class FSMSystem
{
    private List<FSMState> states;
 
	//La SEULE facon de changer de State est en effectuant une transition
	//Ne changez pas le CurrentState directement
    private StateID currentStateID;
    public StateID CurrentStateID { get { return currentStateID; } }
    private FSMState currentState;
    public FSMState CurrentState { get { return currentState; } }
 
    public FSMSystem()
    {
        states = new List<FSMState>();
    }
 
    /// <summary>
    /// Cette methode place les nouveaux State dans la StateMachine
    /// ou affiche un message d'erreur si le States est deja dans la liste
    /// Le premier State ajouter sera le State initiale
    /// </summary>
    public void AddState(FSMState s)
    {
        // Verifie si la reference est null avant d'ajouter le State
        if (s == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
        }
 
        // Le premier State ajouter sera le State initiale,
        //   Le State que la StateMachine effectura lors de son demarrage
        if (states.Count == 0)
        {
            states.Add(s);
            currentState = s;
            currentStateID = s.ID;
            return;
        }
 
        // Ajoute le State a la liste si il ne s'y trouve pas deja
        foreach (FSMState state in states)
        {
            if (state.ID == s.ID)
            {
                Debug.LogError("FSM ERROR: Impossible to add state " + s.ID.ToString() + 
                               " because state has already been added");
                return;
            }
        }
        states.Add(s);
    }
 
    /// <summary>
    /// Cette methode efface un State de la liste si il y existe
    /// ou affiche un message d'erreur si il ne s'y trouve pas
    /// </summary>
    public void DeleteState(StateID id)
    {
        // Verifie si la reference est null avant d'effacer
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }
 
        // Cherche dans la liste et efface le State si il s'y trouve
        foreach (FSMState state in states)
        {
            if (state.ID == id)
            {
                states.Remove(state);
                return;
            }
        }
        Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() + 
                       ". It was not on the list of states");
    }
 
    /// <summary>
    /// Cette methode effectue le passage de State en State dans la StateMachine
    /// Si le State courant n'a pas de State pour la transition passer, un message
    /// d'erreur s'affiche.
    /// </summary>
    public void PerformTransition(Transition trans)
    {
        // Verifie si la reference est null avant de changer de State
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSM ERROR: NullTransition is not allowed for a real transition");
            return;
        }
 
        // Verifie si le State courant contient une transition de la commande venant des arguments de la methode
        StateID id = currentState.GetOutputState(trans);
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: State " + currentStateID.ToString() +  " does not have a target state " + 
                           " for transition " + trans.ToString());
            return;
        }
 
        // Update le currentStateID et le currentState		
        currentStateID = id;
        foreach (FSMState state in states)
        {
            if (state.ID == currentStateID)
            {
                // Effectue la Methode DoBeforeLeaving avant de quitter le State
                currentState.DoBeforeLeaving();
 
                currentState = state;
 
                // Effectue la methode DoBeforeEntering avant d'entrer dans le State
                currentState.DoBeforeEntering();
                break;
            }
        }
 
    } // PerformTransition()
 
} //class FSMSystem
