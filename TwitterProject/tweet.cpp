#include "tweet.h"
#include "user.h"
#include <string> 
#include <iostream>
#include <fstream>
#include <sstream>
#include "util.h"

using namespace std;

void Tweet::parseHashTags(const string& text)
{
	stringstream ssTag(text);
	string word;
	//collect tags
	while (std::getline(ssTag, word, '#'))
	{
		if (word == text)
		{
			break;
		}

		//check if this is reaching @ or just /n
		else if (ssTag >> word)
		{
			convLower(word);
			if(mHashTags.find(word) == mHashTags.end())
			{
				mHashTags.insert(word);
				//cout<<"adding hash: "<<word<<endl;
				//std::cout << word << std::endl;
			}
		}

	}
}
/**
* Default constructor 
*/
Tweet::Tweet()
:mUser(), mTimeStamp(), mText(""), mHashTags()
{
	mHashTags = {};
}

/**
* Constructor 
*/
Tweet::Tweet(User* user, const DateTime& time, const std::string& text)
:mUser(user), mTimeStamp(time), mText(text), mHashTags()
{

	mUser = user;
	trim(mText);
	//cout<<"create new tweet"<<endl;
	parseHashTags(mText);
}
/**
* deconstructor 
*/
Tweet::~Tweet()
{
	//nothing here since user has its own deconstructor

}
/**
* copy constructor 
*/
Tweet::Tweet(const Tweet& t)
: mUser(t.mUser),mTimeStamp(t.mTimeStamp), mText(t.mText), mHashTags(t.mHashTags)
{

}
/**
* assignment operator 
*/
Tweet& Tweet::operator= (const Tweet& t)
{
	this->mTimeStamp = t.mTimeStamp;
	this->mUser = t.mUser;
	this->mText = t.mText;
	this->mHashTags = t.mHashTags;
	return *this;
}
/**
* Gets the timestamp of this tweet
*
* @return timestamp of the tweet
*/
DateTime const & Tweet::time() const
{
	return mTimeStamp;
}

/**
* Gets the actual text of this tweet
*
* @return text of the tweet
*/
std::string const & Tweet::text() const
{
	return mText;
}

/**
* Returns the hashtagged words without the '#' sign
*
*/
std::set<std::string> Tweet::hashTags() const
{
	return mHashTags;
}

/**
* Return true if this Tweet's timestamp is less-than other's
*
* @return result of less-than comparison of tweet's timestamp
*/
bool Tweet::operator<(const Tweet& other) const
{
	if(this->mTimeStamp<other.mTimeStamp)
	{
		return true;
	}
	else
	{
		return false;
	}
}

/**
* Outputs the tweet to the given ostream in format:
*   YYYY-MM-DD HH::MM::SS username tweet_text
*
* @return the ostream passed in as an argument
*/
std::ostream& operator<<(std::ostream& os, const Tweet& t)
{
	os << t.time() << " " << t.user()->name() <<" "<< t.text();
	return os;
}

/* Create any other public or private helper functions you deem 
 necessary */

User* Tweet::user() const
{
	return mUser;
}
