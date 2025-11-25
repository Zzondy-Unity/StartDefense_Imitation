using UnityEngine;

public class MonsterMoveState : MonsterState
{
    private float originalSpeed;
    private float curSpeed;
    
    private Vector3[] waypoints;
    private int curWaypointIdx;
    
    private Rigidbody2D rb;
    
    public MonsterMoveState(MonsterController monsterController) : base(monsterController)
    {
        rb = monsterController.rb2D;
    }

    public override void Enter()
    {
        originalSpeed = monsterController.monsterData.moveSpeed;
        curSpeed = originalSpeed;

        var manager = GameManager.Scene.curSceneManager as GameSceneManager;
        waypoints = manager?.Tile.GetMonsterPath();
        
        curWaypointIdx = 0;
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            return;
        }

        if (curWaypointIdx >= waypoints.Length)
        {
            monsterController.ChangeMonsterState<MonsterAttackState>();
            return;
        }

        Vector2 curPos = rb.position;
        Vector2 targetPos = waypoints[curWaypointIdx];

        Vector2 nextPos = Vector2.MoveTowards(curPos, targetPos, Time.fixedDeltaTime * curSpeed);
        rb.MovePosition(nextPos);

        if (Vector2.Distance(nextPos, targetPos) < 0.01f)
        {
            curWaypointIdx++;
        }
    }
}
