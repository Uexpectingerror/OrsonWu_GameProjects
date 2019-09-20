#include "PlayerMove.h"
#include "Actor.h"
#include "MoveComponent.h"
#include <SDL/SDL.h>
#include "CameraComponent.h"
#include "CollisionComponent.h"
#include "Game.h"
#include "Block.h"
#include <iostream>
#include "Player.h"

PlayerMove::PlayerMove(Actor* owner)
:MoveComponent(owner)
,mCurrentState(Falling)
,isSpacePressed(false)
{
    mRunningSFX= Mix_PlayChannel(-1, mOwner->GetGame()-> GetSound("Assets/Sounds/Running.wav"), -1);
    Mix_Pause(mRunningSFX);
}

void PlayerMove::FixXYZVelocity()
{
    Vector2 temXY = Vector2(mVelocity.x,mVelocity.y);
    if (temXY.Length()>=400.0f)
    {
        temXY= Vector2::Normalize(temXY)*400.0f;
    }
    if (mCurrentState==OnGround||mCurrentState==WallClimb)
    {
        if (Math::NearZero(mAcceleration.x))
        {
            temXY.x*=0.9f;
        }
        if (Math::NearZero(mAcceleration.y))
        {
            temXY.y*=0.9f;
        }
    }
    
    if (mAcceleration.x*temXY.x<0)
    {
        temXY.x*=0.9f;
    }
    if (mAcceleration.y*temXY.y<0)
    {
        temXY.y*=0.9f;
    }
    
    mVelocity.x= temXY.x;
    mVelocity.y=temXY.y;
}
 PlayerMove::~PlayerMove()
{
    Mix_HaltChannel(mRunningSFX);
}

void PlayerMove::PhysicsUpdate(float deltaTime)
{
    mAcceleration = mPendingForces* (1.0f/mMass);
    mVelocity+= mAcceleration*deltaTime;
    
    FixXYZVelocity();
    //update position
    mOwner->SetPosition(mOwner->GetPosition()+mVelocity*deltaTime );
    //set rotation
    mOwner->SetRotation((mOwner->GetRotation())+mAngularSpeed*deltaTime);
    //reset pending forces
    mPendingForces= Vector3::Zero;

}

void PlayerMove::UpdateJump(float deltaTime)
{
   
    AddForce(mGravity);
    PhysicsUpdate(deltaTime);
    
    for (auto i: mOwner->GetGame()->getMyBlocks())
    {
        CollSide touchSide = FixCollision(mOwner->getColli(), i->getColli());
        if (touchSide==Bottom)
        {
            mVelocity.z=0;
        }
        
        if (touchSide==SideX1)
        {
            if(CanWallClimb(touchSide))
            {
                ChangeState(WallClimb);
                mWallClimbTimer=0.0f;
                break;
            }
            else if (CanWallRun(touchSide))
            {
                ChangeState(WallRun);
                mWallRunTimer=0.0f;
                break;
            }
        }
        else if (touchSide==SideX2)
        {
            if(CanWallClimb(touchSide))
            {
                ChangeState(WallClimb);
                mWallClimbTimer=0.0f;
                
                break;
            }
            else if (CanWallRun(touchSide))
            {
                ChangeState(WallRun);
                mWallRunTimer=0.0f;
                break;
            }
        }
        else if (touchSide==SideY1)
        {
            if(CanWallClimb(touchSide))
            {
                ChangeState(WallClimb);
                mWallClimbTimer=0.0f;
                
                break;
            }
            else if (CanWallRun(touchSide))
            {
                ChangeState(WallRun);
                mWallRunTimer=0.0f;
                break;
            }
        }
        else if (touchSide==SideY2)
        {
            if(CanWallClimb(touchSide))
            {
                ChangeState(WallClimb);
                mWallClimbTimer=0.0f;
                
                break;
            }
            else if (CanWallRun(touchSide))
            {
                ChangeState(WallRun);
                mWallRunTimer=0.0f;
                break;
            }
        }
    }
    
    if (mVelocity.z<=0)
    {
        ChangeState(Falling);
    }
    
}
void PlayerMove::UpdateOnGround(float deltaTime)
{
    PhysicsUpdate(deltaTime);
    bool isStillOnGround = false;
    for (auto i: mOwner->GetGame()->getMyBlocks())
    {
        CollSide touchSide = FixCollision(mOwner->getColli(), i->getColli());
        if (touchSide==Top)
        {
            isStillOnGround=true;
        }
        
        if (touchSide==SideX1)
        {
            if(CanWallClimb(touchSide))
            {
                ChangeState(WallClimb);
                mWallClimbTimer=0.0f;
                break;
            }
        }
        else if (touchSide==SideX2)
        {
            if(CanWallClimb(touchSide))
            {
                ChangeState(WallClimb);
                mWallClimbTimer=0.0f;

                break;
            }
        }
        else if (touchSide==SideY1)
        {
            if(CanWallClimb(touchSide))
            {
                ChangeState(WallClimb);
                mWallClimbTimer=0.0f;

                break;
            }
        }
        else if (touchSide==SideY2)
        {
            if(CanWallClimb(touchSide))
            {
                ChangeState(WallClimb);
                mWallClimbTimer=0.0f;

                break;
            }
        }
    }
    if (!isStillOnGround)
    {
        ChangeState(Falling);
    }
}
void PlayerMove::UpdateFalling(float deltaTime)
{
    AddForce(mGravity);
    PhysicsUpdate(deltaTime);
    
    for (auto i: mOwner->GetGame()->getMyBlocks())
    {
        CollSide touchSide = FixCollision(mOwner->getColli(), i->getColli());
        if (touchSide==Top)
        {
            mVelocity.z=0;
            ChangeState(OnGround);
            Mix_PlayChannel(-1, mOwner->GetGame()->GetSound("Assets/Sounds/Land.wav"), 0);

        }
    }
}

