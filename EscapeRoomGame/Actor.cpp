#include "Actor.h"
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
Actor::Actor(Game* game)
	:mGame(game)
	,mState(EActive)
	,mPosition(Vector3::Zero)
	,mScale(1.0f)
	,mRotation(0.0f)
    ,mMesh(nullptr)
    ,mMove(nullptr)
    ,mColli(nullptr)
    ,mCamera(nullptr)
{
	// TODO
    mGame->AddActor(this);
    
}

Actor::~Actor()
{
	// TODO
    mGame->RemoveActor(this);
    
    if (mMove!=nullptr) {
        delete mMove;
    }
    if (mColli!=nullptr) {
        delete mColli;

    }
    if (mMesh!=nullptr) {
        delete mMesh;
        
    }
    if (mCamera!=nullptr) {
        delete mCamera;
        
    }

}

void Actor::Update(float deltaTime)
{
	// TODO
    if (mState==EActive)
    {
        if (mMove!=nullptr)
        {
            mMove->Update(deltaTime);
        }
        //update components
        if (mMesh!=nullptr)
        {
            mMesh->Update(deltaTime);
        }
        if (mCamera!=nullptr)
        {
            mCamera->Update(deltaTime);
        }
        
        UpdateActor(deltaTime);
        
    }
    Matrix4 scale = Matrix4::CreateScale(mScale);
    Matrix4 rotation = Matrix4::CreateRotationZ(mRotation);
    Matrix4 position = Matrix4::CreateTranslation(mPosition);
    mWorldTransform = scale*rotation*Matrix4::CreateFromQuaternion(mQuat)*position;
    
}

void Actor::UpdateActor(float deltaTime)
{
    
}

void Actor::ProcessInput(const Uint8* keyState)
{
	// TODO
    
    if (mState==EActive)
    {
        //call processinput on all components
        const Uint8 *keyState = SDL_GetKeyboardState(NULL);
        ActorInput(keyState);
        //process input on sprite
        //std::cout<<"getcalled....IIII/n";

        if (mMove!=nullptr)
        {
            mMove->ProcessInput(keyState);
            //std::cout<<"getcalled....IIII/n";

        }
       
        if (mColli!=nullptr)
        {
            mColli->ProcessInput(keyState);
        }
        
        if (mMesh!=nullptr)
        {
            mMesh->ProcessInput(keyState);
        }
        if (mCamera!=nullptr)
        {
            mCamera->ProcessInput(keyState);
        }
    }
}

void Actor::ActorInput(const Uint8* keyState)
{
    
}

//for sprite commponents



void Actor::SetMove( MoveComponent *moveComp)
{
    mMove=moveComp;
}

Vector3 Actor::GetForward()
{
    return Vector3(Math::Cos(mRotation), Math::Sin(mRotation), 0.0f);
    
}

Vector3 Actor::GetRight()
{
    return Vector3(Math::Cos(mRotation+Math::PiOver2), Math::Sin(mRotation+Math::PiOver2), 0.0f);
    
}
