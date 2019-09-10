#include "user.h"
#include "tweet.h"
#include "twiteng.h"
#include "util.h"
#include <algorithm>
#include "hsort.h"
#include "tagdata.h"
using namespace std;


TwitEng::TwitEng()
:mUsers(), mHashTagIndex(), trending_(2), curLoginUser(nullptr)
{

}

TwitEng::~TwitEng()
{
  if(!mUsers.empty())
  {
    for (set<User*>::iterator i = mUsers.begin(); i!= mUsers.end(); ++i)
    {
      delete *i;
    }
  }

  
}

set<User*>::iterator TwitEng::isUserExist(const std::string& name)
{
  for(set<User*>::iterator i= mUsers.begin(); i!= mUsers.end(); ++i)
  {
    if((*i)->name()==name)
    {
      return i;
    }
  }
  return mUsers.end();
}

void TwitEng::printUsers()
{
  for (set<User*>::iterator i = mUsers.begin(); i!=mUsers.end(); ++i)
  {
    cout<<"User: "<< (*i)->name()<<endl;
    cout<<"\t"<<"Followings: ";
    for (set<User*>::iterator j = (*i)->following().begin(); j!=(*i)->following().end(); ++j)
    {
      cout<< (*j)->name() <<" ";
    }
    cout<<endl;
    cout<<"\t"<<"Followers: ";

    for (set<User*>::iterator j = (*i)->followers().begin(); j!=(*i)->followers().end(); ++j)
    {
      cout<< (*j)->name() <<" ";
    }
    cout<<endl;
    vector <Tweet*> tweets = (*i)->getFeed();
    for(unsigned int j = 0; j<tweets.size(); ++j)
    {
      cout<< *tweets[j]<<endl;
    }
  }
}
/**
 * Parses the Twitter database and populates internal structures
 * @param filename of the database file
 * @return true if there is an error, false if successful
 */
 bool TwitEng::parse(char* filename)
{
  ifstream input (filename);
  if (!input.is_open())
  {
    std::cout <<"Cant find the input file ... quiting..." << std::endl;
    return true;
  }
  //get the users and their followings
  string fl;
  int numOfUsers;
  getline(input, fl);
  if(fl == "")
  {
    return true;
  }
  numOfUsers = stoi(fl);
  string u_info;
  //loop over the lines of user info
  for(int i =0; i<numOfUsers; ++i)
  {
    getline (input, u_info);
    //test if the line is valid
    if(u_info == "")
    {
      return true;
    }
    stringstream ss (u_info);
    string userName;
    ss>>userName;
    //test if the user already exist
    User* user;
    if(isUserExist(userName) == mUsers.end())
    {
      //create a new user and added to the set
      user = new User(userName);
      //cout<<"create new user from firstName: "<<userName<< user <<endl;
      mUsers.insert(user);
    }
    else
    {
      user = *isUserExist(userName);
    }
    //assign passwords to the users
    unsigned int pwHash;
    ss>>pwHash;
    //check if pw info is valid
    if(pwHash <=0)
    {
      cout<<"Invalid password data."<<endl;
      return true;
    }
    user->setPWHash(pwHash);

    string aFollowing;
    //loop through the rest of the line and add all the followings to the set 
    while (ss>>aFollowing)
    {
      User* f_user;
      set<User*>::iterator i_f = isUserExist(aFollowing);
      if(i_f == mUsers.end())
      {
        //create a new user and added to the set
        f_user = new User(aFollowing);
        //cout<<"create new user from followName: "<<aFollowing<<f_user<<endl;
        mUsers.insert(f_user);
        user->addFollowing (f_user);
        f_user->addFollower(user);
      }
      else
      {
        f_user = *i_f;
        user->addFollowing (f_user);
        f_user->addFollower(user);
      }
    }
  }
  
  string tweetLine;
  while(getline(input, tweetLine))
  {
    if(tweetLine!="")
    {
      trim(tweetLine);
      //get dt for this tweet
      stringstream ss(tweetLine);
      DateTime dt;
      string s1;
      string s2;
      string s3;
      ss>>s1;
      ss>>s2;
      s3=s1+" "+s2;
      istringstream iss(s3);
      iss>>dt;

      //get the name
      string name;
      ss>>name;

      //get the text
      string text;
      getline(ss,text);
      trim(text);
      addTweet(name, dt, text);
      //debug
      //cout<<dt<<endl;
    }
  }
  
  return false;
}

