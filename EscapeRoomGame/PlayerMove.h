#pragma once
#include "MoveComponent.h"
#include "Math.h"


class PlayerMove : public MoveComponent
{
public:
    enum MoveState
    {
        OnGround,
        Jump,
        Falling,
        WallClimb,
        WallRun,
    };
    enum CollSide
    {
        None,
        Top,
        Bottom,
        
        SideX1,
        SideX2,
        SideY1,
        SideY2,
    };
    
    PlayerMove (class Actor* owner);
    ~PlayerMove();
    void Update(float deltaTime) override;
    void ProcessInput(const Uint8 *keyState) override;
    void ChangeState( MoveState state){mCurrentState=state; }
    void PhysicsUpdate (float deltaTime );
    void AddForce (const Vector3& force) { mPendingForces+=force; }
    void FixXYZVelocity();
    bool CanWallClimb(CollSide side);
    bool CanWallRun(CollSide side);
protected:
    void UpdateOnGround(float deltaTime);
    void UpdateJump(float deltaTime);
    void UpdateFalling(float deltaTime);
    void UpdateWallClimb(float deltaTime);
    void UpdateWallRun (float deltaTime);

    CollSide FixCollision(class CollisionComponent* self, class CollisionComponent* block);
private:
    MoveState mCurrentState;
//    const float gravity=-980.0f;
//    const float JumpSpeed=500.0f;
//    float mZSpeed;
    Vector3 mVelocity;
    Vector3 mAcceleration;
    Vector3 mPendingForces;
    float mMass = 1.0f;
    Vector3 mJumpforce = Vector3 (0.0f, 0.0f, 35000.0f);
    Vector3 mClimbforce = Vector3 (0.0f, 0.0f, 1800.0f);
    Vector3 mWallRunforce = Vector3 (0.0f, 0.0f, 1200.0f);

    Vector3 mGravity = Vector3 (0.0f, 0.0f, -980.0f);
    bool isSpacePressed;
    float mWallClimbTimer;
    float mWallRunTimer;
    int mRunningSFX;
    
};
