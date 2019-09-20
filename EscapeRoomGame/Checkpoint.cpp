#include "Checkpoint.h"
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
#include "HUD.h"

Checkpoint::Checkpoint(Game* game)
: Actor(game)
,isActive(false)
{
    mMesh= new MeshComponent(this);
    mMesh->SetMesh(game->GetRenderer()->GetMesh("Assets/Checkpoint.gpmesh"));
   
    //mScale =10.0f;
    mColli= new CollisionComponent(this);
    mColli->SetSize(25, 25, 25);
}

void Checkpoint::UpdateActor(float deltaTime)
{
    if (this == mGame->mCheckpoints.front())
    {
        isActive =true;
    }
    else
    {
        isActive = false;
    }
    
    
    
    if (isActive)
    {
        mMesh->SetTextureIndex(0);
        if (mColli->Intersect(mGame->mPlayer->getColli()))
        {
            mGame->mHUD->UpdateCPText(mTextString);
            Mix_PlayChannel(-1, mGame->GetSound("Assets/Sounds/Checkpoint.wav"), 0);
            mGame->mPlayer->SetRespawnPos(this->mPosition);
            mGame->mCheckpoints.pop();
            if (!mLevelString.empty())
            {
                mGame->SetNextLevel(mLevelString);
            }
            SetState(EDead);
        }
    }
    else
    {
        mMesh->SetTextureIndex(1);
    }
    
    
}
