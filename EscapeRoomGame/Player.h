#pragma once
#include "Actor.h"
#include <SDL/SDL_stdinc.h>
#include "Math.h"
#include <unordered_map>
#include "CollisionComponent.h"

class Player : public Actor
{
public:
    Player (class Game* game);
    void SetRespawnPos (Vector3 pos) {respawnPos = pos; }
    Vector3 getRespawnPos () {return respawnPos;} 
private:
    Vector3 respawnPos ;
    
};
