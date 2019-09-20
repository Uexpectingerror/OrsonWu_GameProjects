#include "MoveComponent.h"
#include "Actor.h"
#include "SDL/SDL.h"
#include <iostream>


MoveComponent::MoveComponent( Actor* owner)
:Component(owner)
,mAngularSpeed(0.0f)
,mForwardSpeed(0.0f)
,mStrafeSpeed(0.0f)
{
	
}

void MoveComponent::Update(float deltaTime)
{
	// TODO: Implement in Part 3
    
    //update rotation
    mOwner->SetRotation((mOwner->GetRotation())+mAngularSpeed*deltaTime);
    
    //update position
    mOwner->SetPosition((mOwner->GetPosition()+mOwner ->GetForward()*mForwardSpeed*deltaTime));
    
    mOwner->SetPosition((mOwner->GetPosition()+ mOwner->GetRight()*mStrafeSpeed*deltaTime));
}

void MoveComponent:: ProcessInput(const Uint8 *keyState)
{
    
}


