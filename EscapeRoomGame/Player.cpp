#include "Player.h"
#include "Game.h"
#include "Component.h"
#include <algorithm>
#include <unordered_map>
#include "MoveComponent.h"
#include <iostream>
#include "CollisionComponent.h"
#include "Math.h"
#include "MeshComponent.h"
#include "CameraComponent.h"
#include "CameraComponent.h"
#include "PlayerMove.h"

Player::Player(Game* game)
:Actor(game)
{
    mMove=new PlayerMove(this);
    mColli=new CollisionComponent(this);
    mColli->SetSize(50, 175, 50);
    mCamera=new CameraComponent(this);
   
}

