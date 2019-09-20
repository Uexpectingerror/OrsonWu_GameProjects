//
//  Game.cpp
//  Game-mac
//
//  Created by Sanjay Madhav on 5/31/17.
//  Copyright © 2017 Sanjay Madhav. All rights reserved.
//

#include "Game.h"
#include <algorithm>
#include "Actor.h"
#include <fstream>
#include "Block.h"
#include "Renderer.h"
#include "LevelLoader.h"
#include "Arrow.h"
#include <SDL/SDL_ttf.h>
#include "HUD.h"

Game::Game()
:mIsRunning(true)
,mArrow(nullptr)
{
	
}

bool Game::Initialize()
{
    //initializa mouse functions
    
    
    
	if (SDL_Init(SDL_INIT_VIDEO|SDL_INIT_AUDIO) != 0)
	{
		SDL_Log("Unable to initialize SDL: %s", SDL_GetError());
		return false;
	}
    

	// TODO: Create renderer
    mRenderer=new Renderer(this);
    if (!mRenderer->Initialize(1024.0f, 768.0f))
    {
        return false;
    }
    //load font
    TTF_Init();
	Mix_OpenAudio(44100, MIX_DEFAULT_FORMAT, 2, 2048);

	LoadData();

	mTicksCount = SDL_GetTicks();
    SDL_SetRelativeMouseMode(SDL_TRUE); // Enable relative mouse mode
    SDL_GetRelativeMouseState(nullptr, nullptr); // Clear any saved values
    
    mArrow = new Arrow(this);

	return true;
}

void Game::RunLoop()
{
	while (mIsRunning)
	{
		ProcessInput();
		UpdateGame();
		GenerateOutput();
        if (!mNextLevel.empty())
        {
            LoadNextLevel();
        }
	}
}
void Game::LoadNextLevel()
{
    while (!mActors.empty())
    {
        delete mActors.back();
    }
    
    while (!mCheckpoints.empty())
    {
        mCheckpoints.pop();
    }
    LevelLoader::Load(this, mNextLevel);
    Arrow* arrow = new Arrow(this);
    mNextLevel.clear();
}

void Game::ProcessInput()
{
	SDL_Event event;
	while (SDL_PollEvent(&event))
	{
		switch (event.type)
		{
			case SDL_QUIT:
				mIsRunning = false;
				break;
		}
	}
	
	const Uint8 *state = SDL_GetKeyboardState(NULL);
	if (state[SDL_SCANCODE_ESCAPE])
	{
		mIsRunning = false;
	}

	for (auto actor : mActors)
	{
		actor->ProcessInput(state);
	}
}

void Game::UpdateGame()
{
	// Compute delta time
	// Wait until 16ms has elapsed since last frame
	while (!SDL_TICKS_PASSED(SDL_GetTicks(), mTicksCount + 16))
		;

	float deltaTime = (SDL_GetTicks() - mTicksCount) / 1000.0f;
	if (deltaTime > 0.05f)
	{
		deltaTime = 0.05f;
	}
	mTicksCount = SDL_GetTicks();
	
	// Make copy of actor vector
	// (iterate over this in case new actors are created)
	std::vector<Actor*> copy = mActors;
	// Update all actors
	for (auto actor : copy)
	{
		actor->Update(deltaTime);
	}

	// Add any dead actors to a temp vector
	std::vector<Actor*> deadActors;
	for (auto actor : mActors)
	{
		if (actor->GetState() == Actor::EDead)
		{
			deadActors.emplace_back(actor);
		}
	}

	// Delete any of the dead actors (which will
	// remove them from mActors)
	for (auto actor : deadActors)
	{
		delete actor;
	}
    
    mHUD->Update(deltaTime);
    //updating the camera
   
}

void Game::GenerateOutput()
{
	// TODO: tell renderer to draw
    mRenderer->Draw();
}

void Game::LoadData()
{
    
	
    LevelLoader::Load(this, "Assets/Tutorial.json");
    //set projection matrix
    mRenderer->SetProjectionMatrix(Matrix4::CreatePerspectiveFOV(1.22f, 1024, 768, 10, 5000.0f));
    //create look at matrix
    mRenderer->SetViewMatrix(Matrix4::CreateLookAt(Vector3(0,0,0), Vector3(5,0,0), Vector3(0,0,1)));
    Mix_PlayChannel(-1, GetSound("Assets/Sounds/Music.ogg"), -1);
    
    mHUD = new HUD(this);
}

//void Game::LoadLevel(const std::string& fileName)
//{
//    std::ifstream file(fileName);
//    if (!file.is_open())
//    {
//        SDL_Log("Failed to load level: %s", fileName.c_str());
//    }
//
//    const float topLeftX = -512.0f + 32.0f;
//    const float topLeftY = -384.0f + 32.0f;
//    size_t row = 0;
//    std::string line;
//    while (!file.eof())
//    {
//        std::getline(file, line);
//        for (size_t col = 0; col < line.size(); col++)
//        {
//            // Calculate position at this row/column
//            Vector3 pos;
//            pos.x = topLeftX + 64.0f * col;
//            pos.y = topLeftY + 64.0f * row;
//
//            if (line[col] == 'B')
//            {
//                // TODO: Create block
//                Block* newBlock = new Block(this);
//                newBlock->SetPosition(pos);
//            }
//            
//        }
//        row++;
//    }
//   
//    
//}

void Game::UnloadData()
{
	// Delete actors
	// Because ~Actor calls RemoveActor, have to use a different style loop
	while (!mActors.empty())
	{
		delete mActors.back();
	}

	// Destroy textures
	for (auto i : mTextures)
	{
		SDL_DestroyTexture(i.second);
	}
	mTextures.clear();

	// Destroy sounds
	for (auto s : mSounds)
	{
		Mix_FreeChunk(s.second);
	}
	mSounds.clear();
}

Mix_Chunk* Game::GetSound(const std::string& fileName)
{
	Mix_Chunk* chunk = nullptr;
	auto iter = mSounds.find(fileName);
	if (iter != mSounds.end())
	{
		chunk = iter->second;
	}
	else
	{
		chunk = Mix_LoadWAV(fileName.c_str());
		if (!chunk)
		{
			SDL_Log("Failed to load sound file %s", fileName.c_str());
			return nullptr;
		}

		mSounds.emplace(fileName, chunk);
	}
	return chunk;
}

void Game::Shutdown()
{
	UnloadData();
	Mix_CloseAudio();
	// TODO: Delete renderer
    mRenderer->Shutdown();
    delete mRenderer;
	SDL_Quit();
}

void Game::AddActor(Actor* actor)
{
	mActors.emplace_back(actor);
}

void Game::RemoveActor(Actor* actor)
{
	auto iter = std::find(mActors.begin(), mActors.end(), actor);
	if (iter != mActors.end())
	{
		// Swap to end of vector and pop off (avoid erase copies)
		auto iter2 = mActors.end() - 1;
		std::iter_swap(iter, iter2);
		mActors.pop_back();
	}
}
