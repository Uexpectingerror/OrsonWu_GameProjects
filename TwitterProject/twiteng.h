#ifndef TWITENG_H
#define TWITENG_H
#include <map>
#include <string>
#include <set>
#include <vector>
#include <fstream>
#include <sstream>
#include <iostream>
#include "user.h"
#include "datetime.h"
#include "heap.h"
#include "tagdata.h"
#include "hash.h"


class TwitEng
{
 public:
  TwitEng();
  ~TwitEng();
  /**
   * Parses the Twitter database and populates internal structures
   * @param filename of the database file
   * @return true if there is an error, false if successful
   */
  bool parse(char* filename);

  /**
   * Allocates a tweet from its parts and adds it to internal structures
   * @param username of the user who made the tweet
   * @param timestamp of the tweet
   * @param text is the actual text of the tweet as a single string
   */
  void addTweet(const std::string& username, const DateTime& time, const std::string& text);

  /**
   * Searches for tweets with the given words and strategy
   * @param words is the hashtag terms in any case without the '#'
   * @param strategy 0=AND, 1=OR
   * @return the tweets that match the search
   */
  std::vector<Tweet*> search(std::vector<std::string>& terms, int strategy);

  /**
   * Dump feeds of each user to their own file
   */
  void dumpFeeds();

  void addFollower(const std::string& followee);

  void saveFile(const std::string& fileName);
  // Computes the intersection of s1 and s2
  void printTrending(int n);

  //try to login with the given key
  bool loginAttempt(std::string username, std::string pw);

  void logout()
  {
    curLoginUser = nullptr;
  }

  User* getLoginedUser()
  {
    return curLoginUser;
  }

  /* You may add other member functions */
 private:
  /* Add any other data members or helper functions here  */
  std::set<User*> mUsers;
  std::map<std::string, std::set<Tweet*>> mHashTagIndex;
  //helper function
  std::set<User*>::iterator isUserExist(const std::string& name);
  //debug func
  void printUsers();
  //mention func helping functions
  void mentHelper(Tweet* tw, User* twOwner, const std::string& text);
  Heap<TagData, TagStringEqual, TagIntGreater, TagStringHasher> trending_;
  User* curLoginUser;
};

  std::set<Tweet*> operator&(const std::set<Tweet*>& s1, 
                                const std::set<Tweet*>& s2);

  // Computes the union of s1 and s2
  std::set<Tweet*> operator|(const std::set<Tweet*>& s1, 
                                const std::set<Tweet*>& s2);


#endif