//mention function helper 
//add tweet to the different data structure in a user class
void TwitEng::mentHelper(Tweet* tw, User* twOwner, const std::string& text)
{
  string mentName;
  stringstream ss (text);
  //if it is starting with @ add it to the dir mention list of both sender and mentioned user.
  if(text[0] == '@')
  {
    getline (ss, mentName, '@');
    twOwner->addDirMent(twOwner->name(),tw);
    //cout << "adding tweet to dirMent... "<< twOwner->name() <<"   "<< *tw <<endl;
    if(ss >> mentName)
    {
      User* mentUser = nullptr;
      set<User*>::iterator i = isUserExist(mentName);
      //test if the user exist in the data base if not then do nothing
      if(i != mUsers.end())
      {
        mentUser = *i;
        mentUser->addDirMent(twOwner->name(),tw);
      }
    }
  }
  //if not starting with @
  else
  {
    //default will add this to the main tweet list 
    twOwner-> addTweet(tw);
    while (getline(ss,mentName,'@'))
    {
      //if has @ inside 
      if(mentName != text)
      {
        if(ss>>mentName)
        {
          User* mentUser = nullptr;
          set<User*>::iterator i = isUserExist(mentName);
          if(i != mUsers.end())
          {
            mentUser = *i;
            mentUser->addNormMent(tw);
          }
        }
      }
    }
  }
}


/**
 * Allocates a tweet from its parts and adds it to internal structures
 * @param username of the user who made the tweet
 * @param timestamp of the tweet
 * @param text is the actual text of the tweet as a single string
 */
void TwitEng::addTweet(const std::string& username, const DateTime& time, const std::string& text)
{
  set<User*>::iterator i_user = isUserExist(username);
  if(i_user != mUsers.end())
  {
    Tweet* tweet = new Tweet(*i_user, time, text);
    //(*i_user)->addTweet(tweet);
    //switch to use ment helper to decide which tweet list of the user it will be added to
    mentHelper(tweet, *i_user, text);
    //below is for managing hashtags 
    set<string> hashtags = tweet->hashTags();
    for (set<string>::iterator i = hashtags.begin(); i!= hashtags.end(); ++i)
    {
      
      map<std::string, std::set<Tweet*>>::iterator i_index = mHashTagIndex.find(*i);
      if( i_index == mHashTagIndex.end())
      {
        set<Tweet*> tweets;
        tweets.insert(tweet);
        mHashTagIndex.insert(make_pair(*i,tweets));
        TagData td = TagData(*i, 1);
        trending_.push(td);
      }
      else
      {
        i_index->second.insert(tweet);
        TagData oldVal = TagData(*i, i_index->second.size()-1);
        TagData newVal = TagData(*i, i_index->second.size());
        trending_.decreaseKey(oldVal, newVal);
      }
    }
  }
  else
  {
    return;
  }
}

/**
 * Searches for tweets with the given words and strategy
 * @param words is the hashtag terms in any case without the '#'
 * @param strategy 0=AND, 1=OR
 * @return the tweets that match the search
 */
std::vector<Tweet*> TwitEng::search(std::vector<std::string>& terms, int strategy)
{
  std::vector<Tweet*> retVal;
   if(terms.size() == 0)
  {
    return retVal;
  } 
  //And strategy
  map<string,set<Tweet*>>::iterator it = mHashTagIndex.find(terms[0]);
  set<Tweet*> tweetSet = {};
  if(it != mHashTagIndex.end())
  {
    tweetSet = it->second;
  }
  else
  {
    retVal.clear();
    return retVal;
  }
  
  for (unsigned int i =1; i<terms.size(); ++i)
  {
    if(strategy == 0)
    {
      map<string,set<Tweet*>>::iterator t = mHashTagIndex.find(terms[i]);
      //if any hashtag input doesnt match any hashtag return a empty vector
      if(t == mHashTagIndex.end())
      {
        retVal.clear();
        return retVal;
      }
      else
      {
        tweetSet = tweetSet & (t->second);
      }
    }
    else if (strategy == 1)
    {
      map<string,set<Tweet*>>::iterator t = mHashTagIndex.find(terms[i]);
      set<Tweet*> temp = t->second;
      tweetSet = tweetSet|temp;
    }
  }

  for (set<Tweet*>::iterator t = tweetSet.begin(); t!= tweetSet.end(); ++t)
  {
    retVal.push_back(*t);
  }

  //sort all the tweets
  hsort(retVal, TweetComp());
  return retVal;
}

/**
 * Dump feeds of each user to their own file
 */
