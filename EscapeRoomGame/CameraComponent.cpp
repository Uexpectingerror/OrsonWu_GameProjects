#include "CameraComponent.h"
#include "Actor.h"
#include "Game.h"
#include "Renderer.h"
#include "Math.h"

CameraComponent::CameraComponent(Actor* owner)
:Component(owner)
,mPitchSpeed(0)
,mPitchAngle(0)
{
    
}

void CameraComponent::Update(float deltaTime)
{
   
    //get the angles
    mPitchAngle+=mPitchSpeed*deltaTime;
    if (mPitchAngle >= 0.5*Math::PiOver2)
    {
        mPitchAngle = 0.5*Math::PiOver2;
    }
    else if (mPitchAngle <= -0.5*Math::PiOver2)
    {
        mPitchAngle = -0.5*Math::PiOver2;
    }
    Matrix4 yaw= Matrix4::CreateRotationZ(mOwner->GetRotation());
    Matrix4 pitch= Matrix4::CreateRotationY(mPitchAngle);
    Vector3 targetPosition= Vector3::Transform(Vector3(1,0,0), pitch*yaw);
    
    //update the view matrix
    mOwner->GetGame()->getRender()->SetViewMatrix(Matrix4::CreateLookAt(mOwner->GetPosition(),mOwner->GetPosition()+targetPosition*10, Vector3(0,0,1)));
    
    
}
