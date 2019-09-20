#pragma once
#include <vector>
#include <SDL/SDL_stdinc.h>
#include "Math.h"
#include <unordered_map>
#include "CollisionComponent.h"
class Actor
{
public:
	enum State
	{
		EActive,
		EPaused,
		EDead
	};
	
	Actor(class Game* game);
	virtual ~Actor();

	// Update function called from Game (not overridable)
	void Update(float deltaTime);
	// Any actor-specific update code (overridable)
	virtual void UpdateActor(float deltaTime);
	// ProcessInput function called from Game (not overridable)
	void ProcessInput(const Uint8* keyState);
	// Any actor-specific update code (overridable)
	virtual void ActorInput(const Uint8* keyState);

	// Getters/setters
	const Vector3& GetPosition() const { return mPosition; }
	void SetPosition(const Vector3& pos) { mPosition = pos; }
    
    
	float GetScale() const { return mScale; }
	void SetScale(float scale) { mScale = scale; }
	float GetRotation() const { return mRotation; }
	void SetRotation(float rotation) { mRotation = rotation; }
	
	State GetState() const { return mState; }
	void SetState(State state) { mState = state; }

	class Game* GetGame() { return mGame; }
    
    //for spritecomponents

    void SetMove( class MoveComponent* moveComp);
    MoveComponent* getMov() {return mMove;}
    //for movements
    Vector3 GetForward ();
    //for collision
    class CollisionComponent* getColli(){return mColli;}
    
    //get worldtransform
    const Matrix4& GetWorldTransform() const { return mWorldTransform; }
    // lab 9 functions
    class MeshComponent* GetMesh (){ return mMesh; }
    class CameraComponent* GetCamera(){ return mCamera; }
    void setQuat (Quaternion quat) {mQuat= quat;}
    Quaternion getQuat () {return mQuat; }
    Vector3 GetRight ();

protected:
    class MoveComponent* mMove;
    class CollisionComponent* mColli;
	class Game* mGame;
	// Actor's state
	State mState;
	// Transform
	Vector3 mPosition;
	float mScale;
	float mRotation;
    Quaternion mQuat;
    Matrix4 mWorldTransform;
    class MeshComponent* mMesh;
    
    class CameraComponent* mCamera;
};
