#pragma once
#include "SDL/SDL.h"
#include "SDL/SDL_mixer.h"
#include <unordered_map>
#include <string>
#include <vector>
#include "Math.h"
#include "queue"

class Game
{
public:
	Game();
	bool Initialize();
	void RunLoop();
	void Shutdown();

	void AddActor(class Actor* actor);
	void RemoveActor(class Actor* actor);

	Mix_Chunk* GetSound(const std::string& fileName);

	//void LoadLevel(const std::string& fileName);

	class Renderer* GetRenderer() {	return mRenderer; }
    
    void AddBlock (class Block* block) {mBlocks.push_back(block); }
    
    void RemoveBlock(class Block* block)
    {
        std::vector<Block*>::iterator theOne;
        theOne= std::find(mBlocks.begin(), mBlocks.end(), block);
        //remove it
        mBlocks.erase(theOne);
    }
    
    std::vector<class Block*> getMyBlocks (){ return mBlocks; }
    
    class Tank* mTankOne;
    class Tank* mTankTwo;
    class Renderer* getRender(){return mRenderer;}
    std::queue<class Checkpoint*> mCheckpoints;
    class Player* mPlayer;
    void SetNextLevel (const std::string& string){mNextLevel=string ;}
    void LoadNextLevel ();
    class HUD * mHUD;

private:
	void ProcessInput();
	void UpdateGame();
	void GenerateOutput();
	void LoadData();
	void UnloadData();

	// Map of textures loaded
	std::unordered_map<std::string, SDL_Texture*> mTextures;
	std::unordered_map<std::string, Mix_Chunk*> mSounds;

	// All the actors in the game
	std::vector<class Actor*> mActors;

	class Renderer* mRenderer;

	Uint32 mTicksCount;
	bool mIsRunning;
    
    class Arrow* mArrow;
    //all blocks
    std::vector<class Block*> mBlocks;
    std::string mNextLevel;
    
};
