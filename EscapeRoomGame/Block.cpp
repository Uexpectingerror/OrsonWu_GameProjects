#include "Block.h"
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

Block::Block(Game* game)
: Actor(game)
{
    mMesh= new MeshComponent(this);
    mMesh->SetMesh(game->GetRenderer()->GetMesh("Assets/Cube.gpmesh"));
    mScale=64.0f;
    
    mColli= new CollisionComponent(this);
    mColli->SetSize(1, 1, 1);
    
    mGame->AddBlock(this);
    
}

Block::~Block()
{
    mGame->RemoveBlock(this);
}