void PlayerMove::UpdateWallClimb(float deltaTime)
{
    bool isCollide=false;
    mWallClimbTimer+=deltaTime;
    if (mWallClimbTimer<=0.4f)
    {
        AddForce(mClimbforce);
    }
    AddForce(mGravity);
    
    PhysicsUpdate(deltaTime);
    
    for (auto i: mOwner->GetGame()->getMyBlocks())
    {
        CollSide touchSide = FixCollision(mOwner->getColli(), i->getColli());
        if (touchSide!=None)
        {
            isCollide=true;
        }
    }
    if (isCollide==false||mVelocity.z<=0)
    {
        mVelocity.z=0.0f;
        ChangeState(Falling);
    }
}

void PlayerMove::UpdateWallRun(float deltaTime)
{
    mWallRunTimer+=deltaTime;
    if (mWallRunTimer<=0.4f)
    {
        AddForce(mWallRunforce);
    }
    AddForce(mGravity);
    
    PhysicsUpdate(deltaTime);
    
    //fix collison
    for (auto i: mOwner->GetGame()->getMyBlocks())
    {
        FixCollision(mOwner->getColli(), i->getColli());
    }
    
    if (mVelocity.z<=0)
    {
        mVelocity.z=0.0f;
        ChangeState(Falling);
    }
    
}

bool PlayerMove::CanWallRun(CollSide side)
{
    Vector3 sideNormal;
    if (side == SideX1)
    {
        sideNormal= Vector3(-1,0,0);
        // std::cout<<"SideX1"<<std::endl;
        
    }
    else if (side == SideY1)
    {
        // std::cout<<"SideY1"<<std::endl;
        
        sideNormal= Vector3(0,-1,0);
    }
    else if (side == SideX2)
    {
        //  std::cout<<"SideX2"<<std::endl;
        
        sideNormal= Vector3(1,0,0);
    }
    else if (side == SideY2)
    {
        // std::cout<<"SideY2"<<std::endl;
        
        sideNormal= Vector3(0,1,0);
    }
    
    Vector3 pForward = mOwner->GetForward();
    float faceAngle = Math::Acos(sideNormal.x*pForward.x+sideNormal.y*pForward.y+sideNormal.z*pForward.z);
    if (faceAngle>0.3f*Math::Pi && faceAngle<0.7f*Math::Pi)
    {
        //remember when calculate the normalized moving normal, donot include Z values which will mess up the detection when you jump on walls
        Vector2 normalizedV = Vector2::Normalize(Vector2( mVelocity.x, mVelocity.y));
        float moveAngle = Math::Acos(pForward.x*normalizedV.x+pForward.y*normalizedV.y);
        
        
        if (mVelocity.Length()>=350.0f && moveAngle>= 0*Math::Pi && moveAngle<0.2f*Math::Pi)
        {
            std::cout<<"wall climb true"<<std::endl;
            
            return true;
        }
    }
    return false;
}

