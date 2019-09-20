#pragma once
#include "Component.h"


class CameraComponent : public Component
{
public:
    CameraComponent(class Actor* owner);
    
    void Update (float deltaTime) override;
    void SetPitchSpeed (float speed) { mPitchSpeed=speed; }
    float GetPitchSpeed (){ return mPitchSpeed; }
    

protected:
    float mPitchAngle;
    float mPitchSpeed;
    
   
};
