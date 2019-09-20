#pragma once
#include <vector>
#include <SDL/SDL_stdinc.h>
#include "Math.h"
#include <unordered_map>
#include "CollisionComponent.h"
#include "Actor.h"

class Arrow: public Actor
{
public:
    Arrow (class Game* game );
    void UpdateActor(float deltaTime) override;
    
    
private:
    
    
};