void TwitEng::dumpFeeds()
{
  for (set<User*>::iterator i= mUsers.begin(); i!= mUsers.end(); ++i)
  {
    string fileName = (*i)->name() + ".feed";
    string mentFileName = (*i) ->name() +".mentions";
    ofstream output (fileName);
    ofstream mentOutFile (mentFileName);
    output<< (*i)->name()<<endl;
    mentOutFile << (*i) ->name()<<endl;
    vector <Tweet*> tweets = (*i)->getFeed();
    vector <Tweet*> mentTweetsV = (*i) ->getMentionFeed();
    for(unsigned int j = 0; j<tweets.size(); ++j)
    {
      output<< *tweets[j]<<endl;
    }
    for(unsigned int j = 0; j< mentTweetsV.size(); ++j)
    {
      mentOutFile<< *mentTweetsV[j]<<endl;
    }
  }
}

// Computes the intersection of s1 and s2
std::set<Tweet*> operator&(const std::set<Tweet*>& s1, const std::set<Tweet*>& s2)
{
  std::set<Tweet*> retVal = {};

  if(s1.empty() || s2.empty())
  {
    return retVal;
  }

  for (set<Tweet*>::iterator i = s2.begin(); i!= s2.end(); ++i)
  {
    set<Tweet*>::iterator dup = s1.find(*i);
    if(dup!= s1.end())
    {
      retVal.insert(*dup);
    }
  }
  return retVal;
}

// Computes the union of s1 and s2
std::set<Tweet*> operator|(const std::set<Tweet*>& s1, const std::set<Tweet*>& s2)
{
  std::set<Tweet*> retVal = s1;
  if(s1.empty())
  {
    return retVal;
  }
  else if (s2.empty())
  {
    std::set<Tweet*> retVal = s2;
    return retVal;
  }

  for (set<Tweet*>::iterator i = s2.begin(); i!= s2.end(); ++i)
  {
    retVal.insert(*i);
  }
  
  return retVal;
}


void TwitEng::addFollower(const std::string& followee)
{
  User* followerPtr = curLoginUser;
  User* followeePtr = nullptr;
  for (set<User*>::iterator i = mUsers.begin(); i!=mUsers.end(); ++i)
  {
    if ((*i)->name() == followee)
    {
      followeePtr = *i;
    }
  }
  //check if the names are valid if not just do nothing and return 
  if (followeePtr == nullptr || followerPtr == nullptr)
  {
    return;
  }

  if (followerPtr == followeePtr)// if it is following itself 
  {
    return;
  }

  //add follower with the following
  followerPtr->addFollowing(followeePtr);
  followeePtr->addFollower(followerPtr);
}

void TwitEng::saveFile(const std::string& fileName)
{
  ofstream output(fileName);
  int size = mUsers.size();
  output << to_string(size);
  for (set<User*>::iterator i = mUsers.begin(); i!=mUsers.end(); ++i)
  {
    output << endl;
    output << (*i)->name() <<" ";
    output << (*i)->getPWHash()<<" ";
    set<User*> followings = (*i)->following();
    for (set<User*>::iterator j = followings.begin(); j!=followings.end(); ++j)
    {
      output << (*j)->name() <<" ";
    }
  }
  for (set<User*>::iterator i = mUsers.begin(); i!=mUsers.end(); ++i)
  {
    list<Tweet*> tweets = (*i)->tweets();
    map<std::string,std::set<Tweet*>> dirMenTws = (*i)-> getmMirMens();
    map<std::string,std::set<Tweet*>>::iterator k = dirMenTws.find((*i)->name());
    //print the dir mentions
    if(k!= dirMenTws.end())
    {
      set<Tweet*> menTws = k->second;
      for ( set<Tweet*>::iterator j = menTws.begin(); j!=menTws.end(); ++j)
      {
        output << endl;
        output << **j;
      }
    }
    //print the normal tws 
    for (list<Tweet*>::iterator j = tweets.begin(); j !=tweets.end(); ++j)
    {
      output << endl;
      output << **j;
    }
  }
}

void TwitEng::printTrending(int n)
{
  //just for testing purposes print out all the hashtags and their trending data 
  int count = 0;
  while(!trending_.empty() && count < n)
  {
    TagData t = trending_.top();
    cout<<t.tag<<" : "<<t.num<<endl;
    trending_.pop();
    count++;
  }
}

bool TwitEng::loginAttempt(string username, string pw)
{
  set<User*>::iterator i= isUserExist(username);
  if(i==mUsers.end()) //invalid username
  {
    return false;
  }
  else
  {
    unsigned int inputHash = calculateHash(pw);
    if((*i)->getPWHash() == inputHash)
    {
      curLoginUser = *i;
      return true;
    }
    else // wrong password 
    {
      return false;
    }
  }
}