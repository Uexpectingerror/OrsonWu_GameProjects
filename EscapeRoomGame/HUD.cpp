#include "HUD.h"
#include "Texture.h"
#include "Shader.h"
#include "Game.h"
#include "Renderer.h"
#include "Font.h"
#include <sstream>
#include <iomanip>
#include <iostream>

HUD::HUD(Game* game)
	:mGame(game)
	,mFont(nullptr)
    ,mCoinCollected(0)
,isCPHIT(false)
,timeLastCPHIT(0)
{
	// Load font
	mFont = new Font();
	mFont->Load("Assets/Inconsolata-Regular.ttf");
    mTimerText = mFont->RenderText("00:00.00");
    mCoinCounText = mFont->RenderText("0/55");
    mCPText = mFont->RenderText("   ");
}

HUD::~HUD()
{
	// Get rid of font
	if (mFont)
	{
		mFont->Unload();
		delete mFont;
	}
}

void HUD::Update(float deltaTime)
{
	// TODO
    mTime+=deltaTime;
    std::cout<<"time:"<< mTime<<std::endl;
    mTimerText->Unload();
    delete mTimerText;
    int MM = mTime/60.0f;
    int SS = mTime- MM*60;
    int FF = mTime*100-MM*6000-SS*100;
    std::string MM_S;
    std::string SS_S;
    std::string FF_S;
    //get MM string
    if (MM<10&&MM>0)
    {
        MM_S = "0"+std::to_string(MM);
    }
    else if(MM==0)
    {
        MM_S = "00";
    }
    else
    {
        MM_S = std::to_string(MM);
    }
    
    //get ss string
    if (SS<10&&SS>0)
    {
        SS_S = "0"+std::to_string(SS);
    }
    else if(SS==0)
    {
        SS_S = "00";
    }
    else
    {
        SS_S = std::to_string(SS);
    }
    
    //get ff string
    if (FF<10&&FF>0)
    {
        FF_S = "0"+std::to_string(FF);
    }
    else if(FF==0)
    {
        FF_S = "00";
    }
    else
    {
        FF_S = std::to_string(FF);
    }
    
    std::string textString= MM_S+":"+SS_S+"."+FF_S;
    //std::cout << MM_S<<":"<<SS_S<<"."<<FF_S<<std::endl;
    mTimerText= mFont->RenderText(textString);
    
}

void HUD::UpdateCoinText()
{
    
    mCoinCounText->Unload();
    delete mCoinCounText;
    
    mCoinCollected+=1;
    std::string CoinText = std::to_string(mCoinCollected)+"/55";
    
    mCoinCounText= mFont->RenderText(CoinText);
}

void HUD::UpdateCPText(const std::string & text )
{
    mCPText->Unload();
    delete mCPText;
    mCPText= mFont->RenderText(text);
    isCPHIT=true;
    timeLastCPHIT = mTime;
}

void HUD::Draw(Shader* shader)
{
	// TODO
    DrawTexture(shader, mTimerText, Vector2(-420.0f, -325.0f));
    DrawTexture(shader, mCoinCounText, Vector2(-420.0f, -300.0f));
    if (isCPHIT)
    {
        DrawTexture(shader, mCPText, Vector2(0.0f, 0.0f));
        if ((mTime-timeLastCPHIT)>=5.0f)
        {
            isCPHIT=false;
        }
    }

    
}

void HUD::DrawTexture(class Shader* shader, class Texture* texture,
				 const Vector2& offset, float scale)
{
	// Scale the quad by the width/height of texture
	Matrix4 scaleMat = Matrix4::CreateScale(
		static_cast<float>(texture->GetWidth()) * scale,
		static_cast<float>(texture->GetHeight()) * scale,
		1.0f);
	// Translate to position on screen
	Matrix4 transMat = Matrix4::CreateTranslation(
		Vector3(offset.x, offset.y, 0.0f));	
	// Set world transform
	Matrix4 world = scaleMat * transMat;
	shader->SetMatrixUniform("uWorldTransform", world);
	// Set current texture
	texture->SetActive();
	// Draw quad
	glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, nullptr);
}
