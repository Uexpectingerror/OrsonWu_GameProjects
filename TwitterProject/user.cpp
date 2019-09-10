#include "user.h"
#include <iostream>
#include <sstream>
#include <string>
#include "tweet.h"
#include "hsort.h"
#include <algorithm>


using namespace std;

User::User()
:mName(""), mFollowers(), mFollowing(), mTweets(), pwHash(0)
{
}


User::User(std::string name)
: mName(name), mFollowers(), mFollowing(), mTweets(), pwHash(0)
{
}

User:: ~User()
{
	if(!mTweets.empty())
	{
		for (list<Tweet*>::iterator i = mTweets.begin(); i!= mTweets.end(); ++i)
		{
			delete *i;
		}
	}
	//delete all dir mentions
	if (!mDirMentions.empty())
	{
		for (map<string, set<Tweet*>>::iterator j = mDirMentions.begin(); j!= mDirMentions.end(); ++j)
		{
			//only free the ones that the user own otherwise it will double delete 
			if(j->first == this->name())
			{
				set<Tweet*> mentTws = j->second;
				for (set<Tweet*>::iterator k = mentTws.begin(); k != mentTws.end(); ++k)
				{
					delete *k;
				}
			}
		}
	}
}
/**
* copy constructor
*/
User::User(const User& u)
: mName(u.mName), mFollowers(u.mFollowers), mFollowing(u.mFollowing), mTweets(u.mTweets)
{

}

/**
  * assignment operator overload
  */
User& User::operator=(const User& u)
{
	this-> mName = u.mName;
	this-> mFollowers = u.mFollowers;
	this-> mFollowing = u.mFollowing;
	this-> mTweets = u.mTweets;
	return *this;
}


std::string User::name() const
{
	return mName;
}

  /**
   * Gets all the followers of this user  
   * 
   * @return Set of Users who follow this user
   */
std::set<User*> User::followers() const
{
	return mFollowers;
}

  /**
   * Gets all the users whom this user follows  
   * 
   * @return Set of Users whom this user follows
   */
std::set<User*> User::following() const
{
	return mFollowing;
}

  /**
   * Gets all the tweets this user has posted
   * 
   * @return List of tweets this user has posted
   */
std::list<Tweet*> User::tweets() const
{
	return mTweets;
}

  /**
   * Adds a follower to this users set of followers
   * 
   * @param u User to add as a follower
   */
void User::addFollower(User* u)
{
	mFollowers.insert(u);
}

  /**
   * Adds another user to the set whom this User follows
   * 
   * @param u User that the user will now follow
   */
void User::addFollowing(User* u)
{
	mFollowing.insert(u);
}

  /**
   * Adds the given tweet as a post from this user
   * 
   * @param t new Tweet posted by this user
   */
void User::addTweet(Tweet* t)
{
	mTweets.push_back(t);
}

  /**
   * Produces the list of Tweets that represent this users feed/timeline
   *  It should contain in timestamp order all the tweets from
   *  this user and all the tweets from all the users whom this user follows
   *
   * @return vector of pointers to all the tweets from this user
   *         and those they follow in timestamp order
   */
std::vector<Tweet*> User::getFeed()
{
	vector<Tweet*> retVal;
	
	//get all the tweets
	
	for (list<Tweet*>::iterator i = mTweets.begin(); i!= mTweets.end(); ++i)
	{
		retVal.push_back(*i);
	}

	for (set<User*>::iterator i = mFollowing.begin(); i!= mFollowing.end(); ++i)
	{
		//cout<<"size: "<<(*i)->tweets().size()<<endl;
		if(!(*i)->tweets().empty())
		{	
			list<Tweet*> temp = (*i)->tweets();
			
			for (list<Tweet*>::iterator j = temp.begin(); j!= temp.end(); ++j)
			{
				retVal.push_back(*j);
			}
		}
	}
	//loop over dir mentioned tweets if the tweet is sent by myself if not see if the user is followed by me 
	for (map<string, set<Tweet*>>::iterator j = mDirMentions.begin(); j!= mDirMentions.end(); ++j)
	{
		if(j->first == this->name())
		{
			set<Tweet*> mentTws = j->second;
			for (set<Tweet*>::iterator k = mentTws.begin(); k != mentTws.end(); ++k)
			{
				retVal.push_back(*k);
			}
		}
		else 
		{
			//test if the two users has follow each other if it does add this into the feed 
			for (set<User*>::iterator i = mFollowing.begin(); i!=mFollowing.end(); ++i)
			{
				if((*i)->name() == j->first)
				{
					set<User*> mFolFoll = (*i)->following();
					for(set<User*>::iterator k = mFolFoll.begin(); k != mFolFoll.end(); ++k)
					{
						if((*k)->name() == this->name())
						{
							set<Tweet*> mentTws = j->second;
							for (set<Tweet*>::iterator q = mentTws.begin(); q != mentTws.end(); ++q)
							{
								retVal.push_back(*q);
							}
						}
					}
					
				}
			}
		}
	}
	
	hsort(retVal, TweetComp());
	//more work here
	return retVal;
}

void User::addNormMent(Tweet* tw)
{
	mMentTweets.push_back(tw);
}

void User::addDirMent(const string& name, Tweet* tw)
{
	map<string, set<Tweet*>>::iterator i = mDirMentions.find(name);
	if(i == mDirMentions.end())
	{
		set<Tweet*> tws;
		tws.insert(tw);
		mDirMentions.insert(make_pair(name, tws));
	}
	else
	{
		(i->second).insert(tw);
	}
	
}

std::vector<Tweet*> User::getMentionFeed()
{
	vector<Tweet*> retVal;
	
	//get all the tweets
	for (list<Tweet*>::iterator i = mMentTweets.begin(); i!= mMentTweets.end(); ++i)
	{
		retVal.push_back(*i);
	}
	// loop over the mentioned list
	for (map<string, set<Tweet*>>::iterator j = mDirMentions.begin(); j!= mDirMentions.end(); ++j)
	{
		if(j->first != this->name())
		{
			set<Tweet*> mentTws = j->second;
			for (set<Tweet*>::iterator k = mentTws.begin(); k != mentTws.end(); ++k)
			{
				retVal.push_back(*k);
			}
		}
	}
	
	hsort(retVal, TweetComp());
	//more work here
	return retVal;
}