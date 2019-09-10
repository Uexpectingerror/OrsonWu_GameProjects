#include "cmdhandler.h"
#include "util.h"
#include "tweet.h"
#include "user.h"
#include <vector>
#include <iostream>
#include <string>
#include "datetime.h"
#include <sstream>
using namespace std;
//quit handler
QuitHandler::QuitHandler()
{

}

QuitHandler::QuitHandler(Handler* next)
  : Handler(next)
{

}

bool QuitHandler::canHandle(const std::string& cmd) const
{
	return cmd == "QUIT";
}

Handler::HANDLER_STATUS_T QuitHandler::process(TwitEng* eng, std::istream& instr) const
{
	eng->dumpFeeds();
	return HANDLER_QUIT;
}

//And functions 
AndHandler::AndHandler()
{

}

AndHandler::AndHandler(Handler* next)
  : Handler(next)
{

}

bool AndHandler::canHandle(const std::string& cmd) const
{
	return cmd == "AND";
}

Handler::HANDLER_STATUS_T AndHandler::process(TwitEng* eng, std::istream& instr) const
{
	string word;
	std::vector<string> hashTags;
	while (instr>>word)
	{
		convLower(word);
		hashTags.push_back(word);
	}
	vector<Tweet*> tweets = eng->search(hashTags, 0);
	displayHits(tweets);
	return HANDLER_OK;

}

//Or handle
OrHandler::OrHandler()
{

}

OrHandler::OrHandler(Handler* next)
  : Handler(next)
{

}

bool OrHandler::canHandle(const std::string& cmd) const
{
	return cmd == "OR";
}

Handler::HANDLER_STATUS_T OrHandler::process(TwitEng* eng, std::istream& instr) const
{
	string word;
	std::vector<string> hashTags;
	while (instr>>word)
	{
		convLower(word);
		hashTags.push_back(word);
	}
	vector<Tweet*> tweets = eng->search(hashTags, 1);
	displayHits(tweets);
	return HANDLER_OK;

}

//Tweet functions 
TweetHandler::TweetHandler()
{

}

TweetHandler::TweetHandler(Handler* next)
  : Handler(next)
{

}

bool TweetHandler::canHandle(const std::string& cmd) const
{
	return cmd == "TWEET";
}

Handler::HANDLER_STATUS_T TweetHandler::process(TwitEng* eng, std::istream& instr) const
{
	if(eng->getLoginedUser()==nullptr)
	{
		cout<<"No user logged in."<<endl;
		return HANDLER_ERROR;
	}
	string line;
	getline(instr, line);
	trim(line);
	DateTime dt;
	instr>>dt;
	stringstream ssline(line);
	string text;
	getline(ssline,text);
	eng->addTweet(eng->getLoginedUser()->name(), dt, text);
	return HANDLER_OK;
}

//Follow functions 
FollowHandler::FollowHandler()
{

}

FollowHandler::FollowHandler(Handler* next)
  : Handler(next)
{

}

bool FollowHandler::canHandle(const std::string& cmd) const
{
	return cmd == "FOLLOW";
}

Handler::HANDLER_STATUS_T FollowHandler::process(TwitEng* eng, std::istream& instr) const
{
	string line;
	getline(instr, line);
	trim(line);
	stringstream ssline(line);
	string followee;
	ssline >> followee;
	if(eng->getLoginedUser()!=nullptr)
	{
		eng->addFollower(followee);
	}
	else
	{
		cout<<"No user logged in."<<endl;
		return HANDLER_ERROR;
	}
	return HANDLER_OK;
}

//Follow functions 
SaveHandler::SaveHandler()
{

}

SaveHandler::SaveHandler(Handler* next)
  : Handler(next)
{

}

bool SaveHandler::canHandle(const std::string& cmd) const
{
	return cmd == "SAVE";
}

Handler::HANDLER_STATUS_T SaveHandler::process(TwitEng* eng, std::istream& instr) const
{
	string line;
	getline(instr, line);
	trim(line);
	stringstream ssline(line);
	string fileName;
	ssline >> fileName;
	eng->saveFile(fileName);
	return HANDLER_OK;
}

//Follow functions 
TrendingHandler::TrendingHandler()
{

}

TrendingHandler::TrendingHandler(Handler* next)
  : Handler(next)
{

}

bool TrendingHandler::canHandle(const std::string& cmd) const
{
	return cmd == "TRENDING";
}

Handler::HANDLER_STATUS_T TrendingHandler::process(TwitEng* eng, std::istream& instr) const
{
	string line;
	getline(instr, line);
	trim(line);
	stringstream ssline(line);
	int numS;
	ssline >> numS;

	if(numS < 0||line =="")
	{
		return HANDLER_ERROR;
	}
	eng->printTrending(numS);
	return HANDLER_OK;
}

//login functions 
LoginHandler::LoginHandler()
{

}

LoginHandler::LoginHandler(Handler* next)
  : Handler(next)
{

}

bool LoginHandler::canHandle(const std::string& cmd) const
{
	return cmd == "LOGIN";
}

Handler::HANDLER_STATUS_T LoginHandler::process(TwitEng* eng, std::istream& instr) const
{
	string line;
	getline(instr, line);
	trim(line);
	stringstream ssline(line);
	string username;
	ssline >> username;
	string pw;
	ssline >> pw;
	if(username == "" || pw == "")
	{
		cout<<"Invalid username/password."<<endl;
		return HANDLER_ERROR;
	}

	if(eng->loginAttempt(username, pw))
	{
		cout<<"Login successful."<<endl;
		return HANDLER_OK;
	}
	else
	{
		cout<<"Invalid username/password."<<endl;
		return HANDLER_ERROR;
	}
	
}

//login functions 
LogoutHandler::LogoutHandler()
{

}

LogoutHandler::LogoutHandler(Handler* next)
  : Handler(next)
{

}

bool LogoutHandler::canHandle(const std::string& cmd) const
{
	return cmd == "LOGOUT";
}

Handler::HANDLER_STATUS_T LogoutHandler::process(TwitEng* eng, std::istream& instr) const
{
	eng->logout();
	return HANDLER_OK;
}