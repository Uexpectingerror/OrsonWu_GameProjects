
#pragma once
#include <vector>
#include <SDL/SDL_stdinc.h>
#include "Math.h"
#include <unordered_map>
#include "CollisionComponent.h"
#include "Actor.h"

class Coin: public Actor
{
public:
    Coin (class Game* game );
    void UpdateActor (float deltaTime); 
private:
    
    
};