bool PlayerMove::CanWallClimb(CollSide side)
{
    Vector3 sideNormal;
    if (side == SideX1)
    {
        sideNormal= Vector3(-1,0,0);
       // std::cout<<"SideX1"<<std::endl;

    }
    else if (side == SideY1)
    {
       // std::cout<<"SideY1"<<std::endl;

        sideNormal= Vector3(0,-1,0);
    }
    else if (side == SideX2)
    {
      //  std::cout<<"SideX2"<<std::endl;

        sideNormal= Vector3(1,0,0);
    }
    else if (side == SideY2)
    {
       // std::cout<<"SideY2"<<std::endl;

        sideNormal= Vector3(0,1,0);
    }
    
    Vector3 pForward = mOwner->GetForward();
    float faceAngle = Math::Acos(sideNormal.x*pForward.x+sideNormal.y*pForward.y+sideNormal.z*pForward.z);
    //std::cout<<"faceangle"<<faceAngle<<std::endl;

    if (faceAngle>0.9f*Math::Pi && faceAngle<1.1f*Math::Pi)
    {
        //remember when calculate the normalized moving normal, donot include Z values which will mess up the detection when you jump on walls 
        Vector2 normalizedV = Vector2::Normalize(Vector2( mVelocity.x, mVelocity.y));
        float moveAngle = Math::Acos(sideNormal.x*normalizedV.x+sideNormal.y*normalizedV.y);
        

        if (mVelocity.Length()>=350.0f && moveAngle> 0.9f*Math::Pi && moveAngle<1.1f*Math::Pi)
        {
            std::cout<<"wall climb true"<<std::endl;

            return true;
        }
    }
    
    return false;
}
// main update
void PlayerMove::Update(float deltaTime)
{
    if (mCurrentState==OnGround)
    {
        UpdateOnGround(deltaTime);
        
    }
    else if (mCurrentState==Falling)
    {
        UpdateFalling(deltaTime);
    }
    else if (mCurrentState==Jump)
    {
        UpdateJump(deltaTime);
    }
    else if (mCurrentState==WallClimb)
    {
        UpdateWallClimb(deltaTime);
    }
    else if (mCurrentState==WallRun)
    {
        UpdateWallRun(deltaTime);
    }
    if ((mCurrentState==OnGround&&mVelocity.Length()>=50.0f) || mCurrentState==WallClimb || mCurrentState==WallRun ) {
        Mix_Resume(mRunningSFX);
    }
    else
    {
        Mix_Pause(mRunningSFX);
    }
    if (mOwner->GetPosition().z<-750.0f)
    {
        Player* myP = (Player*) mOwner;
        mOwner->SetPosition(myP->getRespawnPos());
        mOwner->SetRotation(0.0f);
        mVelocity=Vector3::Zero;
        mPendingForces=Vector3::Zero;
        ChangeState(Falling);
    }
}

void PlayerMove::ProcessInput(const Uint8 *keyState)
{
    if (keyState[SDL_SCANCODE_W]&&!keyState[SDL_SCANCODE_S])
    {
        AddForce(mOwner->GetForward()*700.0f);
    }
    else if (keyState[SDL_SCANCODE_S]&&!keyState[SDL_SCANCODE_W])
    {
        AddForce(mOwner->GetForward()*-700.0f);
    }
    else
    {
        mForwardSpeed=0;

    }
    
    if (keyState[SDL_SCANCODE_D]&&!keyState[SDL_SCANCODE_A])
    {
        AddForce(mOwner->GetRight()*700.0f);
    }
    else if (keyState[SDL_SCANCODE_A]&&!keyState[SDL_SCANCODE_D])
    {
        AddForce(mOwner->GetRight()*-700.0f);
    }
    else
    {
        mStrafeSpeed=0;
        
    }
    
    if (keyState[SDL_SCANCODE_SPACE]&&!isSpacePressed&&mCurrentState==OnGround)
    {
        AddForce(mJumpforce);
        ChangeState(Jump);
        Mix_PlayChannel(-1, mOwner->GetGame()->GetSound("Assets/Sounds/Jump.wav"), 0);

    }
    
     if (keyState[SDL_SCANCODE_SPACE])
     {
         isSpacePressed=true;
     }
    else
    {
        isSpacePressed=false;
    }
    //for mouse movements
    int x, y;
    SDL_GetRelativeMouseState(&x, &y);
    
    float Xspeed = float (x)/500.0f*Math::Pi*10.0f;
    mAngularSpeed=Xspeed;
    
    float Yspeed = float (y)/500.0f*Math::Pi*10.0f;
    mOwner->GetCamera()->SetPitchSpeed(Yspeed);
    
    
}

