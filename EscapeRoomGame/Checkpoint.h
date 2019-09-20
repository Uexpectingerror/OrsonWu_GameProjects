#pragma once
#include <vector>
#include <SDL/SDL_stdinc.h>
#include "Math.h"
#include <unordered_map>
#include "CollisionComponent.h"
#include "Actor.h"
#include <string>

class Checkpoint: public Actor
{
public:
    Checkpoint (class Game* game );
    void UpdateActor(float deltaTime) override;
    void SetActive() { isActive =true; }
    void SetLevelString (std::string string){ mLevelString=string; }
    void SetTextString (std::string string){ mTextString=string; }

private:
    bool isActive;
    std::string mLevelString ;
    std::string mTextString; 
};

