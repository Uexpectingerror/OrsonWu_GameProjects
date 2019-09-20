#include "Coin.h"
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
#include "Player.h"
#include "HUD.h"

Coin::Coin(Game* game)
: Actor(game)
{
    mMesh= new MeshComponent(this);
    mMesh->SetMesh(game->GetRenderer()->GetMesh("Assets/Coin.gpmesh"));
    mScale=64.0f;
    
    mColli= new CollisionComponent(this);
    mColli->SetSize(100, 100, 100);
    
    
}

void Coin::UpdateActor(float deltaTime)
{
    SetRotation(GetRotation()+Math::Pi*deltaTime);
    if (mColli->Intersect(mGame->mPlayer->getColli()))
    {
        Mix_PlayChannel(-1, mGame->GetSound("Assets/Sounds/Coin.wav"), 0);
        mGame->mHUD->UpdateCoinText();

        SetState(EDead);
    }
}