PlayerMove::CollSide PlayerMove::FixCollision( CollisionComponent *self, CollisionComponent *block)
{
    if (block->Intersect(self))
    {
        //stop falling
        Vector3 playerMax = self->GetMax();
        Vector3 playerMin = self->GetMin();
        Vector3 blockMax = block->GetMax();
        Vector3 blockMin = block->GetMin();
        
        float dy1 = Math::Abs(playerMax.y-blockMin.y);
        float dx1 = Math::Abs(playerMax.x-blockMin.x);
        float dy2 = Math::Abs(blockMax.y-playerMin.y);
        float dx2 = Math::Abs(blockMax.x-playerMin.x);
        float dz1= Math::Abs(playerMax.z-blockMin.z);
        float dz2= Math::Abs(blockMax.z-playerMin.z);

        
        //which side it collides
        if (dy1<=dy2&&dy1<=dx1&&dy1<=dx2&&dy1<dz1&&dy1<dz2)
        {
            
            //if touch top
            mOwner->SetPosition((mOwner->GetPosition()+
                                 Vector3(0, -Math::Abs(playerMax.y-blockMin.y),0)));
            AddForce(Vector3(0,-700.0f,0));
            return SideY1;
            
        }
        else if (dy2<=dy1&&dy2<=dx1&& dy2<=dx2&&dy2<dz1&&dy2<dz2)
        {
            //if touch bot
            mOwner->SetPosition((mOwner->GetPosition()+
                                 Vector3(0, Math::Abs(blockMax.y-playerMin.y),0)));
            AddForce(Vector3(0,700.0f,0));

            return SideY2;
        }
        
        else  if (dx2<=dy1&&dx2<=dy2&& dx2<=dx1&&dx2<dz1&&dx2<dz2)
        {
            //if touch right
            //std::cout<<"touch right\n";
            mOwner->SetPosition((mOwner->GetPosition()+
                                 Vector3(Math::Abs(blockMax.x-playerMin.x),0,0)));
            AddForce(Vector3(700.0f,0,0));

            //SetForwardSpeed(0);
            return SideX2;
        }
        else if (dx1<=dy1&&dx1<=dy2&& dx1<=dx1 &&dx1<dz1&&dx1<dz2)
        {
            //if touch left
            //std::cout<<"touch left\n";
            
            mOwner->SetPosition((mOwner->GetPosition()+
                                 Vector3(-Math::Abs(playerMax.x-blockMin.x),0,0)));
            AddForce(Vector3(-700.0f,0,0));

            return SideX1;
        }
        else if (dz1<=dy1&& dz1<=dy2&& dz1<=dx1 &&dz1<dx2 &&dz1<dz2)
        {
            //if touch left
            //std::cout<<"touch left\n";
            
            mOwner->SetPosition((mOwner->GetPosition()+
                                 Vector3(0,0,-Math::Abs(playerMax.z-blockMin.z))));
            return Bottom;
        }
        else if (dz2<=dy1&& dz2<=dy2&& dz2<=dx1 &&dz2<dx2 &&dz2<dz1)
        {
            //if touch left
            //std::cout<<"touch left\n";
            
            mOwner->SetPosition((mOwner->GetPosition()+
                                 Vector3(0,0,Math::Abs(blockMax.z-playerMin.z))));
            return Top;
        }
        else
            
        {
            return None;
        }
        
    }
    else
        
    {
        return None;
    }
    
}


