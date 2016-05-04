using System;
using UnityEngine;

namespace UnityPlatformer {
  /// <summary>
  /// Movement while on air, despite beeing falling, jumping...
  /// </summary>
  public class CharacterActionAirMovement: CharacterAction, IUpdateManagerAttach {
    #region public

    [Comment("Movement speed")]
    public float speed = 6;
    [Comment("Time to reach max speed")]
    public float accelerationTime = .2f;

    #endregion

    float velocityXSmoothing;

    public void Attach(UpdateManager um) {
    }

    /// <summary>
    /// Execute when no collision below.
    /// </summary>
    public override int WantsToUpdate(float delta) {
      // NOTE if Air/Ground are very different maybe:
      // if (pc2d.IsOnGround(<frames>)) it's better
      if (pc2d.collisions.below || character.IsOnState(States.Grabbing)) {
        velocityXSmoothing = 0;
        return 0;
      }
      return -1;
    }

    /// <summary>
    /// Horizontal movement
    /// </summary>
    public override void PerformAction(float delta) {
      Vector2 in2d = input.GetAxisRaw();

      float targetVelocityX = in2d.x * speed;

      character.velocity.x = Mathf.SmoothDamp (
        character.velocity.x,
        targetVelocityX,
        ref velocityXSmoothing,
        accelerationTime
      );
    }

    public override PostUpdateActions GetPostUpdateActions() {
      return PostUpdateActions.WORLD_COLLISIONS | PostUpdateActions.APPLY_GRAVITY;
    }
  }
}
