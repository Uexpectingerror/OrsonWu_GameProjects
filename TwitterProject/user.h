#ifndef USER_H
#define USER_H

#include <string>
#include <set>
#include <list>
#include <vector>
#include <map>

/* Forward Declaration to avoid #include dependencies */
class Tweet;

class User {
public:
User();

/**
 * Constructor 
 */
User(std::string name);

/**
 * Destructor
 */
~User();
/**
* copy constructor
*/
User(const User& u);

/**
  * assignment operator overload
  */
User& operator=(const User& u);

/**
 * Gets the name of the user 
 * 
 * @return name of the user 
 */
std::string name() const; 

/**
 * Gets all the followers of this user  
 * 
 * @return Set of Users who follow this user
 */
std::set<User*> followers() const;

/**
 * Gets all the users whom this user follows  
 * 
 * @return Set of Users whom this user follows
 */
std::set<User*> following() const;

/**
 * Gets all the tweets this user has posted
 * 
 * @return List of tweets this user has posted
 */
std::list<Tweet*> tweets() const; 

/**
 * Adds a follower to this users set of followers
 * 
 * @param u User to add as a follower
 */
void addFollower(User* u);

/**
 * Adds another user to the set whom this User follows
 * 
 * @param u User that the user will now follow
 */
void addFollowing(User* u);

/**
 * Adds the given tweet as a post from this user
 * 
 * @param t new Tweet posted by this user
 */
void addTweet(Tweet* t);

std::list<Tweet*> getmMenTws()
{
	return mMentTweets;
}

std::map<std::string,std::set<Tweet*>> getmMirMens()
{
	return mDirMentions;
}

unsigned int getPWHash()
{
	return pwHash;
}

void setPWHash(unsigned int h)
{
	pwHash = h;
}

/**
 * Produces the list of Tweets that represent this users feed/timeline
 *  It should contain in timestamp order all the tweets from
 *  this user and all the tweets from all the users whom this user follows
 *
 * @return vector of pointers to all the tweets from this user
 *         and those they follow in timestamp order
 */
std::vector<Tweet*> getFeed();

std::vector<Tweet*> getMentionFeed();
//add funcs for mentions and direct mentions
void addNormMent(Tweet* tw);
void addDirMent(const std::string& name, Tweet* tw);


/* You may add other member functions */
private:
/* Add any other data members or helper functions here  */
std::string mName;
std::set<User*> mFollowers;
std::set<User*> mFollowing;
std::list<Tweet*> mTweets;
std::list<Tweet*> mMentTweets;
std::map<std::string,std::set<Tweet*>> mDirMentions;
unsigned int pwHash;

};

#endif
