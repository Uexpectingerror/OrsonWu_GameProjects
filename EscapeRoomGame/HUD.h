#pragma once
#include "Math.h"
#include <string>

class HUD
{
public:
	HUD(class Game* game);
	~HUD();
	// UIScreen subclasses can override these
	virtual void Update(float deltaTime);
	virtual void Draw(class Shader* shader);
    void UpdateCoinText();
    void UpdateCPText(const std::string & text );
    
protected:
	// Helper to draw a texture
	void DrawTexture(class Shader* shader, class Texture* texture,
					 const Vector2& offset = Vector2::Zero,
					 float scale = 1.0f);
	class Game* mGame;
	
	class Font* mFont;
    
    class Texture* mTimerText;
    class Texture* mCoinCounText;
    class Texture* mCPText;

    int mCoinCollected; 
    float mTime;
    bool isCPHIT;
    float timeLastCPHIT;
	// TODO: Add textures/member variables
};
