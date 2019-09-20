#include "Arrow.h"
#include "Game.h"
#include "Component.h"
#include <algorithm>
#include <unordered_map>
#include "MoveComponent.h"
#include <iostream>
#include "CollisionComponent.h"
#include "Math.h"
#include "MeshComponent.h"
#include "Actor.h"
#include "Renderer.h"
#include "CollisionComponent.h"
#include "Player.h"
#include "Checkpoint.h"

Arrow::Arrow(Game* game)
: Actor(game)
{
    mMesh= new MeshComponent(this);
    mMesh->SetMesh(game->GetRenderer()->GetMesh("Assets/Arrow.gpmesh"));
    
    mScale =0.15f;
}

void Arrow::UpdateActor(float deltaTime)
{
    if (mGame->mCheckpoints.front()==nullptr||mGame->mCheckpoints.empty())
    {
        mQuat= Quaternion::Identity;
    }
    else
    {
        Vector3 ArrowToCP= mGame->mCheckpoints.front()->GetPosition()- mPosition;
        ArrowToCP= Vector3::Normalize(ArrowToCP);
        Vector3 front= Vector3(1,0,0);
        float angle = Math::Acos(front.x*ArrowToCP.x+front.y*ArrowToCP.y+front.z*ArrowToCP.z);
        
        Vector3 axis= Vector3 (front.y*ArrowToCP.z-front.z*ArrowToCP.y, front.z*ArrowToCP.x-front.x*ArrowToCP.z,front.x*ArrowToCP.y-front.y*ArrowToCP.x);
        
        
        Quaternion arrowQuat;
        float length =axis.Length();
        if (Math::NearZero(length))
        {
            arrowQuat= Quaternion::Identity;
        }
        else
        {
            axis = Vector3::Normalize (axis);
            arrowQuat = Quaternion(axis, angle);
        }
        
        mQuat=arrowQuat;
        
        mPosition= mGame->getRender()->Unproject(Vector3(0.0f, 250.0f, 0.1f));
        
    }
    
}

